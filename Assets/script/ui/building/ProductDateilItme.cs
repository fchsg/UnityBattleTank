using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProductDateilItme : MonoBehaviour {
	private List<Collider> _colliderList = new List<Collider>();// 所有 Button BoxCollider 

	void Start(){
		Trace.trace("Start",Trace.CHANNEL.UI);

		Trace.trace("_colliderList " + _colliderList.Count,Trace.CHANNEL.UI);
//		StartCoroutine (AddBtnListener ());
		Trace.trace("_colliderList " + _colliderList.Count,Trace.CHANNEL.UI);
	}

	IEnumerator AddBtnListener()
	{
		yield return new WaitForSeconds (0.1f);
		Trace.trace("AddBtnListener",Trace.CHANNEL.UI);
		Collider[] triggers = this.GetComponentsInChildren<Collider>(true);
		int count = triggers.Length;
		for (int i = 0; i < count; ++i)
		{
			Collider trigger = triggers[i];
			if (trigger.gameObject.name.StartsWith("Btn") == true) // 以"Btn"开头命名的按钮才会触发OnClick
			{
				UIEventListener listener = UIEventListener.Get(trigger.gameObject);
				listener.onClick = Click;
				_colliderList.Add(trigger);
			}
		}
	}
		
	// 点击按钮
	public void OnClick(GameObject click) {
		if (click.name.Equals ("Btn_Func_Itme_2")) 
		{
			Trace.trace("Btn_Func_Itme_2",Trace.CHANNEL.UI);
		} else if (click.name.Equals ("Btn_Func_Itme_3")) 
		{
			Trace.trace("Btn_Func_Itme_3",Trace.CHANNEL.UI);
		}
	}
	protected void Click(GameObject target)
	{
		OnClick(target);
	}


}
