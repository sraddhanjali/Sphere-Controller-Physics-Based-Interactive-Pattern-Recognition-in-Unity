using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

	public List<GameObject> go = new List<GameObject>();

	public List<int> redPaths = new List<int>() {1, 6, 11, 16, 21, 22};

	public List<int> yellowPaths = new List<int>() {3, 2, 7, 12, 17};

	public List<int> greenPaths = new List<int>() {8, 13, 18, 23};

	public List<int> bluePaths = new List<int>() {5, 4, 9, 14, 19};

	public List<int> magentaPaths = new List<int>() {10, 15, 20, 25, 24};

	public List<int> currentPaths = new List<int>();

	Collider2D[] cubeColliders;

	public Vector3 lastMousePos;

	bool red = false;
	bool yellow = false;
	bool green = false;
	bool blue = false;
	bool magenta = false;

	private GUIStyle guiStyle = new GUIStyle(); //create a new variable

	public void Start(){
		GetCubes();
		cubeColliders = new Collider2D[0];
	}

	public static int flows = 0;

	public static int clickNum = 0;

	protected void OnGUI(){

		guiStyle.fontSize = 50; //change the font size

		GUILayout.Label ("Flows:" + flows.ToString() + "\n Click Num:" + clickNum.ToString(), guiStyle);

		if (flows == 5) {
			guiStyle.fontSize = 100; //change the font size
			GUILayout.Label ("YOU WON!, guiStyle);

		}

	}
		
	void GetCubes(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 1; i < objects.Length + 1; i++) {
			go.Add(objects[i-1]);
			GameObject b = go [i-1];
			b.layer = 8;
			if (i == 1 || i == 22) {
				b.GetComponent<Renderer> ().material.color = Color.red;
			} else if (i == 3 || i == 17) {
				b.GetComponent<Renderer>().material.color = Color.yellow;
			} else if (i == 8 || i == 23) {
				b.GetComponent<Renderer>().material.color = Color.green;
			} else if (i == 5 || i == 19) {
				b.GetComponent<Renderer>().material.color = Color.blue;
			} else if (i == 10 || i == 24) {
				b.GetComponent<Renderer>().material.color = Color.magenta;
			} 
		}
	}

	bool checkEqual(List<int> List1, List<int> List2){
		if (List1.Count == List2.Count)
		{
			for(int i = 0; i < List1.Count; i++)
			{
				if(List1[i] != List2[i])
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public void PrintCollection<T>(IEnumerable<T> col)
	{
		foreach(var item in col)
			Debug.Log("Item in currentpaths:" + item); // Replace this with your version of printing
	}

	protected void HighlightCubeOnClick(){
		int currentPathSize = currentPaths.Count;
		int currentCube;
		if(Input.GetMouseButton(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask("Cube"));

			if ((Input.mousePosition - lastMousePos).sqrMagnitude > 7) {
				foreach (Collider2D c2 in currentFrame) {
					for (int i = 0; i < cubeColliders.Length; i++) {
						if (c2 == cubeColliders [i]) {
							currentCube = int.Parse (c2.name);
							clickNum += 1;
							if (currentPathSize == 0) {
								switch (currentCube) {
								case 1:
									Debug.Log("red!");
									red = true;
									break;
								case 3:
									Debug.Log("yellow");
									yellow = true;
									break;
								case 8:
									Debug.Log("green");
									green = true;
									break;
								case 5:
									Debug.Log("blue");
									blue = true;
									break;
								case 10:
									Debug.Log("magenta");
									magenta = true;
									break;
								default:
									Debug.Log("Invalid move");
									break;
								}
							}

							//PrintCollection (currentPaths);

							//Debug.Log ("Current Cube: " + currentCube);
							//Debug.Log ("yellow:" + yellowPaths [currentPathSize]);
							//Debug.Log ("Currentpathsize:" + currentPathSize);
							//Debug.Log ("yellow" + yellow);

							if (currentPaths.Contains (currentCube)) {
								Debug.Log ("already exists");
							} else {
								if (red) {
									//Debug.Log ("TRUE");
									if (currentCube == redPaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										Debug.Log ("Current cube added:" + currentCube);
										if (checkEqual (redPaths, currentPaths)) {
											for (int m = 0; m < redPaths.Count; m++) {
												go [redPaths [m] - 1].gameObject.GetComponent<Renderer> ().material.color = Color.red;
												go [redPaths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = false;
											}
											flows += 1;
											Debug.Log ("FALSE");
											red = false;
											currentPaths.Clear ();
										}			
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
											red = false;
											currentPaths.Clear ();
										}
									}
								} 
									
								if (yellow) {
									if (currentCube == yellowPaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										if (checkEqual (yellowPaths, currentPaths)) {
											for (int m = 0; m < yellowPaths.Count; m++) {
												go [yellowPaths [m] - 1].gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
												go [yellowPaths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = false;
											}
											flows += 1;
											Debug.Log ("FALSE");
											yellow = false;
											currentPaths.Clear ();
										}		
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
											yellow = false;
											currentPaths.Clear ();
										}
									}

								}

								if (green) {
									if (currentCube == greenPaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										if (checkEqual (greenPaths, currentPaths)) {
											for (int m = 0; m < greenPaths.Count; m++) {
												go [greenPaths [m] - 1].gameObject.GetComponent<Renderer> ().material.color = Color.green;
												go [greenPaths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = false;
											}
											flows += 1;
											Debug.Log ("FALSE");
											green = false;
											currentPaths.Clear ();
										}
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
											green = false;
											currentPaths.Clear ();
										}
									}
								}

								if (blue) {
									if (currentCube == bluePaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										if (checkEqual (bluePaths, currentPaths)) {
											for (int m = 0; m < bluePaths.Count; m++) {
												go [bluePaths [m] - 1].gameObject.GetComponent<Renderer> ().material.color = Color.blue;
												go [bluePaths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = false;
											}
											flows += 1;
											Debug.Log ("FALSE");
											blue = false;
											currentPaths.Clear ();
										}
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
											blue = false;
											currentPaths.Clear ();
										}
									}
								}
								if (magenta) {
									if (currentCube == magentaPaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										if (checkEqual (magentaPaths, currentPaths)) {
											for (int m = 0; m < magentaPaths.Count; m++) {
												go [magentaPaths [m] - 1].gameObject.GetComponent<Renderer> ().material.color = Color.magenta;
												go [magentaPaths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = false;
											}
											flows += 1;
											Debug.Log ("FALSE");
											magenta = false;
											currentPaths.Clear ();
										}	
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
											magenta = false;
											currentPaths.Clear ();
										}
									}
								}
							}
						}
					}
				}
			}

			cubeColliders = currentFrame; 
			lastMousePos = Input.mousePosition;
		}
	}

	protected void Update () {
		HighlightCubeOnClick ();
	}

}
