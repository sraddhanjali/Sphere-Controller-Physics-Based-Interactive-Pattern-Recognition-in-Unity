using UnityEngine; // vector3
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary
	
class Pattern{
	public List<int> pa = new List<int>();
	public Dictionary<int, Vector3> coordMap = new Dictionary<int, Vector3>();
	public Dictionary<int, String> timeMap = new Dictionary<int, String>();

	public static String GetTimestamp(DateTime value) {
		return value.ToString("yyyyMMddHHmmssffff");
	}

	public void SetPattern(int singleP){
		pa.Add (singleP);
	}

	public void SetCoordinates(int singleP, Vector3 coord){
		Debug.Log (coord.x.ToString ());
		coordMap.Add (singleP, coord);
	}

	public void SetTimestamp(int singleP, DateTime t){
		timeMap.Add (singleP, GetTimestamp (t));
	}	

	public List<int> GetPattern(){
		return pa;
	}

	public Dictionary<int, Vector3> GetCoordinates(){
		return coordMap;
	}

	public Dictionary<int, String> GetTimestamp(){
		return timeMap;
	}
}