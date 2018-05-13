using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData instance = null;
    
    public static List<string> tempData = new List<String>();

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        EventManager.StartListening("save", SaveToFile);
    }

    public void LoadToTemp(string data) {
        tempData.Add(data);
    }

    public void SaveToFile() {
        for (int i = 0; i < tempData.Count; i++) {
            File.AppendAllText(Main.touchDataPath, tempData[i] + Environment.NewLine);    
        }
        tempData.Clear();
    }
}