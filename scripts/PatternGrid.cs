using System; // string
using System.IO; // file
using UnityEngine; // vector3
using System.Collections;
using System.Collections.Generic; // list, dictionary

class PatternGrid{
	public Dictionary<int, int> pattern = new Dictionary<int, int>(); // map of combined grid indices -> android grid indices
	public Dictionary<int, int> patternRev = new Dictionary<int, int>(); // map of android grid indices -> combined grid indices
	//Helper h = new Helper();
	List<int> currentSelPattern = new List<int>();

	public PatternGrid(){
		CreateNumCubeMap ();
	}

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

	List<int> GetFirstGrid(int start){ // get first 3X3 grid
		int startCubeNumber = patternRev [start];
		List<List<int>> firstGridOptions = firstGrid [startCubeNumber];
		int indexFirstGrid = UnityEngine.Random.Range (0, 3);
		List<int> grid = new List<int> ();
		grid.AddRange(firstGridOptions [indexFirstGrid]);
		return grid;
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
			int s = (int)(selectedPattern[i] - '0');
			selectedPatternList.Add(pattern[s]);
		}
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

		/* set total grid to current selected pattern */
		SetCurrentSelPattern (combinedGrid);

		/* get list of pattern gameobjects */
		return GetPatternGameobjects (combinedGrid);
	}

	public List<List<int>> SamplePatterns()
	{
		/*
		List<List<int>> combinedGridList1 = new List<List<int>> () {
			new List<int>() {4, 8, 6, 7, 10, 13, 14, 11},
			new List<int>() {4, 8, 6, 7, 11, 13, 14, 12},
			new List<int>() {4, 8, 6, 7, 14, 15, 17, 18},
			new List<int>() {4, 8, 6, 7, 11, 13, 17, 15, 14},
			new List<int>() {4, 8, 6, 7, 10, 14, 15, 11, 13},
			new List<int>() {4, 8, 6, 7, 10, 14, 12, 17, 13},
			new List<int>() {4, 8, 6, 7, 10, 15, 18, 14, 12},
			new List<int>() {4, 8, 6, 7, 10, 13, 18, 17, 12},
			new List<int>() {4, 3, 5, 8, 11, 14, 10, 14, 18},
			new List<int>() {4, 3, 5, 8, 13, 14, 12, 11, 14, 17},
			new List<int>() {1, 2, 5, 8, 9, 10, 17, 14, 11, 12},
			new List<int>() {5, 6, 8, 7, 10, 11, 14, 16, 17}
		};
		*/
		/* watch data 1
		List<List<int>> combinedGridList2 = new List<List<int>> () {
			new List<int>() {4, 8, 6, 7, 10, 13, 14, 11}, // label a
			new List<int>() {2, 5, 8, 12, 14, 16, 17, 18}, // b
			new List<int>() {4, 3, 5, 8, 10, 11, 14, 17, 18}, // c
			new List<int>() {4, 3, 5, 8, 11, 13, 14, 12}, // d
			new List<int>() {5, 6, 8, 9, 14, 15, 17, 18}, // e
		};
		return combinedGridList2;
		*/

		/*
		//simple, median and complex patterns
		List<List<int>> combinedGridList3 = new List<List<int>>(){
			new List<int>() {10, 11, 14, 17, 18}, // label a
			new List<int>() {10, 14, 18, 15, 17, 16, 14, 12}, // b
			new List<int>() {10, 11, 12, 13, 18, 17, 16, 15, 14}, // c
		};
		return combinedGridList3;
		*/

		// watch_data_3 used these patterns
		/*
		List<List<int>> combinedGridList3 = new List<List<int>>(){
			new List<int>() {10, 11, 14, 17, 18}, // label a
			new List<int>() {10, 11, 14, 16, 17, 18}, // b
			new List<int>() {10, 13, 16, 17, 18, 15, 12}, // c
			new List<int>() {16, 13, 10, 14, 12, 15, 18}, // d
			new List<int>() {13, 16, 17, 18, 15, 12, 11}, // e
		};
		return combinedGridList3;
		*/

		// watch_data_4 3*3
		// TODO: make the input of 3*3 and above grid input insertion into list function instead of manual entry
		/*List<List<int>> combinedGridList4 = new List<List<int>>(){
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9]}, // label a
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9], pattern[6], pattern[3]}, // b
			new List<int>() {pattern[1], pattern[2], pattern[3], pattern[5], pattern[7], pattern[8], pattern[9]}, // c
			new List<int>() {pattern[1], pattern[4], pattern[5], pattern[6], pattern[9]}, // d
		};
		return combinedGridList4;*/

		// watch_data_4 6*3 
		// TODO: Don't know how to proceed
		// TODO: make the input of 3*3 and above grid input insertion into list function instead of manual entry
		/*List<List<int>> combinedGridList5 = new List<List<int>>(){
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9]}, // label a
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9], pattern[6], pattern[3]}, // b
			new List<int>() {pattern[1], pattern[2], pattern[3], pattern[5], pattern[7], pattern[8], pattern[9]}, // c
			new List<int>() {pattern[1], pattern[4], pattern[5], pattern[6], pattern[9]}, // d
		};
		return combinedGridList5;*/

		/*all common patterns*/
		/*List<List<int>> combinedGridList5 = new List<List<int>>(){
			new List<int>() {pattern[3], pattern[2], pattern[5], pattern[8], pattern[7]}, // label a 32587
			new List<int>() {pattern[1], pattern[4], pattern[5], pattern[6], pattern[9]}, // b
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9]}, // c
			new List<int>() {pattern[3], pattern[2], pattern[1], pattern[4], pattern[5], pattern[6], pattern[9], pattern[8], pattern[7]}, // d
			new List<int>() {pattern[1], pattern[4], pattern[7], pattern[8], pattern[9], pattern[6], pattern[3]},
			new List<int>() {pattern[1],pattern[2],pattern[3],pattern[5],pattern[7],pattern[8],pattern[9]},
			new List<int>() {pattern[3],pattern[5],pattern[6],pattern[8]},
			new List<int>() {pattern[1],pattern[5],pattern[4],pattern[2]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[2],pattern[3],pattern[5],pattern[9]},
			new List<int>() {pattern[1],pattern[4],pattern[7],pattern[5],pattern[3],pattern[6],pattern[9]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[5],pattern[9],pattern[6],pattern[3]}, //k
			new List<int>() {pattern[1],pattern[2],pattern[3],pattern[5],pattern[7]}, 
			new List<int>() {pattern[3],pattern[2],pattern[1],pattern[4],pattern[7],pattern[8],pattern[9]},
			new List<int>() {pattern[1],pattern[2],pattern[3],pattern[6],pattern[9],pattern[8],pattern[7]},
			new List<int>() {pattern[3],pattern[2],pattern[1],pattern[4],pattern[7],pattern[8],pattern[9],pattern[6],pattern[5]},
			new List<int>() {pattern[3],pattern[2],pattern[1],pattern[5],pattern[9],pattern[8],pattern[7]}, 
			new List<int>() {pattern[1],pattern[4],pattern[7],pattern[5],pattern[9],pattern[6],pattern[3]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[5],pattern[9],pattern[6],pattern[3]}, //r
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[5],pattern[3],pattern[6],pattern[9]},
			new List<int>() {pattern[9],pattern[8],pattern[7],pattern[4],pattern[1],pattern[2],pattern[3],pattern[6],pattern[5]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[2],pattern[3],pattern[6],pattern[9],pattern[8],pattern[5]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[2],pattern[3],pattern[6],pattern[9]},
			new List<int>() {pattern[1],pattern[4],pattern[8],pattern[6],pattern[3]}, // r
			new List<int>() {pattern[1],pattern[4],pattern[2],pattern[5],pattern[1]},
			new List<int>() {pattern[7],pattern[4],pattern[1],pattern[2],pattern[3],pattern[6], pattern[5]},
		};
		return combinedGridList5;*/
		List<List<int>> combinedGridList6 = new List<List<int>>()
		{
			new List<int>() {1, 4, 5, 6, 9, 12, 11, 14, 17, 16}, //a
			new List<int>() {3, 2, 5, 8, 7, 10, 13, 14, 15, 18}, //b 
			new List<int>() {3, 2, 5, 8, 7, 10, 13, 16, 17, 18}, //c
			new List<int>() {1, 4, 5, 6, 9, 12, 11, 10, 13, 14, 15, 18, 17, 16}, //d
			new List<int>() {3, 2, 5, 8, 7, 10, 13, 16, 17, 18, 15, 12}, //e
			new List<int>() {3, 2, 5, 8, 7, 10, 11, 12, 14, 16, 17, 18}, //f
			new List<int>() {1, 4, 5, 6, 9, 12, 14, 15, 17}, //g
			new List<int>() {3, 2, 5, 8, 7, 10, 14, 13, 11}, //h
			new List<int>() {7, 4, 1, 2, 3, 5, 9, 12, 11, 14, 17, 16}, //i
			new List<int>() {1, 4, 7, 5, 3, 6, 9, 12, 11, 10, 13, 14, 15, 18, 17, 16}, //j
			new List<int>() {16, 13, 10, 14, 18, 15, 12, 9, 6, 5, 4, 1}, //k
			new List<int>() {3, 2, 5, 8, 7, 10, 11, 12, 14, 16}, //l
			new List<int>() {1, 4, 5, 6, 9, 12, 11, 10, 13, 16, 17, 18}, //m
			new List<int>() {3, 2, 5, 8, 7, 10, 11, 12, 15, 18, 17, 16	}, //n
			new List<int>() {1, 4, 5, 6, 9, 12, 11, 10, 13, 16, 17, 18, 15, 14}, //o
			new List<int>() {1, 4, 5, 6, 9, 12, 11, 10, 14, 18, 17, 16}, //p
			new List<int>() {1, 2, 3, 6, 9, 8, 7, 10, 13, 16, 14, 18, 15, 12}, //q
			new List<int>() {2, 4, 5, 7, 10, 11, 12, 14, 16}, //r
			new List<int>() {7, 4, 1, 5, 3, 6, 9, 12, 11, 10, 14, 18, 17, 16}, //s
			new List<int>() {16, 17, 18, 14, 10, 11, 12, 9, 8, 7, 4, 1, 2, 3, 6, 5}, //t
			new List<int>() {12, 15, 18, 14, 16, 13, 10, 7, 4, 1, 2, 3, 6, 5}, //u
			new List<int>() {7, 4, 1, 2, 3, 6, 9, 12, 14, 15, 17}, //v
			new List<int>() {2, 4, 5, 7, 10, 13, 17, 15, 12}, //w
			new List<int>() {1, 2, 3, 5, 7, 10, 13, 11, 14, 10}, //x
			new List<int>() {12, 15, 18, 14, 16, 13, 10, 7, 4, 1, 2, 3, 6, 5}, //y
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
