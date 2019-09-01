using UnityEngine;
using System.Collections;

public class StrengthenWrapManager : MonoBehaviour {

	public GameObject _wrap;
	UIWrapContent _wrapScript;
	public GameObject _box;
	
	// Use this for initialization
	void Awake()
	{
		//获取脚本
		_wrapScript = _wrap.GetComponent<UIWrapContent>();
		
		//绑定方法
		_wrapScript.onInitializeItem = OnUpdateItem;
	}
	
	
	// Update is called once per frame
	void Update()
	{
		//Test
		if (Input.GetKeyDown(KeyCode.A))
		{
			_wrapScript.minIndex = -3;
			_wrapScript.maxIndex = 2;
			
			//启用脚本
			_wrapScript.enabled = true;
			
			
		}
	}
	
	void OnUpdateItem(GameObject go, int index, int realIndex)
	{
		Trace.trace("index = " + index,Trace.CHANNEL.UI);
		Trace.trace("realIndex = " + realIndex,Trace.CHANNEL.UI);
		StrengthenTankItem tb = go.GetComponent<StrengthenTankItem>();
//		tb.SetNumber(realIndex.ToString());
	}
}
