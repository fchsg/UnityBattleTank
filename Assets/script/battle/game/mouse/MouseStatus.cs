using UnityEngine;
using System.Collections;

public class MouseStatus {

	private bool[] _mouseDown = new bool[3];

	private bool[] _mouseJustDown = new bool[3];
	private bool[] _mouseJustUp = new bool[3];

	private bool _mousePosInit = false;
	private Vector3 _mousePos;
	private Vector3 _mouseLastPos;
	private Vector3[] _mouseJustDownPos = new Vector3[3];
	private Vector3[] _mouseJustUpPos = new Vector3[3];

	public enum KEY
	{
		LEFT,
		RIGHT,
		MIDDLE,
	}

	public MouseStatus()
	{
	}

	// Update is called once per frame
	public void Update () {

		for(int key = 0; key < 3; ++key)
		{
			_mouseDown[key] = Input.GetMouseButton(key);
			_mouseJustDown[key] = Input.GetMouseButtonDown(key);
			_mouseJustUp[key] = Input.GetMouseButtonUp(key);

			if(_mouseJustDown[key])
			{
				_mouseJustDownPos[key] = Input.mousePosition;
			}
			if(_mouseJustUp[key])
			{
				_mouseJustUpPos[key] = Input.mousePosition;
			}
		}

		if (!_mousePosInit) {
			_mousePos = Input.mousePosition;
			_mouseLastPos = _mousePos;
		} else {
			_mouseLastPos = _mousePos;
			_mousePos = Input.mousePosition;
		}
		_mousePosInit = true;


//		Debug.Log (_mouseJustUp[(int)KEY.LEFT]);
	}

	public bool GetMouseDown(KEY key = KEY.LEFT)
	{
		return _mouseDown[(int)key];
	}

	public bool GetMouseJustDown(KEY key = KEY.LEFT)
	{
		return _mouseJustDown[(int)key];
	}

	public bool GetMouseJustUp(KEY key = KEY.LEFT)
	{
		return _mouseJustUp[(int)key];
	}

	public Vector3 GetMouseLastPos()
	{
		return _mouseLastPos;
	}
	
	public Vector3 GetMousePos()
	{
		return _mousePos;
	}

	public Vector3 GetMouseJustDownPos(KEY key = KEY.LEFT)
	{
		return _mouseJustDownPos[(int)key];
	}
	
	public Vector3 GetMouseJustUpPos(KEY key = KEY.LEFT)
	{
		return _mouseJustUpPos[(int)key];
	}
	

	public static bool UnprojectMousePosition(out Vector3 worldPosition, Vector3 mousePosition)
	{
		GameObject mainCamera = (GameObject)GameObject.Find ("Main Camera");
		CameraControl cameraControl = mainCamera.GetComponent<CameraControl> ();
		bool result = cameraControl.ProjectScreenPointToPlane (out worldPosition, mousePosition, 0);
		return result;

	}

}
