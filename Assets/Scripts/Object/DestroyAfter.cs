using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour 
{
	public int destroyCnt = 0;
	private int cnt_ = 0;

	void Update() 
	{
		if (cnt_ >= destroyCnt) {
			Destroy(gameObject);
		}
		++cnt_;
	}
}
