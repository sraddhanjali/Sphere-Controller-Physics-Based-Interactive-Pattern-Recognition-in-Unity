using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SphereController : MonoBehaviour {
	
	public static SphereController instance = null;
	public GameObject sphere;
	public Board currentBoard = null;
	
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		sphere = GameObject.FindGameObjectWithTag("Sphere");
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void SetBoard (Board board) {
		currentBoard = board;
		ResetSphere();
	}
	
	private void ResetSphere() {
		StartCoroutine(MoveSphere());
	}
	
	private IEnumerator MoveSphere() {
		float speed = 1.0f / (float) MainMenuButtons.speed;
		foreach (GameObject currentBoardAllPattern in currentBoard.allPatterns) {
			sphere.transform.position = currentBoardAllPattern.transform.position;
			yield return new WaitForSeconds(speed);
		}
	} 
}
