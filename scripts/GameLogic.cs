using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameLogic{
	
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
		string final = " ";
		using (StreamReader sr = File.OpenText(Main.tempDataPath)){
			string s = "";
			while ((s = sr.ReadLine()) != null) {
				final += s;
				final += Environment.NewLine;
			}
		}
		File.AppendAllText(Main.touchDataPath, final);
	}

	public void TempSave(GameObject go, Board b, Vector3 pos) {
		string csvstring = ChunkToSave(go, b, pos);
		if(File.Exists(Main.tempDataPath)) {
			File.AppendAllText(Main.tempDataPath, csvstring + Environment.NewLine);	
		}
	}
		
	public void TouchLogic(Board b) {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
				pos.z = -1;
				
				Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
				foreach (Collider2D c2 in currentFrame)
				{
					GameObject go = c2.gameObject;
					UnityEngine.Debug.Log("object found : " + go.name);
					SphereController.instance.SetCurrentTouchPosition(go);
					//TempSave(go, b, pos);
					if (b.match){
						b.StartMatching(go);
					}
					else {
						if (b.SetPatternToMatchInBoard(go)) {
							UnityEngine.Debug.Log("first matching tip of first pattern");
						}
					}
				}
			}else if(touch.phase == TouchPhase.Ended){
				UnityEngine.Debug.Log("handlifted");
				if (b.AllMatched()) {
					//SaveToFile();
					UnityEngine.Debug.Log("success triggered in GL");
					EventManager.TriggerEvent("success");
				}
				else {
					UnityEngine.Debug.LogWarning("fail triggered in GL");
					EventManager.TriggerEvent("fail");
					/*if (File.Exists(Main.tempDataPath)){
						File.Delete(Main.tempDataPath);
					}*/
				}
			}
		}
	}
}