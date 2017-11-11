using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GridDecorate{

	public float LINEWIDTH = 0.5f;

	void ColorCubePath(List<GameObject> patternCube){
		Color c = Color.grey;
		GameObject b;
		for (int i = 0; i < patternCube.Count; i++) {
			b = patternCube [i];
			b.layer = 8;
		}
	}

	void DrawLines(List<GameObject> patternCube){
		LineRenderer ln;
		for (int i = 0; i < patternCube.Count; i++) {
			// last gameobject doesn't have any more gameobjects to draw line into so i runes only till second last
			if (i != patternCube.Count - 1) {
				GameObject g1 = patternCube [i];
				GameObject g2 = patternCube [i + 1];
				if (g1.GetComponent<LineRenderer> ()) {
					ln = g1.GetComponent<LineRenderer> ();
				} else {
					ln = g1.AddComponent<LineRenderer> ();
				}
				g1.GetComponent<SpriteRenderer> ().material.color = Color.black;
				g2.GetComponent<SpriteRenderer> ().material.color = Color.black;
				ln.SetPosition (0, g1.transform.position);
				ln.SetPosition (1, g2.transform.position);
				ln.material.color = Color.white;
				ln.startWidth = LINEWIDTH;
				ln.endWidth = LINEWIDTH;
			} 
		}
	}

	IEnumerator RemoveLines(List<int> patternCube){
		yield return new WaitForSeconds(3f);

		LineRenderer ln1;
		for (int i = 0; i < patternCube.Count; i++) {
			if (i != patternCube.Count - 1) {
				GameObject p1 = patternCube [i];
				GameObject p2 = patternCube [i + 1];
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

	/*void InitialCubesColor(List<GameObject> patternCube){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 0; i < objects.Length; i++) {
			patternCube.Add (objects [i]);
			GameObject b = patternCube [i];
			b.layer = 8;
			b.GetComponent<Renderer>().material.color = Color.white;
		}
	}*/

	public void DecorateCube(List<GameObject> patternCube){
		DrawLines (patternCube);
		patternCube.Clear ();
		StartCoroutine (RemoveLines(patternCube));	
	}
}
