using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

//	public delegate void DelegateAttackComplete ();

	private BattleGame _game;

	private int _slotIndex;
	public int slotIndex
	{
		get { return _slotIndex; }
	}

	public GameObject launcher;
	public GameObject body;


	private InstanceUnit _unit;
	public InstanceUnit unit
	{
		get { return _unit; }
	}

	private DataConfig.TEAM _team;
	public DataConfig.TEAM team
	{
		get { return _team; }
	}
	

	private AimmingControl _aimmingControl;
	public AimmingControl aimmingControl
	{
		get { return _aimmingControl; }
	}


	private StateMachine _stateMachine = new StateMachine ();
	public StateMachine stateMachine
	{
		get { return _stateMachine; }
	}

	public bool isDead
	{
		get {
			return (_stateMachine.currentState is UnitStateDead);
		}
	}


	private UnitTargetSelect _targetSelect;
	public UnitTargetSelect targetSelect
	{
		get { return _targetSelect; }
	}

	private UnitGridCorrect _gridCorrect;
	public UnitGridCorrect gridCorrect
	{
		get { return _gridCorrect; }
	}

	private UnitFire _unitFire;
	public UnitFire unitFire
	{
		get { return _unitFire; }
	}

	private UnitDriver _unitDriver;
	public UnitDriver unitDriver
	{
		get { return _unitDriver; }
	}
	
	private UnitCollisionDetector _collisionDetector;
	public UnitCollisionDetector collisionDetector
	{
		get { return _collisionDetector; }
	}


	private UnitTrack _unitTrack;

	private const float AIMMING_ROTATE_SPEED = 90;


	// Use this for initialization
	void Start () {
		_game = BattleGame.instance;

		_aimmingControl = new AimmingControl (launcher, AIMMING_ROTATE_SPEED);
		
		_gridCorrect = new UnitGridCorrect (this, _game.mapGrid);
		_targetSelect = new UnitTargetSelect (this, _game.unitGroup);
		_unitFire = new UnitFire (this);
		_unitDriver = new UnitDriver (gameObject, body, unit.dataUnit.speed);
		_collisionDetector = new UnitCollisionDetector (this, _game.unitGroup);

		_stateMachine.Change (new UnitStateIdle (this));

		CreateHPBar ();

		_unitTrack = new UnitTrack (this);

	}

	private void CreateHPBar()
	{
		GameObject bar = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Tank_blood");
		bar.GetComponent<HPBar> ().Init (this);
		bar.transform.parent = transform;
		bar.transform.localPosition = new Vector3 (0, 5, 0);

//		if (team == DataConfig.TEAM.ENEMY) {
//			bar.transform.localScale = new Vector3(-1, 1, 1);
//		}

	}
	
	// Update is called once per frame
	void Update () {

		UpdateStateMachine ();

		_unitTrack.Update ();
	}

	public void Init(InstanceUnit unit, DataConfig.TEAM team, Vector3 fightPos, int slotIndex)
	{
		this._unit = unit;
		this._team = team;

		gameObject.transform.position = fightPos;

		_slotIndex = slotIndex;
		
		if (team == DataConfig.TEAM.ENEMY) {
			RenderHelper.ChangeWholeModelColor (gameObject, Color.blue);
		}

		TankSpineAttach spine = GetComponent<TankSpineAttach> ();
		if (spine != null) {
			spine.Attach(gameObject, unit.dataUnit.asset);
		}

	}



	private void UpdateStateMachine()
	{
		int result = _stateMachine.Tick ();

		IState state = _stateMachine.currentState;
		if (state is UnitStateIdle) {
		}
		else if (state is UnitStateDead) {
		}
		else if (state is UnitStateFighting) {
		}
		else if (state is UnitStateEntering) {
			if(result == (int)UnitStateEntering.RESULT.DONE)
			{
			}
		}
		else if (state is UnitStateFire) {
			if(result == (int)UnitStateFire.RESULT.DONE)
			{
				_stateMachine.Change(new UnitStateFighting(this));
			}
		}

		if (unit.dataUnit.bodyType == DataConfig.BODY_TYPE.CAR_WITH_CANNON) {
			if(_unitDriver.engine.IsStopped())
			{
				float speed = AIMMING_ROTATE_SPEED;
				AimmingControl.RESULT r = AimmingControl.Rotate (
					body.transform.rotation.eulerAngles.y,
					launcher.transform.rotation.eulerAngles.y,
					speed,
					0.01f);
				body.transform.Rotate(new Vector3(0, r.turnDegree, 0));
			}
			else
			{
				launcher.transform.rotation = body.transform.rotation;
			}
		}

	}


	public void BeHurt(UnitFire.ShootParam shootParam)
	{
		if (!isDead) {
			BattleFactory.CreateHUDDamageText (shootParam);

			unit.currentHp = Mathf.Max (unit.currentHp - shootParam.damage, 0);

			if (unit.currentHp <= 0) {
				ToDead ();
			}
			
			_targetSelect.BeHurt (shootParam);
		}
	}

	public void BeHeal(float hp)
	{
		if (!isDead) {
			BattleFactory.CreateHUDHealText(gameObject, hp);

			unit.currentHp = Mathf.Min (unit.currentHp + hp, unit.complexBattleParam.hp);
		}
	}

	public void StartEntering()
	{
		Assert.assert (!isDead);
		_stateMachine.Change (new UnitStateEntering (this));
	}

	public void StartFight()
	{
		Assert.assert (!isDead);
		_stateMachine.Change (new UnitStateFighting (this));
	}
	
	public void WinAndExit()
	{
		Assert.assert (!isDead);
		_stateMachine.Change (new UnitStateWin (this));
	}
	
	public void ToDead()
	{
		Assert.assert (!isDead);
		_stateMachine.Change (new UnitStateDead (this));


	}

	public void ToFire()
	{
		Assert.assert (!isDead);
		stateMachine.Change (new UnitStateFire (this));
	}


	public bool GetFireMousePosition(out Vector3 position)
	{
		TankSpineAttach spineAttach = GetComponent<TankSpineAttach> ();
		Spine.Bone boneMouse = spineAttach.GetBoneMouse ();
		if (boneMouse != null) {
			Vector3 bPoint = new Vector3 (boneMouse.WorldX, boneMouse.WorldY, 0);
			Vector3 wPoint = spineAttach.spine.transform.TransformPoint(bPoint);

//			float height = 2.4f * unit.dataUnit.length / 10;
//			position = GeometryHelper.ProjectPointToPlane(wPoint, height, _game.mapCamera.cameraControl.orientation);

			position = wPoint;
			return true;
		}

		position = transform.position;
		return false;

	}


}
