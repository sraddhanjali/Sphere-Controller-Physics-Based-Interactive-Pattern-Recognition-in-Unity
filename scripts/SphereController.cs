using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class SphereController : MonoBehaviour {
	
	public GameObject sphere;
	public static SphereController instance = null;
	public Board currentBoard = null;
	public List<Vector3> interPoints = new List<Vector3>();
	public float speed;
	
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
		sphere = Instantiate (sphere, sphere.transform.position, sphere.transform.rotation);
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void SetBoard (Board board) {
		currentBoard = board;
		InterpolateDataPoints();
		ResetSphere();
	}

	void InterpolateDataPoints() {
		Vector3 temp = new Vector3();
		interPoints.Clear();
		int i = 0;
		foreach (GameObject go in currentBoard.allPatterns) {
			Vector3 current = go.transform.position;
			if (i != 0) {
				interPoints.Add(Vector3.Lerp(temp, current, 0f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.20f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.40f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.60f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.80f));
			}
			temp = current;
			i += 1;
		}
		interPoints.Add(temp);
	}

	void ResetSphere() {
		StartCoroutine(MoveSphere());	
	}
	
	private IEnumerator MoveSphere() {
		int i = 0;
		foreach (Vector3 points in interPoints) {
			if (i >= 20) {
				Main.enableTouch = true;
			}
			else {
				Main.enableTouch = false;
			}
			sphere.transform.position = points;
			i++;
			yield return new WaitForSeconds(speed);
		}
	}
}
