using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class MainCityBtnItem : MonoBehaviour {
	private UIButton _btn;
	private UILabel _label;
	private UISprite _labelSp;
	//data
	MainCityBtnData _btnData;

	void Awake()
	{
		_btn = transform.Find("FunctionBtn_bg/Function_1_Btn").GetComponent<UIButton>();
		EventDelegate evbtn = new EventDelegate(onFunction);
		_btn.onClick.Add(evbtn);
		_label = transform.Find("FunctionBtn_bg/Function_Label").GetComponent<UILabel>();
		_labelSp = transform.Find("FunctionBtn_bg/Function_Label_1").GetComponent<UISprite>();
	}
	void Start () {

	}

	public void InitData(MainCityBtnData btnData)
	{
		_btnData = btnData;
		_label.text = _btnData.btnConfig.btnName;
		_btn.normalSprite = _btnData.btnConfig.btnIcon;
		_labelSp.spriteName = "main_label_" + btnData.btnID;
	}
	// Update is called once per frame
	void Update () {
	
	}

	void onFunction()
	{
		if(_btnData != null)
		{
			switch(_btnData.btnID)
			{
				case 0:
				//"任务"
				OnClickeTask();//战斗结算
					break;
				case 1:
				//"部队"
					OnClickFormation();
					break;
				case 2:
				//	"将领"
				UIController.instance.CreatePanel(UICommon.UI_PANEL_HERO);
					break;
				case 3:
				//"邮件"
				UIController.instance.CreatePanel(UICommon.UI_PANEL_MAIL);
				//				UIController.instance.CreatePanel(UICommon.UI_PANEL_STRENGTHEN);
					break;
				case 4:
				//"仓库"
				UIController.instance.CreatePanel(UICommon.UI_PANEL_WAREHOUSE);
					break;
				case 5:
				//"设置"

					break;
				case 6:
				//"仓库"
					break;
				case 7:
					break;
			}
		}
	}
	//阵型响应事件
	public void OnClickFormation(){
		Trace.trace ("阵型响应事件",Trace.CHANNEL.UI);
		UIController.instance.CreatePanel (UICommon.UI_PANEL_FORMATION);
	}
 
	
	public void OnClickeTask()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TASK);
	}

	//========================================


}
