using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GridDecorate{
	
	public void Clear(Board board){
		List<GameObject> gameObject = board.ToDraw();
		for (int i = 0; i < gameObject.Count; i++){
			GameObject g1 = gameObject[i];
			g1.GetComponent<SpriteRenderer>().material.color = Color.white;
		}
	}
}