using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
/// <summary>
/// 仓库界面
/// </summary>
public class WarehousePanel : PanelBase {

	private Transform _Warehouse_Container;
	private UIButton _closeBtn;

	private Transform _btn_Container;
	private UIButton _all_Btn;
	private UIButton _boxs_Btn;
	private UIButton _resources_Btn;
	private UIToggle _all_toggle;
	private UIToggle _boxs_toggle;
	private UIToggle _resources_toggle;

	private Transform _items_Container;
	private UIScrollView _all_ScrollView;
	private UIScrollView _boxs_ScrollView;
	private UIScrollView _resources_ScrollView;
	private UIGrid _all_grid;
	private UIGrid _boxs_grid;
	private UIGrid _resources_grid;

	private UILabel _warehouseLable;
	private UILabel _allLable;
	private UILabel _boxsLable;
	private UILabel _resourceLable;

	//data 

	List<UILabel> _labelList = new List<UILabel>();
	List<ItemDataManager.ItemData> _allItemList = new List<ItemDataManager.ItemData>();
	List<ItemDataManager.ItemData> _normalItemList = new List<ItemDataManager.ItemData>();
	List<ItemDataManager.ItemData> _consumeItemList = new List<ItemDataManager.ItemData>();
	Dictionary<int,PropsItem> _allPropsItmeDic = new Dictionary<int,PropsItem>();
	Dictionary<int,PropsItem> _normalPropsItmeDic = new Dictionary<int,PropsItem>();
	Dictionary<int,PropsItem> _consumePropsItmeDic = new Dictionary<int,PropsItem>();
	List<UIWrapContent> _wrapList = new List<UIWrapContent>();
	void Awake()
	{
		_Warehouse_Container = transform.Find("Warehouse_Container");
		_closeBtn = _Warehouse_Container.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_btn_Container = _Warehouse_Container.Find("btn_Container");

		_all_Btn = _btn_Container.Find("all_Btn").GetComponent<UIButton>();
		_boxs_Btn = _btn_Container.Find("boxs_Btn").GetComponent<UIButton>();
		_resources_Btn = _btn_Container.Find("resources_Btn").GetComponent<UIButton>();
		_all_toggle = _btn_Container.Find("all_Btn").GetComponent<UIToggle>();
		_boxs_toggle = _btn_Container.Find("boxs_Btn").GetComponent<UIToggle>();
		_resources_toggle = _btn_Container.Find("resources_Btn").GetComponent<UIToggle>();
		EventDelegate.Add(_all_toggle.onChange,OnAllChange);
		EventDelegate.Add(_boxs_toggle.onChange,OnConsumeChange);
		EventDelegate.Add(_resources_toggle.onChange,OnNormalChange);
		_items_Container = _Warehouse_Container.Find("items_Container");

		_all_ScrollView = _items_Container.Find("all_Container/Scroll View").GetComponent<UIScrollView>();
		_boxs_ScrollView = _items_Container.Find("boxs_Container/Scroll View").GetComponent<UIScrollView>();
		_resources_ScrollView = _items_Container.Find("resources_Container/Scroll View").GetComponent<UIScrollView>();
		_all_grid = _items_Container.Find("all_Container/Scroll View/Grid").GetComponent<UIGrid>();
		_boxs_grid = _items_Container.Find("boxs_Container/Scroll View/Grid").GetComponent<UIGrid>();
		_resources_grid = _items_Container.Find("resources_Container/Scroll View/Grid").GetComponent<UIGrid>();

		_warehouseLable = _Warehouse_Container.Find("warehouse_Label").GetComponent<UILabel>();
		_allLable = _btn_Container.Find("all_Btn/all_Label").GetComponent<UILabel>();
		_boxsLable = _btn_Container.Find("boxs_Btn/boxs_Label").GetComponent<UILabel>();
		_resourceLable = _btn_Container.Find("resources_Btn/res_Label").GetComponent<UILabel>();
		_labelList.Clear();
		_labelList.Add(_warehouseLable);
		_labelList.Add(_allLable);
		_labelList.Add(_boxsLable);
		_labelList.Add(_resourceLable);
		foreach(UILabel label in _labelList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
	}
	// Use this for initialization
	void Start()
	{
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshPropItem,OnRefreshPropItem);
	}

	void OnRefreshPropItem(Notification notification)
	{
		_allItemList.Clear();
		_normalItemList.Clear();
		_consumeItemList.Clear();
		_allPropsItmeDic.Clear();
		_normalPropsItmeDic.Clear();
		_consumePropsItmeDic.Clear();
		_wrapList.Clear();
		ItemDataManager itemManager = new ItemDataManager();
		_allItemList = itemManager.GetAllItemData();
		_normalItemList = itemManager.GetNormalItemData();
		_consumeItemList = itemManager.GetConsumeItemData();
		SetWrapContent(_all_grid,_allItemList,_allPropsItmeDic,OnUpdateItemAll);
		SetWrapContent(_boxs_grid,_consumeItemList,_consumePropsItmeDic,OnUpdateItemConsume);
		SetWrapContent(_resources_grid,_normalItemList,_normalPropsItmeDic,OnUpdateItemNormal);

		UpdataItemInfo(_allItemList,_allPropsItmeDic);
		UpdataItemInfo(_normalItemList,_normalPropsItmeDic);
		UpdataItemInfo(_consumeItemList,_consumePropsItmeDic);

		foreach(UIWrapContent wrap in _wrapList)
		{
			wrap.SortAlphabetically();
		}
	 
	}
	void UpdataItemInfo(List<ItemDataManager.ItemData> dataList,Dictionary<int,PropsItem> dataDic)
	{
		int i = 0;
		foreach(KeyValuePair<int,PropsItem>kvp in dataDic)
		{

			kvp.Value.UpdateData(dataList[i]);
			i++;
		}
	}
	 
	public override void Init ()
	{
		base.Init ();
		_allItemList.Clear();
		_normalItemList.Clear();
		_consumeItemList.Clear();
		_allPropsItmeDic.Clear();
		_normalPropsItmeDic.Clear();
		_consumePropsItmeDic.Clear();
		_wrapList.Clear();
		ItemDataManager itemManager = new ItemDataManager();
		_allItemList = itemManager.GetAllItemData();
		_normalItemList = itemManager.GetNormalItemData();
		_consumeItemList = itemManager.GetConsumeItemData();
	}

	// Update is called once per frame
	void Update () {
	
	}
	void OnAllChange()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_allPropsItmeDic.Clear();
			SetWrapContent(_all_grid,_allItemList,_allPropsItmeDic,OnUpdateItemAll);
		}	 
	}

	void OnConsumeChange()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_consumePropsItmeDic.Clear();
			SetWrapContent(_boxs_grid,_consumeItemList,_consumePropsItmeDic,OnUpdateItemConsume);
		}	 
	}
	void OnNormalChange()
	{
		bool check = UIToggle.current.value;
		if(check)
		{
			_normalPropsItmeDic.Clear();
			SetWrapContent(_resources_grid,_normalItemList,_normalPropsItmeDic,OnUpdateItemNormal);
		}	 
	}
	void OnClose()
	{
		this.Delete();
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RefreshPropItem);
	}

	void SetWrapContent(UIGrid grid,List<ItemDataManager.ItemData> dataList,Dictionary<int,PropsItem> dataDic ,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		grid.enabled = true;
		int tankCont = dataList.Count;
		int gridCont = grid.GetChildList().Count;
		int itemCount = 0;
		if(tankCont % 2 == 0)
		{
			itemCount = tankCont / 2;
		}
		else
		{
			itemCount = tankCont / 2 + 1;
		}
		if(itemCount <= 3)
		{

			if(grid.gameObject.GetComponent<UIWrapContent>())
			{
				NGUITools.Destroy(grid.gameObject.GetComponent<UIWrapContent>());
			}
			if(gridCont != itemCount)
			{
				StartCoroutine(CreateUnit(grid,dataList,dataDic));

			}
		}
		else
		{
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
			_wrapList.Add(wrap);
			if(gridCont != 4)
			{
				if (grid != null) {
					grid.DestoryAllChildren();
				}
				CreateWrapUnit(grid,4);
				if(wrap != null)
				{
					//绑定方法
					wrap.itemSize = (int)grid.cellHeight;
					wrap.minIndex = -(itemCount - 1);
					wrap.maxIndex = 0;
					wrap.onInitializeItem = OnUpdateItemMain;
					wrap.SortAlphabetically();
				}
			} 
		}
	}

	void OnUpdateItemAll(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_allItemList,_allPropsItmeDic);
	}
	void OnUpdateItemNormal(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_normalItemList,_normalPropsItmeDic);
	}
	void OnUpdateItemConsume(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_consumeItemList,_consumePropsItmeDic);
	}

	void OnUpateItem(GameObject go, int index, int realIndex,List<ItemDataManager.ItemData> dataList,Dictionary<int,PropsItem> dataDic)
	{
		int index_ = 0;
		int indexList = Mathf.Abs(realIndex);
		PropsItem Item1 = go.transform.Find("item1").GetComponent<PropsItem>();
		Item1.Init(dataList[indexList * 2]);
		if(!dataDic.ContainsKey(dataList[indexList * 2].id))
		{
			dataDic.Add(dataList[indexList * 2].id,Item1);
		}

		index_ = indexList * 2 + 1;
		int tankCount = dataList.Count;
		if(index_ > (tankCount - 1) )
		{
			PropsItem tankItem2 = go.transform.Find("item2").GetComponent<PropsItem>();
			tankItem2.gameObject.SetActive(false);
		}
		else
		{
			
			PropsItem Item2 = go.transform.Find("item2").GetComponent<PropsItem>();
			Item2.gameObject.SetActive(true);
			Item2.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item2);
			}
		}	 
	}
		

	IEnumerator CreateUnit(UIGrid grid, List<ItemDataManager.ItemData> unitData,Dictionary<int,PropsItem> dataDic){
		yield return new WaitForSeconds (0.01f);	
		if (grid != null) {
			grid.DestoryAllChildren();
		}

		int tankCount = unitData.Count;
		int itemCount = 0;
		if(tankCount % 2 == 0)
		{
			itemCount = tankCount / 2;
		}
		else
		{
			itemCount = tankCount / 2 + 1;
		}
		for(int i = 0; i < itemCount; i++)
		{
			if(grid.gameObject != null)
			{
				int index_ = 0;
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "warehouse/props_items");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
				item.name = "0" + i;
				PropsItem tankItem1 = item.transform.Find("item1").GetComponent<PropsItem>();
				tankItem1.Init(unitData[i*2]);
				dataDic.Add(unitData[i*2].id,tankItem1);
				PropsItem tankItem2 = item.transform.Find("item2").GetComponent<PropsItem>();

				index_ = i*2 + 1;
				if(index_ > (tankCount - 1) )
				{
					NGUITools.Destroy(tankItem2.gameObject);
				}
				else
				{
					tankItem2.Init(unitData[index_]);
					dataDic.Add(unitData[index_].id,tankItem2);
				}

			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}

	void CreateWrapUnit(UIGrid grid,int num){
		if (grid != null) {
			grid.DestoryAllChildren();
		}
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{

				GameObject Item = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "warehouse/props_items");
				GameObject item = NGUITools.AddChild(grid.gameObject,Item);
				item.name = "0" + i;
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}
}
