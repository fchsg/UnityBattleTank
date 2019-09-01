using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_upgradeUnitPart : PBConnect<UpgradeUnitPartRequest, CommonResponse> {

	private UpgradeUnitPartRequest _request;

	public PBConnect_upgradeUnitPart() : base(false)
	{
	}

	public override void Send (UpgradeUnitPartRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}

	override protected string GetUrl()
	{
		return URL_BASE + "UpgradeUnitPart.php"; // 生产
	}
	
	override protected void DisposeContent(CommonResponse content)
	{
		Model_Unit unit = InstancePlayer.instance.model_User.unlockUnits [_request.unitId];
		unit.partLevels [_request.partIndex]++;

		InstancePlayer.instance.model_User.UpdateUserBasic(content.response.user);

	}
	
	override protected int ValidateContent(CommonResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	// ==================================================================
	// helper

	public enum RESULT
	{
		OK,
		UNIT_LOCK,
		MAX_LEVEL,
		LACK_MAIN_LEVEL,
		LACK_PLAYER_LEVEL,
		LACK_ITEM1,
		LACK_ITEM2,
		LACK_RESOURCE,
	}

	public static RESULT CheckUpgrade(int unitId, int partIndex)
	{
		if (!InstancePlayer.instance.model_User.unlockUnits.ContainsKey (unitId)) {
			return RESULT.UNIT_LOCK;
		}

		Model_Unit modelUnit = InstancePlayer.instance.model_User.unlockUnits [unitId];
		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);

		int partId = dataUnit.partsId [partIndex];
		int partLevel = modelUnit.partLevels[partIndex];
		int mainPartLevel = modelUnit.partLevels[0];

		DataUnitPart dataUnitPart = DataManager.instance.dataUnitPartGroup.GetPart (partId, partLevel);


		int maxLevel = DataManager.instance.dataUnitPartGroup.GetPartMaxLevel (partId);
		if (partLevel >= maxLevel) {
			return RESULT.MAX_LEVEL;
		}

		if (partIndex == 0) {
			int playerLevel = InstancePlayer.instance.model_User.honorLevel;
			if (playerLevel < dataUnitPart.mainLevel) {
				return RESULT.LACK_PLAYER_LEVEL;
			}
		} else {
			if(mainPartLevel < dataUnitPart.mainLevel)
			{
				return RESULT.LACK_MAIN_LEVEL;
			}
		}

		int itemCount1 = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount (dataUnitPart.itemCost1.id);
		if (itemCount1 < dataUnitPart.itemCost1.count) {
			return RESULT.LACK_ITEM1;
		}

		int itemCount2 = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount (dataUnitPart.itemCost2.id);
		if (itemCount2 < dataUnitPart.itemCost2.count) {
			return RESULT.LACK_ITEM2;
		}


		bool hasEnoughResource = Model_Helper.HasEnoughResource (dataUnitPart.cost);
		if (!hasEnoughResource) {
			return RESULT.LACK_RESOURCE;
		}


		return RESULT.OK;

	}




	private static PBConnect_upgradeUnitPart.DelegateConnectCallback _upgradeCallback = null;
	private static int _upgradeTimesLeft = 0;
	private static int _upgradeUnitId = 0;
	private static int _upgradePartIndex = 0;

	private static int _originalPartLevel;

	private static int _upgradedLevels;
	public static int upgradedLevels
	{
		get { return _upgradedLevels; }
	}

	public static void UpdateMultyTimes(PBConnect_upgradeUnitPart.DelegateConnectCallback callback, int times,
	                                    int unitId, int partIndex)
	{
		Assert.assert (_upgradeCallback == null);


		_upgradeCallback = callback;

		Assert.assert (times > 0);
		_upgradeTimesLeft = times;

		_upgradeUnitId = unitId;
		_upgradePartIndex = partIndex;


		_originalPartLevel = InstancePlayer.instance.model_User.unlockUnits[unitId].partLevels[partIndex];
		_upgradedLevels = 0;

		UpgradeUnitPart ();
	}

	private static void UpgradeUnitPart()
	{
		UpgradeUnitPartRequest request = new UpgradeUnitPartRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = _upgradeUnitId;
		request.partIndex = _upgradePartIndex;
		
		(new PBConnect_upgradeUnitPart ()).Send (request, OnUpgradeUnitPart);
		
	}

	private static void OnUpgradeUnitPart(bool success, System.Object content)
	{
		if (success) {
			if(--_upgradeTimesLeft > 0)
			{
				UpgradeUnitPart();
			}
		} else {
		}

		Model_Unit unit = InstancePlayer.instance.model_User.unlockUnits [_upgradeUnitId];
		_upgradedLevels = unit.partLevels[_upgradePartIndex] - _originalPartLevel;

		PBConnect_upgradeUnitPart.DelegateConnectCallback cb = _upgradeCallback;
		if(_upgradeTimesLeft <= 0)
		{
			_upgradeCallback = null;	
		}

		cb(success, content);

	}

}
