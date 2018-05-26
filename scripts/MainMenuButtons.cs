using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuButtons: MonoBehaviour {
	static public float speed = 0f;
	
	public void PlayGame () {	
		SceneManager.LoadScene("GameScene");
	}
	
	public void QuitGame () {
		Application.Quit ();
	}
	
	public void GetInput(string userInput) {
		speed = float.Parse(userInput, CultureInfo.InvariantCulture.NumberFormat);
		Debug.Log("You entered " + userInput);
	}
	
}
