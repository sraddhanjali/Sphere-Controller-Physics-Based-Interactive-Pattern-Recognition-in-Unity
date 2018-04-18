using System; // string
using System.IO; // file
using UnityEngine; // vector3
using System.Collections;
using System.Collections.Generic; // list, dictionary

class PatternGrid
{
	public Dictionary<int, int>
		pattern = new Dictionary<int, int>(); // map of combined grid indices -> android grid indices

	public Dictionary<int, int>
		patternRev = new Dictionary<int, int>(); // map of android grid indices -> combined grid indices

	//Helper h = new Helper();
	List<int> currentSelPattern = new List<int>();

	public PatternGrid()
	{
		CreateNumCubeMap();
	}
	
	/* second grid */
	public void CreateNumCubeMap()
	{
		// mapping of second grid in the scheme of 9X3 grid

		pattern.Add(1, 10);
		pattern.Add(2, 11);
		pattern.Add(3, 12);
		pattern.Add(4, 13);
		pattern.Add(5, 14);
		pattern.Add(6, 15);
		pattern.Add(7, 16);
		pattern.Add(8, 17);
		pattern.Add(9, 18);

		patternRev.Add(10, 1);
		patternRev.Add(11, 2);
		patternRev.Add(12, 3);
		patternRev.Add(13, 4);
		patternRev.Add(14, 5);
		patternRev.Add(15, 6);
		patternRev.Add(16, 7);
		patternRev.Add(17, 8);
		patternRev.Add(18, 9);
	}


	List<string> ReadPatternFileIntoList()
	{
		// read the pattern file and add each pattern strings into a list
		List<string> patternStringLists = new List<string>();
		TextAsset wordFile = Resources.Load("easy") as TextAsset;
		if (wordFile)
		{
			string line;
			StringReader textStream = new StringReader(wordFile.text);
			while ((line = textStream.ReadLine()) != null)
			{
				patternStringLists.Add(line);
			}

			textStream.Close();
		}

		return patternStringLists;
	}

	string GetRandomPatternLine(List<string> patternList)
	{
		// get random line from the pattern string list
		return patternList[UnityEngine.Random.Range(0, patternList.Count)];
	}

	List<int> ChangePatternStringToList(string selectedPattern)
	{
		// turns numeric string to list of ints
		List<int> selectedPatternList = new List<int>();
		for (int i = 0; i < selectedPattern.Length; i++)
		{
			int s = (int) (selectedPattern[i] - '0');
			selectedPatternList.Add(pattern[s]);
		}

		return selectedPatternList;
	}

	List<int> CombineGrids(List<int> grid, List<int> selectedPatternList)
	{
		// combination of first and second grids
		grid.AddRange(selectedPatternList);
		selectedPatternList = new List<int>();
		selectedPatternList = grid;
		return selectedPatternList;
	}

	List<GameObject> GetPatternGameobjects(List<int> selectedPatternList)
	{
		// get gameobject for selected pattern
		List<GameObject> patternCube = new List<GameObject>();
		int cubeNumber;
		GameObject g;
		for (int j = 0; j < selectedPatternList.Count; j++)
		{
			cubeNumber = selectedPatternList[j];
			g = GameObject.Find(String.Format("{0}", cubeNumber));
			patternCube.Add(g);
		}

		return patternCube;
	}

	void SetCurrentSelPattern(List<int> curr)
	{
		currentSelPattern = curr;
	}

	public List<int> GetCurrentSelPattern()
	{
		return currentSelPattern;
	}

	public List<List<int>> SamplePatterns()
	{

		List<List<int>> combinedGridList6 = new List<List<int>>()
		{
			new List<int>() {12, 11, 14, 17, 16}, //a
			new List<int>() {10, 13, 14, 15, 18}, //b 
			new List<int>() {10, 13, 16, 17, 18}, //c
			new List<int>() {12, 11, 10, 13, 14, 15, 18, 17, 16}, //d
			new List<int>() {10, 13, 16, 17, 18, 15, 12}, //e
			new List<int>() {10, 11, 12, 14, 16, 17, 18}, //f
			new List<int>() {12, 14, 15, 17}, //g
			new List<int>() {10, 14, 13, 11}, //h
			new List<int>() {16, 13, 10, 11, 12, 14, 18}, //i
			new List<int>() {10, 13, 16, 14, 12, 15, 18}, //j
			new List<int>() {16, 13, 10, 14, 18, 15, 12}, //k
			new List<int>() {10, 11, 12, 14, 16}, //l
			new List<int>() {12, 11, 10, 13, 16, 17, 18}, //m
			new List<int>() {10, 11, 12, 15, 18, 17, 16}, //n
			new List<int>() {12, 11, 10, 13, 16, 17, 18, 15, 14}, //o
			new List<int>() {12, 11, 10, 14, 18, 17, 16}, //p
			new List<int>() {10, 13, 16, 14, 18, 15, 12}, //q
			new List<int>() {11, 13, 14, 16}, //r
			new List<int>() {16, 13, 10, 14, 12, 15, 18}, //s
			new List<int>() {18, 17, 16, 13, 10, 11, 12, 15, 14}, //t
			new List<int>() {16, 13, 10, 11, 12, 15, 18, 17, 14}, //u
			new List<int>() {16, 13, 10, 11, 12, 15, 18}, //v
			new List<int>() {10, 13, 17, 15, 12}, //w
			new List<int>() {10, 11, 13, 14, 10}, //x
			new List<int>() {16, 13, 10, 11, 12, 15, 14}, //y
		};
		return combinedGridList6;
	}


public List<GameObject> GetSimplePatterns(){
		
		// 25 patterns
		List<List<int>> combinedGridList = SamplePatterns ();
		/* get random pattern from list of patterns
		List<int> combinedGrid = combinedGridList[UnityEngine.Random.Range(0, combinedGridList.Count)];
		*/

		List<int> combinedGrid = combinedGridList [Main.patternIndex];
		/* set total grid to current selected pattern */
		SetCurrentSelPattern (combinedGrid);

		/* get list of pattern gameobjects */
		return GetPatternGameobjects (combinedGrid);
	}

	public List<GameObject> GetZPatterns(){
		List<int> zPatternList = new List<int>() {2, 5, 8, 10, 11, 12, 14, 16, 17, 18};
		SetCurrentSelPattern (zPatternList);
		return GetPatternGameobjects(zPatternList);
	}

	public void InitialCubesColor(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Cube");
		for (int i = 0; i < objects.Length; i++)
		{
			GameObject b = objects[i];
			b.layer = 8;
			b.GetComponent<Renderer> ().material.color = Color.white;
		}
	}
}
