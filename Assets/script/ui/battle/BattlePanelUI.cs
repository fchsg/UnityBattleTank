using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlePanelUI : PanelBase {

	private UISlider playerHP;
	private UISlider enemyHp;
	private UILabel tiemLable;
	private UIButton speedBtn;
	private UILabel speedLabel;
	private UIButton skipBtn;
	private UIGrid skillGrid;

	private Transform _player;
	private UILabel _playerName;
	private UILabel _playerLevel;
	private UISprite _playerSprite;

	private Transform _enemy;
	private UILabel _enemyName;
	private UILabel _enemyLevel;
	private UISprite _enemySprite;

	private List<SkillItem> _skillItemList = new List<SkillItem>();
	protected string[] bgName = {"X 1.0","X 2.0","X 3.0"};
	private static int _bgNameNum = 1;
	public float timeScaleFactor = 1.0f;
	public int seconds  = 0;
	public int minutes = 0;
	public float lastTime = 0.0f;
	public float timetemp = 0.0f;
	public float currentTime = 0.0f;

	private GameObject[] _top_ui_container = new GameObject[3];
	private bool _isBottomAnimation = false;
	GameSkill _gameskill ;
	
	override public void Init()
	{
		base.Init ();
		InitUI ();
		_gameskill = BattleGame.instance.gameSkill;
		if(InstancePlayer.instance.pvpUser == null)
		{
			skillGrid.gameObject.SetActive(true);
			InitSkillItem (_gameskill.GetSkillCount());
		}
		else
		{
			skillGrid.gameObject.SetActive(false);

		}

	}

	private void InitUI()
	{
		_player = transform.Find("top_animation_follow/top_left_Container");
		_playerName = _player.Find("playerName_bg/playerName_Label").GetComponent<UILabel>();
		_playerLevel = _player.Find("Sprite/playerLevel_Label").GetComponent<UILabel>();
		_enemy = transform.Find("top_animation_follow/top_right_Container");
		_enemyName = _enemy.Find("enemyName_bg/enemyName_Label").GetComponent<UILabel>();
		_enemyLevel = _enemy.Find("Sprite/enemyLevel_Label").GetComponent<UILabel>();

		playerHP = GameObject.Find("Plyer_HP_Progress Bar").GetComponent<UISlider>();
		enemyHp  = GameObject.Find("enemy_HP_Progress Bar").GetComponent<UISlider>();
		tiemLable = GameObject.Find ("time_Label").GetComponent<UILabel> ();
		speedBtn = GameObject.Find ("Btn_speed").GetComponent<UIButton> (); 
		skipBtn  = GameObject.Find ("Btn_skip").GetComponent<UIButton> ();
		speedLabel = GameObject.Find ("Btn_speed_Label").GetComponent<UILabel> ();
		
		EventDelegate spBtn = new EventDelegate (OnSpeedBtnClick);
		speedBtn.onClick.Add(spBtn);
		EventDelegate skbtn = new EventDelegate (OnSkipBtnClick);
		skipBtn.onClick.Add(skbtn);
		skillGrid = transform.Find ("bottom_center_Container/SkillGrid").GetComponent<UIGrid> ();

	}

	override public void Open(params System.Object[] parameters)
	{
		base.Open (parameters);
		InitFightTimer ();
	}

	IEnumerator PlayBottomAnimation()
	{
		float scaleTime = 0.2f;

//		PlayItemPopupAnimation (speedBtn.gameObject, scaleTime);
//		yield return new WaitForSeconds (scaleTime);
//
//		PlayItemPopupAnimation (skipBtn.gameObject, scaleTime);

		for(int i = 0; i < _skillItemList.Count;i++)
		{
			yield return new WaitForSeconds (scaleTime);
			PlayItemPopupAnimation (_skillItemList [i].gameObject, scaleTime);
		}
	}

	private void PlayItemPopupAnimation(GameObject obj, float time)
	{
		if (obj != null) 
		{
			TweenScale scale = obj.AddComponent<TweenScale> ();
			scale.from = new Vector3 (0, 0, 0);
			scale.to = new Vector3 (1, 1, 1);
			scale.method = UITweener.Method.EaseInOut;
			scale.duration = time;
			scale.PlayForward();
		}
	}
	
	void Update () 
	{
		float currentEnemyHP = BattleGameHelper.GetEnemyTeamCurrentHP ();
		float allEnemyHp = BattleGameHelper.GetEnemyTeamAmountHP ();
		float currentPlayerHp = BattleGameHelper.GetPlayerTeamCurrentHP ();
		float allPlayerHp = BattleGameHelper.GetPlayerTeamAmountHP ();

		if (enemyHp != null && currentEnemyHP != null && allEnemyHp != null) 
		{
			enemyHp.value = currentEnemyHP / allEnemyHp;
		}

		if (playerHP != null && currentPlayerHp != null && allPlayerHp != null) 
		{
			playerHP.value = currentPlayerHp / allPlayerHp;
		}

		if (BattleGame.instance.state == BattleGame.STATE.BATTLE &&
			!_isBottomAnimation) 
		{
			_isBottomAnimation = true;
			StartCoroutine(PlayBottomAnimation()); 
		}
		UpdatePlayerInfo();
	}

	void UpdatePlayerInfo()
	{
		if(InstancePlayer.instance.pvpUser == null)//pve
		{
			int missoinId = InstancePlayer.instance.missionMagicId;
			DataMission mission = DataManager.instance.dataMissionGroup.GetMission(missoinId);
			_enemyName.text = mission.name;
		}
		else//pvp
		{
			SlgPB.PVPUser pvp = InstancePlayer.instance.pvpUser;
			_enemyName.text = pvp.userName;
			_enemyLevel.text = DataManager.instance.dataLeaderGroup.GetLevel( pvp.honor).ToString();
		}
		Model_User modelUser = InstancePlayer.instance.model_User;
		_playerLevel.text = modelUser.honorLevel.ToString();
		_playerName.text = modelUser.userName;

	}

	public string setTimeFormat(int secon_min)
	{
		string secon = Mathf.Round(secon_min).ToString();
		if(secon.Length < 2)
		{
			secon = "0" + secon;
		}
		return secon;
	}
	
	public void InitSkillItem(int itemNum)
	{
		if(_skillItemList == null) _skillItemList = new List<SkillItem>();
		_skillItemList.Clear();

		for (int i = 0; i< itemNum; i++) {

			this._skillItemList.Add(CreateSkillBtnItem(skillGrid.gameObject));
		}
		skillGrid.repositionNow = true;
		skillGrid.Reposition ();

		int count = this._skillItemList.Count;
		int dataCount = _gameskill.GetSkillCount();
		
		for(int index = 0; index < count; index ++)
		{
			if(index < dataCount)
			{
				this._skillItemList[index].UpdateData(index);
			}
		}

		foreach(SkillItem item in _skillItemList)
		{
			item.transform.localScale = new Vector3(0, 0, 0);
		}

//		speedBtn.transform.localScale = new Vector3(0, 0, 0); 
//		skipBtn.transform.localScale = new Vector3(0, 0, 0);

	}

	public SkillItem CreateSkillBtnItem (GameObject parent)
	{
		if (parent == null)return null;
		GameObject skillItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "battle/SkillBtnItem");
		GameObject item = NGUITools.AddChild(parent,skillItem);

		return item.GetComponent<SkillItem>(); 
	}

	private float BATTLE_ROUND_TIME = 120.0f;
	private TimerEx _fightTimer;
	private long _fightStartTimestamp;
	
	private void InitFightTimer ()
	{
		_fightTimer = TimerEx.Init("fightTimer", 1.0f, FightTimerCallBack, true);
		_fightStartTimestamp = TimeHelper.GetCurrentRealTimestamp ();
	}

	private void FightTimerCallBack(System.Object parameter)
	{
		float elapseTime = (float)((TimeHelper.GetCurrentRealTimestamp () - _fightStartTimestamp) / 1000.0f);
		if (elapseTime >= BATTLE_ROUND_TIME)
		{
			_fightTimer.Stop();
		}

		UpdateFightTime(Mathf.FloorToInt(elapseTime));
	}

	public void UpdateFightTime(int time){

		seconds = time % 60;
		minutes = time /  60;
		string second = setTimeFormat(seconds);
		string minute = setTimeFormat(minutes);
		if (tiemLable != null) 
		{
			tiemLable.text = minute + ":" + second ;				
		}
	}
	
	public void OnSpeedBtnClick()
	{
		_bgNameNum = _bgNameNum % 3;
//		speedBtn.normalSprite = bgName[_bgNameNum];
		speedLabel.text = bgName[_bgNameNum];
		_bgNameNum++;

		BattleGame.instance.battleSpeed = _bgNameNum;
	}

	public void OnSkipBtnClick()
	{
//		if(SkillManager.currentSkillID != "0" && !SkillConfigManager.GetItem(SkillManager.currentSkillID).isReleaseSkill)
//		{
//			int currentId = int.Parse (SkillManager.currentSkillID) - 1 ;
//			this._skillItemList [currentId].DisableSkillBtn ();
//		}

		// 跳过 
		if (_fightTimer != null) 
		{
			_fightTimer.Stop();
		}

		if(!InstancePlayer.instance.model_User.isLogin)
		{
			SceneHelper.SwitchScene(AppConfig.SCENE_NAME_BATTLE_OFFLINE);
			return;
		}

		if (GameOffine.START_OF_OFFLINE) 
		{
			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE_OFFLINE);
		} 
		else
		{
			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_UI);
		}

	}
 
}
