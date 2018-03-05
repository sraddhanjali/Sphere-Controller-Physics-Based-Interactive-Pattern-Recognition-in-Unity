using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic{
	public AudioSource chop;
	public bool correctSwipe = false;
	public int oldCube = 0;
	Helper h = new Helper();
	private static float timeLeft = 10.0f;
	public GridDecorate gd = new GridDecorate();
	List<int> currentPaths = new List<int>();
	List<int> currentSelPattern = new List<int>();
	Stopwatch sw = new Stopwatch ();


	public void SetCurrentPattern(List<int> curr){
		currentSelPattern = curr;
	}

	public List<int> GetCurrentPattern(){
		return currentSelPattern;
	}

	void SaveToFile(string path, int currentCube, Vector3 pos){
		string cn = currentCube.ToString ();
		pos = Camera.main.WorldToScreenPoint(pos);
		string x = pos.x.ToString ();
		string y = pos.y.ToString ();
		string z = pos.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string l = Main.currLabel.ToString ();
		string csv = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", Main.level.ToString(), l, cn, x, y, z, ts);
		//Debug.Log (csv);
		File.AppendAllText (path, csv);
	}

	public void TouchLogic(){
		int currentCube;

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			/*
			Vector3 pos = Camera.main.ViewportToWorldPoint(touch.position);
			*/
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			pos.z = -1;
			/* PRINTING */
			if (correctSwipe == false) {
				SaveToFile (Main.allPath, currentSelPattern [0], pos);			
			}
			if (oldCube != 0) {
				SaveToFile (Main.allPath, oldCube, pos);
				UnityEngine.Debug.Log (oldCube.ToString ());
			}
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame) {
				currentCube = int.Parse (c2.name);
				/* save each touch to file*/
				SaveToFile (Main.allPath, currentCube, pos);
				SwipeCube(currentCube, pos);
			}
		}
	}	

	void PrintTime(){
		// Get the elapsed time as a TimeSpan value.
		TimeSpan ts = sw.Elapsed;

		// Format and display the TimeSpan value.
		string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
		UnityEngine.Debug.Log("RunTime " + elapsedTime);
	}

	void SwipeCube(int currentCube, Vector3 pos){
		int t = (int)timeLeft;
		int currentPathSize = currentPaths.Count;
		//Debug.Log (currentCube);
		GameObject a = GameObject.Find (currentCube.ToString ());

		if (currentCube == currentSelPattern [currentPathSize]) {
			correctSwipe = true;
			oldCube = currentCube;
			UnityEngine.Debug.Log (currentCube.ToString ());
			//Console.WriteLine (currentCube.ToString ());
			/* save exact pattern cube data to file*/
			//SaveToFile (Main.pattPath, currentCube, pos);

			//Debug.Log ("here");
			currentPaths.Add (currentCube);
			sw.Start ();
			a.GetComponent<Renderer> ().material.color = Color.red;	
			//chop.Play ();
			//Debug.Log ("Current cube added:" + currentCube);
			if (h.CheckEqual (currentSelPattern, currentPaths)) {
				//gd.ChangePathsColors (currentPatternList);
				PlayerPointsLogic (t);
				currentPaths.Clear ();
				currentSelPattern.Clear ();
				correctSwipe = false;
				Main.increaseLevel = true;
				Main.won += 1;
				//Debug.Log ("DONEEEEE!!!");
				UnityEngine.Debug.Log ("DONE!");
				sw.Stop ();
				PrintTime ();
				sw.Reset ();
			} 
		}
	}

	void PlayerPointsLogic(int left){
		if (left >= 7){
			Main.playerPoints += 50;
		}
		else if (left >= 4){
			Main.playerPoints  += 25;
		}
		else if (left > 0){
			Main.playerPoints  += 10;
		}
		else if (left <= 0){
			Main.gameover = true;
		}
	}
}