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
	
}