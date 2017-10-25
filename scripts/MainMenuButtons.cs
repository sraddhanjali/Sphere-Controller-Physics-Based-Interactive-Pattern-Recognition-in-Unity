using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuButtons : MonoBehaviour {

	/* for functions to show up in the OnClick functions list, the functions ought to be public */
	// Use this for initialization
	public void PlayGame () {
		SceneManager.LoadScene("GameScene");
	}
	
	// Update is called once per frame
	public void QuitGame () {
		Application.Quit ();
	}
}
