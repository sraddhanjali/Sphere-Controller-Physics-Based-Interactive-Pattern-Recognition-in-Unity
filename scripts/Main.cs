using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public Sprite[] sprites;
	List<GameObject> go;
	List<Board> boardList = new List<Board>();

	public static int repetition = 40;
	public int totalRepetition = 0;
	public static int level = 0;
	public static string currLabel = "a";
	public static int patternIndex = 0;
	public static float timeLeft = 15.0f;

	PatternGrid pg = new PatternGrid();
	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();

	/* game state variables */
	private bool settingGame = false;
	public static bool gameover = false;
	public static int won = 0;
	public static bool done = false;
	public static bool increaseLevel = false;
	public static bool top = false;
	public static string allPath;
	public static string pattPath;
	public static int playerPoints = 0;
	private List<string> labels = new List<string>(); //{ "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y"};
	List<string> topLabels = new List<string>(){"i", "r", "s", "t", "u", "v", "y"};
	List<string> bottomLabels = new List<string>(){"a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "l", "m", "n", "o", "p", "q", "w", "x"};
	
	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false && done == false) {
			GUILayout.Label ("\n Level: " + level + "\n Time:" + (int)timeLeft + "\n Points:" + playerPoints, guiStyle);
		} else if(done == true){
			GUILayout.Label ("Won", guiStyle);
		} else {
			GUILayout.Label ("Game Over", guiStyle);
		}
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void DecorateCube(List<GameObject> patternCubeObject){
		gd.DrawLines (patternCubeObject);
		/*StartCoroutine (gd.RemoveLines(patternCubeObject));*/
	}

	void InitSetup(){
		// we are setting_game so this has to be true, set cubes to initial colors, get patterns to draw on, decorate them and store current pattern in a datastructure
		// then we will be done with setting the game hence setting_game = false
		settingGame = true;
		/* original
		go = pg.GetPatterns();
		*/
		/* set initial cubes colors */
		pg.InitialCubesColor();
		// a sample of simple pattern from paper
		go = pg.GetSimplePatterns ();

		/* just z pattern */
		//go = pg.GetZPatterns();

		DecorateCube (go);
		gl.SetCurrentPattern (pg.GetCurrentSelPattern());
		settingGame = false;

	}

	void SaveFile(){
		//  to save the data
		string filePath = Application.persistentDataPath;
		string f1 =  string.Format(@"{0}.csv", Guid.NewGuid());
		string f2 = string.Format("labels.csv");
		allPath = filePath + "/" + f1;
		pattPath = filePath + "/" + f2;
	}

	void Awake(){
		// loading scavengers sprite and set filenames to store data
		LoadSprites ();
		SaveFile ();
	}

	void TotalRepetition(){
		totalRepetition = repetition * labels.Count;
	}

	void Start ()
	{
		ReadFileTest();
		InitSetup ();
		SetLabelLevel ();
		TotalRepetition ();
	}

	void ClearVariables(){
		gd.RemoveLines(go);
		go.Clear ();
	}

	void SetLabelLevel(){
	/* level setup work */
		if (level % repetition == 0 && level != 0) {
			patternIndex += 1;
			currLabel = labels [patternIndex];
			if (topLabels.Contains(currLabel))
			{
				top = true;
			} else if (bottomLabels.Contains(currLabel))
			{
				top = false;
			}
		}
		/*
		Debug.Log (currLabel);
		Debug.Log (level);
		*/
	}

	IEnumerator NewLevelWork(){
		// before new level is loaded, clear variables then wait for 0.8 then increase the level, setlabel of current pattern to be swiped and reset time to 10s
		// TODO: figure out where increaseLevel should be placed
		increaseLevel = false;
		ClearVariables ();	
		yield return new WaitForSeconds (1.5f);
		level += 1;
		SetLabelLevel ();
		InitSetup ();
		timeLeft = 15.0f;
	}

	void Reset(){
		timeLeft = 0;
		gameover = false;
		level = 0;
		Main.increaseLevel = false;
	}

	void GameOver(){
		// game over so timeleft set to 0, load menu and set level to 1
		Reset();
		SceneManager.LoadScene ("Menu");
	}

	void ReadFileTest()
	{
 
		string line;
		System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + "/Resources/freeflow.txt"); //load text file with pattern
		//Board board = new Board();
		int patternNo = 0;
		string patternName;
		int lenSplit2;
		// first pattern
		while ((line = file.ReadLine()) != null)
		{ //while text exists.. repeat
			Board board = new Board();
			char[] delimiterChar = { ',' }; //comma separator
			// split1 holds patterns array where each pattern is separated by spaces, and in the end holds the pattern name 
			string[] split1 = line.Split(delimiterChar, StringSplitOptions.None); //each line splits parts with # character
			int lenSplit1 = split1.Length;
			int lastIndex = lenSplit1 - 1;
			Pattern bogusPattern = new Pattern();
			Pattern obstaclePattern = new Pattern();
			List<int> ob = new List<int>();
			patternNo += 1;
			List<List<int>> bo = new List<List<int>>();
			for (int i = 0; i < lenSplit1 -1; i++)
			{
				// pipes and pattern name
				// splitting first pattern based on space and getting each number one by one below as per pattern type
				string[] split2 = split1[i].Trim().Split(null);
				lenSplit2 = split2.Length;
				// pattern index
				List<int> p = new List<int>();
				for (int j = 0; j < lenSplit2; j++)
				{
					//string eachNo = split2[j];
					p.Add(int.Parse(split2[j]));
				}
				Pattern mainPattern = new Pattern();
				
				if (lenSplit2 != 1){
					if (i == 0 ){
						mainPattern.AddPattern(p, true);
						board.AddPattern(mainPattern);
					}
					else{
						// bogus pattern index
						bo.Add(p);
					}	
				}
				else{
					// list with size 1 values are obstacles
					ob.Add(p[p.Count - 1]);
				}
			}
			
			patternName = split1[lastIndex].Trim();
			//Debug.Log(patternName);
			bogusPattern.AddPattern(bo);
			obstaclePattern.AddPattern(ob);
			board.AddPattern(bogusPattern);
			board.AddPattern(obstaclePattern);
			board.AddPatternName(patternName);
			labels.Add(patternName);
			boardList.Add(board);
		}
		file.Close();
	}

	void Update () {
		if (level <= totalRepetition) {
			if (gameover == false) {
				if (settingGame) {
					return;
				} else {
					if (increaseLevel) {
						StartCoroutine (NewLevelWork ());
					}
					gl.TouchLogic ();
					timeLeft -= Time.deltaTime;
				}
			} else if (gameover == true) {
				GameOver ();
			}
		}
	}
}