using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class UILayerManager : MonoBehaviour
{
    private static UILayerManager _instance;
    public static UILayerManager instance
    {
		get
		{
			if(_instance == null)
			{
				_instance = new GameObject("_UILayerManager").AddComponent<UILayerManager>();
			}
			return _instance;
		}
    }

	public enum UI_LAYER_HIERARCHY
	{
		Scene = 100,  // 场景
		Panel = 200,  // 弹框 
		Tips  = 300,  // 提示
	}

	private Dictionary<UI_LAYER_HIERARCHY, GameObject> _layerDic;
	private GameObject _parent;

	void Awake()
	{
		InitLayers ();
	}

	public void AddLayer(GameObject obj, UI_LAYER_HIERARCHY type)
	{
		obj.transform.parent = _layerDic[type].transform;
		ResetGameObject (obj);
	}

	private void InitLayers()
	{	
		_parent = GameObject.Find("UI Root");
		_layerDic = new Dictionary<UI_LAYER_HIERARCHY, GameObject> ();

		int nums = Enum.GetNames(typeof(UI_LAYER_HIERARCHY)).Length;
		for (int i = 0; i < nums; ++i)
		{
			UI_LAYER_HIERARCHY objType = (UI_LAYER_HIERARCHY)Enum.GetValues(typeof(UI_LAYER_HIERARCHY)).GetValue(i);
			GameObject objValue = CreateLayerGameObject(objType.ToString(), objType);
			objValue.layer = _parent.layer;
			_layerDic.Add(objType, objValue);
		}
	}

	private GameObject CreateLayerGameObject(string name, UI_LAYER_HIERARCHY type)
    {
        GameObject layer = new GameObject(name);
        layer.transform.parent = _parent.transform;
        layer.transform.localPosition = new Vector3(0,0,((int)type) * -1);
        layer.transform.localEulerAngles = Vector3.zero;
        layer.transform.localScale = Vector3.one;
        return layer;
    }
	
	private void ResetGameObject(GameObject obj)
	{
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;		
	}

	void OnDestroy()
	{
		_instance = null;
	}

}