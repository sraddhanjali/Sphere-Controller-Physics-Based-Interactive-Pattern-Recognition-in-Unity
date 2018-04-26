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
	
	public Board(){
		matchingIndex = 0;
	}

	public List<string> GetLabels(){
		return this.labels;
	}

	public void AddPattern(Pattern pattern){
		this.patterns.Add(pattern);
		this.labels.Add(pattern.getName());
	}

	public void AddObstacle(Obstacle obstacle){
		this.obstacles.Add(obstacle);
	}

	public List<Pattern> GetPatterns(){
		return this.patterns;
	}

	public List<Obstacle> GetObstacles(){
		return this.obstacles;
	}

	public string GetCurrentLabel(){
		return this.labels[this.matchingIndex];
	}

	public void ClearVariableState(){
		this.counter = 0;
		this.match = false;
		this.matchingIndex = 0;
	}

	public int GetCurrentPatternSize(){
		return this.patterns.Count;
	}

	public bool AllMatched(){
		if (this.matchedCount == this.GetCurrentPatternSize()){
			this.matchedCount = 0;
			this.ClearVariableState();
			return true;
		}
		else{
			return false;
		}
	}	
	
	public List<GameObject> ToDraw(){
		List<Obstacle> obstacle = this.obstacles;
		List<Pattern> pattern = this.patterns;
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
		List<GameObject> patternGO = this.patterns[this.matchingIndex].sequence;
		if (GameObject.ReferenceEquals(patternGO[counter], go)){
			go.GetComponent<SpriteRenderer> ().material.color = Color.red;
			if (patternGO[counter] == patternGO[patternGO.Count - 1]){
				Debug.Log("Last endpoint matched" + patternGO[counter] + go.name);
				this.ClearVariableState();
				this.matchedCount += 1;
			}
			else{
				Debug.Log("matched");
				this.counter += 1;
			}
		}
	}

	public bool SetPatternToMatchInBoard(GameObject go){
		for (int i = 0; i < this.patterns.Count; i++){
			List<GameObject> patternGO = this.patterns[i].sequence;
			if (GameObject.ReferenceEquals(patternGO[0], go)){
				Debug.Log(patternGO[0] + go.name);
				this.match = true;
				Debug.Log("matching init...");	
				this.matchingIndex = i;
				go.GetComponent<SpriteRenderer> ().material.color = Color.red;
				this.counter += 1;
				return true;
			}
		}
		return false;
	}
}