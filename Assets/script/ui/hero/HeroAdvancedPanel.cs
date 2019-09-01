using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class HeroAdvancedPanel : PanelBase {

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

	private Transform _attribute_Container;
	private UILabel _attriLabel;

	private UILabel _attribute_Label_1;
	private UILabel _current_Label_1;
	private UILabel _willbe_Label_1;
	private UILabel _attribute_Label_2;
	private UILabel _current_Label_2;
	private UILabel _willbe_Label_2;
	private UILabel _attribute_Label_3;
	private UILabel _current_Label_3;
	private UILabel _willbe_Label_3;
	private UILabel _attribute_Label_4;
	private UILabel _current_Label_4;
	private UILabel _willbe_Label_4;
	 
	private UILabel attributeUP_Label_1;
	private UILabel _UPcurrent_Label_1;
	private UILabel _UPwillbe_Label_1;
	private UILabel attributeUP_Label_2;
	private UILabel _UPcurrent_Label_2;
	private UILabel _UPwillbe_Label_2;
	private UILabel attributeUP_Label_3;
	private UILabel _UPcurrent_Label_3;
	private UILabel _UPwillbe_Label_3;
	private UILabel attributeUP_Label_4;
	private UILabel _UPcurrent_Label_4;
	private UILabel _UPwillbe_Label_4;

	private Transform _heroInfo_Container;
	private UIButton _advanced_Btn;
	private UILabel _advanced_Label;
	private UILabel _advancedTitle_Label;
	private UILabel _Level_Label;
	private UILabel _LevelValue_Label;
	private UISprite _props_bg;
	private UISprite _propsIcon;
	private UILabel _number_Label;
	//data 
	List<UILabel> _greyLabelList = new List<UILabel>();
	List<UILabel> _orangeLabelList = new List<UILabel>();
	List<UILabel> _greenLabelList = new List<UILabel>();
	private int MAXSTAGE = 11;
	HeroDataManager.HeroData _heroData;
	void Awake()
	{
		_greyLabelList.Clear();
		_orangeLabelList.Clear();
		_greenLabelList.Clear();
		_panelContainer = transform.Find("HeroAdvanced_Container");
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
		_heroInfo_Container = _panelContainer.Find("heroInfo_Container");
		_advanced_Btn = _panelContainer.Find("manager_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_advanced_Btn,OnAdvanced);
		_advanced_Label = _panelContainer.Find("manager_Btn/Label").GetComponent<UILabel>();
		_advanced_Label.color = UICommon.FONT_COLOR_GOLDEN;
		_advancedTitle_Label = _panelContainer.Find("advanced_Label").GetComponent<UILabel>();
		_greyLabelList.Add(_advancedTitle_Label);
		_Level_Label = _panelContainer.Find("Level_Label").GetComponent<UILabel>(); 
		_greyLabelList.Add(_Level_Label);
		_LevelValue_Label = _panelContainer.Find("Level_Label/Label").GetComponent<UILabel>(); 
//		_LevelValue_Label.color = UICommon.FONT_COLOR_GREEN;
		_props_bg =  _panelContainer.Find("propIconBg").GetComponent<UISprite>(); 
		_propsIcon =  _panelContainer.Find("propIconBg/propIcon").GetComponent<UISprite>(); 
		_number_Label = _panelContainer.Find("number_Label").GetComponent<UILabel>();
//		_number_Label.color = UICommon.FONT_COLOR_GREEN;
		_attribute_Container = _panelContainer.Find("heroInfo_Container/attribute_Container");
		_attriLabel = _attribute_Container.Find("Label").GetComponent<UILabel>();
		_greyLabelList.Add(_attriLabel);

		_attribute_Label_1 = _attribute_Container.Find("attribute_Label_1").GetComponent<UILabel>();
		_greyLabelList.Add(_attribute_Label_1);
		_current_Label_1 = _attribute_Container.Find("attribute_Label_1/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_current_Label_1);
		_willbe_Label_1 = _attribute_Container.Find("attribute_Label_1/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_willbe_Label_1);

		_attribute_Label_2 = _attribute_Container.Find("attribute_Label_2").GetComponent<UILabel>();
		_greyLabelList.Add(_attribute_Label_2);
		_current_Label_2 = _attribute_Container.Find("attribute_Label_2/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_current_Label_2);
		_willbe_Label_2 = _attribute_Container.Find("attribute_Label_2/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_willbe_Label_2);

		_attribute_Label_3 = _attribute_Container.Find("attribute_Label_3").GetComponent<UILabel>();
		_greyLabelList.Add(_attribute_Label_3);
		_current_Label_3 = _attribute_Container.Find("attribute_Label_3/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_current_Label_3);
		_willbe_Label_3 = _attribute_Container.Find("attribute_Label_3/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_willbe_Label_3);

		_attribute_Label_4 = _attribute_Container.Find("attribute_Label_4").GetComponent<UILabel>();
		_greyLabelList.Add(_attribute_Label_4);
		_current_Label_4 = _attribute_Container.Find("attribute_Label_4/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_current_Label_4);
		_willbe_Label_4 = _attribute_Container.Find("attribute_Label_4/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_willbe_Label_4);

		attributeUP_Label_1 = _attribute_Container.Find("attributeUP_Label_1").GetComponent<UILabel>();
		_greyLabelList.Add(attributeUP_Label_1);
		_UPcurrent_Label_1 = _attribute_Container.Find("attributeUP_Label_1/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_UPcurrent_Label_1);
		_UPwillbe_Label_1 = _attribute_Container.Find("attributeUP_Label_1/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_UPwillbe_Label_1);

		attributeUP_Label_2 = _attribute_Container.Find("attributeUP_Label_2").GetComponent<UILabel>();
		_greyLabelList.Add(attributeUP_Label_2);
		_UPcurrent_Label_2 = _attribute_Container.Find("attributeUP_Label_2/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_UPcurrent_Label_2);
		_UPwillbe_Label_2 = _attribute_Container.Find("attributeUP_Label_2/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_UPwillbe_Label_2);

		attributeUP_Label_3 = _attribute_Container.Find("attributeUP_Label_3").GetComponent<UILabel>();
		_greyLabelList.Add(attributeUP_Label_3);
		_UPcurrent_Label_3 = _attribute_Container.Find("attributeUP_Label_3/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_UPcurrent_Label_3);
		_UPwillbe_Label_3 = _attribute_Container.Find("attributeUP_Label_3/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_UPwillbe_Label_3);

		attributeUP_Label_4 = _attribute_Container.Find("attributeUP_Label_4").GetComponent<UILabel>();
		_greyLabelList.Add(attributeUP_Label_4);
		attributeUP_Label_4.gameObject.SetActive(false);
		_UPcurrent_Label_4 = _attribute_Container.Find("attributeUP_Label_4/current_Label").GetComponent<UILabel>();
		_orangeLabelList.Add(_UPcurrent_Label_4);
		_UPwillbe_Label_4 = _attribute_Container.Find("attributeUP_Label_4/willbe_Label").GetComponent<UILabel>();
		_greenLabelList.Add(_UPwillbe_Label_4);

		foreach(UILabel label in _greyLabelList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
		foreach(UILabel label in _orangeLabelList)
		{
			label.color = UICommon.FONT_COLOR_ORANGE;
		}
		foreach(UILabel label in _greenLabelList)
		{
			label.color = UICommon.FONT_COLOR_GREEN;
		}
	}
	void Start () {

	}
	public override void Init ()
	{
		base.Init ();

//		UIPanel panel = gameObject.GetComponent<UIPanel> ();
//		UIHelper.IncreasePanelRender (panel);
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length > 0)
		{
			_heroData = parameters[0] as HeroDataManager.HeroData;
		}
	}

	void Update () {
		UpdataUI();
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

			Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			_rank_sprite.spriteName = UICommon.HERO_RANK_ICON_PATH + model_heroGroup.GetCurrentStage(_heroData.id);
			_heroLevel_Label.text = model_heroGroup.GetCurrentLevel(_heroData.id).ToString();

			int heroLevel = model_heroGroup.GetCurrentLevel(_heroData.id);
			Hero hero = model_heroGroup.GetHero(_heroData.id);
	 
			DataHero dataHero1 = DataManager.instance.dataHeroGroup.GetHero (_heroData.id, hero.exp, hero.stage);
			DataHeroUpgrade dataHeroUpgrade1 = DataManager.instance.dataHeroGroup.GetHeroUpgrade(_heroData.id,hero.stage);
			if(dataHeroUpgrade1.requireLevel > heroLevel)
			{
				_LevelValue_Label.text = UIHelper.SetStringSixteenColor(heroLevel.ToString(),UICommon.SIXTEEN_RED) + UIHelper.SetStringSixteenColor("/" + dataHeroUpgrade1.requireLevel.ToString(),UICommon.SIXTEEN_GREEN);
			}
			else
			{
				_LevelValue_Label.text = UIHelper.SetStringSixteenColor(heroLevel.ToString(),UICommon.SIXTEEN_GREEN) + UIHelper.SetStringSixteenColor("/" + dataHeroUpgrade1.requireLevel.ToString(),UICommon.SIXTEEN_GREEN);
			}
			Item item = InstancePlayer.instance.model_User.model_itemGroup.QueryItem(dataHeroUpgrade1.itemId);
			if(item.num < dataHeroUpgrade1.itemCount)
			{
				_number_Label.text = UIHelper.SetStringSixteenColor(item.num.ToString(),UICommon.SIXTEEN_RED) + UIHelper.SetStringSixteenColor("/" + dataHeroUpgrade1.itemCount.ToString(),UICommon.SIXTEEN_GREEN);
			}
			else
			{
				_number_Label.text = UIHelper.SetStringSixteenColor(item.num.ToString(),UICommon.SIXTEEN_GREEN) + UIHelper.SetStringSixteenColor("/" + dataHeroUpgrade1.itemCount.ToString(),UICommon.SIXTEEN_GREEN);
			}
			//hp
			_current_Label_1.text = ((int)dataHero1.basicParam.hp).ToString();
			//ap
			_current_Label_2.text = ((int)dataHero1.basicParam.damage).ToString();
			//dp
			_current_Label_3.text = ((int)dataHero1.basicParam.ammo).ToString();
			_UPcurrent_Label_1.text = dataHeroUpgrade1.kHP.ToString();
			_UPcurrent_Label_2.text = dataHeroUpgrade1.kAP.ToString();
			_UPcurrent_Label_3.text = dataHeroUpgrade1.kDP.ToString();

			if(hero.stage + 1 > MAXSTAGE)
			{
				_willbe_Label_1.gameObject.SetActive(false);
				_willbe_Label_2.gameObject.SetActive(false);
				_willbe_Label_3.gameObject.SetActive(false);
				_UPwillbe_Label_1.gameObject.SetActive(false);
				_UPwillbe_Label_2.gameObject.SetActive(false);
				_UPwillbe_Label_3.gameObject.SetActive(false);
			}
			else
			{
				DataHero dataHero2 = DataManager.instance.dataHeroGroup.GetHero (_heroData.id, hero.exp, hero.stage + 1);
				DataHeroUpgrade dataHeroUpgrade2 = DataManager.instance.dataHeroGroup.GetHeroUpgrade(_heroData.id,hero.stage + 1);
				_willbe_Label_1.gameObject.SetActive(true);
				_willbe_Label_2.gameObject.SetActive(true);
				_willbe_Label_3.gameObject.SetActive(true);
				_UPwillbe_Label_1.gameObject.SetActive(true);
				_UPwillbe_Label_2.gameObject.SetActive(true);
				_UPwillbe_Label_3.gameObject.SetActive(true);
				_willbe_Label_1.text = ((int)dataHero2.basicParam.hp).ToString();
				_willbe_Label_2.text = ((int)dataHero2.basicParam.damage).ToString();
				_willbe_Label_3.text = ((int)dataHero2.basicParam.ammo).ToString();
				_UPwillbe_Label_1.text = dataHeroUpgrade2.kHP.ToString();
				_UPwillbe_Label_2.text = dataHeroUpgrade2.kAP.ToString();
				_UPwillbe_Label_3.text = dataHeroUpgrade2.kDP.ToString();

			}
			 
			//hitRate
			float hitDodDou = 0.0f;
			string hitDodDouDesc = "";
			if(dataHero1.basicParam.hitRate != 0.0f )
			{
				hitDodDouDesc = "命中";
				hitDodDou = dataHero1.basicParam.hitRate;
			}
			if(dataHero1.basicParam.dodgeRate != 0.0f)
			{
				hitDodDouDesc = "闪避";
				hitDodDou = dataHero1.basicParam.dodgeRate;
			}
			if(dataHero1.basicParam.doubleDamageRate != 0.0f)
			{
				hitDodDouDesc = "暴击";
				hitDodDou = dataHero1.basicParam.doubleDamageRate;
			}
			if(hitDodDou != 0.0f)
			{
				_attribute_Label_4.gameObject.SetActive(true);
				_current_Label_4.text = hitDodDou.ToString();
			}
			else 
			{
				_attribute_Label_4.gameObject.SetActive(false);
			}

		
			if(hero.stage + 1 > MAXSTAGE)
			{
				_willbe_Label_4.gameObject.SetActive(false);
			}
			else
			{
				_willbe_Label_4.gameObject.SetActive(true);
				DataHero dataHero2 = DataManager.instance.dataHeroGroup.GetHero (_heroData.id, hero.exp, hero.stage + 1);
				if(dataHero2.basicParam.hitRate != 0.0f )
				{
					hitDodDouDesc = "命中";
					hitDodDou = dataHero2.basicParam.hitRate;
				}
				if(dataHero2.basicParam.dodgeRate != 0.0f)
				{
					hitDodDouDesc = "闪避";
					hitDodDou = dataHero2.basicParam.dodgeRate;
				}
				if(dataHero2.basicParam.doubleDamageRate != 0.0f)
				{
					hitDodDouDesc = "暴击";
					hitDodDou = dataHero2.basicParam.doubleDamageRate;
				}
				if(hitDodDou != 0.0f)
				{
					_willbe_Label_4.gameObject.SetActive(true);
					_willbe_Label_4.text = hitDodDou.ToString();
				}
				else 
				{
					_willbe_Label_4.gameObject.SetActive(false);
				}
			}
		}

	}

	void OnAdvanced()
	{
		EvolveHero();
	}
	void OnClose()
	{
		
		this.Delete();
		NotificationCenter.instance.DispatchEvent(Notification_Type.SHOWABILITYPANL,new Notification(1));
	}
	// ==========================
	void EvolveHero()
	{
		PBConnect_evolveHero.RESULT r = PBConnect_evolveHero.CheckResult (_heroData.id);
		if (r == PBConnect_evolveHero.RESULT.OK) {
			EvolveHeroRequest request = new EvolveHeroRequest ();
			request.api = new Model_ApiRequest ().api;
			request.heroId = _heroData.id;

			(new PBConnect_evolveHero ()).Send (request, OnEvolveHero);
		} 
		else if(r == PBConnect_evolveHero.RESULT.NEED_LEVEL)
		{
		}
		else if(r == PBConnect_evolveHero.RESULT.NO_HERO)
		{
		}
		else if(r == PBConnect_evolveHero.RESULT.LACK_ITEM)
		{
			UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROPROMPT,_heroData);
		}
		else if(r == PBConnect_evolveHero.RESULT.MAX_STAGE)
		{
		}
	}
	void OnEvolveHero(bool success, System.Object content)
	{
		if (success) {		
			Trace.trace("OnEvolveHero success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnEvolveHero failure",Trace.CHANNEL.UI);
		}
	}

}
