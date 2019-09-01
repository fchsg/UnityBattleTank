using UnityEngine;
using System.Collections;

public class UIHeroBodyItem : MonoBehaviour {

	public int heroId;

	public UISprite _heroSprite;
	public UILabel _nameLabel;
	public UISprite _dutySprite;


	void Awake ()
	{
		_heroSprite = transform.Find ("Hero_Body_Sprite").GetComponent<UISprite> ();
		_nameLabel = transform.Find ("Name_Label").GetComponent<UILabel> ();
		_dutySprite = transform.Find ("Hero_Body_Sprite/Duty_Sprite").GetComponent<UISprite> ();

		UIHelper.AddBtnClick (GetComponent<UIButton> (), HeroDetialPanelCB);
	}
	
	public void UpdateUI(UIHeroCategory.Hero heroCategory)
	{
		SlgPB.Hero pb_hero = heroCategory.pb_Hero;
		heroId = pb_hero.heroId;

		DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (pb_hero.heroId, pb_hero.exp, pb_hero.stage);

		_heroSprite.spriteName = UICommon.HERO_BODY_ICON_PATH + pb_hero.heroId;
		_nameLabel.text = dataHero.name;

		SetOnDutyVisible (heroCategory.isOnDuty);
	}

	public void SetOnDutyVisible(bool isVisible)
	{
		_dutySprite.gameObject.SetActive (isVisible);
	}

	// 点击进入hero 详情页面
	public void HeroDetialPanelCB()
	{
		UIHeroDragItem dragUI = GetComponent<UIHeroDragItem> ();
		if (dragUI.state == UIHeroDragItem.STATE.SCROLLVIEW) 
		{
			HeroDataManager dataHeroManager = new HeroDataManager ();
			DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive (heroId);
			HeroDataManager.HeroData uiHeroData = dataHeroManager.InitHeroData (dataHero);

			UIController.instance.CreatePanel(UICommon.UI_PANEL_HEROUPLEVEL,uiHeroData);
		}
	}
}
