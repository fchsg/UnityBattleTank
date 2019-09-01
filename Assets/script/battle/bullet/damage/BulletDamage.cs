using UnityEngine;
using System.Collections.Generic;

public class BulletDamage {

	public BulletDamage(UnitFire.ShootParam shoot, Vector3 position)
	{
		if (shoot.attacker!= null && shoot.attacker.unit.dataUnit.damageRange > 0) {

			List<Unit> enemy = BattleGame.instance.unitGroup.GetOpponents(shoot.attacker.team);

			foreach(Unit u in enemy)
			{
				Vector3 v = u.transform.position - position;
				if(v.magnitude <= shoot.attacker.unit.dataUnit.damageRange)
				{
					u.BeHurt(shoot);

				}
			}

		} else {
			shoot.target.BeHurt(shoot);

		}

	}

}
