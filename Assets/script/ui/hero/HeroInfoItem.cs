using UnityEngine;
using System.Collections;
using SlgPB;
public class HeroInfoItem : MonoBehaviour {

	private Transform _Container;
	private UISprite _iconBg;
	private UISprite _formation_sprite;
	private UISprite _rank_sprite;
	private UISprite _type_Sprite;
	private UITexture _icon_Texture;
	private UIButton _icon_Btn;
	private UILabel _heroLevel_Label;
	private UILabel _heroName_Label;

	private UIButton _manager_Btn;
	private UIButton _obtain_Btn;
	private UIButton _recruiting_Btn;
	private UILabel _manager_label;
	private UILabel _obtain_label;
	private UILabel _recruiting_label;
	private UILabel _value_Label;
	private UISprite _item_Sprite;

	//data
	HeroDataManager.HeroData _heroData;

	public void Init(HeroDataManager.HeroData heroData)
	{
		_heroData = heroData;
	}
	void Start () {
	
	}
	void UpdateUI()
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
			switch(_heroData.recruitType)
			{
			case HeroDataManager.RecruitType.ALREADYRECRUIT:
				_manager_Btn.gameObject.SetActive(true);
				_obtain_Btn.gameObject.SetActive(false);
				_recruiting_Btn.gameObject.SetActive(false);
				break;
			case HeroDataManager.RecruitType.CANRECRUIT:
				_manager_Btn.gameObject.SetActive(false);
				_obtain_Btn.gameObject.SetActive(false);
				_recruiting_Btn.gameObject.SetActive(true);
				break;
			case HeroDataManager.RecruitType.UNABLERECRUIT:
				_manager_Btn.gameObject.SetActive(false);
				_obtain_Btn.gameObject.SetActive(true);
				_recruiting_Btn.gameObject.SetActive(false);
				break;
			}
			_value_Label.text = _heroData.heroUpgradeData.itemCount.ToString();
			if(_heroData.recruitType == HeroDataManager.RecruitType.ALREADYRECRUIT)
			{
				Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
				_heroLevel_Label.text = model_heroGroup.GetCurrentLevel(_heroData.id).ToString();
				_rank_sprite.spriteName = UICommon.HERO_RANK_ICON_PATH + model_heroGroup.GetCurrentStage(_heroData.id);
			}
			else
			{
				_rank_sprite.spriteName = _heroData.rankName;
				_heroLevel_Label.text = _heroData.level.ToString();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI();
	}
	void OnIcon()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROINFO,_heroData);
	}
	void OnManager()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROINFO,_heroData);
	}
	//获取
	void OnObtain()
	{
		ItemDataManager itemManger = new ItemDataManager();
		ItemDataManager.ItemData itemData = itemManger.getItemDataByID(_heroData.heroUpgradeData.itemId);
		UIController.instance.CreatePanel(UICommon.UI_PANEL_NORMALPROP,itemData);
	}
	//招募
	void OnRecruiting()
	{
		ComposeHero();
	}
	void Awake()
	{
		_Container = this.transform;
		_iconBg = _Container.Find("iconBg").GetComponent<UISprite>();
		_formation_sprite = _Container.Find("formation_sprite").GetComponent<UISprite>();
		_rank_sprite = _Container.Find("rank_sprite").GetComponent<UISprite>();
		_type_Sprite = _Container.Find("type_Sprite").GetComponent<UISprite>();
		_icon_Texture = _Container.Find("iicon_Texture").GetComponent<UITexture>();
		_icon_Btn = _Container.Find("iicon_Texture").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_icon_Btn,OnIcon);
		_heroLevel_Label = _Container.Find("heroLevel_Label").GetComponent<UILabel>();
		_heroLevel_Label.color = UICommon.FONT_COLOR_GREY;

		_manager_Btn = _Container.Find("manager_Btn").GetComponent<UIButton>();
		_manager_label = _Container.Find("manager_Btn/Label").GetComponent<UILabel>();
		_obtain_Btn = _Container.Find("obtain_Btn").GetComponent<UIButton>();
		_obtain_label = _Container.Find("obtain_Btn/Label").GetComponent<UILabel>();
		_recruiting_Btn = _Container.Find("recruiting_Btn").GetComponent<UIButton>();
		_recruiting_label = _Container.Find("recruiting_Btn/Label").GetComponent<UILabel>();
		_value_Label = _Container.Find("recruiting_Btn/value_Label").GetComponent<UILabel>();
		_item_Sprite = _Container.Find("recruiting_Btn/item_Sprite").GetComponent<UISprite>();	
		_heroName_Label = _Container.Find("heroName_Label").GetComponent<UILabel>();
		_manager_label.color = UICommon.FONT_COLOR_GOLDEN;
		_obtain_label.color = UICommon.FONT_COLOR_GOLDEN;
		_recruiting_label.color = UICommon.FONT_COLOR_GOLDEN;
		_value_Label.color = UICommon.FONT_COLOR_ORANGE;
		UIHelper.AddBtnClick(_manager_Btn,OnManager);
		UIHelper.AddBtnClick(_obtain_Btn,OnObtain);
		UIHelper.AddBtnClick(_recruiting_Btn,OnRecruiting);
	}
	// ============================
	void ComposeHero()
	{ 
		PBConnect_composeHero.RESULT r = PBConnect_composeHero.CheckResult (_heroData.id);
		if (r == PBConnect_composeHero.RESULT.OK) {
			ComposeHeroRequest request = new ComposeHeroRequest ();
			request.api = new Model_ApiRequest ().api;
			request.heroId = _heroData.id;

			(new PBConnect_composeHero ()).Send (request, OnComposeHero);
		} else {
			 
		}
	}
	void OnComposeHero(bool success, System.Object content)
	{
		if (success) {		
			Trace.trace("OnComposeHero success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnComposeHero failure",Trace.CHANNEL.UI);

		}
	}

}
