using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Main : MonoBehaviour {
	
	public static int repetition = 10;
	public int totalRepetition = 0;
	public static int rep = 0;
	public static int level = 0;
	public static int patternIndex = 0;

	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();
	GUIStyle boxStyle = new GUIStyle();
	GUIStyle boxStyle1 = new GUIStyle();
	List<Board> boardList = new List<Board>();
	Loader loader = new Loader();
	
	// file paths
	public static string touchDataPath;
	public static string wrongDataPath;
	public static string sensorDataPath;
	
	public static int playerPoints = 0;
	public static string statusText;
	public static string waitText;
	public static bool right = false;
	public Shader shader1;
	public static Sprite[] sprites;
	public AudioClip wrongSound;
	public AudioClip rightSound;
	
	private List<string> labels = new List<string>();
	public static bool enableTouch = false;
	public bool clearing = false;
	public AudioClip moveSound;
	AudioSource audio;
	public GameObject trailPrefab;
	
	// sensor data
	public static List<string> sensorData = new List<string>();
	
	protected void OnGUI(){
		guiStyle.fontSize = 40;
		guiStyle.normal.textColor = Color.white;
		boxStyle.fontSize = 65;
		boxStyle.normal.textColor = Color.green;
		boxStyle1.fontSize = 65;
		boxStyle1.normal.textColor = Color.red;
		GUILayout.Label ("\n Level: " + level + "\n Points: " + playerPoints + "\n Repetition: " + rep, guiStyle);
		if (right) {
			GUI.Box(new Rect(400, 700, 500, 100), statusText, boxStyle);	
		}
		else{
			GUI.Box(new Rect(400, 700, 500, 100), statusText, boxStyle1);
		}
		if(enableTouch){
			GUI.Box(new Rect(400, 400, 500, 100), waitText, boxStyle);
		}
		else {
			GUI.Box(new Rect(400, 400, 500, 100), waitText, boxStyle);
		}
	}

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void SaveFile(){
		string filePath = Application.persistentDataPath;
		DateTime d = DateTime.Now;
		string d1 = d.ToString("yyyyMMddHHmmss");
		string f1 =  string.Format(@"RIGHT{0}-{1}.csv", d1, Guid.NewGuid());
		string f2 =  string.Format(@"WRONG{0}-{1}.csv", d1, Guid.NewGuid());
		string f3 = string.Format(@"Sensor{0}-{1}.csv", d1, Guid.NewGuid());
		touchDataPath = filePath + "/" + f1;
		wrongDataPath = filePath + "/" + f2;
		sensorDataPath = filePate + "/" + f3;
		File.Create(touchDataPath);
		File.Create(wrongDataPath);
		File.Create(sensorDataPath);
	}

	void Awake(){
		LoadSprites();
		SaveFile();
		audio = GetComponent<AudioSource>();
	}

	void SetTotalRepetition(){
		totalRepetition = repetition * labels.Count; // Labels are 14*2 but occur in pair so divide by 2 
	}

	void GameOver() {
		level = 0;
		ListenerStop();
		SceneManager.LoadScene("Menu");
	}

	void ListenerStop() {
		EventManager.StopListening("success", NextBoard);
		EventManager.StopListening("fail", ReloadLevel);
		EventManager.StopListening("gameover", GameOver);
		EventManager.StopListening("record", RecordSensor);
		EventManager.StopListening("saverecord", SaveSensorToFile);
	}
	
	Board GetBoard(){
		return boardList[patternIndex];
	}

	IEnumerator ShowMessEnumerator(string message) {
		statusText = message;
		yield return new WaitForSeconds(0.5f);
		statusText = " ";
	}

	void ListenersInit() {
		EventManager.StartListening("success", NextBoard);
		EventManager.StartListening("fail", ReloadLevel);
		EventManager.StartListening("gameover", GameOver);
		EventManager.StartListening("record", RecordSensor);
		EventManager.StartListening("saverecord", SaveSensorToFile);
	}

	void Start(){
		//boardList = loader.ReadFileTest();
		boardList = loader.ReadFileTextBottom();
		labels = loader.GetLabels();
		SetTotalRepetition ();
		ListenersInit();
		InitBoard();
	}
	
	void ClearBoard() {
		gd.Clear();
		enableTouch = false;
		GetBoard().ClearVariableState();
	}
	
	public string SensorDataCollect() {
		Vector3 acc = Vector3.zero;
		acc.x = Input.acceleration.x;
		acc.y = Input.acceleration.y;
		acc.z = Input.acceleration.z;
		string x = acc.x.ToString();
		string y = acc.y.ToString();
		string z = acc.z.ToString();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string csvstring = string.Format("{0},{1},{2},{3},{4}", Main.level.ToString(), x, y, z, ts);
		return csvstring;
	}
	
	public void RecordSensor() {
		sensorData.Add(SensorDataCollect());
	}
    
	public void SaveSensorToFile() {
		for (int i = 0; i < sensorData.Count; i++) {
			File.AppendAllText(sensorDataPath, sensorData[i] + Environment.NewLine);    
		}
		sensorData.Clear();
	}

	void levelBylevel() {
		if (level < labels.Count / 2 && level != 0) {
			patternIndex += 1;
		} else {
			if (level == 0) {
				rep = 1;
			}else {
				rep += 1;
			}
			level = 0;
			patternIndex = 0;
		}
	}

	void levelRepetition() {
		if (rep == repetition) {
			patternIndex += 1;
			rep = 1;
			level += 1;
		} else {
			rep += 1;
		}
	}

	void SetBoardLevel() {
		//levelBylevel();
		levelRepetition();
	}

	void InitBoard(){
		SetBoardLevel ();
		GetBoard().LoadLinkedList();
		SphereController.instance.SetBoard(GetBoard());
	}

	void NextBoard() {
		StartCoroutine(ShowMessEnumerator("Correct Pattern!"));
		right = true;
		playerPoints += 5;
		ClearBoard ();
		//level += 1;
		InitBoard();	
	}

	public void ReloadLevel() {
		StartCoroutine(ShowMessEnumerator("Wrong Pattern"));
		right = false;
		ClearBoard();
		GetBoard().LoadLinkedList();
		SphereController.instance.SetBoard(GetBoard());
	}

	void Update(){
		//if (rep <= repetition) {
		if(rep <= totalRepetition){
			if (enableTouch == true) {
				waitText = "Start";
				gl.TouchLogic (GetBoard());
			}
			else {
				waitText = "Wait";
			}
		} else {
			Debug.Log("gameover triggered in Main");
			EventManager.TriggerEvent("gameover");
		}
	}
}