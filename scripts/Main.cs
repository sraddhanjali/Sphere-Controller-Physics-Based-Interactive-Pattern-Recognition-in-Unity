using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public Sprite[] sprites;
	List<GameObject> go;

	public static int level = 1;
	public static string currLabel = "";
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
	List<string> labels = new List<string>(){ "a", "b", "c", "d", "e" };

	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level + "\n Time:" + (int)timeLeft + "\n Points:" + playerPoints, guiStyle);
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
		///*  to save the data
		string filePath = Application.persistentDataPath;
		string f1 =  string.Format(@"{0}.txt", Guid.NewGuid());
		string f2 = string.Format("labels.txt");
		allPath = filePath + "/" + f1;
		pattPath = filePath + "/" + f2;
		//*/
	}

	void Awake(){
		LoadSprites ();
		SaveFile ();
	}

	void Start () {
		InitSetup ();
	}

	void ClearVariables(){
		gd.RemoveLines(go);
		go.Clear ();
	}

	void SetLabelLevel(){
		if (level == 0) {
			patternIndex = 0;
		} else {
			patternIndex += 1;
		}
		currLabel = labels [patternIndex];
		Debug.Log (currLabel);
	}

	IEnumerator NewLevelWork(){
		increaseLevel = false;
		ClearVariables ();
		level += 1;
		int oldlevel = level - 1;
		if (oldlevel % 5 == 0) {
			SetLabelLevel ();
		}
		yield return new WaitForSeconds (0.5f);
		InitSetup ();
		timeLeft = 10.0f;
	}

	void GameOver(){
		timeLeft = 0;
		SceneManager.LoadScene ("Menu");
		gameover = false;
		level = 1;
	}

	void Update () {
		if (level <= 25) {
			if (gameover == false) {
				if (settingGame) {
					return;
				} else {
					if (increaseLevel) {
						StartCoroutine (NewLevelWork ());
					}
					gl.TouchLogic ();
				}
			} else if (gameover == true) {
				GameOver ();
			}
			timeLeft -= Time.deltaTime;
		} else {
			GameOver ();
		}
	}
}