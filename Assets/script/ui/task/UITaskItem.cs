using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITaskItem : MonoBehaviour 
{
	private enum BtnType
	{
		ALLOWED = 0,  	 //可以领取			
		NOT_ALLOWED = 1, // 不可领取
		TURN_PAGE = 2,    // 跳转页面
	}

	private  string[] UI_BTN_NAME = {"领取奖励", "不可领取", "前往"}; 

	//data--------------------------

	private int _taskId;
	public int taskId
	{
		get
		{
			return _taskId;
		}
	}

	private string str_progress_formation = "任务进度：（{0}/{1}）";
	private string str_reward_formation = "{0}：{1}  ";

	private UITaskPanel _taskPanel;

	//ui--------------------------
	public UILabel btn_label;


	public void Init(SlgPB.Task pbTask, UITaskPanel taskPanel)
	{
	   _taskId = pbTask.taskId;
		_taskPanel = taskPanel;

		int cur_progress = pbTask.progress;

		DataTask dataTask = DataManager.instance.dataTaskGroup.GetTask (_taskId);

		int total_progress = dataTask.totalProgress;
		int itemGroupId = dataTask.itemGroupID;

		bool isComplete = InstancePlayer.instance.model_User.model_task.IsTaskComplete (_taskId);

		//icon
		UISprite icon = UIHelper.FindChildInObject(this.gameObject, "task_item").GetComponent<UISprite>();
		icon.spriteName = UICommon.TASK_ITEM_ICON + dataTask.icon;

		//name
		UILabel name = UIHelper.FindChildInObject(this.gameObject, "Name_Label").GetComponent<UILabel>();
		name.text = dataTask.name;

		//descript
		UILabel descript = UIHelper.FindChildInObject(this.gameObject, "Description_Label").GetComponent<UILabel>();
		descript.text = dataTask.descript;

		// reward
		UILabel item = UIHelper.FindChildInObject(this.gameObject, "Item_Label").GetComponent<UILabel>();
		item.text = GetRewardDescription (itemGroupId);

		//progess
		UILabel progress = UIHelper.FindChildInObject(this.gameObject, "Progress_Label").GetComponent<UILabel>();
		progress.text = string.Format (str_progress_formation, cur_progress, total_progress);

		// btn name
		btn_label = UIHelper.FindChildInObject (this.gameObject, "Btn_Task/Label").GetComponent<UILabel>();
		if (isComplete) {
			btn_label.text = UI_BTN_NAME [0];
		} else {
			btn_label.text = UI_BTN_NAME [1];
		}

		// btn click
		UIHelper.AddBtnClick (this.gameObject, "Btn_Task", BtnClick);
	}

	public void BtnClick()
	{
		PBConnect_drawTaskReward.USE_ITEM_RESULT reslult = PBConnect_drawTaskReward.DrawTask(RewardCallBack, _taskId);

		switch (reslult) 
		{
		case PBConnect_drawTaskReward.USE_ITEM_RESULT.OK:
			{
				
			}
			break;
		case PBConnect_drawTaskReward.USE_ITEM_RESULT.NOT_COMPLETE:
			{
				UIHelper.ShowTextPromptPanel (_taskPanel.gameObject, "任务未完成");
			}
			break;
		}
	
	}


	public void RewardCallBack(bool success, System.Object content)
	{
		if (success)
		{
			UIHelper.ShowTextPromptPanel (_taskPanel.gameObject, "领取成功");

			// 刷新列表
			_taskPanel.UpdateItems();

		}
		else
		{
			UIHelper.ShowTextPromptPanel (_taskPanel.gameObject, "领取失败");
		}
	}

		
	private string GetRewardDescription(int itemGroupId)
	{
		DataDropGroup.DropCollection dropCollection = DataManager.instance.dataDropGroup.GetDropCollection (itemGroupId);
		List<SlgPB.PrizeItem> dropGroup = dropCollection.ToPrizeItems ();

		string dropDescript = "";
		foreach (SlgPB.PrizeItem prizeItem in dropGroup) 
		{
			UIDropItem item = UIHelper.GetUIDropItemByPrizeItem (prizeItem);
			dropDescript += string.Format (str_reward_formation, item.name, item.count);
		}

		return dropDescript;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
