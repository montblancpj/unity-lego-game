using UnityEngine;
using System.Collections;

public class BeltConveyor : MonoBehaviour {

	public GameObject belt;
	public int createInterval = 120;
	private int cnt_ = 0;
	
	void Update () {
		if (cnt_ % createInterval == 0) {
			var obj = Instantiate(belt) as GameObject;
			obj.transform.parent = transform;
		}
		++cnt_;
	}
}
