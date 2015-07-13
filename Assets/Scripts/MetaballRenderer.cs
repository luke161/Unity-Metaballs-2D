using UnityEngine;

/// <summary>
/// Generates and renders 2D metaballs using marching squares to create a procedural mesh. 
/// Based on: http://jamie-wong.com/2014/08/19/metaballs-and-marching-squares/
/// 
/// Author: Luke Holland (@luke161)
/// http://lukeholland.me/
/// 
/// </summary>

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MetaballRenderer : MonoBehaviour 
{
	
	public int numberOfBalls = 6;
	public Rect bounds = new Rect(-3,-2,6,3);
	public Vector2 gridResolution = new Vector2(80,80);

	private Metaball[] _metaballs;
	private GridSample[] _grid;
	private float _gridSize;

	private Vector3[] _vertices;
	private int[] _triangles;
	private Mesh _mesh;
	private int _verticesLength;
	private int _trianglesLength;

	protected void Awake()
	{
		Application.targetFrameRate = 60;

		// create metaballs
		_metaballs = new Metaball[numberOfBalls];
		int i = 0;
		for(; i<numberOfBalls; ++i){
			_metaballs[i] = new Metaball( new Vector3(	Random.Range(bounds.min.x,bounds.max.x),
			                            				Random.Range(bounds.min.y,bounds.max.y),0));
		}

		// create grid, used for caching samples and position lookups
		int c = 0, r = 0, cols = Mathf.FloorToInt(gridResolution.x), rows = Mathf.FloorToInt(gridResolution.y);
		float xstep = bounds.size.x/gridResolution.x;
		float x = bounds.xMin, y = bounds.yMin;

		_gridSize = xstep*0.5f;
		_grid = new GridSample[cols*rows];

		while(c<cols){		

			r = 0;
			y = bounds.yMin;

			while(r<rows){

				GridSample g = new GridSample(x,y);
				i = (r*cols)+c;
				g.iB = c+1>=cols ? i : (r*cols)+(c+1);
				g.iC = r+1>=rows ? i : ((r+1)*cols)+(c);
				g.iD = c+1>=cols || r+1>=rows ? i : ((r+1)*cols)+(c+1);

				_grid[(r*cols)+c] = g;

				++r;

				y += xstep;
			}		

			++c;
			x += xstep;
		}

		// create mesh and vertex/triangle buffers
		_verticesLength = cols*rows*4;
		_trianglesLength = _verticesLength;
		_vertices = new Vector3[_verticesLength];
		_triangles = new int[_trianglesLength];

		_mesh = new Mesh();
		_mesh.MarkDynamic();

		GetComponent<MeshFilter>().mesh = _mesh;
	}

	protected void Update()
	{
		// update ball positions
		int i = 0;
		for(; i<numberOfBalls; ++i){
			
			Metaball ball = _metaballs[i];
			ball.position += ball.velocity*Time.deltaTime;
			
			if(ball.position.x>bounds.xMax || ball.position.x<bounds.xMin) ball.velocity.x = -ball.velocity.x;
			if(ball.position.y>bounds.yMax || ball.position.y<bounds.yMin) ball.velocity.y = -ball.velocity.y;
		}

		// update grid samples for new metaball positions
		int l = _grid.Length;
		for(i=0; i<l; ++i){
			SampleFunction(_grid[i]);
		}

		// build mesh from grid samples
		int vi = 0, ti = 0;
		for(i=0; i<l; ++i){

			GridSample grid = _grid[i];

			float sampleA = grid.sample;
			float sampleB = _grid[grid.iB].sample;
			float sampleC = _grid[grid.iC].sample;
			float sampleD = _grid[grid.iD].sample;

			// get the index value for this grid position
			int index = IndexFunction(sampleA,sampleB,sampleD,sampleC);
			if(index>0 && index<15){

				// populate vertext and triangle buffers for this grid position
				Vector3 p = new Vector3(grid.x,grid.y,0);

				Vector3[] points = MetaballDefs.pointLookupTable[index];
				int[] triangles = MetaballDefs.triangleLookupTable[index];
				int j = 0, k = triangles.Length;
				for(j = 0; j<k; j+=3){

					int t1 = triangles[j];
					int t2 = triangles[j+1];
					int t3 = triangles[j+2];

					Vector3 p0 = points[t1];
					Vector3 p1 = points[t2];
					Vector3 p2 = points[t3];

					// use SmoothFunction to Lerp points based on sample values
					p0 = SmoothFunction(p0,sampleA,sampleB,sampleC,sampleD);
					p1 = SmoothFunction(p1,sampleA,sampleB,sampleC,sampleD);
					p2 = SmoothFunction(p2,sampleA,sampleB,sampleC,sampleD);

					_vertices[vi] = p+(p0*_gridSize);
					_vertices[vi+1] = p+(p1*_gridSize);
					_vertices[vi+2] = p+(p2*_gridSize);

					_triangles[ti] = vi;
					_triangles[ti+1] = vi+1;
					_triangles[ti+2] = vi+2;

					vi += 3;
					ti += 3;
				}

			} else if(index==15) {

				Vector3 p = new Vector3(grid.x,grid.y,0);
				Vector3[] points = MetaballDefs.pointLookupTable[index];
				int[] triangles = MetaballDefs.triangleLookupTable[index];
				int j = 0, k = triangles.Length;
				for(j = 0; j<k; j+=3){
					
					Vector3 p0 = points[triangles[j]];
					Vector3 p1 = points[triangles[j+1]];
					Vector3 p2 = points[triangles[j+2]];

					_vertices[vi] = p+(p0*_gridSize);
					_vertices[vi+1] = p+(p1*_gridSize);
					_vertices[vi+2] = p+(p2*_gridSize);
					
					_triangles[ti] = vi;
					_triangles[ti+1] = vi+1;
					_triangles[ti+2] = vi+2;
					
					vi += 3;
					ti += 3;
				}

			}
		}

		// clear unused portion of vertex and triangle buffers
		System.Array.Clear(_vertices,vi,_verticesLength-vi);
		System.Array.Clear(_triangles,ti,_trianglesLength-ti);

		// update mesh
		_mesh.Clear(false);
		_mesh.vertices = _vertices;
		_mesh.triangles = _triangles;
	}

	private void SampleFunction(GridSample grid)
	{
		float z = 0;
		int i = 0;
		for(; i<numberOfBalls; ++i){

			Metaball ball = _metaballs[i];
			float x2 = grid.x-ball.position.x;
			float y2 = grid.y-ball.position.y;

			z += (ball.radius*ball.radius)/((x2*x2)+(y2*y2));
		}

		grid.sample = z;
	}

	private int IndexFunction(float b0, float b1, float b2, float b3)
	{
		int result = 0;

		if(b0>=1f) result |= 1;
		if(b1>=1f) result |= 2;
		if(b2>=1f) result |= 4;
		if(b3>=1f) result |= 8;

		return result;
	}

	private Vector3 SmoothFunction(Vector3 p, float sA, float sB, float sC, float sD)
	{
		if(p.z==0) 		p.x = Mathf.Lerp(-1f,1f,(1f-sC)/(sD-sC));
		else if(p.z==1) p.y = Mathf.Lerp(-1f,1f,(1f-sB)/(sD-sB));
		else if(p.z==2) p.x = Mathf.Lerp(-1f,1f,(1f-sA)/(sB-sA));
		else if(p.z==3) p.y = Mathf.Lerp(-1f,1f,(1f-sA)/(sC-sA));

		p.z = 0;

		return p;
	}



	private class Metaball {

		public Vector3 position;
		public Vector3 velocity;
		public float radius;

		public Metaball(Vector3 startPosition)
		{
			velocity = new Vector3(Random.Range(0.01f,0.5f)*(Random.Range(0f,1f)>0.5f ? 1 : -1),
			                       Random.Range(0.01f,0.5f)*(Random.Range(0f,1f)>0.5f ? 1 : -1),
			                       0);
			position = startPosition;
			radius = Random.Range(0.2f,0.7f);
		}
	}

	private class GridSample {

		public float x;
		public float y;
		public float sample;

		public int iB;
		public int iC;
		public int iD;

		public GridSample(float x, float y){
			this.x = x;
			this.y = y;
		}

	}

}
