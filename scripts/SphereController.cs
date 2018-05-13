using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SphereController : MonoBehaviour {
	
	public static SphereController instance = null;
	public GameObject sphere;
	public bool updateComplete = true;
	public GameObject currentTouch;
	public Board currentBoard = null;

	private int speed { get; set; }
	
	void Awake ()
	{
		//Check if there is already an instance of SphereController
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);

		//Set SphereController to Dlevel = MainMenuButtons.speed;ontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		//Debug.Log("sphere controller starting");
		sphere = GameObject.FindGameObjectWithTag("Sphere");
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
		EventManager.StartListening("fail", ResetSphere);
	}

	private void ResetSphere() {
		//if (!updateComplete) {
		Move(currentBoard.allPatterns.First.Value);
		//}
	}
	
	/**
	 * This function should only be called once every time a level is loaded.
	 */
	public void SetBoard (Board board) {
		updateComplete = false; 
		currentBoard = board;
		// This is the first time this level was loaded so reset the sphere and start animation
		ResetSphere();
	}
	
	public void Move(GameObject go) {
		StartCoroutine(MoveSphere(go));
	}

	private IEnumerator MoveSphere(GameObject go) {
		EventManager.TriggerEvent("startmove");
		float speed = 1.0f / (float) MainMenuButtons.speed;
		foreach (GameObject currentBoardAllPattern in currentBoard.allPatterns) {
			sphere.transform.position = currentBoardAllPattern.transform.position;
			yield return new WaitForSeconds(speed);
		}
		updateComplete = true;
		EventManager.TriggerEvent("endmove");
	} 
}
