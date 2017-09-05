using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

	public List<GameObject> go = new List<GameObject>();

	Collider2D[] cubeColliders;

	public Vector3 lastMousePos;

	private GUIStyle guiStyle = new GUIStyle(); //create a new variable

	public static int clickNum = 0;

	public void Start(){
		GetCubes();
		cubeColliders = new Collider2D[0];
	}

	public static int flows = 0;

	public static int moves = 0;

	protected void OnGUI(){

		guiStyle.fontSize = 50; //change the font size

		GUILayout.Label ("Flows:" + flows.ToString() + "\n Moves:" + moves.ToString() + "\n Clicks:" + clickNum.ToString(), guiStyle);

	}
		
	void GetCubes(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Cube");
		for (int i = 1; i < objects.Length + 1; i++) {
			go.Add(objects[i-1]);
			GameObject b = go [i-1];
			b.layer = 8;
			if (i == 1 || i == 22) {
				b.GetComponent<Renderer> ().material.color = Color.red;
			} else if (i == 3 || i == 17) {
				b.GetComponent<Renderer>().material.color = Color.yellow;
			} else if (i == 8 || i == 23) {
				b.GetComponent<Renderer>().material.color = Color.green;
			} else if (i == 5 || i == 19) {
				b.GetComponent<Renderer>().material.color = Color.blue;
			} else if (i == 10 || i == 24) {
				b.GetComponent<Renderer>().material.color = Color.magenta;
			} 
		}
	}

	protected void HighlightCubeOnClick(){
		if(Input.GetMouseButton(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = -1;
			Collider2D[] currentFrame = Physics2D.OverlapPointAll (new Vector2 (pos.x, pos.y), LayerMask.GetMask("Cube"));

			if ((Input.mousePosition - lastMousePos).sqrMagnitude > 9) {
				foreach (Collider2D c2 in currentFrame) {
					for (int i = 0; i < cubeColliders.Length; i++) {
						if (c2 == cubeColliders [i]) {
							Debug.Log (c2.name);
							// change the color after the swipe
							c2.GetComponent<Renderer> ().material.color = Color.gray;
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
