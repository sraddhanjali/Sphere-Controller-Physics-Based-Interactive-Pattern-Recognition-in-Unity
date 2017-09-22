using UnityEngine;

public class AddButtons : MonoBehaviour {

	[SerializeField]
	public Transform flowField;

	[SerializeField]
	public GameObject c; 

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
				GameObject cube = Instantiate(c, new Vector3(-1 + xValue,  2 + yValue, 0), c.transform.rotation) as GameObject;
				cubeNum += 1;
				cube.name = "" + cubeNum;
				xValue += 1;
			}
			yValue -= 1;
			xValue = 0;
		}
	}
}