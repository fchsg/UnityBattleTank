using UnityEngine;
using System.Collections;

public class TankEngine {

	private Unit _unit;

	private ParticleSystem[] _particleSmoke = new ParticleSystem[2];
	private ParticleSystem _particleEngine;
	
	private float _speedScale = 1;
	public float speedScale
	{
		get { return _speedScale; }
	}
	
	private float _speedScaleV = 0;
	public float speedScaleV
	{
		get { return _speedScaleV; }
	}
	
	public TankEngine(Unit unit)
	{
		_unit = unit;

		Transform smoke1 = _unit.transform.Find ("particleSmoke1");
		if (smoke1 != null) {
			smoke1.transform.localPosition = new Vector3(_unit.unit.dataUnit.GetRadius(), 0, 0);

			ParticleSystem particle = smoke1.GetComponentInChildren<ParticleSystem> ();
			particle.enableEmission = false;
			_particleSmoke [0] = particle;
		}

		Transform smoke2 = _unit.transform.Find ("particleSmoke2");
		if (smoke2 != null) {
			smoke2.transform.localPosition = new Vector3(-_unit.unit.dataUnit.GetRadius(), 0, 0);

			ParticleSystem particle = smoke2.GetComponentInChildren<ParticleSystem> ();
			particle.enableEmission = false;
			_particleSmoke [1] = particle;
		}

		Transform engine = _unit.transform.Find ("particleEngine");
		if (engine != null) {
			engine.transform.localPosition = new Vector3(0, 1.5f, -_unit.unit.dataUnit.GetRadius());

			_particleEngine = engine.GetComponentInChildren<ParticleSystem> ();
			_particleEngine.enableEmission = false;
		}

	}
	
	public void Stop(bool immediately)
	{
		
		if (immediately) {
			_speedScale = 0;
			_speedScaleV = 0;
		} else {
			_speedScaleV = -1 / _unit.unit.dataUnit.breakTime;
		}
	}
	
	public void Resume(bool immediately)
	{
		if (immediately) {
			_speedScale = 1;
			_speedScaleV = 0;
		} else {
			_speedScaleV = 1 / (_unit.unit.dataUnit.breakTime * 2);
		}
	}
	
	public void Break()
	{
		_speedScaleV = -1 / _unit.unit.dataUnit.breakTime;
	}
	
	public void Update()
	{
		_speedScale += _speedScaleV * TimeHelper.deltaTime;
		_speedScale = MathHelper.Clip (_speedScale, 0, 1);

		for (int i = 0; i < _particleSmoke.Length; ++i) {
			ParticleSystem particle = _particleSmoke[i];
			if(particle != null)
			{
				if (_speedScale < 0.3f) {
//					particle.Stop();
					particle.enableEmission = false;
				}
				else
				{
//					particle.Play();
					particle.enableEmission = true;
				}
			}
		}

		if (_particleEngine != null) {
			if(_unit.unit.currentHp < _unit.unit.complexBattleParam.hp * 0.6f)
			{
//				_particleEngine.Play();
				_particleEngine.enableEmission = true;
			}
			else
			{
//				_particleEngine.Stop();
				_particleEngine.enableEmission = false;
			}
		}

	}

	public bool IsStopped()
	{
		return _speedScale <= 0;
	}

	public void StopEffect()
	{
		for (int i = 0; i < _particleSmoke.Length; ++i) {
			ParticleSystem particle = _particleSmoke[i];
			if(particle != null)
			{
				particle.enableEmission = false;
			}
		}
		
		if (_particleEngine != null) {
			_particleEngine.enableEmission = false;
		}

	}

}
