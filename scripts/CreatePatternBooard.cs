using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class CreatePatternBooard : MonoBehaviour {

	[SerializeField]
	public float LINEWIDTH = 0.5f;
	public Shader shader1;
	public int SWIPETHRESHOLD = 2;
	private static bool increaseLevel = false;
	private static bool reloadLevel = false;
	private static float timeLeft = 10.0f;
	private static int level = 1;
	bool settingGame = false;
	bool gameover = false;
	private int playerPoints = 0;
	static int pointsAdded = 0 ;

	public Sprite[] sprites;
	public AudioSource chop;
	List<GameObject> go = new List<GameObject>();
	public TextAsset wordFile;
	private List<string> lineList = new List<string>(); 
	Vector3 startPos;	
	Collider2D[] cubeColliders;
	GUIStyle guiStyle = new GUIStyle();
	string currentPattern;
	List<int> currentPatternList = new List<int> ();
	List<int> currentPaths = new List<int>();
	Grid outerGrid = new Grid();
	Helper h = new Helper();
	private string path;

	// pattern object to store during a gameplay
	Pattern p = new Pattern();
	// the list of patterns to be saved in file
	List<Pattern> patC = new List<Pattern>();


	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level + "\n Time:" + (int)timeLeft + "\n Points:" + playerPoints, guiStyle);
		} else {
			GUILayout.Label ("GameOver", guiStyle);
		}
	}

	void ColorCubePath(List<GameObject> patternCube){
		Color c = Color.grey;
		GameObject b;
		for (int i = 0; i < patternCube.Count; i++) {
			b = patternCube [i];
			b.layer = 8;
		}
	}

	void DrawLines(List<GameObject> patternCube){
		LineRenderer ln;
		for (int i = 0; i < patternCube.Count; i++) {
			if (i != patternCube.Count - 1) {
				GameObject g1 = patternCube [i];
				GameObject g2 = patternCube [i + 1];
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer> ();
				} else {
					ln = g1.AddComponent<LineRenderer> ();
				}
				g1.GetComponent<SpriteRenderer> ().material.color = Color.black;
				g2.GetComponent<SpriteRenderer> ().material.color = Color.black;
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.material.color = Color.white;
				ln.startWidth = LINEWIDTH;
				ln.endWidth = LINEWIDTH;
			} 
		}
	}

	IEnumerator RemoveLines(List<int> p){
	//void RemoveLines(List<int> p){
		yield return new WaitForSeconds(3f);
			
		LineRenderer ln1;
		for (int i = 0; i < p.Count; i++) {
			if (i != p.Count - 1) {
				int p1 = p [i];
				int p2 = p [i + 1];
				GameObject g1 = GameObject.Find (String.Format ("{0}", p1));
				ln1 = g1.GetComponent<LineRenderer> ();
				ln1.SetPosition (0, Vector3.zero);
				ln1.SetPosition (1, Vector3.zero);
			}
		}
	}

	void ClearVariables(){
		RemoveLines (currentPatternList);
		currentPaths.Clear ();
		currentPatternList.Clear ();
		lineList.Clear ();
		go.Clear ();
	}

	public string GetRandomLine()
	{
		return lineList[UnityEngine.Random.Range(0, lineList.Count)];
	}
		
	void ChangePathsColors(List<int> paths){
		for (int m = 0; m < paths.Count; m++) {
			go[paths[m]].gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	public void GetPatterns()
	{
		if (wordFile){
			string line;
			StringReader textStream = new StringReader(wordFile.text);
			while((line = textStream.ReadLine()) != null){
				lineList.Add(line);
			}
			textStream.Close();
		}
		currentPattern = GetRandomLine ();
		GetPatternList();
		AddPatternsBothGrids ();
	}

	void GetPatternList(){
		currentPatternList.Clear ();
		for (int i = 0; i < currentPattern.Length; i++) {
			currentPatternList.Add(outerGrid.pattern[(int)(currentPattern[i]-'0')]);
		}
	}
		
	void AddPatternsBothGrids(){
		List<int> grid = outerGrid.GetFirstGrid (currentPatternList[0]);
		grid.AddRange (currentPatternList);
		currentPatternList = grid;
	}

	void InitialCubesColor(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 0; i < objects.Length; i++) {
			go.Add (objects [i]);
			GameObject b = go [i];
			b.layer = 8;
			b.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	void GetCubePatterns(){
		List<GameObject> patternCube = new List<GameObject>();
		GameObject g;
		for (int j = 0; j < currentPatternList.Count; j++) {
			int cubeNumber = currentPatternList [j];
			g = GameObject.Find (String.Format ("{0}", cubeNumber));
			patternCube.Add(g);
		}
		ColorCubePath (patternCube);
		DrawLines (patternCube);
		patternCube.Clear ();
		StartCoroutine (RemoveLines(currentPatternList));	
	}

	void InitSetup(){
		settingGame = true;
		GetPatterns();
		InitialCubesColor ();
		GetCubePatterns ();
		settingGame = false;
	}

	void SwipeCube(int currentCube, Vector3 pos){
		int t = (int)timeLeft;
		int currentPathSize = currentPaths.Count;
		//Debug.Log (currentCube);
		GameObject a = GameObject.Find (currentCube.ToString ());
		if (currentPaths.Contains (currentCube)) {
			//Debug.Log ("already exists");
		} else {
				if (currentCube == currentPatternList [currentPathSize]) {

				string nums = currentCube.ToString ();
				string v1 = pos.ToString ();
				string ts = DateTime.Now.ToString ("yyyyMMddHHmmssffff");
				string together = nums + " " + v1 + " " + ts + "\n";
				Debug.Log (together);
				File.AppendAllText (path, together);

				//Debug.Log ("here");
				/* storing parameters */
				//p.SetPattern (currentCube);
				//p.SetCoordinates (currentCube, pos);
				//p.SetTimestamp (currentCube, DateTime.Now);

				/*------------------*/

				currentPaths.Add (currentCube);
				a.GetComponent<Renderer> ().material.color = Color.red;	
				chop.Play ();

				//Debug.Log ("Current cube added:" + currentCube);
				if (h.CheckEqual (currentPatternList, currentPaths)) {
					ChangePathsColors (currentPatternList);
					increaseLevel = true;
					PlayerPointsLogic (t);
					ClearVariables ();

					// save the pattern data object to a list of such objects
					//patC.Add (p);

					// save a pattern data object to a file
					//SaveTimeCoord (patC);
					//p = new Pattern ();
					//Debug.Log ("DONEEEEE!!!");
				} else {
					if (currentPaths.Contains (currentCube)) {
						//Debug.Log ("already swiped");
					} else {
						// Retry currentPaths.Clear ();
					}
				}
			} 
		}
	}

	void SaveTimeCoord(List<Pattern> pattrnList){
		foreach (Pattern p in pattrnList) { 
			Dictionary<int, Vector3> coordMap = p.GetCoordinates ();
			foreach (KeyValuePair<int, Vector3> kvp in coordMap) {
				int num = kvp.Key;
				string nums = num.ToString ();
				Vector3 v = kvp.Value;
				string v1 = v.ToString ();
				Dictionary<int, String> timeMap = p.GetTimestamp ();
				string ts = timeMap [num];
				string together = nums + " " + v1 + " " + ts + "\n";
				Debug.Log (together);
				File.AppendAllText (path, together);
			}
		}
	}

	void PlayerPointsLogic(int left){
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
			else if (left <= 0){
				gameover = true;
			}
		}

	void TouchLogic(){
		int currentCube;


		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame) {
				currentCube = int.Parse (c2.name);
				SwipeCube (currentCube, pos);
			}
		}
	}

	void Awake(){
		LoadSprites ();
		outerGrid.CreateNumCubeMap ();
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");

		// save each pattern to a file
		string filePath = Application.persistentDataPath;
		string fileName =  string.Format(@"{0}.txt", Guid.NewGuid());
		path = filePath + "/" + fileName;
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
	}

	void Start () {
		InitSetup ();
	}

	IEnumerator NewLevelWork(){
		level += 1;
		currentPaths.Clear ();
		currentPatternList.Clear ();
		lineList.Clear ();
		go.Clear ();
		increaseLevel = false;
		yield return new WaitForSeconds (1f);
		InitSetup ();
	}

	void Update () {
		if (gameover == false) {
			if (settingGame) {
				return;
			} else if (increaseLevel) {
				StartCoroutine (NewLevelWork ());
				timeLeft = 10.0f;
				timeLeft -= Time.deltaTime;
			}
			TouchLogic ();
		} else if (gameover == true) {
			timeLeft = 0;
			GUILayout.Label ("GameOver", guiStyle);
		}
	}
}