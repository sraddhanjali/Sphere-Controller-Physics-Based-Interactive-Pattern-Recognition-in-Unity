using System; // string
using UnityEngine; // vector3
using System.Collections.Generic; // list, dictionary

class Helper{
	
	public static Helper myHelperInstance;

	public bool CheckEqual(List<int> List1, List<int> List2){
		int list1C = List1.Count;
		int list2C = List2.Count;
		if (list1C == list2C){
			for(int i = 0; i < list1C; i++){
				if(List1[i] != List2[i]){
					return false;
				}
			}
			return true;
		}
		return false;
	}
}