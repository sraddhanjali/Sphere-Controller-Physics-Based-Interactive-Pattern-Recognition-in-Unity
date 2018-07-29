using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData instance = null;
    
    public static List<string> tempData = new List<string>();

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadToTemp(string data) {
        tempData.Add(data);
    }

    public void SaveToFile(string location) {
        for (int i = 0; i < tempData.Count; i++) {
            File.AppendAllText(location, tempData[i] + Environment.NewLine);    
        }
        tempData.Clear();
    }
    
}