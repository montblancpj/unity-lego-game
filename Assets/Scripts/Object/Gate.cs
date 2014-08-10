using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	private PlayerAutoController player_;
	private int cnt_ = 0;
	public int openCnt = 60;
	public int playerMoveOffsetCount = 45;
	
	static private bool underFlag = false;
	
	void Start()
	{
		player_ = transform.parent.parent.FindChild("Player").GetComponent<PlayerAutoController>();
		if (underFlag) {
			openCnt *= 4;
		}
		underFlag = !underFlag;
	}
	
	void Update() 
	{
		if (cnt_ > openCnt) {
			float zAngle = -90.0f * ((cnt_ - openCnt) / 60.0f);
			if (zAngle < -90.0f) zAngle = -90.0f;
			transform.localRotation = Quaternion.Euler(new Vector3(0, 0, zAngle)); 
		}
		if (cnt_ > openCnt + playerMoveOffsetCount && player_) {
			player_.SendMessage("Go");	
		}
		++cnt_;
	}
}
