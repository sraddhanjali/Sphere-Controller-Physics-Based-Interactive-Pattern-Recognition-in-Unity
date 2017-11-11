using UnityEngine;
using System.Collections;
using System.IO;                    // For parsing text file, StringReader
using System.Collections.Generic;    // For List

public class GetPattern : MonoBehaviour {
	
	public TextAsset wordFile;

	//public static GetPattern instance;

	private List<string> lineList = new List<string>();        // List to hold all the lines read from the text file

	void Start()
	{
		wordFile = Resources.Load("patterns.txt") as TextAsset;
		ReadWordList();
		// Debug.Log("Random line from list: " + GetRandomLine());
	}

	public void ReadWordList()
	{
		// Check if file exists before reading
		if (wordFile)
		{
			string line;
			StringReader textStream = new StringReader(wordFile.text);

			while((line = textStream.ReadLine()) != null)
			{
				// Read each line from text file and add into list
				lineList.Add(line);
			}

			textStream.Close();
		}
	}

	public string GetRandomLine()
	{
		// Returns random line from list
		Debug.Log(lineList.Count);
		return lineList[Random.Range(0, lineList.Count)];
	}
}