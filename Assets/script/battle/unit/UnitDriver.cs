using UnityEngine;
using System.Collections;

public class UnitDriver {

	private Unit _unit;
	private BattleGame _game;
	private ShortPath _shortPath;
	
	private GameObject _owner;
	private GameObject _body;
	
	private Vector3 _targetPosition;
	
	private bool _isArrived = true;
	public bool isArrived
	{
		get { return _isArrived; }
	}
	
	
	private float _speed;
	public const float ROTATION_SPEED = 15;
	
	private TankEngine _engine;
	public TankEngine engine
	{
		get { return _engine; }
	}

	private long _nextResearchTimestamp = 0;
	private const int RESEARCH_INTERVAL_SEC = 0;
	
	private AimmingControl _turningControl;
	public AimmingControl turningControl
	{
		get { return _turningControl; }
	}


//	private TankSpineAttach _spineAttach;

	
	public UnitDriver (GameObject owner, GameObject body, float speed) {
		_unit = owner.GetComponent<Unit> ();
		_game = BattleGame.instance;
		_shortPath = new ShortPath (_unit, _game.mapGrid);
		
		this._owner = owner;
		this._body = body;

		_engine = new TankEngine (_unit);
		_turningControl = new AimmingControl(body, ROTATION_SPEED);
		
		this._speed = speed;

//		_spineAttach = owner.GetComponent<TankSpineAttach> ();
	}
	
	public void Stop(bool immediately)
	{
		_engine.Stop (immediately);
	}
	
	public void Resume(bool immediately)
	{
		_engine.Resume (immediately);
	}
	
	public float GetEstimateBreakingTime()
	{
		return _engine.speedScale * _unit.unit.dataUnit.breakTime; 
	}
	
	// Update is called once per frame
	public void Update () {

		_engine.Update ();
		if (_engine.IsStopped ()) {
			if(!_unit.unitFire.isAttacking)
			{
//				_spineAttach.ChangeAnimation(SpineAttach.ANIMATION.STAND, true);
			}

			return;
		} else {
			if(!_unit.unitFire.isAttacking)
			{
//				_spineAttach.ChangeAnimation(SpineAttach.ANIMATION.RUN, true);
			}

		}
		
		if (!_isArrived) {
			/*
			if(!_pathFinding.hasPath)
			{
				FindPathToTargetPosition();
			}
			else
			{
				COORD ct = MapGrid.GetTileCoord(_targetPosition.x, _targetPosition.z);
				COORD ce = _pathFinding.path.end;
				if(!ct.IsEqual(ce))
				{
					FindPathToTargetPosition();
				}
			}
			*/
			
			if(_unit.targetSelect.IsTargetInCloseRange(_targetPosition))
			{
				_shortPath.SetPath(null);
				MoveDirectly();
			}
			else
			{
				if (!_shortPath.hasPath) {
					FindPathToTargetPosition ();
				} else {
					if (TimeHelper.GetCurrentTimestampScaled () >= _nextResearchTimestamp) {
						FindPathToTargetPosition ();
					}
				}
				
				if (_shortPath.hasPath) {
					MoveWithPath ();
				}
				else
				{
					MoveDirectly();
				}
			}
		} else {
			MoveForward();
		}
		
		/*
		_pathFinding.Update ();

		if (!_isArrived) {
			if(_pathFinding.hasPath)
			{
				MoveWithPath();
			}
			else
			{
				MoveDirectly();
			}
		}
		*/
		
	}
	
	private void MoveDirectly()
	{
		UpdateTurning (_targetPosition);
		
		MovingControl.RESULT r = Move(_targetPosition, true);
		
	}
	
	private void MoveWithPath()
	{
		UpdateTurning (_shortPath.targetTileCenter);
		
		MovingControl.RESULT r = Move(_shortPath.targetTileCenter, true);
		if (r.blocked) {
			FindPathToTargetPosition();
		}
		
	}
	
	private void MoveForward()
	{
		Vector3 orientation = UnitHelper.GetOrientation (_body.transform);
		VectorHelper.ResizeVector (ref orientation, 10);
		
		Vector3 targetPosition = _owner.transform.position + orientation;
		MovingControl.RESULT r = Move(targetPosition, true);
		
	}
	
	private void FindPathToTargetPosition()
	{
		_nextResearchTimestamp = TimeHelper.GetCurrentTimestampScaled() + RESEARCH_INTERVAL_SEC * 1000;
		
		COORD ts = MapGrid.GetTileCoord (_owner.transform.position.x, _owner.transform.position.z);
		COORD te = MapGrid.GetTileCoord (_targetPosition.x, _targetPosition.z);
		MapAStar.PATH path = _game.mapAStar.Calc(ts.x, ts.z, te.x, te.z);
		_shortPath.SetPath(path);
	}
	
	private void UpdateTurning(Vector3 targetPosition)
	{
		if (!_unit.unit.dataUnit.canStandTurn) {
			_turningControl.speedScale = _engine.speedScale;
		} else {
			_turningControl.speedScale = 1;
		}
		_turningControl.TrunToTarget (targetPosition);
		_turningControl.Update();
	}
	
	public void SetTargetPosition(Vector3 targetPosition)
	{
		this._targetPosition = targetPosition;
		_isArrived = false;

		if (!_unit.unitFire.isAttacking) {
			_engine.Resume(false);
		}
	}
	
	public void ClearTargetPosition()
	{
		_isArrived = true;

		_engine.Stop(false);

	}
	
	// ==================================================================
	// algorithm
	
	private MovingControl.RESULT Move(Vector3 targetPosition, bool testCollision)
	{
		float moveDist;
		float dist;
		float speed = _speed * _engine.speedScale;
		Vector3 orientation = UnitHelper.GetOrientation (_body.transform);
		Vector3 destination = MovingControl.CalcDestinationWithOrientation (_owner.transform.position, targetPosition, speed, orientation,
		                                                      out moveDist, out dist);
		
		if (testCollision) {
			UnitCollisionDetector.RESULT cr =
				_unit.collisionDetector.Detect_withOrientationTest(destination);
			if(cr != null)
			{
				MovingControl.RESULT r = new MovingControl.RESULT();
				r.blocked = true;
				r.arrived = false;
				return r;
			}
		}
		
		bool isBlocked = IsDestinationBlocked (destination);
		if (isBlocked) {
			MovingControl.RESULT r = new MovingControl.RESULT();
			r.blocked = true;
			r.arrived = false;
			return r;
		} else {
			_owner.transform.position = destination;
			
			MovingControl.RESULT r = new MovingControl.RESULT ();
			r.distance = moveDist;
			r.distanceLeft = dist - moveDist;
			r.destination = destination;
			r.arrived = (r.distanceLeft <= 0.01f);
			return r;
		}
		
		
	}
	
	private bool IsDestinationBlocked(Vector3 destination)
	{
		//should already remove self grid block, so don't need to check whether grid is change
		//that means if unit alread inside a grid is blocked, then it is blocked by other unit
		
		//		COORD c1 = MapGrid.GetTileCoord (_owner.transform.position.x, _owner.transform.position.z);
		COORD c2 = MapGrid.GetTileCoord (destination.x, destination.z);
		
		//		if(c1.IsEqual(c2))
		//		{
		//			return false;
		//		}
		
		if (_game.mapGrid.IsBlock (c2.x, c2.z)) {
			return true;
		} else {
			return false;
		}
		
	}

}
