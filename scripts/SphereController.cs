using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereController : MonoBehaviour {
	
	public static SphereController instance = null;
	public GameObject sphere;
	public Board currentBoard = null;
	public bool set = false;
	
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
		Debug.Log("sphere controller starting");
		sphere = GameObject.FindGameObjectWithTag("Sphere");
		sphere.GetComponent<MeshRenderer>().material.color = Color.green;
	}

	public IEnumerator AnimateBoard(Board board) {
		Debug.Log("Animating the board");
		List<GameObject> gObjs = board.ToDraw();
		for (int i = 0; i < gObjs.Count; i++) {
			GameObject g = gObjs[i];
			sphere.transform.position = g.transform.position;
			yield return new WaitForSeconds(0.5f);
		}
		set = false;
	}

	public void SetBoard (Board board) {
		Debug.Log("Setting board");
		currentBoard = board;
	}

	public IEnumerator MoveSphere(List<LinkedListNode<GameObject>> go) {
		foreach (LinkedListNode<GameObject> g in go) {
			sphere.transform.position = g.Value.transform.position;
			yield return new WaitForSeconds(0.5f);
		}
	} 
		
	// Update is called once per frame
	void Update () {
		if (currentBoard != null && set == false) {
			StartCoroutine(AnimateBoard(currentBoard));
			set = true;
		}
	}
}
