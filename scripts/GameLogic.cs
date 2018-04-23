using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic{
	public AudioSource chop;

	void SaveToFile(string path, int currentCube, Vector3 pos){
		string cn = currentCube.ToString ();
		pos = Camera.main.WorldToScreenPoint(pos);
		string x = pos.x.ToString ();
		string y = pos.y.ToString ();
		string z = pos.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string l = Main.currLabel.ToString ();
		string csv = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", Main.level.ToString(), l, cn, x, y, z, ts);
		File.AppendAllText (path, csv);
	}

	public void TouchLogic(Board b){
		int currentCube;
		
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			pos.z = -1;
			
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame)
			{
				GameObject go = c2.gameObject;

				if (b.match){
					b.StartMatching(go);
				}
				else{
					if (b.SetPatternToMatchInBoard(go)){
						UnityEngine.Debug.Log("first endpoint matched");
					}
				}
				if (b.track){
					currentCube = int.Parse (c2.name);
					SaveToFile (Main.allPath, currentCube, pos);	
				}

				if (b.AllMatched()){
					Main.increaseLevel = true;
				}
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