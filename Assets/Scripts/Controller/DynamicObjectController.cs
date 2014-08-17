using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicObjectController : MonoBehaviour
{
	[Serializable]
	public struct SpecialBlocks {
		public int x, y;
		public GameObject specialObject;
	}

	public GameObject stage;
	public Vector3 origin = Vector3.zero;
	public float scale = 1.0f;
	public GameObject effect;
	public List<string> addables;
	public List<GameObject> targetsUpperGround;
	public List<GameObject> targetsUnderGround;
	public List<SpecialBlocks> specialTargetMap;
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
		if ( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << stage.layer) ) {
				var position = new Dictionary<string, int>();
				position["x"] = (int)Mathf.Floor((1.0f - hit.textureCoord.x) * stage.transform.localScale.x);
				position["y"] = (int)Mathf.Floor((hit.textureCoord.y) * stage.transform.localScale.y);
				if ( Input.GetMouseButtonDown(1) ) {
					if (!addBlock(position, 2)) {
						deleteBlock(position);
					}
				} else {
					if (!addBlock(position)) {
						deleteBlock(position);
					}
				}
			}
		}

		/*
		if (Input.GetMouseButtonDown(0)) {
			Vector3 screenPosition = Input.mousePosition;
			screenPosition.z = 0.0f;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			var position = new Dictionary<string, int>();
			position["x"] = (int)Mathf.Floor( worldPosition.x + 0.5f - origin.x);
			position["y"] = (int)Mathf.Floor(-worldPosition.y + 0.5f + origin.y);
			if (!addBlock(position)) {
				deleteBlock(position);
			}
		}
		*/
	}
	

	GameObject getTarget(Vector3 position, int x, int y, int forceKind = -1)
	{
		foreach (var rule in specialTargetMap) {
			if (x == rule.x && y == rule.y) {
				return rule.specialObject;
			}
		}
		if (position.y >= underGroundThreashold) {
			return targetsUpperGround[forceKind != -1 ? forceKind : kind];
		} else {
			return targetsUnderGround[forceKind != -1 ? forceKind : kind];
		}
	}


	bool addBlock(Dictionary<string, int> msg, int forceKind = -1)
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
			StartCoroutine(waitThenInvoke(0.2f, () => {
				Instantiate(getTarget(position, x, y, forceKind), position, Quaternion.identity);
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
					StartCoroutine(waitThenInvoke(0.3f, () => {
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
