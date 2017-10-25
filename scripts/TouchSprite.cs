using UnityEngine;
using System.Collections;

public class TouchSprite : MonoBehaviour {

	public static bool guiTouch = false;

	public void TouchInput(Collider2D collider){
		if (Input.touchCount > 0) {
			if (collider == Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position))) {
				switch (Input.GetTouch (0).phase) {
				case TouchPhase.Began:
					SendMessage ("1st Began", SendMessageOptions.DontRequireReceiver);
					guiTouch = true;
					break;
				case TouchPhase.Moved:
					SendMessage ("1st Moved", SendMessageOptions.DontRequireReceiver);
					guiTouch = true;
					break;
				case TouchPhase.Stationary:
					SendMessage ("1st Stationary", SendMessageOptions.DontRequireReceiver);
					guiTouch = true;
					break;
				case TouchPhase.Ended:
					SendMessage ("1st Ended", SendMessageOptions.DontRequireReceiver);
					guiTouch = false;
					break;
				}
			}
			if (Input.touchCount > 1) {
				if (collider == Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.GetTouch (1).position))) {
					switch (Input.GetTouch (1).phase) {
					case TouchPhase.Began:
						SendMessage ("Began", SendMessageOptions.DontRequireReceiver);
						guiTouch = true;
						break;
					case TouchPhase.Moved:
						SendMessage ("Moved", SendMessageOptions.DontRequireReceiver);
						guiTouch = true;
						break;
					case TouchPhase.Stationary:
						SendMessage ("Stationary", SendMessageOptions.DontRequireReceiver);
						guiTouch = true;
						break;
					case TouchPhase.Ended:
						SendMessage ("Ended", SendMessageOptions.DontRequireReceiver);
						guiTouch = false;
						break;
					}
				}
			}
		}
	}
}
