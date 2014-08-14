using UnityEngine;
using System.Collections;

public class SolenoidBlock : MonoBehaviour 
{
	public  int  pullCnt = 30;
	private int  cnt_    = 0;
	private bool isEmit_ = false; 

	void Update() 
	{
		if (isEmit_) {
			if (cnt_ >= pullCnt) {
				SolenoidController.Instance.Push(false);
				isEmit_ = false;
			}
			++cnt_;
		}
	}

	void Emit()
	{
		SolenoidController.Instance.Push(true);
		isEmit_ = true;
		cnt_ = 0;
	}
}
