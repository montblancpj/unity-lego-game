using UnityEngine;
using System.Collections;
using System.IO;

public static class Utilities 
{
	public static SimpleJSON.JSONNode ReadJSON(string path)
	{
		var fullPath = Path.Combine(Application.streamingAssetsPath, path);
		var fileInfo = new FileInfo(fullPath);
		using ( var stream = new StreamReader(fileInfo.OpenRead(), System.Text.Encoding.UTF8) ) {
			try {
				var jsonStr = stream.ReadToEnd();
				var json = SimpleJSON.JSON.Parse(jsonStr);
				return json;
			} catch (System.Exception e) {
				Debug.LogError(e.StackTrace);
			}
		}
		return null;
	}
}
