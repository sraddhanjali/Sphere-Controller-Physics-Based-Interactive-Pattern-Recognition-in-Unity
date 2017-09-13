using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

	public float levelStartDelay = 0.1f;

	private static float timeLeft = 10.00f;

	List<GameObject> go = new List<GameObject>();

	List<int> redPaths = new List<int>() {1, 6, 11, 16, 21, 22};

	List<int> yellowPaths = new List<int>() {3, 2, 7, 12, 17};

	List<int> greenPaths = new List<int>() {8, 13, 18, 23};

	List<int> bluePaths = new List<int>() {5, 4, 9, 14, 19};

	List<int> magentaPaths = new List<int>() {10, 15, 20, 25, 24};

	List<int> currentPaths = new List<int>();

	Collider2D[] cubeColliders;

	public static GameController instance = null;

	/*TODO public Material circleMat = new Material(shader);*/

	public Vector3 lastMousePos;

	bool red = false;
	bool yellow = false;
	bool green = false;
	bool blue = false;
	bool magenta = false;

	bool doingSetup = false;

	bool over = false;
	bool win = false;

	GUIStyle guiStyle = new GUIStyle(); //create a new variable

	private int level = 1;

	private int playerPoints = 0;

	static int flows = 0;

	static int clickNum = 0;

	static int pointsAdded = 0;

	protected void Awake(){
		SetLevels ();
		GetCubes();
		cubeColliders = new Collider2D[0];
	}
		
	protected void SetLevels(){
		switch (level) {
			case 1:
				red = true;
				break;
			case 2: 
				yellow = true;
				break;
			case 3:
				green = true;
				break;
			case 4:
				blue = true;
				break;
			case 5:
				magenta = true;
				break;
			default:
				break;
		}
	}

	protected void OnGUI(){

		guiStyle.fontSize = 35; //change the font size
		int tLeft = (int)Math.Ceiling(timeLeft);

		GUILayout.Label ("\n Flows: " + flows + " - Points: " + playerPoints + "  - Level: " + 
		                 level+ " - Time left: " + tLeft + " - Points added: "  + pointsAdded, guiStyle);

		guiStyle.fontSize = 70;
		if (over){
			
			GUILayout.Label("\nYOU LOST!!!", guiStyle);
		}
		if (win){
			GUILayout.Label("\n YOU WON!!!", guiStyle);
		}
	}

	void WinCube(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Cube");
		for (int i = 1; i < objects.Length + 1; i++){
			go.Add(objects[i - 1]);
			GameObject b = go[i - 1];
			b.layer = 8;
			b.GetComponent<Renderer>().material.color = Color.grey;
		}
	}
	void GetCubes(){
		doingSetup = true;

		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 1; i < objects.Length + 1; i++) {
			go.Add(objects[i-1]);
			GameObject b = go [i-1];
			b.layer = 8;
			switch (level) {
				case 1:
					if (i == 1 || i == 22)
					{
						//TODO create a small circle in it and color it red
						/*CircleCollider2D c1 = new CircleCollider2D();
						c1.radius = 1;
						c1.transform.position = b.transform.position;
						c1.GetComponent<Renderer> ().material.color = Color.red;*/
						b.GetComponent<Renderer>().material.color = Color.red;
					}
					else if (redPaths.Contains(i)){
						if (b.GetComponent<Renderer>().material.color == Color.red){
							continue;
						}
						else {
							b.GetComponent<Renderer>().material.color = Color.grey;
						}
					}

					break;
				case 2:
					if (i == 3 || i == 17) {
						b.GetComponent<Renderer> ().material.color = Color.yellow;
					}
					else if (yellowPaths.Contains(i))
					{
						if (b.GetComponent<Renderer>().material.color == Color.yellow)
						{
							continue;
						}
						else {
							b.GetComponent<Renderer>().material.color = Color.grey;
						}
					}
					break;
				case 3: 
					if (i == 8 || i == 23) {
						b.GetComponent<Renderer> ().material.color = Color.green;
					}
					else if (greenPaths.Contains(i))
					{
						if (b.GetComponent<Renderer>().material.color == Color.green)
						{
							continue;
						}
						else {
							b.GetComponent<Renderer>().material.color = Color.grey;
						}
					}
					break;
				case 4:
					if (i == 5 || i == 19)
					{
						b.GetComponent<Renderer>().material.color = Color.blue;
					}
					else if (bluePaths.Contains(i))
					{
						if (b.GetComponent<Renderer>().material.color == Color.blue)
						{
							continue;
						}
						else {
							b.GetComponent<Renderer>().material.color = Color.grey;
						}
					}
					break;
				case 5:
					if (i == 10 || i == 24) {
						b.GetComponent<Renderer> ().material.color = Color.magenta;
					} 
					else if (magentaPaths.Contains(i))
					{
						if (b.GetComponent<Renderer>().material.color == Color.magenta)
						{
							continue;
						}
						else {
							b.GetComponent<Renderer>().material.color = Color.grey;
						}
					}
					break;
				default:
					break;
			}
		}
		doingSetup = false;
	}

	bool CheckEqual(List<int> List1, List<int> List2){
		int list1C = List1.Count;
		int list2C = List2.Count;
		if (list1C == list2C){
			for(int i = 0; i < list1C; i++){
				if(List1[i] != List2[i]){
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

	protected Color GetCurrentLevelColor(){
		Color setColor = new Color();
		switch (level) {
			case 1:
				setColor = Color.red;
				break;
			case 2:
				setColor =  Color.yellow;
				break;
			case 3:
				setColor =  Color.green;
				break;
			case 4:
				setColor =  Color.blue;
				break;
			case 5:
				setColor =  Color.magenta;
				break;
			default:
				break;
		}
		return setColor;
	}

	void ChangePathsColors(List<int> paths, bool setPath){
		Color c;
		if (setPath){
			c = Color.white;
			setPath = false;
		}
		else {
			c = GetCurrentLevelColor();
		}
		for (int m = 0; m < paths.Count; m++) {
			go[paths[m] - 1].gameObject.GetComponent<Renderer>().material.color = c;
			go[paths [m] - 1].gameObject.GetComponent<Collider2D> ().enabled = setPath;
		}
	}

	void PlayerPointsLogic(float left){
		if (left > 7){
			playerPoints += 50;
			pointsAdded = 50;
		}
		else if (left > 4){
			playerPoints += 25;
			pointsAdded = 25;
		}
		else if (left > 0){
			playerPoints += 10;
			pointsAdded = 10;
		}
	}

	IEnumerator HighlightCubeOnClick(){
		int currentPathSize = currentPaths.Count;
		int currentCube;
		timeLeft -= Time.deltaTime;
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
							// print stuff
							/*
							PrintCollection (currentPaths);
							int reverseSize = redPaths.Count - currentPathSize - 1;
							Debug.Log("redpaths size" + redPaths.Count);
							Debug.Log ("Current Cube: " + currentCube);
							Debug.Log ("red:" + redPaths [currentPathSize]);
							Debug.Log ("Currentpathsize:" + currentPathSize);
							Debug.Log ("red" + red);
							Debug.Log("reverse" + redPaths [reverseSize]);
							*/

						if (currentPaths.Contains (currentCube)) {
								Debug.Log ("already exists");
							} else {
								if (red) {
									if (currentCube == redPaths [currentPathSize]) {
										currentPaths.Add (currentCube);
										Debug.Log ("Current cube added:" + currentCube);
										if (CheckEqual (redPaths, currentPaths)) {
											float t = timeLeft;
											ChangePathsColors (redPaths, false);
											Debug.Log ("FALSE");
										/*	red = false;
											level += 1;
											flows += 1;*/
											yield return new WaitForSeconds(levelStartDelay);
											ChangePathsColors(redPaths, true);
											currentPaths.Clear();
											red = false;
											level += 1;
											flows += 1;
											PlayerPointsLogic(t);
											timeLeft = 10;
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
										if (CheckEqual (yellowPaths, currentPaths)) {
											float t = timeLeft;
											ChangePathsColors(yellowPaths, false);
											Debug.Log ("FALSE");
											/*yellow = false;
											level += 1;
											flows += 1;*/
											yield return new WaitForSeconds(levelStartDelay);
											ChangePathsColors(yellowPaths, true);
											currentPaths.Clear();
											yellow = false;
											level += 1;
											flows += 1;
											PlayerPointsLogic(t);
											timeLeft = 10;
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
										if (CheckEqual (greenPaths, currentPaths)) {
											float t = timeLeft;
											ChangePathsColors(greenPaths, false);
											/*green = false;
											level += 1;
											flows += 1;*/
											Debug.Log ("FALSE");
											yield return new WaitForSeconds(levelStartDelay);
											ChangePathsColors(greenPaths, true);
											currentPaths.Clear();
											green = false;
											level += 1;
											flows += 1;
											PlayerPointsLogic(t);
											timeLeft = 10;
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
										if (CheckEqual (bluePaths, currentPaths)) {
											float t = timeLeft;
											ChangePathsColors(bluePaths, false);
											/*blue = false;
											level += 1;
											flows += 1;*/
											Debug.Log ("FALSE");
											yield return new WaitForSeconds(levelStartDelay);
											ChangePathsColors(bluePaths, true);
											currentPaths.Clear();
											blue = false;
											level += 1;
											flows += 1;
											PlayerPointsLogic(t);
											timeLeft = 10;
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
										if (CheckEqual (magentaPaths, currentPaths)) {
											float t = timeLeft;
											ChangePathsColors(magentaPaths, false);
											/*magenta = false;
											flows += 1;*/
											Debug.Log ("FALSE");
											yield return new WaitForSeconds(levelStartDelay);
											ChangePathsColors(magentaPaths, true);
											currentPaths.Clear ();
											magenta = false;
											flows += 1;
											PlayerPointsLogic(t);
											Won();
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
		if (timeLeft <= 0){
			GameOver();
			return;
		}
		if (doingSetup){
			return;
		}
		if (win)
		{
			WinCube();
			return;
		}
		SetLevels ();
		GetCubes ();
		StartCoroutine(HighlightCubeOnClick ());
	}

	public void GameOver(){
		over = true;
	}

	public void Won(){
		win = true;
	}
}
