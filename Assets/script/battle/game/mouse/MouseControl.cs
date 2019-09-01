using UnityEngine;
using System.Collections;

public class MouseControl {

	private BattleGame _game;
	private MouseStatus _mouseStatus;
	
	private long _touchBeganTime;
	private long _touchEndTime;
	private Vector3 _touchBeganPos;
	private Vector3 _touchEndPos;

	private bool _isFirstTouch = false;
	
	public enum STATE
	{
		IDLE,
		HOLDING, //just mouse key down, but not moved
		DRAGGING,
		EASING,
	}
	private STATE _state = STATE.IDLE;
	public STATE state
	{
		get { return _state; }
	}

	public MouseControl(BattleGame game)
	{
		_game = game;
		_mouseStatus = game.mouseStatus;
	}


	public void Update()
	{
		InitManaulCamera ();

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

	private void InitManaulCamera()
	{
		if (_game.state == BattleGame.STATE.BATTLE &&
		    _mouseStatus.GetMouseJustDown (MouseStatus.KEY.LEFT))
		{
			if(!_isFirstTouch)
			{
				_isFirstTouch = true;
				
				_game.mapCamera.cManualMove.cameraCenter = _game.mapCamera.cameraCenter;
				_game.mapCamera.SetState (MapCamera.STATE.MANUAL_MOVE);
			}
		}
	}
	
	private void UpdateIdle()
	{
		if (_game.state != BattleGame.STATE.BATTLE) {
			return;
		}
			
		if(_mouseStatus.GetMouseJustDown(MouseStatus.KEY.LEFT))
		{
			_touchBeganTime = TimeHelper.GetCurrentRealTimestamp();
			_touchBeganPos = _mouseStatus.GetMousePos ();
			
			_state = STATE.HOLDING;
		}
	}
	
	private void UpdateHolding()
	{
		_game.mapCamera.cManualMove.ResetMove();
		
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
		Vector3 dragDistance = new Vector3 ((_mouseStatus.GetMousePos () - _mouseStatus.GetMouseLastPos ()).x, 0, 0);
		_game.mapCamera.cManualMove.UpdateMovePos (dragDistance);

		if(_mouseStatus.GetMouseJustUp(MouseStatus.KEY.LEFT))
		{
			_touchEndTime = TimeHelper.GetCurrentRealTimestamp();
			_touchEndPos = _mouseStatus.GetMousePos();

			float dragTime = (float)(_touchEndTime - _touchBeganTime) / 1000.0f;
			if (dragTime < 0.5f) 
			{
				 dragDistance = new Vector3((_touchBeganPos - _touchEndPos).x, 0, 0);
				_game.mapCamera.cManualMove.UpdateEasingPos(dragTime, dragDistance);
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
			return;
		}
	}
	
	private void  EasingFinish(System.Object parameter)
	{
		if (_state == STATE.EASING) 
		{
			_state = STATE.IDLE;
		}
	}

}
