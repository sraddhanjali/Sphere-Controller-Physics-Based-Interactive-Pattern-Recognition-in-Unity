using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GridDecorate{

	public float LINEWIDTH = 0.1f;

	void ColorCubePath(List<GameObject> patternCubeObject){
		Color c = Color.grey;
		GameObject b;
		for (int i = 0; i < patternCubeObject.Count; i++) {
			b = patternCubeObject [i];
			b.layer = 8;
		}
	}

	public void DrawLines(List<GameObject> patternCubeObject){
		LineRenderer ln;
		for (int i = 0; i < patternCubeObject.Count; i++) {
			// last gameobject doesn't have any more gameobjects to draw line into so i runs only till second last
			if (i != patternCubeObject.Count - 1) {
				GameObject g1 = patternCubeObject [i];
				GameObject g2 = patternCubeObject [i + 1];
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer> ();
				} else {
					ln = g1.AddComponent<LineRenderer> ();
				}

				if (i == 0)
				{
					g1.GetComponent<SpriteRenderer> ().material.color = Color.red;
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

	/*public IEnumerator RemoveLines(List<GameObject> patternCubeObject){
	    yield return new WaitForSeconds(3f);*/
	public void RemoveLines(List<GameObject> patternCubeObject){

		LineRenderer ln1;
		for (int i = 0; i < patternCubeObject.Count; i++) {
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
		for (int m = 0; m < paths.Count; m++) {
			go[paths[m]].gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
	}
}