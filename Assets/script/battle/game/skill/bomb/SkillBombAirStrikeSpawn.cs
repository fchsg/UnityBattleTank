using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBombAirStrikeSpawn : SkillBombBase {

	private bool _isStart = false;

	private const float DROP_INTERVAL = 0.2f;
	private const int COUNT = 10;

	List<SkillBombBase> spawnBombs = new List<SkillBombBase>();

	override protected void Update()
	{
		for(int i = spawnBombs.Count - 1; i >= 0; --i)
		{
			if(spawnBombs[i].isOver)
			{
				GameObject.DestroyImmediate(spawnBombs[i].gameObject);
				spawnBombs.RemoveAt(i);
			}
		}

		if (spawnBombs.Count == 0 && _isStart)
		{
			_isOver = true;
		}
	}

	override public void Init(InstanceSkill skill, Vector3 targetPosition)
	{
		_skill = skill;
		_targetPosition = targetPosition;

		StartCoroutine (createBombs());
	}

	IEnumerator createBombs()
	{
		DataSkill dataSkill = _skill.GetDataSkill ();
		float range = dataSkill.hintRange;

		List<Vector3> offsets = new List<Vector3> ();
		int n = COUNT;
		for (int i = 0; i < n; ++i) {
			float theta = RandomHelper.Range01();
			float radius = Mathf.Sqrt( RandomHelper.Range01() * RandomHelper.Range01()) * range;
				
			float x = radius * Mathf.Cos(2 * Mathf.PI * theta);
			float z = radius * Mathf.Sin(2 * Mathf.PI * theta);
			Vector3 v = new Vector3(x, 0, z);

			offsets.Add(v);
		}

		/*
		Vector3[] dropPositionOffsets = new Vector3[]{
			
			new Vector3 (-range * 0.8f, 0, range * 0.2f),
			new Vector3 (-range * 0.8f, 0, -range * 0.2f),
			new Vector3 (0, -1000, 0),
			
			new Vector3 (-range * 0.4f, 0, range * 0.4f),
			new Vector3 (-range * 0.4f, 0, 0),
			new Vector3 (-range * 0.4f, 0, -range * 0.4f),
			new Vector3 (0, -1000, 0),

			new Vector3 (0, 0, +range * 0.6f),
			new Vector3 (0, 0, +range * 0.2f),
			new Vector3 (0, 0, -range * 0.2f),
			new Vector3 (0, 0, -range * 0.6f),
			new Vector3 (0, -1000, 0),

			new Vector3 (range * 0.4f, 0, range * 0.4f),
			new Vector3 (range * 0.4f, 0, 0),
			new Vector3 (range * 0.4f, 0, -range * 0.4f),
			new Vector3 (0, -1000, 0),
			
			new Vector3 (range * 0.8f, 0, range * 0.2f),
			new Vector3 (range * 0.8f, 0, -range * 0.2f),
			new Vector3 (0, -1000, 0),
			
		};
		*/

		for (int i = 0; i < offsets.Count; i++) 
		{
			Vector3 offset = offsets[i];

			GameObject bomoObject = ResourceHelper.Load(AppConfig.FOLDER_PROFAB + "skill/BombAirStrike");
			BattleFactory.AddUnitToLayer(bomoObject, BattleConfig.LAYER.BULLET);

			SkillBombBase bomb = bomoObject.GetComponent<SkillBombBase>();
			bomb.Init(_skill, _targetPosition + offset);
			
			spawnBombs.Add(bomb);

			yield return new WaitForSeconds(DROP_INTERVAL);
			/*
			if(offset.y < 0)
			{
				yield return new WaitForSeconds(DROP_INTERVAL_LONG);
			}
			else
			{
				yield return new WaitForSeconds(DROP_INTERVAL_SHORT);
			}
			*/
		}
		
		_isStart = true;
	}



}
