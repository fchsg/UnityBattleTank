using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDepthManager {

	private int currentMaxDepth; // 当前最高depth
	public UIDepthManager(int startDepth)
	{
		currentMaxDepth = startDepth;
	}
	
	private class DepthPanel
	{
		public int originDepth; 
		public UIPanel panel;
		public DepthPanel(int depth, UIPanel panel)
		{
			this.originDepth = depth;
			this.panel = panel;
		}
	}

	private class DepthPanelCollection
	{
		public Transform transform;              // 父Panel
		public List<DepthPanel> depthList;       // 父Panel下的所有panel深度数据列表

		public DepthPanelCollection(Transform transform)
		{
			this.transform = transform;
			depthList = new List<DepthPanel>();
		}

		public void Add(DepthPanel depthPanel)
		{
			depthList.Add (depthPanel);
		}	
	}
	
	List<DepthPanelCollection> DepthPanelList = new List<DepthPanelCollection> ();

	public void AddPanelDepth (Transform parent)
	{
		if(DepthPanelList.Count > 0)
		{
			if(DepthPanelList[DepthPanelList.Count-1].transform == parent)
			{
				return;
			}
		}

		currentMaxDepth += UICommon.UI_DEPTH_INCREMENTAL;

		DepthPanelCollection depthPanelCollection = new DepthPanelCollection (parent);

		UIPanel[] panels = parent.GetComponentsInChildren<UIPanel> (true);

		foreach(UIPanel panel in panels)
		{
			if(panel != null)
			{
				DepthPanel depthPanel =new DepthPanel(panel.depth, panel);
				depthPanelCollection.Add(depthPanel);

				panel.depth += currentMaxDepth;
			}
		}

		DepthPanelList.Add(depthPanelCollection);
	}
	
	public void RemovePanelDepth(Transform root)
	{
		int index = -1;
		for(int i = 0; i < DepthPanelList.Count; i++)
		{
			if(DepthPanelList[i].transform == root)
			{
				index = i;
				break;
			}
		}
	
		if(index != -1)
		{
			foreach(DepthPanel depthPanel in DepthPanelList[index].depthList)
			{
				depthPanel.panel.depth = depthPanel.originDepth;
			}

			DepthPanelList.RemoveAt(index);

			currentMaxDepth -= UICommon.UI_DEPTH_INCREMENTAL;
		}

		if(DepthPanelList.Count == 0)
		{
			currentMaxDepth = UICommon.UI_PANEL_DEPTH;
		}
	}

}
