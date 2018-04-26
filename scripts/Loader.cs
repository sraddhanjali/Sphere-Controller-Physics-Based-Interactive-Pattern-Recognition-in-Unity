using System; // string
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic; // list, dictionary
using UnityEngine; // vector3
using System.Text.RegularExpressions;

public class Loader{
	List<string> labels = new List<string>();
	
	public void LabelsStore(string label){
		labels.Add(label);
	}
	
	public List<Board> ReadFileTest(){
		string cubeNumber;
		TextAsset ft = Resources.Load("topbottommix") as TextAsset;
		string ftstring = ft.text.Trim();
		string[] fLines = Regex.Split(ftstring, "\n|\r|\r\n");
		int lenSplit1;
		int lenSplit2;
		List<Board> boardList = new List<Board>();
		for (int i = 0; i < fLines.Length; i++)
		{
			Board board = new Board();
			string[] allPatterns = fLines[i].Split(new char[] {','});
			lenSplit1 = allPatterns.Length;
			for (int j = 0; j < lenSplit1-2; j++){
				string[] pattern = allPatterns[j].Trim().Split(new char[] {' '});
				List<GameObject> p = new List<GameObject>();
				lenSplit2 = pattern.Length;
				for (int k = 0; k < lenSplit2; k++){
					cubeNumber = pattern[k];
					p.Add(GameObject.Find(String.Format("{0}", cubeNumber)));
				}
				if (lenSplit2 != 1){
					if (j == 0){
						labels.Add(allPatterns[lenSplit1 - 2]);
					}
					else{
						labels.Add(allPatterns[lenSplit1 - 1]);
					}
					Pattern pat = new Pattern(p, labels[labels.Count-1]);
					board.AddPattern(pat);
				}
				else{
					Obstacle obstacle = new Obstacle(p[p.Count - 1]);
					board.AddObstacle(obstacle);
				}
			}
			boardList.Add(board);
		}
		return boardList;
	}

	public List<string> GetLabels(){
		return this.labels;
	}
}