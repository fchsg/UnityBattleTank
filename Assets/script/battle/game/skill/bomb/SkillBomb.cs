using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBomb : SkillBombBase {

	public GameObject explode;

	private GameObject _shadow;

	// Use this for initialization
	override protected void Start () {
		base.Start ();
		
		string name = AppConfig.FOLDER_PROFAB + "skill/BombShadow";
		_shadow = ResourceHelper.Load (name);
	}

	// Update is called once per frame
	override protected void Update () {
		base.Update ();

		if (!isOver) {
			Vector3 p = GeometryHelper.ProjectPointToPlane (transform.position, 0, AppConfig.SHADOW_DIRECTION);
			_shadow.transform.position = p;
		}
	}


	override protected void Bomb()
	{
		DestroyImmediate (_shadow);
		_shadow = null;

		CreateDamage ();
		CreateEffect ();

	}

	private void CreateDamage()
	{
		List<Unit> enemies = BattleGame.instance.unitGroup.enemyUnits;
		foreach (Unit tank in enemies) {
			if(!tank.isDead)
			{
				if(UnitHelper.IsTouchEdge(tank, transform.position, _skill.GetDataSkill().effectRange))
				{
					UnitFire.ShootParam param = new UnitFire.ShootParam ();
					param.damage = _skill.GetDataSkill().effect;
					param.target = tank;
					
					new BulletDamage(param, transform.position);
				}

			}
		}

	}

	private void CreateEffect()
	{
		AudioGroup.Play (GetComponent<AudioGroup> ().hit, gameObject);

		if (BattleGame.instance.mapCamera.cameraControl.IsPointInsideCamera (gameObject.transform.position)) {
			BattleGame.instance.mapCamera.cameraControl.Shake ();
		}

		GameObject effectObj = GameObject.Instantiate(explode) as GameObject;
		RenderHelper.ChangeWholeModelColor (effectObj, Color.red);
		effectObj.transform.position = new Vector3(transform.position.x, 1, transform.position.z);

	}

}
