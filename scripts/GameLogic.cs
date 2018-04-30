using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GameLogic{
	public AudioSource chop;

	public string ChunkToSave(GameObject go, Board b, Vector3 pos) {
		string cn = int.Parse(go.name).ToString ();
		string label = b.GetCurrentLabel();
		Vector3 n = Camera.main.WorldToScreenPoint(pos);
		string x = n.x.ToString ();
		string y = n.y.ToString ();
		string z = n.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string csvstring = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", Main.level.ToString(), label, cn, x, y, z, ts);
		return csvstring;
	}
	
	void SaveToFile() {
		//Open the stream and read it back.
		using (FileStream fs = File.OpenRead(Main.tempDataPath)){
			byte[] b = new byte[1024];
			UTF8Encoding temp = new UTF8Encoding(true);
			while (fs.Read(b,0,b.Length) > 0){
				 (Main.touchDataPath, temp.GetString(b));
			}
		}
	}

	public void TempSave(GameObject go, Board b, Vector3 pos) {
		if (Main.reload && Main.increaseLevel) {
			if (File.Exists(Main.tempDataPath)){
				File.Delete(Main.tempDataPath);
			}
		}
		using (FileStream fs = File.Create(Main.tempDataPath)) {
			string csvstring = ChunkToSave(go, b, pos);
			fs.AppendAllText(fs, csvstring);
		}
	}
		
	public void TouchLogic(Board b) {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				UnityEngine.Debug.Log("Continuous touch");
				Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
				pos.z = -1;
				
				Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
				foreach (Collider2D c2 in currentFrame)
				{
					GameObject go = c2.gameObject;
					TempSave(go, b, pos);
					if (b.match){
						b.StartMatching(go);
					}
				
					else{
						if (b.SetPatternToMatchInBoard(go)){
							UnityEngine.Debug.Log("first endpoint matched");
						}
					}

					if (b.AllMatched()){
						SaveToFile();
						Main.increaseLevel = true;
						Main.reload = false;
						Main.playerPoints += 100;
					}

					SphereController.instance.SetCurrentTouchPosition(go);
				}
			}
			else {
				UnityEngine.Debug.Log("hand lifted");
				UnityEngine.Debug.Log("loading the same level");
				if (Main.increaseLevel == false) {
					Main.reload = true;	
				}
			}
		}
	}
}