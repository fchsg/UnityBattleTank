using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class MailInfo : MonoBehaviour {
	
	private Transform _parent;
	private UILabel _from_Label;
	private UILabel _fromName_Label;
	private UILabel _title_Label;
	private UILabel _titleValue_Label;
	private UILabel _playName_Label;
	private UILabel _info_Label;
	private UIButton _delete_Btn;
	private UIButton _get_Btn;
	private UILabel _delete_Label;
	private UILabel _get_Label;
	private UIGrid _Grid;

	//data 
	SlgPB.Notify _notify;
	Model_NotificationGroup _model_notificationGroup;
	Dictionary<int,MailPrizeItem> _mailItemDic = new Dictionary<int, MailPrizeItem>();
	int[] ids = new int[1];
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
		_playName_Label = _parent.Find("playName_Label").GetComponent<UILabel>();
		_info_Label = _parent.Find("info_Label").GetComponent<UILabel>();
		_delete_Btn = _parent.Find("delete_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_delete_Btn,OnClickDeleteMail);
		_get_Btn = _parent.Find("get_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_get_Btn,OnGet);
		_delete_Label = _parent.Find("delete_Btn/Label").GetComponent<UILabel>();
		_get_Label = _parent.Find("get_Btn/Label").GetComponent<UILabel>();
		_delete_Label.color = UICommon.FONT_COLOR_GOLDEN;
		_get_Label.color = UICommon.FONT_COLOR_GOLDEN;
		_Grid = _parent.Find("Grid").GetComponent<UIGrid>();
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		UpdateUI();
	}
	void UpdateUI()
	{
		if(_notify != null)
		{
			
			_fromName_Label.text = _notify.senderName;
			_titleValue_Label.text = _notify.title;
			_info_Label.text = "　　"+ _notify.content;


		}
	}
	public void UpdateData(SlgPB.Notify notify)
	{
		if(notify != null)
		{
			this.gameObject.SetActive(true);
			_notify = notify;
			_model_notificationGroup = InstancePlayer.instance.model_User.model_notificationGroup;
			List<PrizeItem> prizeItems = new List<PrizeItem>();
			_Grid.DestoryAllChildren();
			if(_model_notificationGroup.HasBonus(_notify.notifyId))
			{
				prizeItems = _notify.prizeItems;
				CreateItem(prizeItems);
				_get_Btn.gameObject.SetActive(true);
			}
			else
			{
				_get_Btn.gameObject.SetActive(false);
			}
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}

	void CreateItem(List<PrizeItem> dataList)
	{
		
		_mailItemDic.Clear();
		_Grid.DestoryAllChildren();
		int dataCount = dataList.Count;
		for(int i= 0; i< dataCount; i++)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "mail/PrizeItemContainer");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,prefab);
			MailPrizeItem playitem = item.GetComponent<MailPrizeItem>();
			playitem.Init(dataList[i]);
			_mailItemDic.Add(dataList[i].itemId,playitem);
			item.name = "0" + i;
		}
		_Grid.Reposition();
	}

	void OnClickDeleteMail()
	{
//		_model_notificationGroup = InstancePlayer.instance.model_User.model_notificationGroup;
//		_model_notificationGroup.Delete(_notify.notifyId);
		DeleteMail();

	}
	void OnGet()
	{
		GetPrizeItem();
	}
	// ======================
	void GetPrizeItem()
	{
		UIHelper.LoadingPanelIsOpen(true);
		ids[0] = _notify.notifyId;
		PBConnect_readNotify.RESULT r = PBConnect_readNotify.ReadNotification(ids,OnGetPrizeItem);
		switch(r)
		{
		case PBConnect_readNotify.RESULT.CANT_PICK:
			UIHelper.ShowTextPromptPanel(this.gameObject,"不能提取");
			break;
		case PBConnect_readNotify.RESULT.OK:
			
			break;
		}
	}

	void OnGetPrizeItem(bool success, System.Object content)
	{
		if (success) {

			_get_Btn.gameObject.SetActive(false);
			_Grid.DestoryAllChildren();
			UIHelper.LoadingPanelIsOpen(false);
			Trace.trace("OnGetPrizeItem  success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnGetPrizeItem  failure",Trace.CHANNEL.UI);
		}
	}

	void DeleteMail()
	{
		UIHelper.LoadingPanelIsOpen(true);
		ids[0] = _notify.notifyId;
		PBConnect_delNotify.RESULT r =	PBConnect_delNotify.DelNotification(ids,OnDeleteMail);
		switch(r)
		{
		case  PBConnect_delNotify.RESULT.CANT_DEL:
			Trace.trace("DeleteMail  CANT_DEL",Trace.CHANNEL.UI);
			UIHelper.LoadingPanelIsOpen(false);
			break;
		case  PBConnect_delNotify.RESULT.OK:
			Trace.trace("DeleteMail  OK",Trace.CHANNEL.UI);
			break;
		}
	}
	void OnDeleteMail(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {

			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshMailInfo,new Notification(_notify));
			UpdateData(null);
			Trace.trace("OnDeleteMail  success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnDeleteMail  failure",Trace.CHANNEL.UI);
		}
	}
}
