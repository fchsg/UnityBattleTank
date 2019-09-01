using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_evolveHero : PBConnect<EvolveHeroRequest, EvolveHeroResponse> {

	private EvolveHeroRequest _request;

	public PBConnect_evolveHero() : base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "evolveHero.php";
	}

	public override void Send (EvolveHeroRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);
		_request = content;
	}
	
	override protected void DisposeContent(EvolveHeroResponse content)
	{
		int heroId = _request.heroId;
		Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);
		hero.stage++;
		Assert.assert (hero.stage < DataHero.STAGE_MAX);

		DataHeroUpgrade dataHeroUpgrade = DataManager.instance.dataHeroGroup.GetHeroUpgrade (_request.heroId, hero.stage);
		int itemCount = InstancePlayer.instance.model_User.model_itemGroup.UseItem (dataHeroUpgrade.itemId, dataHeroUpgrade.itemCount);



	}
	
	override protected int ValidateContent(EvolveHeroResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public enum RESULT
	{
		OK,
		LACK_ITEM,
		MAX_STAGE,
		NO_HERO,
		NEED_LEVEL,

	}

	public static RESULT CheckResult(int heroId)
	{
		bool hasHero = InstancePlayer.instance.model_User.model_heroGroup.HasHero (heroId);
		if (!hasHero) {
			return RESULT.NO_HERO;
		}

		bool maxStage = InstancePlayer.instance.model_User.model_heroGroup.IsHeroArriveMaxStage (heroId);
		if (maxStage) {
			return RESULT.MAX_STAGE;
		}

		bool hasEnoughChip = InstancePlayer.instance.model_User.model_heroGroup.HasEnoughItemToUpgrade (heroId);
		if (!hasEnoughChip) {
			return RESULT.LACK_ITEM;
		}

		bool hasEnoughLevel = InstancePlayer.instance.model_User.model_heroGroup.HasEnoughLevelToUpgrade (heroId);
		if (!hasEnoughLevel) {
			return RESULT.NEED_LEVEL;
		}

		return RESULT.OK;
	}





}
