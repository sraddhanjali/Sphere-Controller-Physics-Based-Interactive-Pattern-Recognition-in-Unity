using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic{
	public AudioSource chop;
	
	public void Save(GameObject go, Board b, Vector3 pos){
		int currentCube = int.Parse (go.name);
		UnityEngine.Debug.Log("writing to file");
		UnityEngine.Debug.Log(currentCube);
		SaveToFile (Main.allPath, currentCube, b.GetCurrentLabel(), pos);
	}
	
	void SaveToFile(string path, int currentCube, string label, Vector3 pos){
		string cn = currentCube.ToString ();
		pos = Camera.main.WorldToScreenPoint(pos);
		string x = pos.x.ToString ();
		string y = pos.y.ToString ();
		string z = pos.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string csv = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", Main.level.ToString(), label, cn, x, y, z, ts);
		File.AppendAllText (path, csv);
	}
	
	public void TouchLogic(Board b){
		
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			pos.z = -1;
			
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame)
			{
				GameObject go = c2.gameObject;
				Save(go, b, pos);
				if (b.match){
					b.StartMatching(go);
				}
				
				else{
					if (b.SetPatternToMatchInBoard(go)){
						UnityEngine.Debug.Log("first endpoint matched");
					}
				}

				if (b.AllMatched()){
					Main.increaseLevel = true;
					Main.playerPoints += 100;
				}
			}
		}
	}	
}