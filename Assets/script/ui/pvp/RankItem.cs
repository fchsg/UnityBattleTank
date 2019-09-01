using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;


public class RankItem : MonoBehaviour {
	private Transform _transform;
	private UILabel _playName_Label;
	private UILabel _playLevel_Label;
	private UILabel _rank_Label;
	private UILabel _rankValue_Label;
	private UILabel _fight_Label;
	private UILabel _fightValue_Label;

	private UILabel _challenge_Label;
	private UILabel _record;
	private UILabel _record_value;
	private UILabel _gem_Label_1;
	private UILabel _Item_Label_1;
	private UILabel _Item_Label_2;
	private UITexture _Item_1;
	private UITexture _Item_2;
	List<UILabel> _grayList = new List<UILabel>(); 
	//data 
	private SlgPB.PVPUser _pvpUser;
	void Awake()
	{
		_grayList.Clear();
		_transform = this.transform;

		_playName_Label = _transform.Find("name").GetComponent<UILabel>();
		_grayList.Add(_playName_Label);
		_playLevel_Label = _transform.Find("level").GetComponent<UILabel>();
		_grayList.Add(_playLevel_Label);
		 
		_rankValue_Label = _transform.Find("rank").GetComponent<UILabel>();
		 
		 
		_fightValue_Label = _transform.Find("fight").GetComponent<UILabel>();
		_fightValue_Label.color = UICommon.FONT_COLOR_ORANGE;

		_record = _transform.Find("record").GetComponent<UILabel>();
		_record_value = _transform.Find("record/record_value").GetComponent<UILabel>();
		_gem_Label_1 = _transform.Find("gem_Sprite/Label").GetComponent<UILabel>();
		_Item_Label_1 = _transform.Find("gem_Sprite_1/Label").GetComponent<UILabel>();
		_Item_Label_2 = _transform.Find("gem_Sprite_2/Label").GetComponent<UILabel>();
		_grayList.Add(_record);
		_grayList.Add(_record_value);
		_grayList.Add(_gem_Label_1);
		_grayList.Add(_Item_Label_2);
		_Item_1 = _transform.Find("gem_Sprite_1").GetComponent<UITexture>();
		_Item_2 = _transform.Find("gem_Sprite_2").GetComponent<UITexture>();
		_Item_2.gameObject.SetActive(false);
		foreach(UILabel label in _grayList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
	}
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
		UpdateUI ();
	}
	void UpdateUI () {
		if(_pvpUser != null)
		{
			_playName_Label.text = _pvpUser.userName;
			_playLevel_Label.text = DataManager.instance.dataLeaderGroup.GetLevel(_pvpUser.honor).ToString();
			_fightValue_Label.text = _pvpUser.fightPower.ToString();
			string NOStr = "";
			if(_pvpUser.rank == 1)
			{
				NOStr = "NO.1";
				_rankValue_Label.color = UICommon.UNIT_NAME_COLOR_4;
			}
			else if(_pvpUser.rank == 2)
			{
				NOStr = "NO.2";
				_rankValue_Label.color = UICommon.UNIT_NAME_COLOR_3;
			}
			else if(_pvpUser.rank == 3)
			{
				NOStr = "NO.3";
				_rankValue_Label.color = UICommon.UNIT_NAME_COLOR_2;
			}
			else
			{
				NOStr = _pvpUser.rank.ToString() ;
				_rankValue_Label.color = UICommon.FONT_COLOR_GREY;
			}
			_rankValue_Label.text = NOStr;
			DataLadder ladder = DataManager.instance.dataLadderGroup.GetLadder(_pvpUser.rank);
			if(ladder != null)
			{
				_gem_Label_1.text = ladder.cash.ToString();
				if(ladder.itemId != 0)
				{
					_Item_1.gameObject.SetActive(true);
					_Item_1.SetItemTexture(ladder.itemId);
					_Item_Label_1.text = ladder.itemNum.ToString();
				}
				else
				{
					_Item_1.gameObject.SetActive(false);
				}


				_record_value.text = ladder.combat.ToString();
			}
		}
	}
}
