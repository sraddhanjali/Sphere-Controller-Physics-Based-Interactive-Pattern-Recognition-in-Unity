using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {
	
	public static SphereController instance = null;
	public GameObject sphere;
	public bool updateComplete = true;
	public GameObject currentTouch;
	public Board currentBoard = null;
	
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

		//Set SphereController to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		//Debug.Log("sphere controller starting");
		sphere = GameObject.FindGameObjectWithTag("Sphere");
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void SetBoard (Board board) {
		///Debug.Log("Setting board");
		currentBoard = board;
		sphere.transform.position = currentBoard.allPatterns.First.Value.transform.position;
	}

	public void SetCurrentTouchPosition(GameObject g) {
		if (!GameObject.ReferenceEquals(currentTouch, g)) {
			updateComplete = false;
			currentTouch = g;
			Debug.LogWarning("inside sphere");
		}
	}

	public void Move(List<LinkedListNode<GameObject>> go) {
		StartCoroutine(MoveSphere(go));
		updateComplete = true;
	}

	public IEnumerator MoveSphere(List<LinkedListNode<GameObject>> go) {
		//Debug.Log("Moving Sphere");
		foreach(LinkedListNode<GameObject> g in go) {
			sphere.transform.position = g.Value.transform.position;
			yield return new WaitForSeconds(0.2f);
		}
	} 
		
	// Update is called once per frame
	void Update () {
		List<LinkedListNode<GameObject>> nxtNode = currentBoard.GetNextNode(currentTouch);
		if (updateComplete == false) {
			Debug.LogWarning("moving");
			Move(nxtNode);	
		}
	}
}
