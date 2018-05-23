using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class SphereController : MonoBehaviour {
	
	public static SphereController instance = null;
	public GameObject sphere;
	public Board currentBoard = null;
	public float speed; 
	public List<Vector3> interPoints = new List<Vector3>();
	
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		speed = 1.0f / (float) MainMenuButtons.speed;
		sphere = GameObject.FindGameObjectWithTag("Sphere");
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void SetBoard (Board board) {
		currentBoard = board;
		InterpolateDataPoints();
		ResetSphere();
	}

	void InterpolateDataPoints() {
		interPoints.Clear();
		Vector3 temp = new Vector3();
		int i = 0;
		foreach (GameObject currentBoardAllPattern in currentBoard.allPatterns) {
			if (i != 0) {
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.10f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.20f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.30f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.40f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.50f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.60f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.70f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.80f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 0.90f));
				interPoints.Add(Vector3.Lerp(temp, currentBoardAllPattern.transform.position, 1f));
			}
			temp = currentBoardAllPattern.transform.position;
			i += 1;
		}
	}

	private void ResetSphere() {
		StartCoroutine(MoveSphere());
	}
	
	/*private IEnumerator MoveSphere() {
		foreach (GameObject currentBoardAllPattern in currentBoard.allPatterns) {
			sphere.transform.position = currentBoardAllPattern.transform.position;
			yield return new WaitForSeconds(speed);
		}
	}*/

	private IEnumerator MoveSphere() {
		for (int i = 0; i < interPoints.Count; i++) {
			sphere.transform.position = interPoints[i];
			yield return new WaitForSeconds(speed);
		}
	}
}
