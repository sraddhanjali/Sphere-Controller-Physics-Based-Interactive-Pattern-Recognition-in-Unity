using UnityEngine; // vector3
using System;
using System.CodeDom.Compiler;
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

	public int GetCurrentPatternSize() {
		return patterns.Count;
	}

	public bool AllMatched(){
		if (matchedCount == GetCurrentPatternSize()){
			matchedCount = 0;
			ClearVariableState();
			return true;
		}
		return false;
	}
	
	public List<GameObject> ToDraw(){
		List<Obstacle> obstacle = obstacles;
		List<Pattern> pattern = patterns;
		List<GameObject> bigChunk = new List<GameObject>();
		for (int i = 0; i < pattern.Count; i++){
			bigChunk.AddRange(pattern[i].sequence);
			if (i == 0){
				if(obstacle.Count > 0){
					bigChunk.Add(obstacle[0].gameObject);
				}
			}
		}
		return bigChunk;
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
		//allPatterns = new LinkedList<GameObject>();
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
		string csvstring = string.Format("{0},{1},{2},{3},{4},{5},{6}", Main.level.ToString(), label, cn, x, y, z, ts);
		return csvstring;
	}
	
	public void StartMatching(GameObject go, Vector3 pos){
		List<GameObject> patternGO = patterns[matchingIndex].sequence;
		if (GameObject.ReferenceEquals(patternGO[counter], go)){
			GameData.instance.LoadToTemp(ChunkToSave(go, pos));
			go.GetComponent<SpriteRenderer> ().material.color = Color.black;
			EventManager.TriggerEvent("matches");
			GameObject g1 = patternGO[counter];
			GameObject g2 = patternGO[patternGO.Count-1];
			if (g1 == g2){
				Debug.Log("Last endpoint matched");
				if (AllMatched()) {
					Debug.Log("success triggered in B 0");
					//GameData.instance.SaveToFile();
					EventManager.TriggerEvent("success");
				}
				else {
					matchedCount += 1;
					match = false;
					counter = 0;
					Debug.Log("start matching next pattern");
				}
			}
			else{
				counter += 1;
				Debug.Log("matched in B");
				EventManager.TriggerEvent("matches");
			}
		}
		else if (GameObject.ReferenceEquals(patternGO[counter-1], go)) {
			GameData.instance.LoadToTemp(ChunkToSave(go, pos));
			Debug.Log("Pressing previous region");
		}
		else{
			Debug.LogWarning("fail triggered in B 1");
			EventManager.TriggerEvent("fail");
		}
	}

	public bool SetPatternToMatchInBoard(GameObject go, Vector3 pos) {
		for (int i = 0; i < patterns.Count; i++){
			List<GameObject> patternGO = patterns[i].sequence;
			if (GameObject.ReferenceEquals(patternGO[0], go)) {
				GameData.instance.LoadToTemp(ChunkToSave(go, pos));
				match = true;
				matchingIndex = i;
				go.GetComponent<SpriteRenderer> ().material.color = Color.black;
				counter += 1;
				EventManager.TriggerEvent("matches");
				return true;
			}
		}
		return false;
	}

	public void MatchPatterns(GameObject go, Vector3 pos) {
		int number = Int32.Parse(go.name);
		if (number > 9) {
			if (matchingIndex == 0) {
				matchingIndex += 1;	
			}
		}
		else {
			matchingIndex = 0;
		}
		GameData.instance.LoadToTemp(ChunkToSave(go, pos));
		swipedPatterns.Add(go.name);
	}

	public bool PatternsMatch() {
		List<string> distinct = swipedPatterns.ToList();
		bool equal = distinct.SequenceEqual(actualPatterns);
		return equal;
	}
}