using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIParticleManager {

	private static UIParticleManager _instance;
	public  static UIParticleManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new UIParticleManager ();
			}
			return _instance;
		}
	}

	private List<GameObject> particle_List = new List<GameObject>();

	public void AddParticle(GameObject obj)
	{
		particle_List.Add (obj);
	}

	public void DestoryParticle()
	{
		if (particle_List != null && particle_List.Count > 0)
		{
			for (int i = particle_List.Count - 1; i >= 0; --i) 
			{
				GameObject obj = particle_List [i];
				if (obj != null) 
				{
					GameObject.DestroyImmediate (obj);
				}
			}
		}
		particle_List.Clear ();
	}


}
