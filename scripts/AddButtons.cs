using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AddButtons : MonoBehaviour {

	[SerializeField]
	private Transform flowField;

	[SerializeField]
	private GameObject btn;

	void Awake(){
		for (int i = 1; i < 26; i++) {
			GameObject button = Instantiate (btn);
			button.name = "" + i;
			button.transform.SetParent (flowField, false);
		}
	}
}
