using UnityEngine;

public class UnitFire {

	public class ShootParam
	{
		public Unit attacker = null;
		public Unit target = null;
		public float damage = 0;
		public bool isMiss = false;
		public bool isDoubleDamage = false;
	}

	private Unit _unit;

	private long _lastAttackTime = 0;
	private int _shootTimes;
	private long _lastShootTime;

	private bool _isAttacking = false;
	public bool isAttacking
	{
		get { return _isAttacking; }
	}
	
//	private TankSpineAttach _spineAttach;
	private const bool ALWAYS_MISS = false;
	
	public UnitFire(Unit unit)
	{
		_unit = unit;
//		_spineAttach = unit.GetComponent<TankSpineAttach> ();
	}

	public void Update()
	{
		if (IsTargetAvailable()) {
			if(_unit.targetSelect.IsTargetInShootRange(currentTarget)
			   && IsAimmingTo(currentTarget.target.transform.position))
			{
				CheckCD ();
			}

			UpdateFire ();
		} else {
			AttackEnd();
		}

	}

	private bool IsTargetAvailable()
	{
		return _unit.targetSelect.IsTargetAvailable ();
	}

	private UnitTargetSelect.RESULT currentTarget
	{
		get { return _unit.targetSelect.currentTarget; }
	}
	
	private bool IsAimmingTo(Vector3 point)
	{
		float dif = AimmingControl.CalcRotationTo(
			_unit.launcher.transform.position, point, _unit.launcher.transform.rotation.eulerAngles.y);
		return dif < 1;
	}

	private void CheckCD()
	{
		if (!_isAttacking) {

			long ct = TimeHelper.GetCurrentTimestampScaled();
			long dt = ct - _lastAttackTime;
			if (dt >= _unit.unit.dataUnit.shootCD * 1000) {
				_isAttacking = true;

//				_lastAttackTime = ct;
				_shootTimes = 0;
				_lastShootTime = ct;

				if(_unit.unit.dataUnit.stopToFire)
				{
					_unit.unitDriver.Stop(false);
					float delay = 
						_unit.unitDriver.GetEstimateBreakingTime() +
							_unit.unit.dataUnit.firePrepareTime;
					_lastShootTime += (long)delay * 1000;
				}
			}
		}
	}

	private void UpdateFire()
	{
		if (_isAttacking) 
		{
			long ct = TimeHelper.GetCurrentTimestampScaled();
			long dt = ct - _lastShootTime;
			float interval = _unit.unit.dataUnit.fireInterval * 1000;
			if (dt >= interval)
			{
				Shoot();
				_lastAttackTime = ct;
				_lastShootTime = ct;

				++_shootTimes;
				if(_shootTimes >= _unit.unit.dataUnit.fireCount)
				{
					AttackEnd();
				}
			}
		}
	}

	private void AttackEnd()
	{
		_isAttacking = false;
		
		if(_unit.unit.dataUnit.stopToFire)
		{
			_unit.unitDriver.Resume(false);
		}
	}

	private void Shoot()
	{
		ShootParam shoot = new ShootParam ();
		shoot.attacker = _unit;
		shoot.target = currentTarget.target;
		shoot.isMiss = IsMiss ();
		shoot.isDoubleDamage = IsDoubleDamage ();
		shoot.damage = CalcDamage (shoot.isDoubleDamage);
		BattleFactory.CreateBattleBullet (_unit.unit.dataUnit.bulletType, shoot);

		_unit.ToFire ();
	}

	private bool IsMiss()
	{
		if (ALWAYS_MISS) 
		{
			return true;
		}
		float random = RandomHelper.Range01();
		
		float a = _unit.unit.complexBattleParam.hitRate;
		float d = currentTarget.target.unit.complexBattleParam.dodgeRate;
		float DODGE_EFFECT_RATE = 0.25f;
		float rate = a * (1 - d * DODGE_EFFECT_RATE);
		
		bool isMiss = random > rate;
		return isMiss;
	}

	private bool IsDoubleDamage()
	{
		bool isDoubleDamage = false;
		
		InstanceUnit instanceUnit = _unit.unit;
		float randRate = RandomHelper.Range01 ();
		
		if (randRate < instanceUnit.complexBattleParam.doubleDamageRate) {
			isDoubleDamage = true;		
		}
		
		return isDoubleDamage;
	}

	private float CalcDamage(bool doubleDamage)
	{
		float power = _unit.unit.complexBattleParam.damage;
		float ammo = currentTarget.target.unit.complexBattleParam.ammo;
		
		float damage = power * power / (power + ammo);
		if (damage < 1) {
			damage = 1;
		}
		damage *= _unit.unit.GetLiveCount ();

		if (doubleDamage) {
			float DOUBLE_DAMAGE_RATE = 2;
			damage *= DOUBLE_DAMAGE_RATE;
		}
		
		Assert.assert(damage >= 0);
		damage = Mathf.Min (currentTarget.target.unit.currentHp, damage);
		
		return damage;
	}
	
}
