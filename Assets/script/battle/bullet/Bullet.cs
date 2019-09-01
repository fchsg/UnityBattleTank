using UnityEngine;

public class Bullet : MonoBehaviour {

	protected bool _isOver = false;
	protected UnitFire.ShootParam _shoot;

	protected Vector3 startPosition;
	protected Vector3 targetOffset;

	protected Vector3 lastPosition;

	public bool shaking = false;
	public GameObject explodeProfab;
	public GameObject explodeMissProfab;

	public const int FIRE_HEIGHT = 2;


	// Use this for initialization
	void Start () {

	}


	// ==========================================================================
	// create flow

	public void Create (UnitFire.ShootParam shoot)
	{
		_shoot = shoot;

		ShootBegan ();
	}

	protected virtual void ShootBegan()
	{
		startPosition = GetShootPos ();
		targetOffset = GetTargetOffset ();

		this.transform.position = startPosition;
		transform.rotation = _shoot.attacker.launcher.transform.rotation;
		lastPosition = startPosition;


		AudioGroup audioGroup = GetComponent<AudioGroup> ();
		if (audioGroup != null) {
			AudioGroup.Play(audioGroup.fire, gameObject);
		}

	}


	protected Vector3 GetShootPos()
	{
		Vector3 p;
		bool result = _shoot.attacker.GetFireMousePosition (out p);
		if (result) {
			return p;
		} else {
			return _shoot.attacker.transform.position + new Vector3 (0, FIRE_HEIGHT, 0);
		}
	}
	
	protected Vector3 GetTargetOffset()
	{
		Vector3 offset = CalcNearerOffset ();

		if (_shoot.isMiss) {
			offset = CalcMissOffset ();
		} else if (shaking) {
			offset = CalcShakeOffset();
		}

		return offset;
	}

	// ==========================================================
	// miss

	protected Vector3 CalcMissOffset()
	{
		float r = _shoot.target.unit.dataUnit.GetCollisionRadius ();
		Vector3 v = _shoot.attacker.transform.position - _shoot.target.transform.position;
		VectorHelper.ResizeVector (ref v, r);

		Vector3 offset = GeometryHelper.RotateVector (v, 0, RandomHelper.Range (-60, 60), 0);
		return offset;
		
	}
	

	/*
	protected Vector3 CalcMissOffset()
	{
		if (RandomHelper.Range01 () > 0.5f) {
			return CalcMissOffset_sidely ();
		} else {
			return CalcMissOffset_nearly ();
		}
	}

	protected Vector3 CalcMissOffset_sidely()
	{
		float r = _shoot.target.unit.dataUnit.GetRadius ();
		r = (RandomHelper.Range01 () > 0.5f) ? r : -r;

		Vector3 v = _shoot.target.transform.position - _shoot.attacker.transform.position;
		Vector3 vp = new Vector3 (v.z, 0, -v.x);
		VectorHelper.ResizeVector (ref vp, r);

		Vector3 offset = CalcRandomNearFarOffset ();
		offset += vp;
		return offset;
		
	}
	
	protected Vector3 CalcRandomNearFarOffset()
	{
		float r = _shoot.target.unit.dataUnit.GetCollisionRadius ();
		r = RandomHelper.Range (-r, r);
		
		Vector3 v = _shoot.attacker.transform.position - _shoot.target.transform.position;
		VectorHelper.ResizeVector (ref v, r);
		
		return v;
	}


	protected Vector3 CalcMissOffset_nearly()
	{
		float r = _shoot.target.unit.dataUnit.GetCollisionRadius ();
		
		Vector3 v = _shoot.attacker.transform.position - _shoot.target.transform.position;
		VectorHelper.ResizeVector (ref v, r);

		v += CalcRandomSideOffset ();
		return v;
	}

	protected Vector3 CalcRandomSideOffset()
	{
		float r = _shoot.target.unit.dataUnit.GetRadius ();
		r = RandomHelper.Range (-r, r);

		Vector3 v = _shoot.target.transform.position - _shoot.attacker.transform.position;
		Vector3 vp = new Vector3 (v.z, 0, -v.x);
		VectorHelper.ResizeVector (ref vp, r);

		return vp;
		
	}
	*/


	// ==========================================================
	// hit
	

	protected Vector3 CalcNearerOffset()
	{
		float r = _shoot.target.unit.dataUnit.GetHurtRadius ();
		
		Vector3 v = _shoot.attacker.transform.position - _shoot.target.transform.position;
		VectorHelper.ResizeVector (ref v, r);

		return v;
	}


	// ==========================================================
	// shake
	

	protected Vector3 CalcShakeOffset()
	{
		float r = _shoot.target.unit.dataUnit.GetRadius ();
		
		float ox = RandomHelper.Range(-r, r);
		float oz = RandomHelper.Range(-r, r);
		Vector3 offset = new Vector3(ox, 0, oz);
		return offset;
	}


	// ==========================================================
	// helper
	

	protected void CorrectDirection () {
		if (lastPosition != transform.position) {
			Vector3 direction = AppConfig.DEFAULT_DIRECTION;// UnitHelper.GetOrientation(transform);
			Vector3 toDirection = transform.position - lastPosition;
			Quaternion q = Quaternion.FromToRotation(direction, toDirection);
			transform.rotation = q;
		}

		lastPosition = transform.position;

	}

	protected virtual void ShootEnd()
	{
		Assert.assert (!_isOver);
		_isOver = true;

//		Unit unit = _attackTank.GetComponent<Unit> ();
//		unit.DeleteBulletReference ();

		/*
		if (_shoot.isMiss) {
			Assert.assert(_shoot.damage == 0);
		}

		new BulletDamage (_attackTank, _shoot);
		if (!_shoot.isMiss) {
			GameObject targetTank = UnitHelper.GetTank (_shoot.target);
			CreateEffect (targetTank);
		}
		*/

		CreateEffect ();

		CalcDamage ();


		if (_shoot.isMiss) {
			AudioGroup audioGroup = GetComponent<AudioGroup> ();
			if (audioGroup != null) {
				AudioGroup.Play(audioGroup.miss, gameObject);
			}
		} else {
			AudioGroup audioGroup = GetComponent<AudioGroup> ();
			if (audioGroup != null) {
				AudioGroup.Play(audioGroup.hit, gameObject);
			}
		}


		DestroyImmediate (this.gameObject);
	}

	protected void CalcDamage()
	{
		if (!_shoot.isMiss) {
			new BulletDamage (_shoot, transform.position);
		} else {
			BattleFactory.CreateHUDMissText(_shoot.target.gameObject);
		}

	}

	protected void CreateEffect()
	{
		GameObject profab = !_shoot.isMiss ? explodeProfab: explodeMissProfab;
		if (profab == null) {
			profab = explodeProfab;
		}

		if (profab != null) {
//			GameObject effectObj = ResourceHelper.Load(AppConfig.FOLDER_PROFAB_ANIMATION + "testExplode/testExplode");
			GameObject effectObj = Instantiate(profab) as GameObject;
			BattleFactory.AddUnitToLayer(effectObj, BattleConfig.LAYER.EFFECT);
//			RenderHelper.ChangeWholeModelColor (effectObj, Color.green);
			effectObj.transform.position = transform.position;
		}

	}


	protected Vector3 GetCurrentDestination()
	{
		Vector3 destination = targetOffset + _shoot.target.transform.position;
		return destination;
	}




}
