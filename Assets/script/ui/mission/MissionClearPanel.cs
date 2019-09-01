using UnityEngine;
using System.Collections;

public class MissionClearPanel : PanelBase {

	// UI---------------------------
	private UILabel _Left_Label;

	// data------------------------

	private int magicId;
	private int clear_count;


	//test
	void Awake()
	{
		//Init ();
	}

	public override void Init ()
	{
		base.Init ();

		// 关闭
		UIButton closedBtn = UIHelper.FindChildInObject (this.gameObject, "Bg").GetComponent<UIButton> ();
		UIHelper.AddBtnClick (closedBtn, OnClosed);

		//  扫荡1次
		UIButton BtnOne = UIHelper.FindChildInObject (this.gameObject, "Btn_One").GetComponent<UIButton> ();
		UIHelper.AddBtnClick (BtnOne, MissionClearOne);
		// 扫荡10次
		UIButton BtnTen = UIHelper.FindChildInObject (this.gameObject, "Btn_Ten").GetComponent<UIButton> ();
		UIHelper.AddBtnClick (BtnTen, MissionClearTen);

		_Left_Label = UIHelper.FindChildInObject (this.gameObject, "Left_Label").GetComponent<UILabel> ();
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if (parameters != null) 
		{
			magicId	= (int)parameters [0];

			int left_count = InstancePlayer.instance.model_User.model_level.GetMission (magicId).remainFightNum;
			_Left_Label.text = left_count.ToString ();
		}
	}

	public void MissionClearOne()
	{
		MissionClearCallBack (1);
		clear_count = 1;
	}
		
	public void MissionClearTen()
	{
		MissionClearCallBack (10);
		clear_count = 10;
	}

	public void MissionClearCallBack(int count)
	{
		BattleConnection.instance.SrartClearBattle (magicId, count, MultiFightCallback, this.gameObject);
	}

	public  void MultiFightCallback(bool success, System.Object content)
	{
		if (success) 
		{
			OnClosed ();
			UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION_ClEAR_RESULT, magicId, clear_count);
		}
		else
		{
			Trace.trace ("扫荡请求失败", Trace.CHANNEL.UI);
		}
	}


	public void OnClosed()
	{
		base.Delete ();
	}
}
