using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class PromptPanel : PanelBase {
	 
	private Transform _Panel;
	private UILabel _prompt;
	private UILabel _describe_Label;
	private UILabel _need_Label;
	private UILabel _needValue_Label;
	private UILabel _current_Label;
	private UILabel _currentValue_Label;
	private UIButton _cancel_Btn;
	private UILabel _cancelLabel;
	private UIButton _ok_Btn;
	private UILabel _okLabel;

	//data
	HeroDataManager.HeroData _heroData;

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length > 0)
		{
			_heroData = parameters[0] as HeroDataManager.HeroData;
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdataUI();
	}
	void UpdataUI()
	{
		if(_heroData != null)
		{
			Model_HeroGroup model_heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			Hero hero = model_heroGroup.GetHero(_heroData.id);
			DataHeroUpgrade dataHeroUpgrade1 = DataManager.instance.dataHeroGroup.GetHeroUpgrade(_heroData.id,hero.stage);
			 
			Item item = InstancePlayer.instance.model_User.model_itemGroup.QueryItem(dataHeroUpgrade1.itemId);
			_currentValue_Label.text = item.num.ToString();
			_needValue_Label.text = dataHeroUpgrade1.itemCount.ToString();
		}

	}
	void OnOK()
	{
		OnClose();
	}
	void OnCancel()
	{
		OnClose();
	}
	void OnClose()
	{
		this.Delete();
	}
	void Awake()
	{
		_Panel = transform.Find("Prompt_Container");
		_prompt = _Panel.Find("labelBg/Label").GetComponent<UILabel>();
		_prompt.color = UICommon.FONT_COLOR_GREY;
		_describe_Label = _Panel.Find("panelName_Label").GetComponent<UILabel>();
		_describe_Label.color = UICommon.FONT_COLOR_GREY;
		_need_Label = _Panel.Find("need_Label").GetComponent<UILabel>();
		_need_Label.color = UICommon.FONT_COLOR_GREY;
		_needValue_Label = _Panel.Find("need_Label/Label").GetComponent<UILabel>();
		_needValue_Label.color = UICommon.FONT_COLOR_GREEN;
		_current_Label = _Panel.Find("current_Label").GetComponent<UILabel>();
		_current_Label.color = UICommon.FONT_COLOR_GREY;
		_currentValue_Label = _Panel.Find("current_Label/Label").GetComponent<UILabel>();
		_currentValue_Label.color = UICommon.FONT_COLOR_RED;
		_cancel_Btn = _Panel.Find("cancel_Btn").GetComponent<UIButton>();
		_ok_Btn = _Panel.Find("ok_Btn").GetComponent<UIButton>();
		_cancelLabel = _cancel_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_okLabel = _ok_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_cancelLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_okLabel.color = UICommon.FONT_COLOR_GOLDEN;
		UIHelper.AddBtnClick(_ok_Btn,OnOK);
		UIHelper.AddBtnClick(_cancel_Btn,OnCancel);
	}
}
