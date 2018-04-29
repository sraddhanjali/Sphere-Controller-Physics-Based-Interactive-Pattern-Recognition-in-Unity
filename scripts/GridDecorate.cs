using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GridDecorate{
	
	public float LINEWIDTH = 0.1f;

	public void Clear(Board board){
		List<GameObject> gameObject = board.ToDraw();
		for (int i = 0; i < gameObject.Count; i++){
			GameObject g1 = gameObject[i];
			g1.GetComponent<SpriteRenderer>().material.color = Color.white;
		}
	}

	public IEnumerator Draw(Board board){
		List<GameObject> gameObject = board.ToDraw();	
		LineRenderer ln;
		for (int i = 0; i < gameObject.Count; i++) {
			if (i != gameObject.Count - 1) {
				GameObject g1 = gameObject[i];
				GameObject g2 = gameObject[i + 1];
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer>();
				} else {
					ln = g1.AddComponent<LineRenderer>();
				}
				if (i == 0){
					g1.GetComponent<SpriteRenderer>().material.color = Color.red;
				} 
				else{
					g1.GetComponent<SpriteRenderer>().material.color = Color.black; 
				}
				g2.GetComponent<SpriteRenderer>().material.color = Color.black;
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.material.color = Color.white;
				ln.startWidth = LINEWIDTH;	
				ln.endWidth = LINEWIDTH;
				yield return new WaitForSeconds(0.1f);
			} 
		}
	}

	public IEnumerator Remove(Board board){
		List<GameObject> gameObject = board.ToDraw();
		LineRenderer ln;
		for (int i = 0; i < gameObject.Count; i++){
			GameObject g1 = gameObject[i];
			if (i != gameObject.Count - 1){
				GameObject g2 = gameObject[i + 1];
				if (g1.GetComponent<LineRenderer>()){
					ln = g1.GetComponent<LineRenderer>();
				}
				else{
					ln = g1.AddComponent<LineRenderer>();
				}

				ln = g1.GetComponent<LineRenderer>();
				ln.SetPosition(0, Vector3.zero);
				ln.SetPosition(1, Vector3.zero);
				g1.GetComponent<SpriteRenderer>().material.color = Color.white;
				g2.GetComponent<SpriteRenderer>().material.color = Color.white;
			}
			else{
				g1.GetComponent<SpriteRenderer>().material.color = Color.red;
			}

			if (i == gameObject.Count / 2){
				g1.GetComponent<SpriteRenderer>().material.color = Color.red;
			}
			else if (i == 0){
				g1.GetComponent<SpriteRenderer>().material.color = Color.red;
			}
		}yield return new WaitForSeconds(0.1f);
	}
}