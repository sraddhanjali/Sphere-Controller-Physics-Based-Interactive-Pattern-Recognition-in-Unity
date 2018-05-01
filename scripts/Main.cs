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
	public AudioClip moveSound;
	public static int repetition = 1;
	public int totalRepetition = 0;
	public static int level = 0;
	public static int patternIndex = 0;

	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();
	List<Board> boardList = new List<Board>();
	Loader loader = new Loader();
	
	public static string touchDataPath;
	public static string tempDataPath; 
	public static int playerPoints = 0;
	private List<string> labels = new List<string>();
	
	protected void OnGUI(){
		guiStyle.fontSize = 60; 
		GUILayout.Label ("\n Level: " + level + "\n Points:" + playerPoints, guiStyle);
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
		totalRepetition = (repetition * labels.Count)/2;
	}

	Board GetBoard(){
		return boardList[patternIndex];
	}

	/*void PrintList(List<string> obj){		pattPath = filePath + "/" + f2;
		for (int i = 0; i < obj.Count; i++){
			Debug.Log(obj[i]);
		}
	}*/
	
	void Start(){
		boardList = loader.ReadFileTest();
		labels = loader.GetLabels();
		SetTotalRepetition ();
		InitBoard();
		ListenersInit();
	}
	
	void Reset(){
		level = 0;
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
		EventManager.StartListening("matches", Vibrate);
	}
	
	void ClearBoard() {
		Debug.Log("clearing " + level.ToString() + " in main.cs");
		gd.Clear(GetBoard());
		GetBoard().ClearVariableState();
	}
	
	void SetBoardLevel(){
		if (level % repetition == 0 && level != 0) {
			patternIndex += 1;
		}
	}

	void Vibrate() {
		Handheld.Vibrate();
	}

	void PlayWrongMoveSound() {
		AudioSource audio = GetComponent<AudioSource>();
		audio.pitch = 0.95f;
		audio.clip = moveSound;
		audio.Play();
	}

	void InitBoard(){
		SetBoardLevel ();
		GetBoard().LoadLinkedList();
		SphereController.instance.SetBoard(GetBoard());
	}

	void NextBoard(){
		playerPoints += 100;
		ClearBoard ();
		level += 1;
		InitBoard();
	}

	void ReloadLevel() {
		PlayWrongMoveSound();
		ClearBoard();
		SphereController.instance.SetBoard(GetBoard());
	}

	void Update(){
		if (level < totalRepetition) {
			gl.TouchLogic (GetBoard());
		}
		else {
			Debug.Log("gameover triggered in Main");
			EventManager.TriggerEvent("gameover");
		}
	}
}