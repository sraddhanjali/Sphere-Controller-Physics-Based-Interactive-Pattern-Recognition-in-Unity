using UnityEngine;
using System.Collections.Generic;

public class AddButtons : MonoBehaviour {

	[SerializeField]
	public Transform flowField;
	public GameObject cube; 
	public GameObject outsideCube; 

	int worldWidth  = 6;
	int worldHeight  = 3;
	int cubeNum = 0;

	void Awake () {
		CreateWorld ();
	}

	void CreateWorld () {
		float xValue = 0.0f;
		float yValue = 0.0f;
		float xoffset = 1.58f;
		float yoffset = 4.1f;

		for(int y = 0; y < worldWidth; y++) {
			for(int x = 0; x < worldHeight; x++) {                
				GameObject c1 = Instantiate (cube, new Vector3( -xoffset + xValue,  yoffset + yValue, 0), cube.transform.rotation) as GameObject;
				GameObject c2 = Instantiate(outsideCube, new Vector3(-xoffset + xValue, yoffset + yValue, 0), cube.transform.rotation) as GameObject;
				Color color = Color.white;
				color.a = 0.5f; // 50 % transparent
				c2.GetComponent<Renderer> ().material.color = color;
				cubeNum += 1;
				c1.name = "" + cubeNum;
				xValue += 1.48f;
				c2.layer = 8;
			}
			yValue -= 1.4f;
			xValue = 0;
		}
	}
}