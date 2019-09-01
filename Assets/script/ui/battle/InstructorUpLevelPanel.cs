using UnityEngine;
using System.Collections;

public class InstructorUpLevelPanel : PanelBase {

	private Transform _upLevelContainer;

	private UILabel Level_Label;
	private UILabel Level_Label_2;

	private UILabel Leader_Label;
	private UILabel Leader_Label_2;

	private UILabel Control_Label;
	private UILabel Control_Label_2;

	private UIButton _closeBtn;

	public override void Init ()
	{
		base.Init ();

		_upLevelContainer = transform.Find("upLevelContainer");

		Level_Label = 	_upLevelContainer.Find("Level_Label").GetComponent<UILabel>();
		Level_Label_2 = _upLevelContainer.Find("Level_Label/willBe_ValueLabel").GetComponent<UILabel>();

		Leader_Label = 	_upLevelContainer.Find("Leader_Label").GetComponent<UILabel>();
		Leader_Label_2 = _upLevelContainer.Find("Leader_Label/willBe_ValueLabel").GetComponent<UILabel>();

		Control_Label = 	_upLevelContainer.Find("Control_Label").GetComponent<UILabel>();
		Control_Label_2 = _upLevelContainer.Find("Control_Label/willBe_ValueLabel").GetComponent<UILabel>();

		_closeBtn = _upLevelContainer.Find("closeBtn").GetComponent<UIButton>();
		EventDelegate evclos = new EventDelegate(OnClose);
		_closeBtn.onClick.Add(evclos);
	} 

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);

		Model_User model_User = InstancePlayer.instance.model_User;

		int curLevel = model_User.honorLevel;
		int preLevel = model_User.honorLevel - model_User.honorLevelChanged;

		int curLeadership = Model_User.CalcPlayerUnitCapacity (curLevel);
		int preLeadership = Model_User.CalcPlayerUnitCapacity (preLevel);

		Level_Label.text = "等级:    " + preLevel;
		Level_Label_2.text = "" + curLevel;
	
		Leader_Label.text = "统帅:    " + preLeadership;
		Leader_Label_2.text = "" +curLeadership;

		Control_Label.text = "指挥所上限:    " + preLevel;
		Control_Label_2.text = "" +curLevel;

	}

	void OnClose()
	{
		this.Delete();
	}
}
