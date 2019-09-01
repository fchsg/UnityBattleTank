using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class msgUnit {

	public class Unit
	{
		public int unitId;
		public int count;

		public Unit(int unitId, int count)
		{
			this.unitId = unitId;
			this.count = count;
		}
	}

	public List<msgUnit.Unit> listUnit = new List<msgUnit.Unit>();

}
