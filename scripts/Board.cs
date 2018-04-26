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
	
	void SaveToFile(string path, int currentCube, string label, Vector3 pos){
		string cn = currentCube.ToString ();
		pos = Camera.main.WorldToScreenPoint(pos);
		string x = pos.x.ToString ();
		string y = pos.y.ToString ();
		string z = pos.z.ToString ();
		string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		string csv = string.Format("{0},{1},{2},{3},{4},{5},{6}\n", Main.level.ToString(), label, cn, x, y, z, ts);
		File.AppendAllText (path, csv);
	}

	public void Save(GameObject go, Vector3 pos){
		int currentCube = int.Parse (go.name);
		UnityEngine.Debug.Log("writing to file");
		UnityEngine.Debug.Log(currentCube);
		SaveToFile (Main.allPath, currentCube, this.GetCurrentLabel(), pos);
	}

	public void StartMatching(GameObject go, Vector3 pos){
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
			this.Save(go, pos);
		}
	}

	public bool SetPatternToMatchInBoard(GameObject go, Vector3 pos){
		for (int i = 0; i < this.patterns.Count; i++){
			List<GameObject> patternGO = this.patterns[i].sequence;
			if (GameObject.ReferenceEquals(patternGO[0], go)){
				Debug.Log(patternGO[0] + go.name);
				this.match = true;
				Debug.Log("matching init...");	
				this.matchingIndex = i;
				go.GetComponent<SpriteRenderer> ().material.color = Color.red;
				this.counter += 1;
				Save(go, pos);
				return true;
			}
		}
		return false;
	}
}