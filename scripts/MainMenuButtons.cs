using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuButtons: MonoBehaviour {
	static public int speed = 2;
	
	public void PlayGame () {	
		SceneManager.LoadScene("GameScene");
	}
	
	public void QuitGame () {
		Application.Quit ();
	}
	
	public void GetInput(string userInput) {
		speed = Int32.Parse(userInput);
		Debug.Log("You entered " + userInput);
	}
	
}
