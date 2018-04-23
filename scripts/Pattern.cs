using UnityEngine;
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary

public class Pattern{
    public List<GameObject> sequence { get; private set; }
    public int Length = 0;

    public string name { get; private set; }

    public Pattern(List<GameObject> sequence, string name){
        this.sequence = sequence;
        this.Length = sequence.Count;
        this.name = name;
    }

    public int getLength(){
        return this.sequence.Count;
    }

    public override string ToString(){
        return this.name;
    }
}    