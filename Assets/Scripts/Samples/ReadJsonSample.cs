using UnityEngine;
using System.Collections;

public class ReadJsonSample : MonoBehaviour 
{
	// file path from 'Assets/StreamingAssets'
	public string jsonPath = "Samples/sample.json"; 

	void Awake()
	{
		var json = Utilities.ReadJSON(jsonPath);

		var title = json["title"].ToString();
		var hogeFuga = json["hoge"]["fuga"].AsInt;
		var hogeMoge = json["hoge"]["moge"].AsFloat;
		var piyo = json["piyo"].AsArray;

		Debug.Log(title);     // string
		Debug.Log(hogeFuga);  // int
		Debug.Log(hogeMoge);  // float
		Debug.Log(piyo);      // SimpleJSON.JSONArray

		for (int i = 0; i < piyo.Count; ++i) {
			var val = piyo[i].AsInt;
			Debug.Log(val);   // int
		}
	}
}
