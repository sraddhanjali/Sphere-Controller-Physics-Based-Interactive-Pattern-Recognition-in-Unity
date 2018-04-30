using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public static Sprite[] sprites;
	public static int repetition = 1;
	public int totalRepetition = 0;
	public static int level = 0;
	public static int patternIndex = 0;

	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();
	List<Board> boardList = new List<Board>();
	Loader loader = new Loader();
	
	private bool settingGame = false;
	public static bool reload = false;
	public static bool gameover = false;
	public static bool increaseLevel = false;
	public static string touchDataPath;
	public static string tempDataPath; 
	public static int playerPoints = 0;
	private List<string> labels = new List<string>();
	
	protected void OnGUI(){
		guiStyle.fontSize = 50; 
		if (gameover == false) {
			GUILayout.Label ("\n Level: " + level + "\n Points:" + playerPoints, guiStyle);
		}
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void SaveFile(){
		string filePath = Application.persistentDataPath;
		string f1 =  string.Format(@"{0}.csv", Guid.NewGuid());
		string f2 = string.Format("tmp.csv");
		touchDataPath = filePath + "/" + f1;
		tempDataPath = filePath + "/"  + f2;
	}

	void Awake(){
		LoadSprites();
		SaveFile();
	}

	void SetTotalRepetition(){
		totalRepetition = repetition * labels.Count;
	}

	Board GetBoard(){
		return boardList[patternIndex];
	}

	/*void PrintList(List<string> obj){		pattPath = filePath + "/" + f2;
		for (int i = 0; i < obj.Count; i++){
			Debug.Log(obj[i]);
		}
	}*/
	
	void LoadingLinkedListOfPatterns() {
		GetBoard().LoadLinkedList();
	}
	
	void ClearBoard() {
		Debug.Log("clearing");
		gd.Clear(GetBoard());
		GetBoard().ClearVariableState();
	}
	
	void SetBoardLevel(){
		if (level % repetition == 0 && level != 0) {
			patternIndex += 1;
		}
	}

	void Start(){
		boardList = loader.ReadFileTest();
		labels = loader.GetLabels();
		SetTotalRepetition ();
		SetBoardLevel ();
		InitBoard();
		ListenersInit();
	}
	
	void Reset(){
		gameover = false;
		level = 0;
		increaseLevel = false;
	}
	
	void GameOver(){
		Reset();
		ListenerStop();
		SceneManager.LoadScene ("Menu");
	}

	void ListenerStop() {
		EventManager.StopListening("success", NextBoard);
		EventManager.StopListening("fail", ReloadLevel);
	}

	void ListenersInit() {
		EventManager.StartListening("success", NextBoard);
		EventManager.StartListening("fail", ReloadLevel);
		EventManager.StartListening("gameover", GameOver);
	}
	
	void InitBoard(){
		settingGame = true;
		LoadingLinkedListOfPatterns();
		SphereController.instance.SetBoard(GetBoard());
		settingGame = false;
	}

	void NextBoard(){
		//increaseLevel = false;
		ClearBoard ();
		level += 1;
		SetBoardLevel ();
		InitBoard();
	}

	void ReloadLevel() {
		ClearBoard();
	}

	void Update(){
		if (level <= totalRepetition) {
			gl.TouchLogic (GetBoard());
		}
	}
}