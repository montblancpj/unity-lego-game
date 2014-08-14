using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour 
{
	private Dictionary<string, AudioClip> sounds_ = new Dictionary<string, AudioClip>();

	void Awake() 
	{
		Load("Coin",      "Sound/coin");
		Load("Jump",      "Sound/jump");
		Load("Jump2",     "Sound/jump2");
		Load("Die",       "Sound/die");
		Load("Explosion", "Sound/explosion");
		Load("Pipe",      "Sound/pipe");
		Load("Weight",    "Sound/weight");
		Load("Block",     "Sound/block");
	}
	
	public void Load(string key, string path) 
	{
		try {
			var sound = Resources.Load(path, typeof(AudioClip)) as AudioClip;	
			sounds_.Add(key, sound);
		} catch (System.Exception) {
			Debug.LogError(path + " doesn't exist!");
		}
	}
	
	public void Play(string key)
	{
		if ( !IsExists(key) ) return;
		
		Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
		AudioSource.PlayClipAtPoint(sounds_[key], playerPosition);
	}
	
	public void Stop(string key)
	{
		if ( !IsExists(key) ) return;
		
		Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
		AudioSource.PlayClipAtPoint(sounds_[key], playerPosition);
	}
	
	bool IsExists(string key)
	{
		if (!sounds_.ContainsKey(key)) {
			Debug.LogError("The sound named " + key + " has not been loaded!");
			return false;
		}
		return true;
	}
}
