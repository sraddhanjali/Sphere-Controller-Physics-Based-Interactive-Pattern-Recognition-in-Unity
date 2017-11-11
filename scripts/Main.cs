using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour{

	public static TextAsset wordFile; // pattern file
	public Shader shader1;
	public Sprite[] sprites;
	private string path;
	List<GameObject> go;

	private static int level = 1;
	private static float timeLeft = 10.0f;

	PatternGrid g = new PatternGrid();
	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();

	/* game state variables */
	private bool settingGame = false;
	public static bool gameover = false;
	public static bool increaseLevel = false;

	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level + "\n Time:" + (int)timeLeft + "\n Points:" + gl.playerPoints, guiStyle);
		} else {
			GUILayout.Label ("GameOver", guiStyle);
		}
	}

	void LoadFile(){
		wordFile = Resources.Load("Resources/easy.txt") as TextAsset; 
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void InitSetup(){
		settingGame = true;
		go = g.GetPatterns();
		gl.SetCurrentPattern (g.GetCurrentSelPattern());
		gd.DecorateCube (go);
		settingGame = false;
	}

	void Awake(){
		LoadSprites ();
		LoadFile ();
		/*  to save the data
		string filePath = Application.persistentDataPath;
		string fileName =  string.Format(@"{0}.txt", Guid.NewGuid());
		path = filePath + "/" + fileName;
		*/
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