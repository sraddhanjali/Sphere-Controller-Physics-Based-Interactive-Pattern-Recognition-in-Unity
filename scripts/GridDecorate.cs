using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GridDecorate{
	
	public void Clear(){
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Cube");
		for(int i = 0; i < objs.Length; i++){
			objs[i].GetComponent<SpriteRenderer>().material.color = Color.white;
		}
	}
}