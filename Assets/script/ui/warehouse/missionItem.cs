using UnityEngine;
using System.Collections;

public class missionItem : MonoBehaviour {

	private UISprite _iconbg;
	private UISprite _icon;
	private UILabel _missionName_Label;
	private UILabel _mission_Label;
	private UIButton _gotoBtn;
	private UILabel _goLabel;

	void Awake()
	{
		_iconbg = transform.Find("headIconbg").GetComponent<UISprite>();
		_icon = transform.Find("headIconbg/Sprite").GetComponent<UISprite>();
		_missionName_Label = transform.Find("missionName_Label").GetComponent<UILabel>();
		_mission_Label = transform.Find("mission_Label").GetComponent<UILabel>();
		_missionName_Label.color = UICommon.FONT_COLOR_GREY;
		_mission_Label.color = UICommon.FONT_COLOR_GREY;
		_gotoBtn = transform.Find("gotoBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_gotoBtn,OnGOTO);
		_goLabel = transform.Find("gotoBtn/Label").GetComponent<UILabel>();
		_goLabel.color = UICommon.FONT_COLOR_GOLDEN;
	}

	//data
	DataMission _missionData ;
	public void Init(DataMission data)
	{
		_missionData = data;
	}
	// Use this for initialization
	void Start () {
		if(_missionData != null)
		{
			_missionName_Label.text = _missionData.name;
		}
	}

	void UpdateUI () {
	
	}
	void OnGOTO()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION, _missionData.magicId);
	}
}
