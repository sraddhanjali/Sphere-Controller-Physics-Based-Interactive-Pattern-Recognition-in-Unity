using System; // string
using System.IO; // file
using UnityEngine; // vector3
using System.Collections;
using System.Collections.Generic; // list, dictionary

class PatternGrid{
	public Dictionary<int, int> pattern = new Dictionary<int, int>(); // map of combined grid indices -> android grid indices
	public Dictionary<int, int> patternRev = new Dictionary<int, int>(); // map of android grid indices -> combined grid indices
	Helper h = new Helper();
	List<int> currentSelPattern = new List<int>();

	/* first static grid */
	public Dictionary<int, List<List<int>>> firstGrid = new Dictionary<int, List<List<int>>>() {
		{1, new List<List<int>>{ new List<int>{1, 2, 3, 5, 7, 8, 9}, new List<int>{3, 5, 6, 8}, new List<int>{7, 4, 1, 2, 3, 5, 9}, new List<int>{1, 4, 7, 5, 3, 6, 9}, new List<int>{3, 2, 1, 4, 7, 8, 9}, new List<int>{7, 4, 1, 2, 3, 6, 9} } }, 
		{2, new List<List<int>>{ new List<int>{3, 2, 1, 4, 5, 6, 9, 8, 7}, new List<int>{1, 2, 3, 5, 7}, new List<int>{1, 2, 3, 6, 9, 8, 7}, new List<int>{3, 2, 1, 5, 9, 8, 7}, new List<int>{1, 2, 3, 5, 7, 8, 9}, new List<int>{3, 5, 6, 8}, new List<int>{7, 4, 1, 2, 3, 5, 9}, new List<int>{1, 4, 7, 5, 3, 6, 9}, new List<int>{3, 2, 1, 4, 7, 8, 9}, new List<int>{7, 4, 1, 2, 3, 6, 9} } },
		{3, new List<List<int>>{ new List<int>{1, 2, 3, 5, 7, 8, 9}, new List<int>{3, 5, 6, 8}, new List<int>{7, 4, 1, 2, 3, 5, 9}, new List<int>{1, 4, 7, 5, 3, 6, 9}, new List<int>{3, 2, 1, 4, 7, 8, 9}, new List<int>{7, 4, 1, 2, 3, 6, 9} } },
		{4, new List<List<int>>{ new List<int>{3, 2, 1, 4, 5, 6, 9, 8, 7}, new List<int>{1, 2, 3, 5, 7}, new List<int>{1, 2, 3, 6, 9, 8, 7}, new List<int>{3, 2, 1, 5, 9, 8, 7}, new List<int>{1, 2, 3, 5, 7, 8, 9}, new List<int>{3, 5, 6, 8}, new List<int>{7, 4, 1, 2, 3, 5, 9}, new List<int>{1, 4, 7, 5, 3, 6, 9}, new List<int>{3, 2, 1, 4, 7, 8, 9}, new List<int>{7, 4, 1, 2, 3, 6, 9} }},
		{5, new List<List<int>>{ new List<int>{1, 8, 13}, new List<int>{3, 8, 9, 11}, new List<int>{1, 5, 4, 11} } },	
		{6, new List<List<int>>{ new List<int>{4, 11, 14}, new List<int>{2, 4, 11, 12}, new List<int>{1, 8, 10, 13, 14} } },
		{7, new List<List<int>>{ new List<int>{1, 2, 3, 5, 9, 11}, new List<int>{4, 8, 9, 12, 15}, new List<int>{7, 8, 4} } },
		{8, new List<List<int>>{ new List<int>{4, 11, 14}, new List<int>{9, 6, 5, 12, 14, 17}, new List<int>{1, 8, 10, 13} } },
		{9, new List<List<int>>{ new List<int>{2, 9, 11}, new List<int>{6, 8, 15}, new List<int>{4, 7, 10, 17} } }
	}; 

	List<int> GetFirstGrid(int start){ // get first 3X3 grid
		int startCubeNumber = patternRev [start];
		List<List<int>> firstGridOptions = firstGrid [startCubeNumber];
		int indexFirstGrid = UnityEngine.Random.Range (0, 3);
		List<int> grid = new List<int> ();
		grid.AddRange(firstGridOptions [indexFirstGrid]);
		return grid;
	}

	/* second grid */
	public void CreateNumCubeMap(){ // mapping of second grid in the scheme of 9X3 grid
		
		pattern.Add (1, 10);
		pattern.Add (2, 11);
		pattern.Add (3, 12);
		pattern.Add (4, 13);
		pattern.Add (5, 14);
		pattern.Add (6, 15);
		pattern.Add (7, 16);
		pattern.Add (8, 17);
		pattern.Add (9, 18);

		patternRev.Add (10, 1);
		patternRev.Add (11, 2);
		patternRev.Add (12, 3);
		patternRev.Add (13, 4);
		patternRev.Add (14, 5);
		patternRev.Add (15, 6);
		patternRev.Add (16, 7);
		patternRev.Add (17, 8);
		patternRev.Add (18, 9);
	}

	List<string> ReadPatternFileIntoList(){ // read the pattern file and add each pattern strings into a list
		List<string> patternStringLists = new List<string>();
		TextAsset wordFile = Resources.Load("easy") as TextAsset; 
		if (wordFile){
			string line;
			StringReader textStream = new StringReader(wordFile.text);
			while((line = textStream.ReadLine()) != null){
				patternStringLists.Add(line);
			}
			textStream.Close();
		}
		return patternStringLists;
	}

	string GetRandomPatternLine(List<string> patternList){ // get random line from the pattern string list
		return patternList[UnityEngine.Random.Range(0, patternList.Count)];
	}

	List<int> ChangePatternStringToList(string selectedPattern){ // turns numeric string to list of ints
		List<int> selectedPatternList = new List<int>();
		for (int i = 0; i < selectedPattern.Length; i++) {
			selectedPatternList.Add(pattern[(int)(selectedPattern[i]-'0')]);
		}
		SetCurrentSelPattern (selectedPatternList);
		return selectedPatternList;
	}

	List<int> CombineGrids(List<int> grid, List<int> selectedPatternList){ // combination of first and second grids
		grid.AddRange (selectedPatternList);
		selectedPatternList = new List<int> ();
		selectedPatternList = grid;
		return selectedPatternList;
	}

	List<GameObject> GetPatternGameobjects(List<int> selectedPatternList){ // get gameobject for selected pattern
		List<GameObject> patternCube = new List<GameObject>();
		int cubeNumber;
		GameObject g;
		for (int j = 0; j < selectedPatternList.Count; j++) {
			cubeNumber = selectedPatternList [j];
			g = GameObject.Find (String.Format ("{0}", cubeNumber));
			patternCube.Add(g);
		}
		return patternCube;
	}

	void SetCurrentSelPattern(List<int> curr){
		currentSelPattern = curr;
	}

	public List<int> GetCurrentSelPattern(){
		return currentSelPattern;
	}

	public List<GameObject> GetPatterns(){
		
		/* get second grid */
		List<string> patternStringLists = ReadPatternFileIntoList ();
		string selectedPattern = GetRandomPatternLine (patternStringLists);
		List<int> selectedPatternList = ChangePatternStringToList(selectedPattern);

		/* get first grid */
		List<int> grid = GetFirstGrid (selectedPatternList[0]);

		/* combine grids */
		List<int> combinedGrid = CombineGrids (grid, selectedPatternList);

		/* get list of pattern gameobjects */
		return GetPatternGameobjects (selectedPatternList);
	}
}
