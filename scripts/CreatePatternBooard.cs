using System;
using UnityEngine;
using System.Collections;
using System.IO;                    // For parsing text file, StringReader
using System.Collections.Generic;


public class CreatePatternBooard : MonoBehaviour {

	//sprites
	public Sprite[] sprites;

	// line width for the pattern lines
	public float LINEWIDTH = 0.1f;

	// stores the gameobjects
	List<GameObject> go = new List<GameObject>();

	/* different levels file*/
	public TextAsset easyPatt;
	public TextAsset mediumPatt;
	public TextAsset hardPatt;
	public TextAsset randomPatt;

	private List<string> lineList = new List<string>(); 

	// for touch position and colliders
	Vector3 lastMousePos;
	Collider2D[] cubeColliders;

	GUIStyle guiStyle = new GUIStyle(); //create a new variable

	// level change signal
	private static bool levelChanged = false;

	// level
	private static int level = 1;

	// current randomly selected pattern
	string currentPattern;

	// number list of selected pattern
	List<int> currentPatternList = new List<int> ();

	// current list of swiped patterns
	List<int> currentPaths = new List<int>();

	// mapping of 1..9 numbers to our preferred cube area (3*3)
	Dictionary<int, int> pattern = new Dictionary<int, int>();
	Dictionary<int, int> patternRev = new Dictionary<int, int>();

	// setting up game
	bool settingGame = false;

	protected void OnGUI(){

		guiStyle.fontSize = 40; //change the font size
		GUILayout.Label ("\n Level: " + level, guiStyle);
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
	}

	void Awake(){
		LoadSprites ();
		CreateNumCubeMap ();
	}

	// Use this for initialization
	void Start () {
		InitSetup ();
	}

	void InitSetup(){
		settingGame = true;
		GetPatterns();
		InitialCubesColor ();
		GetCubePatterns (true);
		GetCubePatterns (false);
		settingGame = false;
	}

	// mapping of 1..9 numbers to our preferred cube area (3*3)		
	void CreateNumCubeMap(){

		// pattern map
		pattern.Add (1, 10);
		pattern.Add (2, 11);
		pattern.Add (3, 12);
		pattern.Add (4, 13);
		pattern.Add (5, 14);
		pattern.Add (6, 15);
		pattern.Add (7, 16);
		pattern.Add (8, 17);
		pattern.Add (9, 18);

		// reverse  pattern map
		patternRev.Add (10, 1);
		patternRev.Add (11, 2);
		patternRev.Add (12, 3);
		patternRev.Add (13, 4);
		patternRev.Add (14, 5);
		patternRev.Add (15, 6);
		patternRev.Add (16, 7);
		patternRev.Add (17, 8);
		patternRev.Add (18, 9);
	}

	void GetPatternList(string p){
		for (int i = 0; i < currentPattern.Length; i++) {
			//Debug.Log (currentPattern [i]);
			currentPatternList.Add((int)(currentPattern[i]-'0'));
		}
	}

	// color the cubes for gamescene
	void InitialCubesColor(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 0; i < objects.Length; i++) {
			go.Add (objects [i]);
			GameObject b = go [i];
			b.layer = 8;
			//b.GetComponent<Renderer>().material.color = Color.white;
			b.GetComponent<SpriteRenderer> ().sprite = sprites [6];
		}
	}

	void ColorCubePath(List<GameObject> patternCube){
		Color c = Color.grey;
		GameObject b;
		for (int i = 0; i < patternCube.Count; i++) {
			b = patternCube [i];
			b.layer = 8;
			if (i == 0) {
				c = Color.red;	
			} else {
				c = Color.grey;
			}
			b.GetComponent<Renderer>().material.color = c;
		}
	}

	void DrawLines(List<GameObject> patternCube){
		LineRenderer ln;
		for (int i = 0; i < patternCube.Count; i++) {
			GameObject g1 = patternCube [i];
			if (i != patternCube.Count - 1) {
				GameObject g2 = patternCube [i + 1];
				if (i == 0) {
					g1.GetComponent<SpriteRenderer> ().sprite = sprites [0];
				} else {
					g1.GetComponent<SpriteRenderer> ().sprite = sprites [19];
				}
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer> ();
				} else {
					ln = g1.AddComponent<LineRenderer> ();
				}
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.material.color = Color.white;
				ln.startWidth = LINEWIDTH;
				ln.endWidth = LINEWIDTH;
			} else {
				if (i == patternCube.Count - 1) {
					g1.GetComponent<SpriteRenderer> ().sprite = sprites [20];
				}
			}
			g1.AddComponent<TrailRenderer> ();
		}
	}

	IEnumerator RemoveLines(List<int> p){
		yield return new WaitForSeconds(1f);
		LineRenderer ln, ln1;
		for (int i = 0; i < p.Count; i++) {
			GameObject g1 = GameObject.Find (String.Format ("{0}", pattern[p [i]]));
			g1.GetComponent<SpriteRenderer> ().sprite = sprites [6];
			GameObject g3 = GameObject.Find (String.Format ("{0}", p[i]));
			g3.GetComponent<SpriteRenderer> ().sprite = sprites [6];
			if (i != p.Count - 1) {
				//second block
				//GameObject g2 = GameObject.Find (String.Format ("{0}", pattern[p [i+1]]));
				ln = g1.GetComponent<LineRenderer> ();
				ln.SetPosition (0, Vector3.zero);
				ln.SetPosition (1, Vector3.zero);
				// first block
				//GameObject g4 = GameObject.Find (String.Format ("{0}", p[i+1]));
				ln1 = g3.GetComponent<LineRenderer> ();
				ln1.SetPosition (0, Vector3.zero);
				ln1.SetPosition (1, Vector3.zero);
			}
		}
	}
		
	// get the cube numbers which are to be overlayed over the number sequence
	void GetCubePatterns(Boolean first){
		// Get Cubes with pattern numbers
		List<GameObject> patternCube = new List<GameObject>();
		GameObject g;
		for (int j = 0; j < currentPatternList.Count; j++) {
			//Debug.Log (currentPatternList [j]);

			// pattern to copy
			if (first) {
				g = GameObject.Find (String.Format ("{0}", currentPatternList [j]));
			} else { 
				// overlay to replicate the pattern over 
				int cubeNumber = pattern [currentPatternList [j]];
				//Debug.Log (cubeNumber);
				g = GameObject.Find (String.Format ("{0}", cubeNumber));
			}
			patternCube.Add(g);
		}
		//ColorCubePath (patternCube);
		AnimateFood(patternCube);
		patternCube.Clear ();
	}

	void AnimateFood(List<GameObject> patternCube){
		DrawLines (patternCube);
		StartCoroutine (RemoveLines(currentPatternList));		
	}

	IEnumerator NewLevelWork(){
		level += 1;
		currentPaths.Clear ();
		currentPatternList.Clear ();
		lineList.Clear ();
		go.Clear ();
		levelChanged = false;
		yield return new WaitForSeconds (1f);
		InitSetup ();
	}

	// Update is called once per frame
	void Update () {
		if (settingGame) {
			return;
		} else if (levelChanged) {
			StartCoroutine (NewLevelWork());
		}
		TouchLogic();		
	}


	// get the pattern sequence from file
	public void GetPatterns()
	{
		// Check if file exists before reading
		if (easyPatt){
			string line;
			StringReader textStream = new StringReader(easyPatt.text);
			while((line = textStream.ReadLine()) != null){
				// Read each line from text file and add into list
				lineList.Add(line);
			}
			textStream.Close();
		}

		// get the pattern
		currentPattern = GetRandomLine ();
		//Debug.Log(currentPattern);

		GetPatternList(currentPattern);
	}

	// get random line from the file as random number sequence
	public string GetRandomLine()
	{
		// Returns random line from list
		return lineList[UnityEngine.Random.Range(0, lineList.Count)];
	}

	// check if two lists are exactly the same
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

	// change the path colors of which the user successfully traversed
	void ChangePathsColors(List<int> paths){
		int n;
		for (int m = 0; m < paths.Count; m++) {
			n = pattern [paths [m]] - 1;
			Debug.Log("Changing paths' color");
			go[n].gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	// sense the touches and mark the correct one and move to new level
	void TouchLogic(){
		int currentPathSize = currentPaths.Count;
		int currentCube;
		if (Input.GetMouseButton (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			if ((Input.mousePosition - lastMousePos).sqrMagnitude > 7) {
				foreach (Collider2D c2 in currentFrame) {
					for (int i = 0; i < cubeColliders.Length; i++) {
						if (c2 == cubeColliders [i]) {
							currentCube = int.Parse (c2.name);
							Debug.Log (currentCube);
							if (currentCube > 9) {
								currentCube = patternRev [currentCube];
							}
							if (currentPaths.Contains (currentCube)) {
								Debug.Log ("already exists");
							} else {
								if (currentCube == currentPatternList [currentPathSize]) {
									currentPaths.Add (currentCube);
									GameObject g1 = GameObject.Find (String.Format ("{0}", currentCube));
									GameObject g2 = GameObject.Find (String.Format ("{0}", pattern[currentCube]));
									if (currentPathSize == 0) {
										g1.GetComponent<SpriteRenderer> ().sprite = sprites [0];
										g2.GetComponent<SpriteRenderer> ().sprite = sprites [0];
									} else if (currentPathSize < currentPatternList.Count - 1) {
										g1.GetComponent<SpriteRenderer> ().sprite = sprites [19];
										g2.GetComponent<SpriteRenderer> ().sprite = sprites [19];
									} else {
										g1.GetComponent<SpriteRenderer> ().sprite = sprites [20];
										g2.GetComponent<SpriteRenderer> ().sprite = sprites [20];
									}
									Debug.Log ("Current cube added:" + currentCube);
									if (CheckEqual (currentPatternList, currentPaths)) {
										ChangePathsColors (currentPaths);
										RemoveLines (currentPaths);
										levelChanged = true;
									} else {
										if (currentPaths.Contains (currentCube)) {
											Debug.Log ("already swiped");
										} else {
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
}
