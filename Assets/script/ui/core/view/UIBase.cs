using UnityEngine;
using System;
using System.Collections.Generic;

public class UIBase : MonoBehaviour 
{
	private string _baseName;
	public string baseName 
	{
		set { _baseName = value;}
		get { return _baseName; }
	}

	private Transform _rootTransform = null;
	public Transform rootTransform
	{
		get{ return _rootTransform; }
	}
	
	virtual public void Init()
	{
		_rootTransform = this.transform;
	}

	virtual public void Delete() 
	{
		StopAllCoroutines();
		GameObject.Destroy (this.gameObject);
	}

	virtual public void Open(params System.Object[] parameters) {}

	virtual public void Closed() {}

	virtual public void Reset() {}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
	

}