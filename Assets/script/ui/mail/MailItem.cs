using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class MailItem : MonoBehaviour {
	private Transform _parent;
	private UILabel _from_Label;
	private UILabel _fromName_Label;
	private UILabel _title_Label;
	private UILabel _titleValue_Label;
	private UIToggle _bg_btn;
	private UIButton _btn;
	private UISprite _icon;
	public UISprite _iconLine;
	private UISprite _attachment_Sprite;
	private UILabel _validity_Label;
	private UILabel _validityValue_Label;
	private UILabel _date_Label;

	//data 
	public SlgPB.Notify _notify;
	Model_NotificationGroup _notifyGroup;
	public bool IsChecked = false;
	void Awake()
	{
		_parent = this.transform;
		_from_Label = _parent.Find("from_Label").GetComponent<UILabel>();
		_from_Label.color = UICommon.FONT_COLOR_ORANGE;
		_fromName_Label = _parent.Find("fromName_Label").GetComponent<UILabel>();
		_fromName_Label.color = UICommon.FONT_COLOR_ORANGE;
		_title_Label = _parent.Find("title_Label").GetComponent<UILabel>();
		_title_Label.color = UICommon.FONT_COLOR_GREY;
		_titleValue_Label = _parent.Find("titleValue_Label").GetComponent<UILabel>();
		_titleValue_Label.color = UICommon.FONT_COLOR_GREY;

		_btn = _parent.Find("bg_btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_btn,OnSeleted);
		_bg_btn = _parent.Find("bg_btn").GetComponent<UIToggle>();
//		EventDelegate.Add(_bg_btn.onChange,OnToggle);
		_validity_Label = _parent.Find("validity_Label").GetComponent<UILabel>();
		_validity_Label.gameObject.SetActive(false);
		_validityValue_Label = _parent.Find("validity_Label/Label").GetComponent<UILabel>();
		_date_Label = _parent.Find("date_Label").GetComponent<UILabel>();
		_date_Label.color = UICommon.FONT_COLOR_GREEN;
		_icon = _parent.Find("icon").GetComponent<UISprite>();
		_iconLine = _parent.Find("line").GetComponent<UISprite>();
		_attachment_Sprite = _parent.Find("attachment_Sprite").GetComponent<UISprite>();
	}
	void Start () {
	
	}
	public void Init(SlgPB.Notify notify)
	{
		if(notify != null)
		{
			_notify = notify;

		}
	}
	// Update is called once per frame
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		if(_notify != null)
		{
			_notifyGroup = InstancePlayer.instance.model_User.model_notificationGroup;
			_fromName_Label.text = _notify.senderName;
			_titleValue_Label.text = _notify.title;
			System.DateTime time = GetTime(_notify.sendTime.ToString());
			_date_Label.text =  time.ToString("yyyy/MM/dd");
			if(_notifyGroup.IsRead(_notify.notifyId))
			{
				_icon.spriteName = "mail_Icon_1";
			}
			else
			{
				_icon.spriteName = "mail_Icon_0";
			}
			_attachment_Sprite.gameObject.SetActive(_notifyGroup.HasBonus(_notify.notifyId));
			_iconLine.gameObject.SetActive(IsChecked);
		}
	}
	void OnToggle()
	{
//		bool check = UIToggle.current.value;
//		if(check)
//		{
////			_notifyGroup.MarkRead(_notify.notifyId);
//			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshMailItem,new Notification(_notify));
//		}

	}
	void OnSeleted()
	{
		IsChecked = true;
		_notifyGroup.MarkRead(_notify.notifyId);
		NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshMailItem,new Notification(_notify));
	}

	private System.DateTime GetTime(string timeStamp)
	{
		System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		long lTime = long.Parse(timeStamp + "0000000");
		System.TimeSpan toNow = new System.TimeSpan(lTime); 
		return dtStart.Add(toNow);
	}
}
