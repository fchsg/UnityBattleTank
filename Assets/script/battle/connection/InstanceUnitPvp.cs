using UnityEngine;
using System.Collections;

public class InstanceUnitPvp : InstanceUnit {

	public SlgPB.Hero pbHero;
	public SlgPB.Unit pbUnit;

	public InstanceUnitPvp(bool isPlayerUnit, int heroId, int unitId, int unitCount, float powerScale, SlgPB.Hero pbHero, SlgPB.Unit pbUnit)
		:base(isPlayerUnit, heroId, unitId, unitCount, powerScale)
	{
		this.pbHero = pbHero;
		this.pbUnit = pbUnit;
	}

	public override void Init ()
	{
		complexBattleParam = new DataUnit.BasicBattleParam ();
		complexBattleParam.Copy (dataUnit.battleParam);

		Model_Unit modelUnit = new Model_Unit ();
		modelUnit.Parse (pbUnit);
		DataUnitPart[] dataParts = modelUnit.GetDataParts ();
		foreach (DataUnitPart part in dataParts) {
			complexBattleParam.Add(part.battleParam);
		}

		if (heroId > 0) {
			DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, pbHero.exp, pbHero.stage);
			complexBattleParam.Add(dataHero.basicParam);
		}

		complexBattleParam.damage *= powerScale;
		complexBattleParam.ammo *= powerScale;
		complexBattleParam.hp *= unitCount;
		currentHp = complexBattleParam.hp;

	}

}
