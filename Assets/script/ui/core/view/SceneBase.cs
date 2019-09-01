using UnityEngine;
using System.Collections;

public class SceneBase : UIBase
{
	override public void Init()
	{
		base.Init ();
		UIParticleManager.instance.DestoryParticle ();
	}

	override public void Delete()
	{
		base.Delete ();
	}

	override public void Open(params System.Object[] parameters) {}
	
	override public void Closed() {}
	
	override public void Reset() {}
}