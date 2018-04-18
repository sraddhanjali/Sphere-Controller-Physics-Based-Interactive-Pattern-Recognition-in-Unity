using UnityEngine; // vector3
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary
	
public class Board
{
	private List<Pattern> patterns;
	private string pattName;

	public int trackingIndex;
	public int matchingIndex;

	private int pipeCount;

	public Board(){
		this.patterns = new List<Pattern>();
		this.trackingIndex = 0;
		this.matchingIndex = 0;
	}

	public void AddPattern(Pattern n){
		this.patterns.Add(n);
		this.pipeCount += 1;
	}

	public void AddPatternName(string name){
		this.pattName = name;
	}

	public int getPipeCount(){
		return this.pipeCount;
	}
	
	public string getBoardString(){
		return string.Format("*** Board name: {0}", this.pattName);
	}
}