using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class MailPanel : PanelBase {
	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;
 	
	private Transform _mailItem_Container;
	private UIGrid _Grid;
	private UIScrollView _scrollview;
	private UIButton _allget_Btn;
	private UIButton _alldelete_Btn;
	private UILabel _allget_Label;
	private UILabel _alldelete_Label;
	//data
	List<UILabel> _grayList = new List<UILabel>(); 
	List<UILabel> _greenList = new List<UILabel>(); 
	List<UILabel> _orangeList = new List<UILabel>();
	List<RankItem> _playItemList = new List<RankItem>();
 
	Model_NotificationGroup _model_notificationGroup;
	 
	List<SlgPB.Notify> _notifys = new List<SlgPB.Notify>();
	Dictionary<int,MailItem> _mailItemDic = new Dictionary<int,MailItem>();
	MailInfo _mailInfo = null;
	int[] _ids;
	void Awake()
	{
		_grayList.Clear();
		_greenList.Clear();
		_orangeList.Clear();
		_Container = transform.Find("MailContainer");
		_close_Btn = _Container.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_namePanel = _Container.Find("panelName_Label").GetComponent<UILabel>();
		_grayList.Add(_namePanel);

		_mailItem_Container = _Container.Find("mailItem_Container");
		_scrollview = _mailItem_Container.Find("ScrollView").GetComponent<UIScrollView>();
		_Grid = _mailItem_Container.Find("ScrollView/Grid").GetComponent<UIGrid>();
		_mailInfo = _Container.Find("mailInfo_Container").GetComponent<MailInfo>();
		_alldelete_Btn = _mailItem_Container.Find("alldelete_Btn").GetComponent<UIButton>();
		_allget_Btn = _mailItem_Container.Find("allget_Btn").GetComponent<UIButton>();
		_allget_Label = _mailItem_Container.Find("allget_Btn/Label").GetComponent<UILabel>();
		_allget_Label.color = UICommon.FONT_COLOR_GOLDEN;
		_alldelete_Label = _mailItem_Container.Find("alldelete_Btn/Label").GetComponent<UILabel>();
		_alldelete_Label.color = UICommon.FONT_COLOR_GOLDEN;
		UIHelper.AddBtnClick(_allget_Btn,OnAllGet);
		UIHelper.AddBtnClick(_alldelete_Btn,OnAllDelete);
		foreach(UILabel label in _grayList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
		foreach(UILabel label in _greenList)
		{
			label.color = UICommon.FONT_COLOR_GREEN;
		}
		foreach(UILabel label in _orangeList)
		{
			label.color = UICommon.FONT_COLOR_ORANGE;
		}
	}
	public override void Init ()
	{
		base.Init ();
		_model_notificationGroup = InstancePlayer.instance.model_User.model_notificationGroup;
		_notifys = _model_notificationGroup.GetAllNotifications();
//		InitPVPItem();
		CreateItem(_notifys);
	}
	void Start () {
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshMailItem,OnMailItem);
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshMailInfo,OnMailInfo);
		_mailInfo.UpdateData(null);
	}

	void OnMailItem(Notification data)
	{
		if(data != null)
		{
			SlgPB.Notify notify = data._data as SlgPB.Notify;

			foreach(KeyValuePair<int,MailItem> kvp in _mailItemDic)
			{
				if(notify.notifyId != kvp.Value._notify.notifyId)
				{ 
					kvp.Value._iconLine.gameObject.SetActive(false);
					kvp.Value.IsChecked = false;
				}
			}
			_mailInfo.UpdateData(notify);
			 
		}
	}
	void OnMailInfo(Notification data)
	{
		if(data != null)
		{
			SlgPB.Notify notify = data._data as SlgPB.Notify;
			foreach(KeyValuePair<int,MailItem> kvp in _mailItemDic)
			{
				if(notify.notifyId == kvp.Key)
				{
					NGUITools.Destroy(kvp.Value.gameObject);
					_Grid.Reposition();
					_mailItemDic.Remove(kvp.Key);
					return;
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
	void InitPVPItem()
	{
		CreateItem(_notifys);
		_mailItemDic.Clear();
		SetWrapContent(_Grid,_scrollview,_notifys,OnUpdateItemMain);
	}
	void SetWrapContent(UIGrid grid,UIScrollView scrollview,List<SlgPB.Notify> dataList,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int dataCount = dataList.Count;
		if(dataCount <= 4 )
		{
			scrollview.enabled = false;
		}
		else
		{
			scrollview.enabled = true;
		}
		Trace.trace("dataCount  " + dataCount,Trace.CHANNEL.UI);
		UIWrapContent wrap = null;
		if(grid.gameObject.GetComponent<UIWrapContent>())
		{
			wrap = grid.gameObject.GetComponent<UIWrapContent>();
		}
		else
		{
			grid.gameObject.AddComponent<UIWrapContent>();
			wrap = grid.gameObject.GetComponent<UIWrapContent>();

		}
		if(wrap != null)
		{
			//绑定方法
			wrap.itemSize = (int)grid.cellHeight;
			wrap.minIndex = -(dataCount -1);
			wrap.maxIndex = 0;

			wrap.onInitializeItem = OnUpdateItemMain;
			wrap.enabled = true;
			wrap.SortAlphabetically();
		}
	}
	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_notifys,_mailItemDic);
	}
	void OnUpateItem(GameObject go, int index, int realIndex,List<SlgPB.Notify> dataList,Dictionary<int,MailItem> dataDic)
	{
		int dataCount = dataList.Count;
		int indexList = Mathf.Abs(realIndex);
		MailItem Item1 = go.GetComponent<MailItem>();
		int index_ = indexList ;

		if(index_ > (dataCount - 1) )
		{
			Item1.gameObject.SetActive(false);

			return;
		}
		else
		{
			Item1.gameObject.SetActive(true);
			Item1.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].notifyId))
			{
				dataDic.Add(dataList[index_].notifyId,Item1);
			}
		}
	}
	void CreateItem(List<SlgPB.Notify> dataList)
	{
		_mailItemDic.Clear();
		_Grid.DestoryAllChildren();
		int dataCount = dataList.Count;
		for(int i= 0; i< dataCount; i++)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "mail/mailItemContainer");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,prefab);
			MailItem playitem = item.GetComponent<MailItem>();
			playitem.Init(dataList[i]);
			_mailItemDic.Add(dataList[i].notifyId,playitem);
			item.name = "0" + i;
		}
		_Grid.Reposition();
	}
	void OnAllDelete()
	{
		DeleteMail();

	}
	void OnAllGet()
	{
		
	}
	void OnClose()
	{
		this.Delete();
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RefreshMailItem);
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RefreshMailInfo);
	}
	void DeleteAllMailItem(int[] ids)
	{
		if(ids != null)
		{
			int count = ids.Length;
			for(int i = 0; i <count;i++)
			{
				MailItem mailitem = null;
				_mailItemDic.TryGetValue(ids[i],out mailitem);
				NGUITools.Destroy(mailitem.gameObject);
				_Grid.Reposition();
				_mailItemDic.Remove(ids[i]);
			}
		}
	}
	//==============================

	void DeleteMail()
	{
		UIHelper.LoadingPanelIsOpen(true);
		_ids = _model_notificationGroup.CollectCanBeDelIds();
		PBConnect_delNotify.RESULT r =	PBConnect_delNotify.DelNotification(_ids,OnDeleteMail);
		switch(r)
		{
		case  PBConnect_delNotify.RESULT.CANT_DEL:
			Trace.trace("DeleteMail  CANT_DEL",Trace.CHANNEL.UI);
			break;
		case  PBConnect_delNotify.RESULT.OK:
			Trace.trace("DeleteMail  OK",Trace.CHANNEL.UI);
			break;
		}
	}
	void OnDeleteMail(bool success, System.Object content)
	{
		if (success) {
			DeleteAllMailItem(_ids);
			UIHelper.LoadingPanelIsOpen(false);
			Trace.trace("OnDeleteMail  success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnDeleteMail  failure",Trace.CHANNEL.UI);
		}
	}

//	void GetNotify()
//	{
//		UIHelper.LoadingPanelIsOpen(true);
//		PBConnect_getNotification.GetNotification(OnNotify);
//	}
//	void OnNotify(bool success, System.Object content)
//	{
//		if (success) {	
//			
//
//			UIHelper.LoadingPanelIsOpen(false);
//			Trace.trace("OnNotify  success",Trace.CHANNEL.UI);
//		} else {
//			Trace.trace("OnNotify  failure",Trace.CHANNEL.UI);
//		}
//	}
}
