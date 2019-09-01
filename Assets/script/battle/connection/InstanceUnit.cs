using UnityEngine;
using System.Collections;

public class InstanceUnit {

	public bool isPlayerUnit;

	public int heroId;

	private DataUnit _dataUnit;
	public DataUnit dataUnit
	{
		get { return _dataUnit; }
	}

	public int unitId;
	public int unitCount;
	public float powerScale = 1;

	public DataUnit.BasicBattleParam complexBattleParam;
	public float currentHp;

	public InstanceUnit(bool isPlayerUnit, int heroId, int unitId, int unitCount, float powerScale)
	{
		this.isPlayerUnit = isPlayerUnit;
		this.heroId = heroId;
		this.unitId = unitId;
		this.unitCount = unitCount;
		this.powerScale = powerScale;

		_dataUnit = DataManager.instance.dataUnitsGroup.GetUnit(unitId);
	}

	virtual public void Init()
	{
		complexBattleParam = new DataUnit.BasicBattleParam ();
		complexBattleParam.Copy (_dataUnit.battleParam);

		if (isPlayerUnit) {
			if (InstancePlayer.instance.model_User.isLogin) {
				Model_Unit modelUnit = InstancePlayer.instance.model_User.unlockUnits [unitId];
				DataUnitPart[] dataParts = modelUnit.GetDataParts ();
				foreach (DataUnitPart part in dataParts) {
					complexBattleParam.Add(part.battleParam);
				}
			}

			if (heroId > 0) {
				SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, hero.exp, hero.stage);
				complexBattleParam.Add(dataHero.basicParam);
			}
		}

		complexBattleParam.damage *= powerScale;
		complexBattleParam.ammo *= powerScale;
		complexBattleParam.hp *= unitCount;
		currentHp = complexBattleParam.hp;

	}

	public bool IsAlive()
	{
		return currentHp > 0;
	}

	public int GetLiveCount()
	{
		int c = (int)Mathf.Ceil (currentHp / complexBattleParam.hp * unitCount);
		return c;
	}

	public int GetDeadCount()
	{
		return unitCount - GetLiveCount();
	}

}
