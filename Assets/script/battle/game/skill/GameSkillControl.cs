using UnityEngine;
using System.Collections;

public class GameSkillControl {

	private BattleGame _game;
	private MouseControl _mouseControl;
	private MouseStatus _mouseStatus;

	private Vector3 _worldPosition;
	private InstanceSkill _firingSkill = null;

	private GameObject _firingSkillObject = null;
	
	private GameObject _animtaionObject = null;
	private GameObject _targetAreaObject = null;
	
	public enum STATE
	{
		IDLE,
		DETECT,
		ANIMATION,
		WORKING,
	}
	private STATE _state = STATE.IDLE;
	public STATE state
	{
		get { return _state; }
	}


	private const float DROP_DELAY = 0.5f;


	public GameSkillControl(BattleGame game)
	{
		this._game = game;
		_mouseControl = game.mouseContorl;
		_mouseStatus = game.mouseStatus;
	}

	public void Update()
	{
		switch (_state) {
		case STATE.IDLE:
			UpdateIdle();
			break;
		case STATE.DETECT:
			UpdateDetect();
			break;
		case STATE.ANIMATION:
			UpdateAnimation();
			break;
		case STATE.WORKING:
			UpdateWorking();
			break;
		}
	}
	
	private void UpdateIdle()
	{
		_firingSkill = null;

		if (_game.state != BattleGame.STATE.BATTLE) {
			return;
		}

		if (!_game.gameSkill.IsSelecting ()) {
			return;
		}
		
		if (_mouseStatus.GetMouseJustDown () && 
		    !UICamera.Raycast (Input.mousePosition)) 
		{
			_state = STATE.DETECT;

			_firingSkill = _game.gameSkill.GetCurrentSelectSkill();
			return;
		}

	}
	
	private void UpdateDetect()
	{
		if (!_game.gameSkill.IsSelecting ()) 
		{
			_state = STATE.IDLE;
			return;
		}
		
		if (_mouseControl.state == MouseControl.STATE.DRAGGING || 
		    _mouseControl.state == MouseControl.STATE.EASING)
		{
			_state = STATE.IDLE;
			return;
		}
		
		if (_mouseStatus.GetMouseJustUp ()) {
			Vector3 mousePosition = _game.mouseStatus.GetMouseJustUpPos();
			bool cast = MouseStatus.UnprojectMousePosition(out _worldPosition, mousePosition);
			if(cast)
			{
				CreateAnimation(mousePosition);

				//TODO create target area
				CreateTargetArea();
				_game.gameSkill.Use();
				_state = STATE.ANIMATION;
				return;

			}
		}
		

	}

	private void CreateAnimation(Vector3 mousePosition)
	{
		_animtaionObject = ResourceHelper.Load(AppConfig.FOLDER_PROFAB + "skill/aircraft");
		_animtaionObject.GetComponent<SkillAnimation>().Init(_worldPosition, mousePosition);

	}

	private void CreateTargetArea()
	{
		_targetAreaObject = ResourceHelper.Load(AppConfig.FOLDER_PROFAB + "skill/TargetArea");

//		InstanceSkill skill = _game.gameSkill.GetCurrentSelectSkill();
		DataSkill dataSkill = _firingSkill.GetDataSkill ();

		float scale = dataSkill.hintRange * 2.0f;
		_targetAreaObject.transform.localScale = new Vector3 (scale, 1.0f, scale);

		_targetAreaObject.transform.position = _worldPosition;
	}
	
	private void UpdateAnimation()
	{
	
		SkillAnimation skillAnimation = _animtaionObject.GetComponent<SkillAnimation>();
		if (skillAnimation != null) {
			if(skillAnimation.flyOver)
			{
				_game.StartCoroutine(CreateBomb());
				
				_state = STATE.WORKING;
			}
		}
	}


	private IEnumerator CreateBomb()
	{
		yield return new WaitForSeconds(DROP_DELAY);


		string profabName = "";

//		InstanceSkill skill = _game.gameSkill.GetCurrentSelectSkill();
		switch (_firingSkill.type) {
		case DataSkill.TYPE.BOMB:
			profabName = AppConfig.FOLDER_PROFAB + "skill/Bomb";
			break;

		case DataSkill.TYPE.AIR_STRIKE:
			profabName = AppConfig.FOLDER_PROFAB + "skill/BombAirStrikeSpawn";
			break;

		case DataSkill.TYPE.HEAL:
			profabName = AppConfig.FOLDER_PROFAB + "skill/BombHealer";
			break;

		}

		_firingSkillObject = ResourceHelper.Load(profabName);
		_firingSkillObject.GetComponent<SkillBombBase>().Init(_firingSkill, _worldPosition);

	}
	
	private void UpdateWorking()
	{
		bool destroy = false;

		if (_firingSkillObject != null) {
			SkillBombBase skillBomb = _firingSkillObject.GetComponent<SkillBombBase>();
			if (skillBomb != null) {
				if(skillBomb.isOver)
				{
					destroy = true;
				}
			}
		}

		if (destroy) {
			MonoBehaviour.DestroyImmediate(_firingSkillObject);
			_firingSkillObject = null;


			MonoBehaviour.DestroyImmediate(_targetAreaObject);
			_targetAreaObject = null;
			
			_state = STATE.IDLE;
			return;
		}

	}

}
