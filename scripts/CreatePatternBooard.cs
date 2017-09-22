using System;
using UnityEngine;
using System.Collections;
using System.IO;                    // For parsing text file, StringReader
using System.Collections.Generic;


public class CreatePatternBooard : MonoBehaviour {

	public float LINEWIDTH = 0.1f;
	List<GameObject> go = new List<GameObject>();
	public TextAsset wordFile;
	private List<string> lineList = new List<string>(); 
	Vector3 lastMousePos;
	Collider2D[] cubeColliders;

	// current randomly selected pattern
	string currentPattern;

	// number list of selected pattern
	List<int> currentPatternList = new List<int> ();

	// current list of swiped patterns
	List<int> currentPaths = new List<int>();

	// mapping of 1..9 numbers to our preferred cube area (3*3)
	Dictionary<int, int> pattern = new Dictionary<int, int>();
	Dictionary<int, int> patternRev = new Dictionary<int, int>();

	void Awake(){
		ReadWordList();
	}

	// Use this for initialization
	void Start () {
		// get the pattern
		currentPattern = GetRandomLine ();
		//Debug.Log(currentPattern);
		GetPatternList(currentPattern);
		Setup ();
	}

	void Setup(){
		CreateNumCubeMap ();
		InitialCubesColor ();
		GetCubePatterns ();
		GetCubePatterns (false);
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
		//char[] cp = currentPattern.ToCharArray();
		for (int i = 0; i < currentPattern.Length; i++) {
			//Debug.Log (currentPattern [i]);
			currentPatternList.Add((int)(currentPattern[i]-'0'));
		}
	}

	// color the cubes for gamescene
	void InitialCubesColor(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 1; i < objects.Length + 1; i++) {
			go.Add (objects [i - 1]);
			GameObject b = go [i - 1];
			b.layer = 8;
			b.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	void ColorPath(List<GameObject> patternCube){
		Color c = Color.grey;
		for (int i = 0; i < patternCube.Count; i++) {
			GameObject b = patternCube [i];
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
		for (int i = 0; i < patternCube.Count; i++) {
			if (i != patternCube.Count - 1) {
				GameObject g1 = patternCube [i];
				GameObject g2 = patternCube [i + 1];
				LineRenderer ln = g1.AddComponent<LineRenderer> ();
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.SetWidth(LINEWIDTH, LINEWIDTH); 
			}
		}
	}

	void GetCubePatterns(Boolean first=true){
		// Get Cubes with pattern numbers
		List<GameObject> patternCube = new List<GameObject>();
		GameObject g;
		for (int j = 0; j < currentPatternList.Count; j++) {
			//Debug.Log (currentPatternList [j]);
			if (first) {
				g = GameObject.Find (String.Format ("{0}", currentPatternList [j]));
			} else {
				int cubeNumber = pattern [currentPatternList [j]];
				//Debug.Log (cubeNumber);
				g = GameObject.Find (String.Format ("{0}", cubeNumber));
			}
			patternCube.Add(g);
		}
		ColorPath (patternCube);
		DrawLines (patternCube);
	}

	// Update is called once per frame
	void Update () {
		TouchLogic ();
	}

	public void ReadWordList()
	{
		// Check if file exists before reading
		if (wordFile)
		{
			string line;
			StringReader textStream = new StringReader(wordFile.text);
			while((line = textStream.ReadLine()) != null){
				// Read each line from text file and add into list
				lineList.Add(line);
			}
			textStream.Close();
		}
	}

	public string GetRandomLine()
	{
		// Returns random line from list
		return lineList[UnityEngine.Random.Range(0, lineList.Count)];
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

	void ChangePathsColors(List<int> paths, bool setPath, bool pat){
		Color c;
		if (setPath){
			c = Color.white;
			setPath = false;
		}
		else {
			c = Color.white;
		}

		int n;

		for (int m = 0; m < paths.Count; m++) {
			if (pat) {
				n = pattern [paths [m]] - 1;
			} else {
				n = paths [m] - 1;
			}
			go[n].gameObject.GetComponent<Renderer>().material.color = c;
			go[n].gameObject.GetComponent<Collider2D> ().enabled = setPath;
		}
	}

	void TouchLogic(){
		int currentPathSize = currentPaths.Count;
		int currentCube;
		bool pat = false;
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
								pat = true;
							} else {
								pat = false;
							}

							if (currentPaths.Contains (currentCube)) {
								Debug.Log ("already exists");
							} else {
									
								if (currentCube == currentPatternList [currentPathSize]) {
									currentPaths.Add (currentCube);
									Debug.Log ("Current cube added:" + currentCube);
									if (CheckEqual (currentPatternList, currentPaths)) {
										ChangePathsColors (currentPaths, false, pat);
										Debug.Log ("FALSE");
										ChangePathsColors (currentPaths, true, pat);
										currentPaths.Clear ();
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
