using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour{

	public Shader shader1;
	public Sprite[] sprites;
	public static int repetition = 2;
	public int totalRepetition = 0;
	public static int level = 0;
	public static string currLabel = "a";
	public static int patternIndex = 0;
	public static float timeLeft = 15.0f;

	GridDecorate gd = new GridDecorate();
	GameLogic gl = new GameLogic();
	GUIStyle guiStyle = new GUIStyle();
	List<Board> boardList = new List<Board>();
	Loader loader = new Loader();
	
	private bool settingGame = false;
	public static bool gameover = false;
	public static int won = 0;
	public static bool done = false;
	public static bool increaseLevel = false;
	public static bool top = false;
	public static string allPath;
	public static string pattPath;
	public static int playerPoints = 0;
	private List<string> labels = new List<string>(); 
	
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

	void SaveFile(){
		string filePath = Application.persistentDataPath;
		string f1 =  string.Format(@"{0}.csv", Guid.NewGuid());
		string f2 = string.Format("labels.csv");
		allPath = filePath + "/" + f1;
		pattPath = filePath + "/" + f2;
	}

	void Awake(){
		//LoadSprites();
		SaveFile();
	}

	void SetTotalRepetition(){
		totalRepetition = repetition * labels.Count;
	}

	Board GetPattern()
	{
		return boardList[patternIndex];
	}

	void Start(){
		boardList = loader.ReadFileTest();
		labels = loader.GetLabels();
		InitBoard ();
		SetTotalRepetition ();
		SetBoardLevel ();
	}
	
	void InitBoard(){
		settingGame = true;
		gd.Draw(GetPattern());
		settingGame = false;
	}

	void ClearBoard(){
cd		gd.Remove(GetPattern());
	}

	void SetBoardLevel(){
		if (level % repetition == 0 && level != 0) {
			patternIndex += 1;
		}
		currLabel = labels [patternIndex];
	}

	IEnumerator NextBoard(){
		increaseLevel = false;
		ClearBoard ();	
		yield return new WaitForSeconds (1.5f);
		level += 1;
		SetBoardLevel ();
		InitBoard ();
		timeLeft = 15.0f;
	}

	void Reset(){
		timeLeft = 0;
		gameover = false;
		level = 0;
		Main.increaseLevel = false;
	}

	void GameOver(){
		Reset();
		SceneManager.LoadScene ("Menu");
	}

	void Update(){
		if (level <= totalRepetition) {
			if (gameover == false) {
				if (settingGame) {
					return;
				} else {
					if (increaseLevel) {
						StartCoroutine (NextBoard ());
					}
					gl.TouchLogic (GetPattern());
					timeLeft -= Time.deltaTime;
				}
			} else if (gameover == true) {
				GameOver ();
			}
		}
	}
}