using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : SceneBase{

	public enum STATE
	{
		IDLE,
		HOLDING, //just mouse key down, but not moved
		DRAGGING,
		EASING,
	}

	private GameObject touch_Panel;
		
	// data-----------------------------------

	private float limit_length;

	private MouseStatus _mouseStatus;

	private	STATE _state;

	private long _touchBeganTime;
	private long _touchEndTime;
	private Vector3 _touchBeganPos;
	private Vector3 _touchEndPos;

	private float _easingTime;
	private float _easingSpeed;


	override public void Init()
	{
		base.Init ();

		float length_sum = UIHelper.FindChildInObject (this.gameObject, "mian_Bg_sp").GetComponent<UIWidget>().localSize.x;
		float center_sum = UIHelper.FindChildInObject (this.gameObject, "MainUIContainer").GetComponent<UIWidget>().localSize.x;
		limit_length = (length_sum - center_sum) / 2.0f;

		_mouseStatus = new MouseStatus ();
		_state = STATE.IDLE;

		touch_Panel = UIHelper.FindChildInObject (this.gameObject, "Touch_Panel");
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
	}
	
	void Update()
	{
		_mouseStatus.Update ();

		switch(_state)
		{
		case STATE.IDLE:
			UpdateIdle();
			break;
		case STATE.HOLDING:
			UpdateHolding();
			break;
		case STATE.DRAGGING:
			UpdateDragging();
			break;
		case STATE.EASING:
			UpdateEasing();
			break;
		}
	}

	public bool IsTouchBgLayer(GameObject target)
	{
		bool bTouch = false;

		Ray ray= UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		bool isHit = Physics.Raycast (ray, out hit);
		if (isHit) 
		{
			if (hit.collider.gameObject.name == target.name) 
			{
				bTouch = true;
			}
		}
		return bTouch;
	}

	private void UpdateIdle()
	{
		if(_mouseStatus.GetMouseJustDown(MouseStatus.KEY.LEFT)
			&& IsTouchBgLayer(touch_Panel)) 
		{
			_touchBeganTime = TimeHelper.GetCurrentRealTimestamp();
			_touchBeganPos = _mouseStatus.GetMousePos ();

			_state = STATE.HOLDING;
		}
	}

	private void UpdateHolding()
	{
		Vector3 deltaTotalMove = _mouseStatus.GetMousePos () - _touchBeganPos;
		if (Mathf.Abs (deltaTotalMove.x) > 5.0f)
		{
			_state = STATE.DRAGGING;
			return;
		}

		if(_mouseStatus.GetMouseJustUp(MouseStatus.KEY.LEFT))
		{
			_touchEndTime = TimeHelper.GetCurrentRealTimestamp();
			_touchEndPos = _mouseStatus.GetMousePos();

			_state = STATE.IDLE;
			return;
		}
	}

	private void UpdateDragging()
	{
		float dragDistance = (_mouseStatus.GetMousePos () - _mouseStatus.GetMouseLastPos ()).x;
		MovePanel (dragDistance);

		if(_mouseStatus.GetMouseJustUp(MouseStatus.KEY.LEFT))
		{
			_touchEndTime = TimeHelper.GetCurrentRealTimestamp();
			_touchEndPos = _mouseStatus.GetMousePos();

			float dragTime = (float)(_touchEndTime - _touchBeganTime) / 1000.0f;
			if (dragTime < 0.5f) 
			{
				CreateEasingData (dragTime);
				TimerEx.Init ("MouseControl", dragTime, EasingFinish);

				_state = STATE.EASING;
			}
			else
			{
				_state = STATE.IDLE;
			}
			return;
		}
	}

	private void UpdateEasing()
	{
		if(_mouseStatus.GetMouseJustDown(MouseStatus.KEY.LEFT))
		{
			_touchBeganTime = TimeHelper.GetCurrentRealTimestamp();
			_touchBeganPos = _mouseStatus.GetMousePos ();

			_state = STATE.HOLDING;

			_easingTime = 0;

			return;
		}

		if (_easingTime >= 0) 
		{
			_easingTime -= TimeHelper.unscaledDeltaTime;
			MovePanel (_easingSpeed);
		}
	}

	private void CreateEasingData(float dragTime)
	{
		float dragDistance = (_touchEndPos - _touchBeganPos).x * 2.0f;

		_easingSpeed = dragDistance / limit_length /  dragTime;
		dragTime = 0.5f - dragTime;
		_easingTime = dragTime;
	}
		
	private void  EasingFinish(System.Object parameter)
	{
		if (_state == STATE.EASING) 
		{
			_state = STATE.IDLE;
		}
	}

	private void MovePanel(float length)
	{
		Vector3 pos = transform.localPosition;
		float pos_x = Mathf.Clamp (pos.x + length, -limit_length, limit_length);
		transform.localPosition = new Vector3 (pos_x, pos.y, pos.z);
	}

	private void MovePanelEasing(float length)
	{
		Vector3 pos = transform.localPosition;
		float pos_x = Mathf.Clamp (pos.x + length, -limit_length, limit_length);

		transform.localPosition = new Vector3 (pos_x, pos.y, pos.z);
	}

	override public void Delete()
	{

		base.Delete ();
	}




}
