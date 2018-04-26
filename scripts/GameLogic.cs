using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic{
	public AudioSource chop;

	public void TouchLogic(Board b){
		
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 pos = Camera.main.ScreenToWorldPoint (touch.position);
			pos.z = -1;
			
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));
			foreach (Collider2D c2 in currentFrame)
			{
				GameObject go = c2.gameObject;
				if (b.match){
					b.StartMatching(go, pos);
				}
				
				else{
					if (b.SetPatternToMatchInBoard(go, pos)){
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