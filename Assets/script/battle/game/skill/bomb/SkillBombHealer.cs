using UnityEngine;
using System.Collections;

public class SkillBombHealer : SkillBombBase {

	override protected float speed
	{
		get { return -13; }
	}

	override protected void Bomb()
	{
		string name = AppConfig.FOLDER_PROFAB + "skill/HealArea";
		GameObject healArea = ResourceHelper.Load (name);

		Vector3 pos = transform.position;
		pos.y = 0;
		healArea.transform.position = pos;

//		float range = _skill.GetDataSkill ().effectRange;
//		healArea.transform.localScale = new Vector3 (range, 1, range);

		healArea.GetComponent<SkillHealer> ().init (_skill);

	}

}
