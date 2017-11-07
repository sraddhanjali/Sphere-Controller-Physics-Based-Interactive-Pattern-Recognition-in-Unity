using UnityEngine; // vector3
using System; // string
using System.IO;
using System.Text;
using System.Collections.Generic; // list, dictionary
	
class Pattern{
	private List<int> pa = new List<int>();
	private Dictionary<int, Vector3> coordMap = new Dictionary<int, Vector3>();
	private Dictionary<int, String> timeMap = new Dictionary<int, String>();
	private string filePath = Application.persistentDataPath;
	private string fileName = "p1.txt";

	public static String GetTimestamp(DateTime value) {
		return value.ToString("yyyyMMddHHmmssffff");
	}

	public void SetPattern(int singleP){
		pa.Add (singleP);
	}

	public void SetCoordinates(int singleP, Vector3 coord){
		coordMap.Add (singleP, coord);
	}

	public void SetTimestamp(int singleP, DateTime t){
		timeMap.Add (singleP, GetTimestamp (t));
	}	

	private static void AddText(FileStream fs, string value)
	{
		byte[] info = new UTF8Encoding(true).GetBytes(value);
		fs.Write(info, 0, info.Length);
	}

	public void SaveTimeCoord(){
		string path = filePath + "/" + fileName;
		if (!File.Exists (path)) {
			foreach (KeyValuePair<int, Vector3> kvp in coordMap) {
				int num = kvp.Key;
				Vector3 v = kvp.Value;
				String v1 = v.ToString ();
				String ts = timeMap [num];
				File.WriteAllText (path, num.ToString ());
				File.AppendAllText (path, v1);
				File.AppendAllText (path, ts);
			}
		}
	}
}