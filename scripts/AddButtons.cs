using UnityEngine;
using System.Collections.Generic;

public class AddButtons : MonoBehaviour {

	[SerializeField]
	public GameObject cube; 
	
	[SerializeField]
	float xoffset = 0.5f;
	[SerializeField]
	float yoffset = 0.5f;
	
	int worldWidth  = 6;
	int worldHeight  = 3;
	int cubeNum = 0;

	void Awake () {
		CreateWorld ();
	}

	void CreateWorld () {
		for(int y = 3; y < worldWidth; y++) {
			for(int x = 0; x < worldHeight; x++) {
				GameObject c1 = Instantiate (cube, cube.transform.position + new Vector3(x*xoffset, -y*yoffset), cube.transform.rotation);
				cubeNum += 1;
				c1.name = "" + cubeNum;
			}
		}
	}
}