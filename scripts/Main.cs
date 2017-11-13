using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public Sprite[] sprites;
	List<GameObject> go;

	private static int level = 1;
	private static float timeLeft = 10.0f;

	PatternGrid pg = new PatternGrid();
	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();

	/* game state variables */
	private bool settingGame = false;
	public static bool gameover = false;
	public static bool increaseLevel = false;
	public static string path;

	public static int playerPoints = 0;

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
		//go = pg.GetSimplePatterns ();

		/* just z pattern */
		go = pg.GetZPatterns();

		DecorateCube (go);
		gl.SetCurrentPattern (pg.GetCurrentSelPattern());
		settingGame = false;
	}

	void SaveFile(){
		///*  to save the data
		string filePath = Application.persistentDataPath;
		string fileName =  string.Format(@"{0}.txt", Guid.NewGuid()	);
		path = filePath + "/" + fileName;
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

	IEnumerator NewLevelWork(){
		increaseLevel = false;
		ClearVariables ();
		level += 1;
		yield return new WaitForSeconds (0.5f);
		InitSetup ();
		timeLeft = 10.0f;
	}

	void GameOver(){
		timeLeft = 0;
		SceneManager.LoadScene ("Menu");
		gameover = false;
	}

	void Update () {
		while (level <= 2) {
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
		}
		GameOver ();
	}
}