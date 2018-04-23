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
	public bool track = false;
	private int counter = 0;
	private int matchedCount = 0;
	
	public Board(){
		trackingIndex = 0;
		matchingIndex = 0;
	}

	public void AddPattern(Pattern pattern){
		this.patterns.Add(pattern);
	}

	public void AddObstacle(Obstacle obstacle){
		this.obstacles.Add(obstacle);
	}

	public List<Pattern> GetPatterns(){
		return this.patterns;
	}

	public void ClearVariableState(){
		this.counter = 0;
		this.match = false;
		this.track = false;
		this.matchingIndex = 0;
	}

	public int GetCurrentPatternSize(){
		return this.patterns.Count;
	}

	public bool AllMatched()
	{
		if (this.matchedCount == this.GetCurrentPatternSize()){
			this.matchedCount = 0;
			this.ClearVariableState();
			return true;
		}
		else{
			return false;
		}
	} 
		
	public void StartMatching(GameObject go){
		List<GameObject> patternGO = this.patterns[this.matchingIndex].sequence;
		if (GameObject.ReferenceEquals(patternGO[counter], go)){
			go.GetComponent<Renderer> ().material.color = Color.red;
			if (patternGO[counter] == patternGO[patternGO.Count - 1]){
				Debug.Log("Last endpoint matched" + patternGO[counter] + go.name);
				this.matchedCount += 1;
				this.ClearVariableState();
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
				go.GetComponent<Renderer> ().material.color = Color.red;
				this.counter += 1;
				if (this.matchingIndex == this.trackingIndex){
					this.track = true;
				}
				return true;
			}
		}
		return false;
	}
}