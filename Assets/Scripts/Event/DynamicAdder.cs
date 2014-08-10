using UnityEngine;
using System.Collections;

public class DynamicAdder : MonoBehaviour {
	public GameObject AddedObject;
	public KeyCode AddKey;
	private bool isAdded_ = false;
	
	void Update () {
		if (!isAdded_ && Input.GetKeyDown(AddKey)) {
			isAdded_ = true;
			Instantiate(AddedObject, transform.position, transform.rotation);
		}
	}
}
