using UnityEngine;
using System.Collections.Generic;

public class AddButtons : MonoBehaviour {

	[SerializeField]
	public Transform flowField;

	[SerializeField]
	public GameObject cube; 
	public GameObject outsideCube; 

	int worldWidth  = 6;
	int worldHeight  = 3;
	int cubeNum = 0;

	void  Awake () {
		CreateWorld ();
	}

	void CreateWorld () {
		int xValue = 0;
		int yValue = 0;
		for(int y = 0; y < worldWidth; y++) {
			for(int x = 0; x < worldHeight; x++) {                
				GameObject c1 = Instantiate(cube, new Vector3(-1 + xValue,  2 + yValue, 0), cube.transform.rotation) as GameObject;
				GameObject c2 = Instantiate(outsideCube, new Vector3(-1 + xValue,  2 + yValue, 0), cube.transform.rotation) as GameObject;
				//Color color = c2.GetComponent<Renderer> ().material.color;
				Color color = Color.white;
				color.a = 0.5f; // 50 % transparent
				c2.GetComponent<Renderer> ().material.color = color;
				cubeNum += 1;
				c1.name = "" + cubeNum;
				xValue += 1;
				c2.layer = 8;
			}
			yValue -= 1;
			xValue = 0;
		}
	}
}