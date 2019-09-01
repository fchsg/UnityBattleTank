using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillHealer : MonoBehaviour {

	private bool _over = false;

	private InstanceSkill _skill;
	private float _life;
	private long _lastActiveTimestamp = 0;

	private const float HEAL_INTERVAL = 1;

	// Use this for initialization
	void Start () {
		BattleFactory.AddUnitToLayer (gameObject, BattleConfig.LAYER.EFFECT);

	}
	
	// Update is called once per frame
	void Update () {
		if (_over) {
			return;
		}


		long ct = TimeHelper.GetCurrentTimestampScaled ();
		long dt = ct - _lastActiveTimestamp;
		if (dt >= HEAL_INTERVAL * 1000) {
			_lastActiveTimestamp = ct;

			Active();
			CreateEffect();
		}

		_life -= TimeHelper.deltaTime;
		if (_life <= 0) {
			_over = true;
			DestroyImmediate(gameObject);
		}

	}

	public void init(InstanceSkill skill)
	{
		_skill = skill;
		_life = skill.GetDataSkill ().duration;
	}

	private void Active()
	{
		List<Unit> tanks = BattleGame.instance.unitGroup.myUnits;
		foreach (Unit tank in tanks) {
			if(!tank.isDead)
			{
				if(UnitHelper.IsTouchEdge(tank, transform.position, _skill.GetDataSkill().effectRange))
				{
					tank.BeHeal(_skill.GetDataSkill().effect);
				}
			}
		}

	}

	private void CreateEffect()
	{
	}

}
