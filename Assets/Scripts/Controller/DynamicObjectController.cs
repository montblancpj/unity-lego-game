using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicObjectController : MonoBehaviour
{
	public Vector3 origin = Vector3.zero;
	public float scale = 1.0f;
	public GameObject effect;
	public List<string> addables;
	public List<GameObject> targetsUpperGround;
	public List<GameObject> targetsUnderGround;
	public int kind;
	public int underGroundThreashold = 0;
	
	// Sound
	private SoundController Sound;
	
	
	void Start() 
	{
		Sound = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
	}
	
	
	void Update() 
	{
		if (Input.GetMouseButtonDown(0)) {
			Vector3 screenPosition = Input.mousePosition;
			screenPosition.z = 0.0f;
			Debug.Log (screenPosition);
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			Debug.Log (worldPosition);
			var position = new Dictionary<string, int>();
			position["x"] = (int)Mathf.Floor( worldPosition.x + 0.5f - origin.x);
			position["y"] = (int)Mathf.Floor(-worldPosition.y + 0.5f + origin.y);
			if (!addBlock(position)) {
				deleteBlock(position);
			}
		}
	}
	

	GameObject getTarget(Vector3 position)
	{
		if (position.y >= underGroundThreashold) {
			return targetsUpperGround[kind];
		} else {
			return targetsUnderGround[kind];
		}
	}


	bool addBlock(Dictionary<string, int> msg)
	{
		int x = msg["x"], y = msg["y"];
		Vector3 from = origin + new Vector3(x * scale, -y * scale, -1.0f);
		Vector3 to   = Vector3.forward;

		RaycastHit hit;
		if (Physics.Raycast(from, to, out hit, 1.0f)) {
			Debug.LogWarning("An object already exists at (" + from.x + ", " + from.y + ")");
			return false;
		} else {
			Debug.Log("add block at (" + from.x + ", " + from.y + ")");
			Vector3 position = origin + new Vector3(x * scale, -y * scale, 0.0f);
			var obj = Instantiate(effect, position + Vector3.back, Quaternion.identity) as GameObject;
			StartCoroutine(waitThenInvoke(2.0f, () => {
				if (obj != null) Destroy(obj);
			}));
			StartCoroutine(waitThenInvoke(1.0f, () => {
				Instantiate(getTarget(position), position, Quaternion.identity);
			}));
			Sound.Play("Explosion");
		}
		return true;
	}


	bool deleteBlock(Dictionary<string, int> msg)
	{
		int x = msg["x"], y = msg["y"];
		Vector3 from = origin + new Vector3(x * scale, -y * scale, -1.0f);
		Vector3 to   = Vector3.forward;

		var hits = Physics.RaycastAll(from, to, 0.5f);
		bool deleted = false;
		foreach (RaycastHit hit in hits) {
			foreach (string name in addables) {
				if (hit.transform.name == name || hit.transform.name == name + "(Clone)") {
					Debug.Log("delete from (" + from.x + ", " + from.y + ")");
					var obj = Instantiate(effect, hit.transform.position + Vector3.back, Quaternion.identity) as GameObject;
					StartCoroutine(waitThenInvoke(2.0f, () => {
						Destroy(obj);
					}));
					StartCoroutine(waitThenInvoke(1.0f, () => {
						Destroy(hit.transform.gameObject);
					}));
					deleted = true;
					break;
				}
			}
		}
		if (!deleted) {
			Debug.LogWarning("No object exists at (" + from.x + ", " + from.y + ")");
			return false;
		}
		Sound.Play("Explosion");
		return true;
	}


	void changeBlock(Dictionary<string, int> msg)
	{
		kind = msg["kind"];
		Debug.Log ("Change block to " + kind);
		Sound.Play("Button");
	}


	IEnumerator waitThenInvoke(float sec, Action callback)
	{
		yield return new WaitForSeconds(sec);
		callback();
	}
}
