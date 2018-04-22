using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GridDecorate{

	public float LINEWIDTH = 0.1f;
	
	public void InitialCubesColor(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Cube");
		for (int i = 0; i < objects.Length; i++){
			GameObject b = objects[i];
			b.layer = 8;
			b.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	public void Draw(Board board){
		List<Pattern> pattern = board.GetPatterns();
		for (int i = 0; i < pattern.Count; i++){
			this.DrawLines(pattern[i].sequence);
		}
	}

	public void DrawLines(List<GameObject> gameObject){
		LineRenderer ln;
		for (int i = 0; i < gameObject.Count; i++) {
			if (i != gameObject.Count - 1) {
				GameObject g1 = gameObject[i];
				GameObject g2 = gameObject[i+1];
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer>();
				} else {
					ln = g1.AddComponent<LineRenderer>();
				}
				if (i == 0){
					g1.GetComponent<SpriteRenderer> ().material.color = Color.yellow;
				}
				else{
					g1.GetComponent<SpriteRenderer> ().material.color = Color.black;
				}
				g2.GetComponent<SpriteRenderer> ().material.color = Color.black;
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.material.color = Color.white;
				ln.startWidth = LINEWIDTH;
				ln.endWidth = LINEWIDTH;
			} 
		}
	}

	public void Remove(Board board){
		List<Pattern> pattern = board.GetPatterns();
		for (int i = 0; i < pattern.Count; i++){
			this.RemoveLines(pattern[i].sequence);
		}
	}
	
	public void RemoveLines(List<GameObject> patternCubeObject){
		LineRenderer ln1;
		for (int i = 0; i < patternCubeObject.Count; i++){
			if (i != patternCubeObject.Count - 1) {
				GameObject p1 = patternCubeObject [i];
				GameObject p2 = patternCubeObject [i + 1];
				ln1 = p1.GetComponent<LineRenderer> ();
				ln1.SetPosition (0, Vector3.zero);
				ln1.SetPosition (1, Vector3.zero);
			}
		}
	}

	void ChangePathsColors(List<int> paths, List<GameObject> go){
		for (int m = 0; m < paths.Count; m++){
			go[paths[m]].gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}
}