using UnityEngine;
using System.Collections;

public class ParticleHelper {

	public static float GetlifeCycleSec(Transform transform)
	{
		ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
		float maxDuration = 0;

		foreach(ParticleSystem ps in particleSystems)
		{
			if(ps.enableEmission)
			{
				if(ps.loop)
				{

					return -1f;
				}

				float dunration = 0f;
				if(ps.emissionRate <=0)
				{
					dunration = ps.startDelay + ps.startLifetime;
				}
				else
				{
					dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
				}

				if (dunration > maxDuration) 
				{
					maxDuration = dunration;
				}
			}
		}

		return maxDuration;
	}

}
