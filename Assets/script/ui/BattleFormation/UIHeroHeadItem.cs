using UnityEngine;
using System.Collections;

public class UIHeroHeadItem : MonoBehaviour {

	public UISprite _heroSprite;
	public UILabel _nameLabel;

	public int _initDepth;

	public int _heroId;

	void Awake () 
	{
		_heroSprite = transform.Find ("Hero_Head_Sprite").GetComponent<UISprite> ();
		_nameLabel = transform.Find ("Name_Label").GetComponent<UILabel> ();

		_initDepth = _heroSprite.depth;
	}

	public void UpdateUI(SlgPB.Hero hero)
	{
		int heroId = hero.heroId;
		_heroId = heroId;

		DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, hero.exp, hero.stage);

		_heroSprite.spriteName = UICommon.HERO_HEAD_ICON_PATH + hero.heroId;
		_nameLabel.text = dataHero.name;

		this.GetComponent<UIHeroBodyItem> ().heroId = heroId;
	}

	public void UpdateUI(int heroId)
	{
		_heroId = heroId;

		SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);
		DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, hero.exp, hero.stage);

		_heroSprite.spriteName = UICommon.HERO_HEAD_ICON_PATH + hero.heroId;
		_nameLabel.text = dataHero.name;

		this.GetComponent<UIHeroBodyItem> ().heroId = heroId;
	}

	public void IncreaseDepth()
	{
		_heroSprite.depth = _initDepth * 100;
	}

	public void ResetDepth()
	{
		_heroSprite.depth = _initDepth;
	}

}
