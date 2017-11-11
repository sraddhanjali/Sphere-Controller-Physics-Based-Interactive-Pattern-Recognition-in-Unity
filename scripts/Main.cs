using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	public Shader shader1;
	public Sprite[] sprites;
	private string path;
	private bool settingGame = false;
	private bool gameover = false;
	private bool increaseLevel = false;
	List<GameObject> go;
	PatternGrid g = new PatternGrid();
	GridDecorate gd = new GridDecorate();
	private static int level = 1;
	private static float timeLeft = 10.0f;
	GameLogic gl = new GameLogic();

	void LoadSprites(){
		sprites = Resources.LoadAll<Sprite>("sprites/Scavengers_SpriteSheet");
		shader1 = Shader.Find ("Outlined/Silhouetted Diffuse");
	}

	void InitSetup(){
		settingGame = true;
		go = g.GetPatterns();
		settingGame = false;
	}

	void Awake(){
		LoadSprites ();
		string filePath = Application.persistentDataPath;
		string fileName =  string.Format(@"{0}.txt", Guid.NewGuid());
		path = filePath + "/" + fileName;
	}

	void Start () {
		InitSetup ();
	}

	IEnumerator NewLevelWork(){
		level += 1;
		go.Clear ();
		/*currentPaths.Clear ();
		currentPatternList.Clear ();
		lineList.Clear ();*/
		increaseLevel = false;
		yield return new WaitForSeconds (1f);
		InitSetup ();
	}

	void Update () {
		if (gameover == false) {
			if (settingGame) {
				return;
			} else if (increaseLevel) {
				StartCoroutine (NewLevelWork ());
				timeLeft = 10.0f;
				timeLeft -= Time.deltaTime;
			}
			gl.TouchLogic ();
		} else if (gameover == true) {
			timeLeft = 0;
			GUILayout.Label ("GameOver", guiStyle);
		}
	}
}
