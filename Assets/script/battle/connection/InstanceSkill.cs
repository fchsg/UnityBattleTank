using UnityEngine;
using System.Collections;

public class InstanceSkill {

	public DataSkill.TYPE type;
	public int level;
	public int count;

	public InstanceSkill(DataSkill.TYPE type, int level, int count)
	{
		this.type = type;
		this.level = level;
		this.count = count;
	}

	public DataSkill GetDataSkill()
	{
		return DataManager.instance.dataSkillGroup.GetSkill(type, level);
	}

}
