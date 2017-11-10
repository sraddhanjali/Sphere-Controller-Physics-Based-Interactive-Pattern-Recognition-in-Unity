using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameLogic{
	bool settingGame = false;
	bool gameover = false;
	public AudioSource chop;
	Helper h = new Helper();
	private static float timeLeft = 10.0f;
	private string path;
	private int playerPoints = 0;
	static int pointsAdded = 0 ;
	public GridDecorate gd = new GridDecorate();
	
	public void TouchLogic(){
		int currentCube;
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			/*         Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane)); */
			//Vector3 pos = Camera.main.ViewportToWorldPoint(touch.position);
			Debug.Log (pos.ToString ());
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame) {
				currentCube = int.Parse (c2.name);
				SwipeCube (currentCube, pos);
			}
		}
	}

	void SwipeCube(List<int> currentPaths, List<int> currentPatternList, int currentCube, Vector3 pos){
		int t = (int)timeLeft;
		int currentPathSize = currentPaths.Count;
		//Debug.Log (currentCube);
		GameObject a = GameObject.Find (currentCube.ToString ());
		if (currentPaths.Contains (currentCube)) {
			//Debug.Log ("already exists");
		} else {
			if (currentCube == currentPatternList [currentPathSize]) {

				string nums = currentCube.ToString ();
				pos = Camera.main.WorldToScreenPoint(pos);
				string v1 = pos.ToString ();
				string ts = DateTime.Now.ToString ("yyyyMMddHHmmssffff");
				string together = nums + " " + v1 + " " + ts + "\n";
				Debug.Log (together);
				File.AppendAllText (path, together);

				//Debug.Log ("here");
				currentPaths.Add (currentCube);
				a.GetComponent<Renderer> ().material.color = Color.red;	
				chop.Play ();
				//Debug.Log ("Current cube added:" + currentCube);
				if (h.CheckEqual (currentPatternList, currentPaths)) {
					gd.ChangePathsColors (currentPatternList);
					increaseLevel = true;
					PlayerPointsLogic (t);
					ClearVariables ();
					// save the pattern data object to a list of such objects
					//patC.Add (p);
					// save a pattern data object to a file
					//SaveTimeCoord (patC);
					//p = new Pattern ();
					//Debug.Log ("DONEEEEE!!!");
				} else {
					if (currentPaths.Contains (currentCube)) {
						//Debug.Log ("already swiped");
					} else {
						// Retry currentPaths.Clear ();
					}
				}
			} 
		}
	}

	void ClearVariables(){
		gd.RemoveLines (currentPatternList);
		currentPaths.Clear ();
		currentPatternList.Clear ();
		lineList.Clear ();
		go.Clear ();
	}

	void PlayerPointsLogic(int left){
		if (left > 7){
			playerPoints += 50;
			pointsAdded = 50;
		}
		else if (left > 4){
			playerPoints += 25;
			pointsAdded = 25;
		}
		else if (left > 0){
			playerPoints += 10;
			pointsAdded = 10;
		}
		else if (left <= 0){
			gameover = true;
		}
	}
	
}