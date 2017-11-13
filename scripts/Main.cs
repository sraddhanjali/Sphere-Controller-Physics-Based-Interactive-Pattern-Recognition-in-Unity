using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level + "\n Time:" + (int)timeLeft + "\n Points:" + gl.playerPoints, guiStyle);
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
		StartCoroutine (gd.RemoveLines(patternCubeObject));	
	}

	void InitSetup(){
		settingGame = true;
		/* original
		go = pg.GetPatterns();
		*/
		// a sample of simple pattern from paper
		go = pg.GetSimplePatterns ();

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
		go.Clear ();
	}

	IEnumerator NewLevelWork(){
		level += 1;
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
				ClearVariables ();
				StartCoroutine (NewLevelWork ());
				timeLeft = 10.0f;
				timeLeft -= Time.deltaTime;
			} 
			gl.TouchLogic ();
		} else if (gameover == true) {
			timeLeft = 0;
		}
	}
}