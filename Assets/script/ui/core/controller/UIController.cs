using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController {

	private static UIController _instance;
	public static UIController instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new UIController();
			}
			return _instance;
		}
	}

	// scene
	public SceneBase CreateScene(string name, params System.Object[] parameters)
	{
		return UISceneManager.instance.CreateScene(name, parameters);
	}

	public T GetCurrentScene<T>() where T : SceneBase
	{
		T sceneBase =  UISceneManager.instance.sceneBase as T;
		return sceneBase;
	}

	// panel
	public PanelBase CreatePanel(string name, params System.Object[] parameters)
	{
		return UIPanelManager.instance.CreatePanel (name, parameters);
	}

	public void ClosedPanel(string name)
	{
		PanelBase panelBase = UIPanelManager.instance.GetPanel (name);
		if (panelBase != null) 
		{
			panelBase.Closed();
		}
	}

	public void DeletePanel(string name)
	{
		PanelBase panelBase = UIPanelManager.instance.GetPanel (name);
		if (panelBase != null) 
		{
			panelBase.Delete();
		}
	}

	public void DeleteImmediatelyPanel(string name)
	{
		PanelBase panelBase = UIPanelManager.instance.GetPanel (name);
		if (panelBase != null) 
		{
			panelBase.DeleteImmediately();
		}
	}



	public T GetPanel<T>(string name) where T : PanelBase
	{
		T panelBase = UIPanelManager.instance.GetPanel (name) as T;
		return panelBase;
	}
}
