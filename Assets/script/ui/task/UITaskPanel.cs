using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UITaskPanel : PanelBase {

	private const string TASK_ITEM_PATH = "profab/ui/Task/ui_task_item";

	private const int SHOW_MAX_COUNT = 3;

	//data------------------------------



	//ui---------------------------------
	private UIScrollView scrollview;
	private UIGrid grid;


	// test
	void Start()
	{
	//	Init ();
	//	CreateItems ();
	}

	public override void Init ()
	{
		base.Init ();

		scrollview = UIHelper.FindChildInObject (this.gameObject, "Scroll_View").GetComponent<UIScrollView> ();
		grid = UIHelper.FindChildInObject (this.gameObject, "Grid").GetComponent<UIGrid> ();


		UIHelper.AddBtnClick (this.gameObject, "Closed_Btn", OnClosed);


	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);

		UpdateItems ();
	}
		
	public void UpdateItems()
	{
		List<SlgPB.Task> tasks = InstancePlayer.instance.model_User.model_task.activeTasks;
		int tasksCount = tasks.Count;

		grid.DestoryAllChildren ();

		for (int i = 0; i < tasksCount; ++i) 
		{
			GameObject prefab = Resources.Load (TASK_ITEM_PATH) as GameObject;
			GameObject obj = NGUITools.AddChild (grid.gameObject, prefab);
			grid.AddChild (obj.transform);

			obj.name = UIHelper.GetItemSuffix (i);

			UITaskItem taskItem = obj.GetComponent<UITaskItem> ();
			taskItem.Init (tasks[i], this);
		}
			
		grid.animateSmoothly = false;
		grid.repositionNow = true;

		if (tasksCount <= SHOW_MAX_COUNT) 
		{
			scrollview.enabled = false;
		}

	}

	private void OnClosed()
	{
		base.Delete ();
	}


	// Update is called once per frame
	void Update () {
	
	}
}
