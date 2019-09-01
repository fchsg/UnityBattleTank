using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class HeroInfoPanel : PanelBase {
	private Transform _panelContainer;
	private UIButton _closeBtn;
	private UILabel _panelName;

	private Transform _Container;
	private UISprite _iconBg;
	private UISprite _formation_sprite;
	private UISprite _rank_sprite;
	private UISprite _type_Sprite;
	private UITexture _icon_Texture;
	private UILabel _heroLevel_Label;
	private UILabel _heroName_Label;

	private UISlider _Timer_Colored_Slider;
	private UILabel  _expLabel;

	private Transform _heroInfo_Container;
	private UILabel _info_Label;
	private Transform _attribute_Container;
	private UILabel _attributeLabel;
	private UILabel _attribute_Label_1;
	private UILabel _attributeValue_Label_1;
	private UILabel _attribute_Label_2;
	private UILabel _attributeValue_Label_2;
	private UILabel _attribute_Label_3;
	private UILabel _attributeValue_Label_3;
	private UILabel _attribute_Label_4;
	private UILabel _attributeValue_Label_4;
	private UISprite _skillBg;
	private UISprite _skillIcon;
	private UILabel _skillName;
	private UILabel _skillAttribute_Label;
	private UIButton _upLevel_Btn;
	private UILabel _uplevellabel;
	private UIButton _manager_Btn;
	private UILabel _managerLabel;

	private Transform _ability_Container;
	private UILabel _ability_Label;
	private UILabel _ability_Label_1;
	private UILabel _ability_Label_2;
	private UILabel _ability_Label_3;
	private UILabel _ability_Label_4;
	private UILabel _ability_Label_5;
	//data 
	List<UILabel> _greyLabelList = new List<UILabel>();
	HeroDataManager.HeroData _heroData;
	UIHeroDetialPanel  _heroDetialPanel = null;


	private bool last_top_layer = false;
	private GameObject ployPlane;

	public override void Init ()
	{
		base.Init ();
		NotificationCenter.instance.AddEventListener(Notification_Type.SHOWABILITYPANL,OnShow);

	}
	void OnShow(Notification data)
	{
		if(data._data != null)
		{
			bool isshow = ((int)data._data > 0)?true:false;
			_ability_Container.gameObject.SetActive(isshow);
		}
	}
	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length > 0)
		{
			_heroData = parameters[0] as HeroDataManager.HeroData;
		}
		_heroDetialPanel.Init(_heroData);
		ployPlane = _heroDetialPanel.ployPlane;


	}
	
	// Update is called once per frame
	void Update () {
		UpdataUI();

		UpdateLayer ();
	}

	private void UpdateLayer()
	{
		bool isTop = UIPanelManager.instance.IsTopPanel (this);
		if (last_top_layer != isTop)
		{
			int renderQuene = 3000;
			int sortingOrder = 0;

			if (isTop) {
				renderQuene = 3000;
				sortingOrder = 1;
			} else {
				renderQuene = 1000;
				sortingOrder = 0;
			}
			if (ployPlane != null) 
			{
				RenderHelper.SetObjectRenderQuene (ployPlane, renderQuene, sortingOrder);
			}
			last_top_layer = isTop;
		}
	}

	void UpdataUI()
	{

		if(_heroData != null)
		{
			_iconBg.spriteName = _heroData.IconBgName;
			_icon_Texture.SetHeroSmallTexture(_heroData.id);
			switch(_heroData.team)
			{
			case HeroDataManager.TEAM.NOTEAM:
				_formation_sprite.gameObject.SetActive(false);
				break;
			case HeroDataManager.TEAM.FIRSTTEAM:
				_formation_sprite.gameObject.SetActive(true);
				_formation_sprite.spriteName = "heroTeam_" + (int)_heroData.team;
				break;
			case HeroDataManager.TEAM.SECONDTEAM:
				_formation_sprite.gameObject.SetActive(true);
				_formation_sprite.spriteName = "heroTeam_" + (int)_heroData.team;
				break;
			case HeroDataManager.TEAM.THIRDTEAM:
				_formation_sprite.gameObject.SetActive(true);
				_formation_sprite.spriteName = "heroTeam_" + (int)_heroData.team;
				break;
			}
			_type_Sprite.spriteName = "arms_" + (int)_heroData.heroType;
			_heroName_Label.color = _heroData.nameColor;
			_heroName_Label.text = _heroData.data.name; 
			int exp = 0;
			Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			if(_heroData.recruitType == HeroDataManager.RecruitType.ALREADYRECRUIT)
			{
				
				Hero hero = model_heroGroup.GetHero(_heroData.id);
				exp = hero.exp;
				_manager_Btn.gameObject.SetActive(true);
				_upLevel_Btn.gameObject.SetActive(true);
				_heroLevel_Label.gameObject.SetActive(true);
				_heroLevel_Label.text = model_heroGroup.GetCurrentLevel(_heroData.id).ToString();
				_rank_sprite.gameObject.SetActive(true);
				_rank_sprite.spriteName = UICommon.HERO_RANK_ICON_PATH + model_heroGroup.GetCurrentStage(_heroData.id);
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (_heroData.id, hero.exp, hero.stage);
				//hp
				_attributeValue_Label_1.text = ((int)dataHero.basicParam.hp).ToString();
				//ap
				_attributeValue_Label_2.text = ((int)dataHero.basicParam.damage).ToString();
				//dp
				_attributeValue_Label_3.text = ((int)dataHero.basicParam.ammo).ToString();
				//hitRate
				float hitDodDou = 0.0f;
				string hitDodDouDesc = "";
				if(dataHero.basicParam.hitRate != 0.0f)
				{
					hitDodDouDesc = "命中";
					hitDodDou = dataHero.basicParam.hitRate;
				}
				if(dataHero.basicParam.dodgeRate != 0.0f)
				{
					hitDodDouDesc = "闪避";
					hitDodDou = dataHero.basicParam.dodgeRate;
				}
				if(dataHero.basicParam.doubleDamageRate != 0.0f)
				{
					hitDodDouDesc = "暴击";
					hitDodDou = dataHero.basicParam.doubleDamageRate;
				}
				if(hitDodDou != 0.0f)
				{
					_attribute_Label_4.gameObject.SetActive(true);
					_attributeValue_Label_4.text = hitDodDou.ToString();
				}
				else 
				{
					_attribute_Label_4.gameObject.SetActive(false);
				}

			}
			else
			{
				_manager_Btn.gameObject.SetActive(false);
				_upLevel_Btn.gameObject.SetActive(false);
				exp = _heroData.exp;
				_heroLevel_Label.gameObject.SetActive(false);
				_rank_sprite.gameObject.SetActive(false);
				//hp
				_attributeValue_Label_1.text = ((int)_heroData.data.basicParam.hp).ToString();
				//ap
				_attributeValue_Label_2.text = ((int)_heroData.data.basicParam.damage).ToString();
				//dp
				_attributeValue_Label_3.text = ((int)_heroData.data.basicParam.ammo).ToString();
				//hitRate
				_attribute_Label_4.gameObject.SetActive(false);
				_attributeValue_Label_4.text = (_heroData.data.basicParam.hitRate).ToString();

			}
		 
			float heroExpToNextLevel = (float)DataManager.instance.dataHeroGroup.GetHeroExpToNextLevel(_heroData.id,exp);
			float heroTotalExpToNextLevel = (float)DataManager.instance.dataHeroGroup.GetHeroTotalExpToNextLevel(_heroData.id,exp);
			if(heroExpToNextLevel == 0 || heroTotalExpToNextLevel == 0)
			{
				_expLabel.text = "军官已满级";
			}
			{
				_Timer_Colored_Slider.value = (heroTotalExpToNextLevel - heroExpToNextLevel) / heroTotalExpToNextLevel;
				_expLabel.text = (heroTotalExpToNextLevel - heroExpToNextLevel)  + "/" + heroTotalExpToNextLevel;	
			}
				
			DataHeroLeadership heroLeaderData = DataManager.instance.dataHeroGroup.GetHeroLeadership(_heroData.id);
			DataSkill skillData = DataManager.instance.dataSkillGroup.GetSkill(heroLeaderData.skill,1);
			_skillName.text = skillData.name;
			_skillIcon.spriteName = "SkillICONS_" + (int)heroLeaderData.skill;
//			_skillAttribute_Label.text = skillData.
 
			 
			_ability_Label_1.text = "坦克" + ((int)(heroLeaderData.kTank * 100)).ToString() + "%";
			_ability_Label_2.text = "导弹" + "\n" + ((int)(heroLeaderData.kMissile * 100)).ToString()  + "%";
			_ability_Label_3.text = "反坦" + ((int)(heroLeaderData.kUnknown * 100)).ToString()  + "%";
			_ability_Label_4.text = "装甲" + ((int)(heroLeaderData.kGun * 100)).ToString()  + "%";
			_ability_Label_5.text = "火炮" + "\n" + ((int)(heroLeaderData.kCannon * 100)).ToString()  + "%";

		}

	}
	void OnManager()
	{
		NotificationCenter.instance.DispatchEvent(Notification_Type.SHOWABILITYPANL,new Notification(0));
		UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROADVANCED,_heroData);
	}
	void OnUpLevel()
	{
		//NotificationCenter.instance.DispatchEvent(Notification_Type.SHOWABILITYPANL,new Notification(0));
		UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROUPLEVEL,_heroData);
	}
	void OnClose()
	{
		this.Delete();
		NotificationCenter.instance.RemoveEventListener(Notification_Type.SHOWABILITYPANL);
	}
	void Awake()
	{
		_greyLabelList.Clear();
		_panelContainer = transform.Find("HeroInfo_Container");
		_closeBtn = _panelContainer.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_panelName = _panelContainer.Find("panelName_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_panelName);

		_Container = _panelContainer.Find("heroInfo_Item");
		_iconBg = _Container.Find("iconBg").GetComponent<UISprite>();
		_formation_sprite = _Container.Find("formation_sprite").GetComponent<UISprite>();
		_rank_sprite = _Container.Find("rank_sprite").GetComponent<UISprite>();
		_type_Sprite = _Container.Find("type_Sprite").GetComponent<UISprite>();
		_icon_Texture = _Container.Find("iicon_Texture").GetComponent<UITexture>();
		_heroLevel_Label = _Container.Find("heroLevel_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_heroLevel_Label);
		_heroName_Label = _Container.Find("heroName_Label").GetComponent<UILabel>();

		_Timer_Colored_Slider = _panelContainer.Find("Timer_Colored_Slider").GetComponent<UISlider>();
		_expLabel = _panelContainer.Find("Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_expLabel.color = UICommon.FONT_COLOR_ORANGE;
		_heroInfo_Container = _panelContainer.Find("heroInfo_Container");
		_info_Label = _heroInfo_Container.Find("info_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_info_Label);
		_attribute_Container = _heroInfo_Container.Find("attribute_Container");
		_attributeLabel = _attribute_Container.Find("Label").GetComponent<UILabel>();
		_greyLabelList.Add(_attributeLabel);

		_attribute_Label_1 = _attribute_Container.Find("attribute_Label_1").GetComponent<UILabel>();
		_attributeValue_Label_1 = _attribute_Container.Find("attribute_Label_1/Label").GetComponent<UILabel>();
		_attribute_Label_2 = _attribute_Container.Find("attribute_Label_2").GetComponent<UILabel>();
		_attributeValue_Label_2 = _attribute_Container.Find("attribute_Label_2/Label").GetComponent<UILabel>();
		_attribute_Label_3 = _attribute_Container.Find("attribute_Label_3").GetComponent<UILabel>();
		_attributeValue_Label_3 = _attribute_Container.Find("attribute_Label_3/Label").GetComponent<UILabel>();
		_attribute_Label_4 = _attribute_Container.Find("attribute_Label_4").GetComponent<UILabel>();
		_attributeValue_Label_4 = _attribute_Container.Find("attribute_Label_4/Label").GetComponent<UILabel>();
		_greyLabelList.Add(_attribute_Label_1);
		_greyLabelList.Add(_attribute_Label_2);
		_greyLabelList.Add(_attribute_Label_3);
		_greyLabelList.Add(_attribute_Label_4);
		_attributeValue_Label_1.color = UICommon.FONT_COLOR_GREEN;
		_attributeValue_Label_2.color = UICommon.FONT_COLOR_GREEN;
		_attributeValue_Label_3.color = UICommon.FONT_COLOR_GREEN;
		_attributeValue_Label_4.color = UICommon.FONT_COLOR_GREEN;
		_skillBg = _attribute_Container.Find("skill_bg").GetComponent<UISprite>();
		_skillIcon = _attribute_Container.Find("skill_bg/skillIcon").GetComponent<UISprite>();

		_skillName = _attribute_Container.Find("skillName_Label").GetComponent<UILabel>();
		_skillName.color = UICommon.FONT_COLOR_ORANGE;
		_skillAttribute_Label = _attribute_Container.Find("skillAttribute_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_skillAttribute_Label);

		_upLevel_Btn = _heroInfo_Container.Find("upLevel_Btn").GetComponent<UIButton>();
		_uplevellabel = _heroInfo_Container.Find("upLevel_Btn/Label").GetComponent<UILabel>();
		_manager_Btn = _heroInfo_Container.Find("manager_Btn").GetComponent<UIButton>();
		_managerLabel = _heroInfo_Container.Find("manager_Btn/Label").GetComponent<UILabel>();
		UIHelper.AddBtnClick(_manager_Btn,OnManager);
		UIHelper.AddBtnClick(_upLevel_Btn,OnUpLevel);
		_uplevellabel.color = UICommon.FONT_COLOR_GOLDEN;
		_managerLabel.color = UICommon.FONT_COLOR_GOLDEN;


		_heroDetialPanel = _heroInfo_Container.Find("ability_Container/Panel").GetComponent<UIHeroDetialPanel>();
		_ability_Container = _heroInfo_Container.Find("ability_Container/Panel");
		_ability_Label = _ability_Container.Find("ability_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_ability_Label);
		_ability_Label_1 = _ability_Container.Find("ability_Label_1").GetComponent<UILabel>();
		_ability_Label_2 = _ability_Container.Find("ability_Label_2").GetComponent<UILabel>();
		_ability_Label_3 = _ability_Container.Find("ability_Label_3").GetComponent<UILabel>();
		_ability_Label_4 = _ability_Container.Find("ability_Label_4").GetComponent<UILabel>();
		_ability_Label_5 = _ability_Container.Find("ability_Label_5").GetComponent<UILabel>();
		List<UILabel> abilityLabe =  new List<UILabel>();
		abilityLabe.Clear();
		abilityLabe.Add(_ability_Label_1);
		abilityLabe.Add(_ability_Label_2);
		abilityLabe.Add(_ability_Label_3);
		abilityLabe.Add(_ability_Label_4);
		abilityLabe.Add(_ability_Label_5);
		foreach(UILabel label in _greyLabelList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
		foreach(UILabel label in abilityLabe)
		{
			label.color = UICommon.FONT_COLOR_BROWN;
		}
	}
}
