using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class HeroUpLevelPanel : PanelBase {

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

	private UIGrid _Grid;

	//data
	HeroDataManager.HeroData _heroData;
	List<int> expItemList = new List<int>();

//	void Start () {
//		InitUI();
//	}

	public override void Init ()
	{
		base.Init ();
		InitUI();
	}


	void InitUI()
	{
		_panelContainer = transform.Find("HeroUpLevel_Container");
		_closeBtn = _panelContainer.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_panelName = _panelContainer.Find("panelName_Label").GetComponent<UILabel>();
		_panelName.color = UICommon.FONT_COLOR_GREY;

		_Container = _panelContainer.Find("heroInfo_Item");
		_iconBg = _Container.Find("iconBg").GetComponent<UISprite>();
		_formation_sprite = _Container.Find("formation_sprite").GetComponent<UISprite>();
		_rank_sprite = _Container.Find("rank_sprite").GetComponent<UISprite>();
		_type_Sprite = _Container.Find("type_Sprite").GetComponent<UISprite>();
		_icon_Texture = _Container.Find("iicon_Texture").GetComponent<UITexture>();
		_heroLevel_Label = _Container.Find("heroLevel_Label").GetComponent<UILabel>();
		_heroLevel_Label.color = UICommon.FONT_COLOR_GREY;
		_Timer_Colored_Slider = _panelContainer.Find("Timer_Colored_Slider").GetComponent<UISlider>();
		_expLabel = _panelContainer.Find("Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_expLabel.color = UICommon.FONT_COLOR_ORANGE;
		_Grid = _panelContainer.Find("heroInfo_Container/Grid").GetComponent<UIGrid>();
		_heroName_Label = _Container.Find("heroName_Label").GetComponent<UILabel>();

		// 提升panel renderqueue
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

		UpdateDataUI();
	}

	void UpdateDataUI()
	{
		expItemList.Clear();
		expItemList.Add(100006);
		expItemList.Add(100007);
		expItemList.Add(100008);
		expItemList.Add(100009);

		_Grid.DestoryAllChildren();
		for(int i = 0;i < 4; i++)
		{
			GameObject expItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "hero/EXP_ContainerItem");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,expItem);
			UseExpItem useExpItem =  item.GetComponent<UseExpItem>();
			useExpItem.Init(expItemList[i],_heroData.id);
			item.name = i.ToString();
		}
		_Grid.repositionNow = true;
		_Grid.Reposition();
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
			_rank_sprite.spriteName = _heroData.rankName;
			_type_Sprite.spriteName = "arms_" + (int)_heroData.heroType;

			_heroName_Label.color = _heroData.nameColor;
			_heroName_Label.text = _heroData.data.name; 

			int exp = 0;
			int level = 0;
			Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			Hero hero = model_heroGroup.GetHero(_heroData.id);
			exp = hero.exp;
			level = model_heroGroup.GetCurrentLevel(_heroData.id);
			float heroExpToNextLevel = (float)DataManager.instance.dataHeroGroup.GetHeroExpToNextLevel(_heroData.id,exp);
			float heroTotalExpToNextLevel = (float)DataManager.instance.dataHeroGroup.GetHeroTotalExpToNextLevel(_heroData.id,exp);
			if(heroExpToNextLevel == 0 || heroTotalExpToNextLevel == 0)
			{
				_expLabel.text = "军官已满级";
			}
			if(exp == 0 && level == 1  )
			{
				_Timer_Colored_Slider.value = 0;
				_expLabel.text = 0 + "/" + heroTotalExpToNextLevel;	
			}
			else
			{
				_Timer_Colored_Slider.value = (heroTotalExpToNextLevel - heroExpToNextLevel) / heroTotalExpToNextLevel;
				_expLabel.text = (heroTotalExpToNextLevel - heroExpToNextLevel)  + "/" + heroTotalExpToNextLevel;	
			}
			_heroLevel_Label.text = level.ToString();
		}
	}

	void OnClose()
	{
		this.Delete();
		NotificationCenter.instance.DispatchEvent(Notification_Type.SHOWABILITYPANL,new Notification(1));
	}

}
