using UnityEngine;
using System.Collections;

public class ResearchSuccessPanel : PanelBase {

	private UIButton _closeBtn;
	private UIButton _okBtn;
	private UISprite _icon;
	private UISprite _iconBg;
	private UILabel _tankName;
	//data
	TankDataManager.UnitData _unitData;

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length > 0)
		{
			_unitData = parameters[0] as TankDataManager.UnitData;
		}
	}
	// Use this for initialization
	void Start () {
		_iconBg.spriteName = _unitData.iconBgName;
		_icon.spriteName = _unitData.iconName;
		_tankName.color = _unitData.nameColor;
		_tankName.text = _unitData.unitData.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClose()
	{
		this.Delete();
	}
	void OnOK()
	{
		this.Delete();
	}
	void Awake()
	{
		_closeBtn = transform.Find("effect_Container/CloseBtn").GetComponent<UIButton>();
		_okBtn = transform.Find("effect_Container/okBtn").GetComponent<UIButton>();
		EventDelegate evclos = new EventDelegate(OnClose);
		_closeBtn.onClick.Add(evclos);
		EventDelegate evok = new EventDelegate(OnOK);
		_okBtn.onClick.Add(evok);
		_iconBg = transform.Find("effect_Container/icon_bg").GetComponent<UISprite>();
		_icon = _iconBg.gameObject.transform.Find("icon_Sprite").GetComponent<UISprite>();
		_tankName = transform.Find("effect_Container/tankName_Label").GetComponent<UILabel>();
	}
}
