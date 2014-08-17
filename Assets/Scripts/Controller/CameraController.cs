using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	#region [Public class/enum]
	public enum CalibMode {
		None, Move, Rotate, Scale
	}
	#endregion

	#region [Public members]
	public Camera uiCamera;
	public GUIText modeGuiText, stateGuiText;
	public CalibMode calibMode = CalibMode.None;
	#endregion

	#region [Input]
	private bool isKeyPressing_ = false;
	private int longPressCnt_   = 0;
	private static readonly int longPressFrame_ = 30;
	#endregion

	#region [Move]
	public float deltaPosition = 0.1f;
	public float deltaRotation = 0.1f;
	public float deltaScale    = 0.1f;
	#endregion

	#region [Scale]
	public Vector3 originalCameraPosition = Vector3.zero;
	#endregion


	void Start()
	{
		originalCameraPosition = Camera.main.transform.position;
	}


	void Update()
	{
		if ( Input.GetKeyDown(KeyCode.F1) ) {
			calibMode = CalibMode.None;
		}
		if ( Input.GetKeyDown(KeyCode.F2) ) {
			calibMode = CalibMode.Move;
		}
		if ( Input.GetKeyDown(KeyCode.F3) ) {
			calibMode = CalibMode.Rotate;
		}
		if ( Input.GetKeyDown(KeyCode.F4) ) {
			calibMode = CalibMode.Scale;
		}

		if (longPressCnt_ == 0 || longPressCnt_ > longPressFrame_) {
			switch (calibMode) {
				case CalibMode.None   : None();   break;
				case CalibMode.Move   : Move();   break;
				case CalibMode.Rotate : Rotate(); break;
				case CalibMode.Scale  : Scale();  break;
			}
		}

        if (Input.anyKey) {
			isKeyPressing_ = true;
			++longPressCnt_;
		} else {
			isKeyPressing_ = false;
			longPressCnt_ = 0;
		}
	}


    void None()
    {
		SetUICamera(false);
    }


    void Move()
    {
        if ( Input.GetKey(KeyCode.LeftArrow) ) {
			Camera.main.transform.position += Vector3.right * deltaPosition;
		}
        if ( Input.GetKey(KeyCode.RightArrow) ) {
			Camera.main.transform.position -= Vector3.right * deltaPosition;
		}
        if ( Input.GetKey(KeyCode.UpArrow) ) {
			Camera.main.transform.position -= Vector3.up * deltaPosition;
		}
        if ( Input.GetKey(KeyCode.DownArrow) ) {
			Camera.main.transform.position += Vector3.up * deltaPosition;
		}

		var pos = Camera.main.transform.position - originalCameraPosition;
		SetUICamera(true, "Calib Mode: Move", pos.ToString());
    }


    void Rotate()
    {
        if ( Input.GetKey(KeyCode.LeftArrow) ) {
			Camera.main.transform.Rotate(Camera.main.transform.up, deltaRotation);
		}
        if ( Input.GetKey(KeyCode.RightArrow) ) {
			Camera.main.transform.Rotate(Camera.main.transform.up, -deltaRotation);
		}
        if ( Input.GetKey(KeyCode.UpArrow) ) {
			Camera.main.transform.Rotate(Camera.main.transform.right, deltaRotation);
		}
        if ( Input.GetKey(KeyCode.DownArrow) ) {
			Camera.main.transform.Rotate(Camera.main.transform.right, -deltaRotation);
		}

		SetUICamera(true, "Calib Mode: Rotate", Camera.main.transform.rotation.eulerAngles.ToString());
    }


    void Scale()
    {
        if ( Input.GetKey(KeyCode.UpArrow) ) {
			Camera.main.transform.position += Vector3.forward * deltaScale;
		}
        if ( Input.GetKey(KeyCode.DownArrow) ) {
			Camera.main.transform.position += Vector3.back * deltaScale;
		}

		var pos = Camera.main.transform.position - originalCameraPosition;
		SetUICamera(true, "Calib Mode: Scale", pos.ToString());
    }


	void SetUICamera(bool active, string mode = "", string state = "")
	{
		uiCamera.gameObject.SetActive(active);
		modeGuiText.text  = mode;
		stateGuiText.text = state;
	}
}
