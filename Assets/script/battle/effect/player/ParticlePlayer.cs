using UnityEngine;
using System.Collections;

public class ParticlePlayer : MonoBehaviour {


	public BattleConfig.LAYER layer = BattleConfig.LAYER.NONE;

	private float lifeCycleSec;
	private bool isLoop = false;

	void Start () 
	{
		BattleFactory.AddUnitToLayer (gameObject, layer);

		lifeCycleSec =  ParticleHelper.GetlifeCycleSec (transform);
		if (lifeCycleSec < 0) 
		{
			isLoop = true;
		}
	}
	
	void Update () 
	{
		if (!isLoop) 
		{
			lifeCycleSec -= TimeHelper.deltaTime;
			if (lifeCycleSec < 0) 
			{
				Destroy(gameObject);
			}
		}
	}

}
