using UnityEngine; // vector3
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary

public class Pattern
{
    private List<int> main;
    private List<List<int>> bogus;
    private List<int> obstacle;
    private int pattLen;
    private string pattName;

    private bool mainP;
    private bool bogusP;
    private bool obstacleP;
    public enum PattType {ma, bo, ob};

    public bool toTrack = false;

    private PattType p;

    public Pattern(){
        this.mainP = false;
        this.bogusP = false;
        this.obstacleP = false;
        this.main = new List<int>();
        this.bogus = new List<List<int>>();
        this.obstacle = new List<int>();
    }
    
    public void AddPattern(List<int> patt, bool track){
        this.mainP = true;
        this.toTrack = track;
        this.p = PattType.ma;
        this.main = patt;
        this.setLength(this.main);
        this.pattName = "main pattern";
    }

    public void AddPattern(List<List<int>> bog){
        this.bogusP = true;
        this.p = PattType.bo;
        this.bogus = bog;
        this.setLength(this.bogus);
        this.pattName = "bogus";
    }

    public void AddPattern(List<int> obs){
        this.obstacleP = true;
        this.p = PattType.ob;
        this.obstacle = obs;
        this.setLength(this.obstacle);
        this.pattName = "obstacle";
    }

    public void setLength<T> (List<T> o){
        this.pattLen = o.Count;
    }

    public int getLength(){
        return this.pattLen;
    }
    
    public string getPatternString(){
        return string.Format("**** PatternName: {0}", this.pattName);
    }

    public PattType getType()
    {
        return this.p;
    }
}    