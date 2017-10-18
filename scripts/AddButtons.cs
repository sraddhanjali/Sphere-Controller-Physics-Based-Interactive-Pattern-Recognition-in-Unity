using UnityEngine;

public class AddButtons : MonoBehaviour {

	[SerializeField]
	public Transform flowField;

	[SerializeField]
	public GameObject insideCube; 
	public GameObject outsideCube; 

	public int columns = 8;                                         //Number of columns in our game board.
	public int rows = 8;                                            //Number of rows in our game board.

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
				GameObject c = Instantiate(insideCube, new Vector3(-1 + xValue,  2 + yValue, 0), insideCube.transform.rotation) as GameObject;
				GameObject c1 = Instantiate(outsideCube, new Vector3(-1 + xValue,  2 + yValue, 0), insideCube.transform.rotation) as GameObject;

				Color color = c1.GetComponent<Renderer> ().material.color;
				color.a = 0.5f; // 50 % transparent
				c1.GetComponent<Renderer> ().material.color = color;
				cubeNum += 1;
				c.name = "" + cubeNum;
				xValue += 1;
			}
			yValue -= 1;
			xValue = 0;
		}
	}
}