using UnityEngine;
using System;
// string
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

// list, dictionary

public class Board{
	List<Pattern> patterns = new List<Pattern>();
	List<Obstacle> obstacles = new List<Obstacle>();
	private int trackingIndex;
	private int matchingIndex;
	public bool match = false;
	private int counter = 0;
	private int matchedCount = 0;
	public List<string> labels = new List<string>();
	public LinkedList<GameObject> allPatterns = new LinkedList<GameObject>();
	public List<string> swipedPatterns = new List<string>();
	public List<string> actualPatterns = new List<string>();
		
	public Board(){
		matchingIndex = 0;
		LoadLinkedList();
	}

	public void AddPattern(Pattern pattern){
		patterns.Add(pattern);
		labels.Add(pattern.getName());
	}

	public void AddObstacle(Obstacle obstacle){
		obstacles.Add(obstacle);
	}

	public string GetCurrentLabel(){
		return labels[matchingIndex];
	}

	public void ClearVariableState(){
		counter = 0;
		match = false;
		matchingIndex = 0;
		ClearAllPatterns();
	}

	public void LoadLinkedList() {
		LinkedListNode<GameObject> tipNode = null;
		foreach (Pattern p in patterns) {
			foreach (GameObject g in p.sequence) {
				if (tipNode == null) {
					allPatterns.AddFirst(g);	
				}
				else {
					allPatterns.AddAfter(tipNode, g);
				}
				tipNode = allPatterns.Last;
				actualPatterns.Add(g.name);
			}
		}
	}

	public void ClearAllPatterns() {
		allPatterns.Clear();
		swipedPatterns.Clear();
		actualPatterns.Clear();
	}
	
	public string ChunkToSave(GameObject go, Vector3 pos) {
		string cn = int.Parse(go.name).ToString ();
		string label = GetCurrentLabel();
		Vector3 n = Camera.main.WorldToScreenPoint(pos);
		string x = n.x.ToString ();
		string y = n.y.ToString ();
		string z = n.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string csvstring = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Main.level.ToString(), label, cn, x, y, z, ts, MainMenuButtons.speed, Main.sphereAnimationTs);
		return csvstring;
	}
	
	public void MatchPatterns(GameObject go, Vector3 pos) {
		int number = Int32.Parse(go.name);
		if (number > 9) {
			if (matchingIndex == 0) {
				matchingIndex += 1;	
			}
		}else {
			matchingIndex = 0;
		}
		GameData.instance.LoadToTemp(ChunkToSave(go, pos));
		swipedPatterns.Add(go.name);
	}

	public bool PatternsMatch() {
		if (swipedPatterns.Count != 0 && actualPatterns.Count != 0) {
			List<string> distinct = swipedPatterns.ToList();
			bool equal = distinct.SequenceEqual(actualPatterns);
			return equal;	
		}
		return false;
	}
}