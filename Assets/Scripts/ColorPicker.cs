using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Luke Holland (@luke161)
/// http://lukeholland.me/
/// 
/// </summary>

public class ColorPicker : MonoBehaviour {

	public Color[] colorMaterials;
	public Color[] colorBackgrounds;

	public Camera targetCamera;
	public Material targetMaterial;

	protected void Awake()
	{
		int index = Random.Range(0,colorMaterials.Length);

		targetCamera.backgroundColor = colorBackgrounds[index];
		targetMaterial.color = colorMaterials[index];
	}


}
