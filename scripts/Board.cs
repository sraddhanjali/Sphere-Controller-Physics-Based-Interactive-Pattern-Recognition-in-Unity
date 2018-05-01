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
		LoadLinkedList();
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

	public string GetCurrentLabel(){
		return labels[matchingIndex];
	}

	public void ClearVariableState(){
		counter = 0;
		match = false;
		matchingIndex = 0;
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

	//TODO: Obstacle needs to be added in line with the pattern list
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
		return refList;
	}

	public List<LinkedListNode<GameObject>> GetNextNode(GameObject g, int count=3) {
		List<LinkedListNode<GameObject>> refList = new List<LinkedListNode<GameObject>>();
		LinkedListNode<GameObject> gll = allPatterns.Find(g);
		return GetNextNodeImp(gll, refList, count);
	}
	
	public void StartMatching(GameObject go){
		List<GameObject> patternGO = patterns[matchingIndex].sequence;
		if (GameObject.ReferenceEquals(patternGO[counter], go)){
			go.GetComponent<SpriteRenderer> ().material.color = Color.black;
			GameObject g1 = patternGO[counter];
			GameObject g2 = patternGO[patternGO.Count-1];
			if (g1 == g2){
				Debug.Log("Last endpoint matched");
				if (AllMatched()) {
					Debug.Log("success triggered in B 0");
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
			}
			EventManager.TriggerEvent("matches");
		}
		else if (GameObject.ReferenceEquals(patternGO[counter-1], go)) {
			Debug.Log("Pressing previous region");
		}
		else{
			Debug.LogWarning("fail triggered in B 1");
			EventManager.TriggerEvent("fail");
		}
	}

	public bool SetPatternToMatchInBoard(GameObject go) {
		for (int i = 0; i < patterns.Count; i++){
			List<GameObject> patternGO = patterns[i].sequence;
			if (GameObject.ReferenceEquals(patternGO[0], go)){
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
}