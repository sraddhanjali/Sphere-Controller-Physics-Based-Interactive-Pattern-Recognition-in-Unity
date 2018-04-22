using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Obstacle
{
    public string color { get; private set; }

    public GameObject gameObject { get; private set; }
    
    public Obstacle(GameObject gameObject){
        this.gameObject = gameObject;
        this.color = "black";
    }
    
}