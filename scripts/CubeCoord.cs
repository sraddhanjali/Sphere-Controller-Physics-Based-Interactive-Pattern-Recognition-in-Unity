using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* to map coordinates to actual pixel coordinates on the screen */
public class CubeCoord{
	int xoffset = 16;
	int yoffset = 24;
	int cubeheight = 168;
	int cubewidth = 145;
	int row = 3;
	int column = 6;

	int x1 = 305; // left x 
	int x2 = 450; // right x
	int y1 = 684; // up y
	int y2 = 852; // down y

	Dictionary<int, List<List<int>>> cubeCoMap = new Dictionary<int, List<List<int>>>();

	void ComputeOtherCubeCoord(){
		for (int i = 1; i < row + 1; i++) {
			for(int j = 1; j < column + 1; j++){
				cubeCoMap.Add (i, new List<List<int>> {
					new List<int>{ x1 + (cubewidth + xoffset)*(i-1), y1}, // top left
					new List<int>{ x2 + (cubewidth + xoffset)*(i-1) , y1 }, // top right
					new List<int>{ x1, y2 + (cubeheight + yoffset)*(i-1)}, // bottom left
					new List<int>{ x2, y2 + (cubeheight + yoffset)*(i-1)} // bottom right
				});
			}
		}
	}
}