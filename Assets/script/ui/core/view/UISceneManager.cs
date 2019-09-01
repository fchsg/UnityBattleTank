using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UISceneManager
{
    protected static UISceneManager _instance;
	public static UISceneManager instance
	{
		get
		{		
			if ( _instance == null)
			{
				_instance = new UISceneManager();
			}
			return _instance;
		}
	}

    private SceneBase _sceneBase = null;
	public SceneBase sceneBase 
	{
		get { return _sceneBase; }
	}


	public SceneBase CreateScene(string name, params System.Object[] parameters)
    {
        if(_sceneBase != null)
        {
			if (name == _sceneBase.baseName)
            {
				return _sceneBase;
            }
        }

		DeleteUIScene ();
		OpenScene(name, parameters);

		return _sceneBase;
    }

	private void OpenScene(string name, params System.Object[] parameters)
    { 	
		GameObject gameObj = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_UI + name);
		_sceneBase = gameObj.GetComponent<SceneBase> ();

		Assert.assert ((_sceneBase != null), "not add " + name + " scripts");

		_sceneBase.Init ();
		_sceneBase.baseName = name;

		_sceneBase.Open (parameters);

		UILayerManager.instance.AddLayer(_sceneBase.gameObject, UILayerManager.UI_LAYER_HIERARCHY.Scene);

    }
	
	public void DeleteUIScene()
    {
	    if (_sceneBase != null)
		{
			_sceneBase.Delete ();
			_sceneBase = null;
		}

		UIPanelManager.instance.DeleteAllPanels ();

		Resources.UnloadUnusedAssets();
		GC.Collect();  
    }

	public void SetActive(bool bActive)
	{
		if (sceneBase != null)
		{
			sceneBase.gameObject.SetActive (bActive);
		} 
		else if(bActive && InstancePlayer.instance.model_User.isLogin)
		{
			UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
		}
	}
}