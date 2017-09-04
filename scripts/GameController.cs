using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

	public List<Button> btns = new List<Button>();

	Collider2D[] cubeColliders;

	public Vector3 lastMousePos;

	public static int clickNum = 0;

	public void Start(){
		GetButtons();
		AddListeners();
		cubeColliders = new Collider2D[0];
	}

	public static int flows = 0;

	public static int moves = 0;

	protected void OnGUI(){
		GUI.Label (new Rect (25, 25, 100, 500), "Flows:" + flows.ToString() + "\n Moves:" + moves.ToString());

	}
		
	void GetButtons(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Flow Button");

		for (int i = 1; i < objects.Length + 1; i++) {
			btns.Add(objects[i-1].GetComponent<Button>());
			Button b = btns [i-1].gameObject.GetComponent<Button> ();
			if (i == 1 || i == 22) {
				b.GetComponent<Image> ().color = Color.red;
			} else if (i == 3 || i == 17) {
				b.GetComponent<Image> ().color = Color.yellow;
			} else if (i == 8 || i == 23) {
				b.GetComponent<Image> ().color = Color.green;
			} else if (i == 5 || i == 19) {
				b.GetComponent<Image> ().color = Color.blue;
			} else if (i == 10 || i == 24) {
				b.GetComponent<Image> ().color = Color.magenta;
			} 
		}
	}

	void AddListeners(){
		foreach (Button btn in btns) {
			btn.onClick.AddListener (() => ClickIndicator ());
		}
	}

	public void ClickIndicator(){
		string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
		Debug.Log ("You clicked a button" + name);
	}

	protected void HighlightCubeOnClick(){
		if(Input.GetMouseButton(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask ("Cube"));

			if ((Input.mousePosition - lastMousePos).sqrMagnitude > 10) {
				foreach (Collider2D c2 in currentFrame) {
					for (int i = 0; i < cubeColliders.Length; i++) {
						if (c2 == cubeColliders [i]) {
							Debug.Log (c2.name);
							// change the color after the swipe
							c2.gameObject.GetComponent<Renderer> ().material.color = new Color (0, 255, 0);
							clickNum += 1;
							// disable the collider once a collision has already been detected
							c2.enabled = false;
						}
					}
				}
			}

			cubeColliders = currentFrame;
			lastMousePos = Input.mousePosition;
		}
	}

	protected void Update () {
		HighlightCubeOnClick ();
	}

}
