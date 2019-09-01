using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIPanelManager
{
    private static UIPanelManager _instance;
    public static UIPanelManager instance
    {
		get
		{
			if (null == _instance)
			{
				_instance = new UIPanelManager ();
			}
			return _instance;
		}  
    }

	UIDepthManager _depthManager = new UIDepthManager(UICommon.UI_PANEL_DEPTH);
	
	private Dictionary<string, PanelBase> _panels = new Dictionary<string, PanelBase>();
	public Dictionary<string, PanelBase> panels
	{
		get { return _panels; }
	}

	public PanelBase GetPanel(string panelName)
	{
		if (_panels.ContainsKey (panelName)) 
		{
			return _panels[panelName];
		}

		return null;
	}

	public PanelBase CreatePanel(string panelName, params System.Object[] parameters)
    {
        if (_panels.ContainsKey(panelName))
        {
			PanelBase panelBase = _panels[panelName];
			panelBase.Open(parameters);
	
			return panelBase;
        }
        else
        {
			return InitPanel(panelName, parameters);
        }  
    }

	private PanelBase InitPanel(string name, params System.Object[] parameters)
	{
		GameObject gameObj = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_UI + name);
		PanelBase panelBase = gameObj.GetComponent<PanelBase> ();

		Assert.assert ((panelBase != null), "not add " + name + " scripts");

		panelBase.baseName = name;
		panelBase.Init();
			
		_panels.Add(panelBase.baseName, panelBase);	
		UILayerManager.instance.AddLayer (gameObj, UILayerManager.UI_LAYER_HIERARCHY.Panel);
		panelBase.Open (parameters);

		return panelBase;
	}

	private void SetActive(PanelBase panelBase, bool isActive)
	{
		SetDepth (panelBase, isActive);
		NGUITools.SetActive (panelBase.gameObject, isActive);

		return;
	}

	public void SetDepth(PanelBase panelBase, bool isActive)
	{
		if (isActive) 
		{
			_depthManager.AddPanelDepth (panelBase.rootTransform);
		}
		else 
		{
			_depthManager.RemovePanelDepth(panelBase.rootTransform);
		}

		return;
	}
	
	public void DestoryPanel(PanelBase panelBase)
	{
		if (_panels.ContainsKey(panelBase.baseName))
		{
			_panels.Remove(panelBase.baseName); 
			_depthManager.RemovePanelDepth(panelBase.rootTransform);
		}
		return;
	}

	public void DeleteAllPanels()
	{
		if (_panels != null) 
		{
			List<string> keys = new List<string> (_panels.Keys);
			foreach (string key in keys) 
			{
				if(panels[key] != null)
				{
					panels[key].DeleteImmediately();
				}
			}
			_panels.Clear();
		}
	}

	public bool IsTopPanel(PanelBase targetPanel)
	{
		bool result = true;
		int targetDepth = targetPanel.gameObject.GetComponent<UIPanel> ().depth;

		if (_panels.Count > 0) 
		{
			foreach(PanelBase panelbase in _panels.Values)
			{
				int sourceDepth = panelbase.gameObject.GetComponent<UIPanel> ().depth;

				if (sourceDepth > targetDepth) 
				{
					result = false;
				}
			}
		}

		return result;
	}

}
