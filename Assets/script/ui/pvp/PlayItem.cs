using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class PlayItem : MonoBehaviour {
	private Transform _transform;
	private UISprite _itmebg;
	private UISprite _rank_Sprites;
	private UISprite _headIcon;
	private UILabel _playName_Label;
	private UILabel _playLevel_Label;
	private UILabel _rank_Label;
	private UILabel _rankValue_Label;
	private UILabel _fight_Label;
	private UILabel _fightValue_Label;
	private UIButton _challenge_btn;
	private UILabel _challenge_Label;
	private UIButton _headIcon_btn;
	//data 
	private SlgPB.PVPUser _pvpUser;
	void Awake()
	{
		_transform = this.transform;
		_itmebg = _transform.Find("itmebg").GetComponent<UISprite>();
		_rank_Sprites = _transform.Find("rank_Sprite").GetComponent<UISprite>();
		_headIcon = _transform.Find("headIcon_Bg/headIcon").GetComponent<UISprite>();
		_playName_Label = _transform.Find("playName_Label").GetComponent<UILabel>();
		_playName_Label.color= UICommon.FONT_COLOR_GREY;
		_playLevel_Label = _transform.Find("playLevel_Label").GetComponent<UILabel>();
		_playLevel_Label.color= UICommon.FONT_COLOR_GREY;
		_rank_Label = _transform.Find("rank_Label").GetComponent<UILabel>();
		_rank_Label.color= UICommon.FONT_COLOR_GREY;
		_rankValue_Label = _transform.Find("rank_Label/Label").GetComponent<UILabel>();
		_rankValue_Label.color = UICommon.FONT_COLOR_GREEN;
		_fight_Label = _transform.Find("fight_Label").GetComponent<UILabel>();
		_fight_Label.color= UICommon.FONT_COLOR_GREY;
		_fightValue_Label = _transform.Find("fight_Label/Label").GetComponent<UILabel>();
		_fightValue_Label.color = UICommon.FONT_COLOR_ORANGE;
		_challenge_btn = _transform.Find("challenge_btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_challenge_btn,OnChallenge);
		_challenge_Label = _transform.Find("challenge_btn/Label").GetComponent<UILabel>();
		_challenge_Label.color = UICommon.FONT_COLOR_GOLDEN;
		_headIcon_btn = _transform.Find("headIcon_Bg/headIcon").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_headIcon_btn,OnHeadIcon);
	}
	// Use this for initialization
	void Start () {
	
	}

	public void Init(SlgPB.PVPUser pvpUser)
	{
		if(pvpUser != null)
		{
			_pvpUser = pvpUser;
		}
	}
	// Update is called once per frame
	void Update () {
		UpdateUI();
	}
	void UpdateUI () {
		if(_pvpUser != null)
		{
			string bgName = "";
			string rankName = "";
			if(_pvpUser.rank <= 3 && _pvpUser.rank > 0)
			{
				bgName = "pvp_herobg_" + _pvpUser.rank;
				rankName = "pvp_Label_rank_" + _pvpUser.rank;
				_rank_Sprites.spriteName = rankName;
				_rank_Sprites.gameObject.SetActive(true);

			}
			else
			{
				bgName = "pvp_herobg_0";
				_rank_Sprites.gameObject.SetActive(false);
			}
			_itmebg.spriteName = bgName;
			_playName_Label.text = _pvpUser.userName;
			_playLevel_Label.text = DataManager.instance.dataLeaderGroup.GetLevel(_pvpUser.honor).ToString();
			_rankValue_Label.text = _pvpUser.rank.ToString();
			_fightValue_Label.text = _pvpUser.fightPower.ToString();
		}
	}
	void OnChallenge()
	{
		InstancePlayer.instance.uiDataStatus.state = UIDataStatus.STATE.PVP;
		BattleConnection.instance.StartPvpFight(_pvpUser);
	}
	void OnHeadIcon()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_PVPENEMYFORMATION,_pvpUser);
	}
}
