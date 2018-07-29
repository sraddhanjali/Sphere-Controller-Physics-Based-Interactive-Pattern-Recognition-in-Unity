using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameLogic {

	public static GameObject previousGO = null;

	public GameObject trail;
		
	public void TouchLogic(Board b) {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				//pos.z = -1;
				pos.z = 0;
				trail = SpecialEffectsScript.MakeTrail(pos);
				Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
				foreach (Collider2D c2 in currentFrame)
				{
					GameObject go = c2.gameObject;
					//b.SaveChunk(go, pos);
					if (previousGO != go) {
						b.MatchPatterns(go, pos);
						previousGO = go;
						EventManager.TriggerEvent("matches");
						go.GetComponent<SpriteRenderer> ().material.color = Color.red;
						UnityEngine.Debug.Log("object found : " + go.name);
					}
				}
			}else if(touch.phase == TouchPhase.Ended){
				UnityEngine.Debug.Log("handlifted");
				if (b.PatternsMatch()) {
					UnityEngine.Debug.Log("success triggered in GL");
					GameData.instance.SaveToFile(Main.touchDataPath);
					GameData.instance.SaveSensorToFile(Main.sensorDataPath);
					EventManager.TriggerEvent("success");
				}
				else {
					UnityEngine.Debug.LogWarning("fail triggered in GL");
					GameData.instance.SaveToFile(Main.wrongDataPath);
					EventManager.TriggerEvent("fail");
				}
				SpecialEffectsScript.Destroy(trail, trail.GetComponent<TrailRenderer>().time);
			}
		}
	}
}