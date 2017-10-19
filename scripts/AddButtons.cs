using UnityEngine;

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
				LineRenderer ln = c1.AddComponent<LineRenderer>();
				ln.sortingOrder = 1; 
				ln.sortingLayerName = "LineRenderer";
				Color color = c1.GetComponent<Renderer> ().material.color;
				color.a = 0.5f; // 50 % transparent
				c1.GetComponent<Renderer> ().material.color = color;
				cubeNum += 1;
				cube.name = "" + cubeNum;
				xValue += 1;
			}
			yValue -= 1;
			xValue = 0;
		}
	}
}