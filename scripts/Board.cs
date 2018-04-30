using UnityEngine; // vector3
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary

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
	
	public Board(){
		matchingIndex = 0;
	}

	public List<string> GetLabels(){
		return labels;
	}

	public void AddPattern(Pattern pattern){
		patterns.Add(pattern);
		labels.Add(pattern.getName());
	}

	public void AddObstacle(Obstacle obstacle){
		obstacles.Add(obstacle);
	}

	public List<Pattern> GetPatterns(){
		return patterns;
	}

	public List<Obstacle> GetObstacles(){
		return obstacles;
	}

	public string GetCurrentLabel(){
		return labels[matchingIndex];
	}

	public void ClearVariableState(){
		counter = 0;
		match = false;
		matchingIndex = 0;
	}

	public int GetCurrentPatternSize(){
		return patterns.Count;
	}

	public bool AllMatched(){
		if (matchedCount == GetCurrentPatternSize()){
			matchedCount = 0;
			ClearVariableState();
			return true;
		}
		else{
			return false;
		}
	}
	
	//TODO: Obstacle needs to be added in line with the pattern list
	public void LoadLinkedList() {
		List<Obstacle> obstacle = obstacles;
		List<Pattern> pattern = patterns;
		LinkedListNode<GameObject> tipNode = null;
		foreach (Pattern p in pattern) {
			foreach (GameObject g in p.sequence) {
				if (tipNode == null) {
					allPatterns.AddFirst(g);	
				}
				else {
					allPatterns.AddAfter(tipNode, g);
				}
				tipNode = allPatterns.Last;
			}
		}
		// Print the linkedlist to see if they arrive in order
		/*foreach (GameObject g in allPatterns) {
			Debug.Log(g.name);
		}*/
	}

	public List<LinkedListNode<GameObject>> GetNextNodeImp(LinkedListNode<GameObject> g, List<LinkedListNode<GameObject>> refList, int count) {
		if (g != null) {
			LinkedListNode<GameObject> next = g.Next;
			if (count == 0 || next == null) {
				return refList;
			}
			refList.Add(next);
			return GetNextNodeImp(next, refList, --count);
		}
		else {
			return refList;
		}
	}

	public List<LinkedListNode<GameObject>> GetNextNode(GameObject g, int count=3) {
		List<LinkedListNode<GameObject>> refList = new List<LinkedListNode<GameObject>>();
		LinkedListNode<GameObject> gll = allPatterns.Find(g);
		return GetNextNodeImp(gll, refList, count);
	}
	
	public List<GameObject> ToDraw(){
		List<Obstacle> obstacle = obstacles;
		List<Pattern> pattern = patterns;
		List<GameObject> bigChunk = new List<GameObject>();
		for (int i = 0; i < pattern.Count; i++){
			bigChunk.AddRange(pattern[i].sequence);
			if (i == 0){
				if(obstacle.Count > 0){
					bigChunk.Add(obstacle[0].gameObject); // 8
				}
			}
		}
		return bigChunk;
	}
	
	public void StartMatching(GameObject go){
		List<GameObject> patternGO = patterns[matchingIndex].sequence;
		if (GameObject.ReferenceEquals(patternGO[counter], go)){
			go.GetComponent<SpriteRenderer> ().material.color = Color.black;
			GameObject g1 = patternGO[counter];
			Debug.Log(g1.name);
			GameObject g2 = patternGO[patternGO.Count - 1];
			Debug.Log(g2.name);
			if (g1 == g2){
				Debug.Log("Last endpoint matched" + patternGO[counter] + go.name);
				ClearVariableState();
				matchedCount += 1;
			}
			else{
				Debug.Log("matched");
				counter += 1;
			}
		}
		else {
			//Main.reload = true;
			EventManager.TriggerEvent("fail");
			//ClearVariableState();
		}
	}

	public bool SetPatternToMatchInBoard(GameObject go){
		for (int i = 0; i < patterns.Count; i++){
			List<GameObject> patternGO = patterns[i].sequence;
			if (GameObject.ReferenceEquals(patternGO[0], go)){
				match = true;
				matchingIndex = i;
				go.GetComponent<SpriteRenderer> ().material.color = Color.black;
				counter += 1;
				return true;
			}
		}
		//Main.reload = true;
		EventManager.TriggerEvent("fail");
		//ClearVariableState();
		return false;
	}
}