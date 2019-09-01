using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGameEntering {

	public enum STATE
	{
		IDLE,
		MOVE_TO_ENEMY,
		LOOK_AT_ENEMY,
		MOVE_TO_PLAYER,
		LOOK_AT_PLAYER,
		OVER,
	}
	private STATE _state = STATE.IDLE;
	public STATE state
	{
		get { return _state; }
	}

	private BattleGame _game;

	private float _lookAtLeftTime;
	private const float LOOK_AT_DURATION_ENEMY = 1;
	private const float LOOK_AT_DURATION_PLAYER = 3;

	public BattleGameEntering(BattleGame game)
	{
		_game = game;
	}

	public void Update()
	{
		switch (_state) {
		case STATE.MOVE_TO_ENEMY:
			UpdateMoveToEnemy();
			break;
		case STATE.LOOK_AT_ENEMY:
			UpdateLookAtEnemy();
			break;
		case STATE.MOVE_TO_PLAYER:
			UpdateMoveToPlayer();
			break;
		case STATE.LOOK_AT_PLAYER:
			UpdateLookAtPlayer();
			break;
		}
	}

	public void Start()
	{
		Vector3 lookAtPoint = new Vector3 (
			_game.mapGrid.GetMapWidth(),
			0,
			_game.mapGrid.GetMapHeight() / 2);
		_game.mapCamera.cLookAt.Init (lookAtPoint, lookAtPoint, 2);
		_game.mapCamera.SetState (MapCamera.STATE.LOOK_AT);

		_state = STATE.MOVE_TO_ENEMY;
	}
	
	private void UpdateMoveToEnemy()
	{
		if (_game.mapCamera.cLookAt.isArrive) {
			/*
			List<Unit> units = _game.unitGroup.enemyUnits;
			foreach (Unit unit in units) {
				unit.StartEntering();
			}
			*/

			_lookAtLeftTime = LOOK_AT_DURATION_ENEMY;
			_state = STATE.LOOK_AT_ENEMY;
		}

	}

	private void UpdateLookAtEnemy()
	{
		_lookAtLeftTime -= TimeHelper.deltaTime;
		if (_lookAtLeftTime <= 0) {
			Vector3 lookAtPoint = new Vector3 (
				0,
				0,
				_game.mapGrid.GetMapHeight() / 2);
			_game.mapCamera.cLookAt.Init (lookAtPoint, 2);
			_game.mapCamera.SetState (MapCamera.STATE.LOOK_AT);
			
			_state = STATE.MOVE_TO_PLAYER;

		}

	}
	
	private void UpdateMoveToPlayer()
	{
		if (_game.mapCamera.cLookAt.isArrive) {
			/*
			List<Unit> units = _game.unitGroup.myUnits;
			foreach (Unit unit in units) {
				unit.StartEntering();
			}
			*/

			List<Unit> units = _game.unitGroup.allUnits;
			foreach (Unit unit in units) {
				unit.StartEntering();
			}

			_lookAtLeftTime = LOOK_AT_DURATION_PLAYER;
			_state = STATE.LOOK_AT_PLAYER;
		}

	}
	
	private void UpdateLookAtPlayer()
	{
		_lookAtLeftTime -= TimeHelper.deltaTime;
		if (_lookAtLeftTime <= 0) {

			_state = STATE.OVER;
			
		}


	}
	
}
