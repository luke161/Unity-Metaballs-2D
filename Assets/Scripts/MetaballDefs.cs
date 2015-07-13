using UnityEngine;

/// <summary>
/// Lookup tables for marching square implementation.
/// 
/// Author: Luke Holland (@luke161)
/// http://lukeholland.me/
/// 
/// </summary>

public static class MetaballDefs 
{

	public static Vector3[][] pointLookupTable = new Vector3[][]{
		// 0
		new Vector3[]{ },
		// 1
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(0,-1,2), new Vector3(-1,-1,-1) },
		// 2
		new Vector3[]{ 	new Vector3(0,-1,2),new Vector3(1,0,1), new Vector3(1,-1,-1) },	
		// 3
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(1,0,1), new Vector3(-1,-1,-1), new Vector3(1,-1,-1) },	
		// 4
		new Vector3[]{ 	new Vector3(0,1,0), new Vector3(1,0,1), new Vector3(1,1,-1) },	
		// 5
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(0,-1,2), new Vector3(-1,-1,-1),
			new Vector3(0,1,0), new Vector3(1,0,1), new Vector3(1,1,-1) },
		// 6
		new Vector3[]{ 	new Vector3(0,1,0), new Vector3(0,-1,2), new Vector3(1,1,-1), new Vector3(1,-1,-1)},	
		// 7
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(0,1,0), new Vector3(1,1,-1), new Vector3(1,-1,-1), new Vector3(-1,-1,-1) },	
		// 8
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(0,1,0), new Vector3(-1,1,-1)},	
		// 9
		new Vector3[]{ 	new Vector3(0,1,0), new Vector3(0,-1,2), new Vector3(-1,1,-1), new Vector3(-1,-1,-1)},
		// 10
		new Vector3[]{ 	new Vector3(0,-1,2),new Vector3(1,0,1), new Vector3(1,-1,-1),
			new Vector3(-1,0,3),new Vector3(0,1,0), new Vector3(-1,1,-1) },
		// 11
		new Vector3[]{ 	new Vector3(0,1,0), new Vector3(1,0,1),new Vector3(-1,1,-1), new Vector3(-1,-1,-1), new Vector3(1,-1,-1)},
		// 12
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(1,0,1), new Vector3(-1,1,-1), new Vector3(1,1,-1)},
		// 13
		new Vector3[]{ 	new Vector3(0,-1,2),new Vector3(1,0,1), new Vector3(-1,1,-1), new Vector3(1,1,-1), new Vector3(-1,-1,-1)},
		// 14
		new Vector3[]{ 	new Vector3(-1,0,3),new Vector3(0,-1,2), new Vector3(-1,1,-1), new Vector3(1,1,-1), new Vector3(1,-1,-1)},
		// 15
		new Vector3[]{ new Vector3(-1,1,0), new Vector3(1,1,0), new Vector3(1,-1,0), new Vector3(-1,-1,0) }											
	};
	
	public static int[][] triangleLookupTable = new int[][]{
		// 0
		new int[] {},
		// 1
		new int[] {	0,1,2},
		// 2
		new int[] {	0,1,2},
		// 3
		new int[] {	0,1,2,
				   	1,2,3},
		// 4
		new int[] {	0,1,2},
		// 5
		new int[] {	0,1,2,
					5,4,3},
		// 6
		new int[] {	0,1,2,
				   	1,2,3},
		// 7
		new int[] {	0,4,3,
				  	0,1,3,
				   	1,2,3},
		// 8
		new int[] {	0,1,2},
		// 9
		new int[] {	0,1,2,
			       	1,2,3},
		// 10
		new int[] {	0,1,2,
					3,4,5},
		// 11
		new int[] {	0,2,3,
					0,3,1,
					1,3,4},
		// 12
		new int[] {	0,1,2,
					1,2,3},
		// 13
		new int[] {	2,3,1,
					2,1,0,
					2,0,4},
		// 14
		new int[] {	0,2,3,
					0,3,1,
					1,3,4},
		// 15
		new int[] {	0,1,3,
					1,2,3}
	};

}

