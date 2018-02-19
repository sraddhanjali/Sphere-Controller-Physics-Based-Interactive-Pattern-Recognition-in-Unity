using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public Sprite[] sprites;
	List<GameObject> go;

	public static int repetition = 1;
	public int totalRepetition = 0;
	public static int level = 0;
	public static string currLabel = "a";
	public static int patternIndex = 0;
	public static float timeLeft = 10.0f;

	PatternGrid pg = new PatternGrid();
	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();

	/* game state variables */
	private bool settingGame = false;
	public static bool gameover = false;
	public static bool increaseLevel = false;
	public static string allPath;
	public static string pattPath;
	public static int playerPoints = 0;
	List<string> labels = new List<string>(){ "a", "b", "c", "d" , "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y"};

	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level+ "\n Time:" + (int)timeLeft + "\n Points:" + playerPoints, guiStyle);
		} else {
			GUILayout.Label ("GameOver", guiStyle);
		}
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void DecorateCube(List<GameObject> patternCubeObject){
		gd.DrawLines (patternCubeObject);
		/*StartCoroutine (gd.RemoveLines(patternCubeObject));*/
		//
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
		//
	}

	void Awake(){
		// loading scavengers sprite and set filenames to store data
		LoadSprites ();
		SaveFile ();
	}

	void TotalRepetition(){
		totalRepetition = repetition * labels.Count;
	}

	void Start () {
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
		timeLeft = 10.0f;
	}

	void GameOver(){
		// game over so timeleft set to 0, load menu and set level to 1
		timeLeft = 0;
		SceneManager.LoadScene ("Menu");
		gameover = false;
		level = 1;
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
		} else {
			GameOver ();
		} 
	}
}