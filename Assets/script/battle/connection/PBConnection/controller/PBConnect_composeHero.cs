using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_composeHero : PBConnect<ComposeHeroRequest, ComposeHeroResponse> {

	private ComposeHeroRequest _request;

	public PBConnect_composeHero() : base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "composeHero.php";
	}

	public override void Send (ComposeHeroRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);
		_request = content;
	}

	override protected void DisposeContent(ComposeHeroResponse content)
	{
		InstancePlayer.instance.model_User.model_heroGroup.AddHero (content.hero);

		DataHeroUpgrade dataHeroUpgrade = DataManager.instance.dataHeroGroup.GetHeroUpgrade (_request.heroId, content.hero.stage);
		int itemCount = InstancePlayer.instance.model_User.model_itemGroup.UseItem (dataHeroUpgrade.itemId, dataHeroUpgrade.itemCount);

	}

	override protected int ValidateContent(ComposeHeroResponse content)
	{
		return ValidateApiResponse(content.response);
	}



	public enum RESULT
	{
		OK,
		LACK_ITEM,
		LACK_ALREADY_HAVE,
	}

	public static RESULT CheckResult(int heroId)
	{
		SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);
		bool hasHero = (hero != null);
		if (hasHero) {
			return RESULT.LACK_ALREADY_HAVE;
		}

		if (!InstancePlayer.instance.model_User.model_heroGroup.HasEnoughItemToCompose (heroId)) {
			return RESULT.LACK_ITEM;
		}


		return RESULT.OK;
	}


}
