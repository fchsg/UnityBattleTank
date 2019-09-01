using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Hero panel.
/// </summary>
public class HeroPanel : PanelBase {

	private Transform _panelContainer;
	private UIButton _closeBtn;
	private UILabel _panelName;
	private Transform _Checkbox_Container;

	private UIToggle _Checkbox_1;
	private UILabel _Checkbox_Label_1;
	private UIToggle _Checkbox_2;
	private UILabel _Checkbox_Label_2;
	private UIToggle _Checkbox_3;
	private UILabel _Checkbox_Label_3;
	private UIToggle _Checkbox_4;
	private UILabel _Checkbox_Label_4;
	private UIToggle _Checkbox_5;
	private UILabel _Checkbox_Label_5;
	List<UILabel> _unitTypeList = new List<UILabel>();

	private UIToggle _Checkbox_color_1;
	private UILabel _Checkbox_color_Label_1;
	private UIToggle _Checkbox_color_2;
	private UILabel _Checkbox_color_Label_2;
	private UIToggle _Checkbox_color_3;
	private UILabel _Checkbox_color_Label_3;
	private UIToggle _Checkbox_color_4;
	private UILabel _Checkbox_color_Label_4;
	private UIToggle _Checkbox_color_5;
	private UILabel _Checkbox_color_Label_5;

	private UIScrollView _scrollview;
	private UIGrid _grid;

	//data 
	List<HeroDataManager.HeroData>  _heroDataList ;
	HeroDataManager _heroDataManager;
	List<int> _typeList = new List<int>();
	List<int> _colorList = new List<int>();
	List<GameObject> _gridChildList = new List<GameObject>();
	Dictionary<int,HeroInfoItem> _heroInfoItemDic = new Dictionary<int, HeroInfoItem>();

	void Start () {
		 
	}
	public override void Init ()
	{
		base.Init ();
		_heroDataManager = new HeroDataManager();
		_typeList.Clear();
		_colorList.Clear();
		_gridChildList.Clear();
		_heroInfoItemDic.Clear();
	}
	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		CreateWrapUnit(_grid,3);
		 
	}
	void Update () {
		

	}
	void SetWrapContent(UIGrid grid,UIScrollView scrollview,List<HeroDataManager.HeroData> heroDataList,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int dataCount = heroDataList.Count;
		if(dataCount <= 10 )
		{
			_gridChildList[2].SetActive(false);
			scrollview.enabled = false;
		}
		else
		{
			_gridChildList[2].SetActive(true);
			scrollview.enabled = true;
		}
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
			wrap.minIndex = -(dataCount - 1);
			wrap.maxIndex = 0;
			wrap.onInitializeItem = OnUpdateItemMain;
			wrap.enabled = true;
			wrap.SortAlphabetically();
		}
	}
	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_heroDataList,_heroInfoItemDic);
	}
	void OnUpateItem(GameObject go, int index, int realIndex,List<HeroDataManager.HeroData> dataList,Dictionary<int,HeroInfoItem> dataDic)
	{
		int dataCount = dataList.Count;
		int indexList = Mathf.Abs(realIndex);
		HeroInfoItem Item1 = go.transform.Find("heroInfo_Item1").GetComponent<HeroInfoItem>();
		HeroInfoItem Item2 = go.transform.Find("heroInfo_Item2").GetComponent<HeroInfoItem>();
		HeroInfoItem Item3 = go.transform.Find("heroInfo_Item3").GetComponent<HeroInfoItem>();
		HeroInfoItem Item4 = go.transform.Find("heroInfo_Item4").GetComponent<HeroInfoItem>();
		HeroInfoItem Item5 = go.transform.Find("heroInfo_Item5").GetComponent<HeroInfoItem>();
		int index_ = indexList * 5 + 0;

		if(index_ > (dataCount - 1) )
		{
			Item1.gameObject.SetActive(false);
			Item2.gameObject.SetActive(false);
			Item3.gameObject.SetActive(false);
			Item4.gameObject.SetActive(false);
			Item5.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item1.gameObject.SetActive(true);
			Item1.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item1);
			}
		}

		index_ = indexList * 5 + 1;
		if(index_ > (dataCount - 1) )
		{
			Item2.gameObject.SetActive(false);
			Item3.gameObject.SetActive(false);
			Item4.gameObject.SetActive(false);
			Item5.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item2.gameObject.SetActive(true);
			Item2.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item2);
			}
		}

		index_ = indexList * 5 + 2;
		if(index_ > (dataCount - 1) )
		{
			Item3.gameObject.SetActive(false);
			Item4.gameObject.SetActive(false);
			Item5.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item3.gameObject.SetActive(true);
			Item3.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item3);
			}
		}
		index_ = indexList * 5 + 3;
		if(index_ > (dataCount - 1) )
		{
			Item4.gameObject.SetActive(false);
			Item5.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item4.gameObject.SetActive(true);
			Item4.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item4);
			}
		}	 

		index_ = indexList * 5 + 4;
		if(index_ > (dataCount - 1) )
		{
			Item5.gameObject.SetActive(false);
			return;
		}
		else
		{
			Item5.gameObject.SetActive(true);
			Item5.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].id))
			{
				dataDic.Add(dataList[index_].id,Item5);
			}
		}	 
	}
	void CreateWrapUnit(UIGrid grid,int num){
		if (grid != null) {
			grid.DestoryAllChildren();
		}
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{

				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "hero/HeroInfoItemsContainer");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
				item.name = "0" + i;
				_gridChildList.Add(item);
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}

	void UpdateData(bool check,List<int> dataList,int box)
	{
		if(check)
		{
			if(dataList.Contains(box))
			{
				dataList.Remove(box);
			}
		}
		else
		{
			if(!dataList.Contains(box))
			{
				dataList.Add(box);
			}
		}
		_heroDataList = _heroDataManager.ShowHeroData(_typeList,_colorList);
		foreach(HeroDataManager.HeroData hero in _heroDataList)
		{
//			Trace.trace(" hero.id  " + hero.id ,Trace.CHANNEL.UI);
		}
		_heroInfoItemDic.Clear();
		SetWrapContent(_grid,_scrollview,_heroDataList,OnUpdateItemMain);

	}
	/// <summary>
	/// 坦克
	/// </summary>
	void CheckboxOnChange_1()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_typeList,1);
	}
	/// <summary>
	/// 装甲
	/// </summary>
	void CheckboxOnChange_2()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_typeList,2);
	}
	/// <summary>
	/// 导弹
	/// </summary>
	void CheckboxOnChange_3()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_typeList,3);
	}
	/// <summary>
	/// 火炮
	/// </summary>
	void CheckboxOnChange_4()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_typeList,4);
	}
	/// <summary>
	/// 反坦
	/// </summary>
	void CheckboxOnChange_5()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_typeList,5);
	}
	/// <summary>
	/// 灰色
	/// </summary>
	void CheckboxColorOnChange_1()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_colorList,1);
	}
	/// <summary>
	/// 绿色
	/// </summary>
	void CheckboxColorOnChange_2()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_colorList,2);
	}
	/// <summary>
	/// 蓝色
	/// </summary>
	void CheckboxColorOnChange_3()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_colorList,3);
	}
	/// <summary>
	/// 紫色
	/// </summary>
	void CheckboxColorOnChange_4()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_colorList,4);
	}
	/// <summary>
	/// 橙色
	/// </summary>
	void CheckboxColorOnChange_5()
	{
		bool check = UIToggle.current.value;
		UpdateData(check,_colorList,5);
	}

	void OnClose()
	{
		this.Delete();
	}
	void Awake()
	{
		_panelContainer = transform.Find("Hero_Container");
		_closeBtn = _panelContainer.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_panelName = _panelContainer.Find("panelName_Label").GetComponent<UILabel>();
		_panelName.color = UICommon.FONT_COLOR_GREY;
		_Checkbox_Container = _panelContainer.Find("Checkbox_Container");
		_Checkbox_1 = _Checkbox_Container.Find("Checkbox_1").GetComponent<UIToggle>();
		_Checkbox_Label_1 = _Checkbox_Container.Find("Checkbox_1/Label").GetComponent<UILabel>();
		_Checkbox_2 = _Checkbox_Container.Find("Checkbox_2").GetComponent<UIToggle>();
		_Checkbox_Label_2 = _Checkbox_Container.Find("Checkbox_2/Label").GetComponent<UILabel>();
		_Checkbox_3 = _Checkbox_Container.Find("Checkbox_3").GetComponent<UIToggle>();
		_Checkbox_Label_3 = _Checkbox_Container.Find("Checkbox_3/Label").GetComponent<UILabel>();
		_Checkbox_4 = _Checkbox_Container.Find("Checkbox_4").GetComponent<UIToggle>();
		_Checkbox_Label_4 = _Checkbox_Container.Find("Checkbox_4/Label").GetComponent<UILabel>();
		_Checkbox_5 = _Checkbox_Container.Find("Checkbox_5").GetComponent<UIToggle>();
		_Checkbox_Label_5 = _Checkbox_Container.Find("Checkbox_5/Label").GetComponent<UILabel>();
		EventDelegate.Add(_Checkbox_1.onChange,CheckboxOnChange_1);
		EventDelegate.Add(_Checkbox_2.onChange,CheckboxOnChange_2);
		EventDelegate.Add(_Checkbox_3.onChange,CheckboxOnChange_3);
		EventDelegate.Add(_Checkbox_4.onChange,CheckboxOnChange_4);
		EventDelegate.Add(_Checkbox_5.onChange,CheckboxOnChange_5);
		_unitTypeList.Clear();
		_unitTypeList.Add(_Checkbox_Label_1);
		_unitTypeList.Add(_Checkbox_Label_2);
		_unitTypeList.Add(_Checkbox_Label_3);
		_unitTypeList.Add(_Checkbox_Label_4);
		_unitTypeList.Add(_Checkbox_Label_5);
		foreach(UILabel label in _unitTypeList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
		_Checkbox_color_1 = _Checkbox_Container.Find("Checkbox_color_1").GetComponent<UIToggle>();
		_Checkbox_color_Label_1 = _Checkbox_Container.Find("Checkbox_color_1/Label").GetComponent<UILabel>();
		_Checkbox_color_2 = _Checkbox_Container.Find("Checkbox_color_2").GetComponent<UIToggle>();
		_Checkbox_color_Label_2 = _Checkbox_Container.Find("Checkbox_color_2/Label").GetComponent<UILabel>();
		_Checkbox_color_3 = _Checkbox_Container.Find("Checkbox_color_3").GetComponent<UIToggle>();
		_Checkbox_color_Label_3 = _Checkbox_Container.Find("Checkbox_color_3/Label").GetComponent<UILabel>();
		_Checkbox_color_4 = _Checkbox_Container.Find("Checkbox_color_4").GetComponent<UIToggle>();
		_Checkbox_color_Label_4 = _Checkbox_Container.Find("Checkbox_color_4/Label").GetComponent<UILabel>();
		_Checkbox_color_5 = _Checkbox_Container.Find("Checkbox_color_5").GetComponent<UIToggle>();
		_Checkbox_color_Label_5 = _Checkbox_Container.Find("Checkbox_color_5/Label").GetComponent<UILabel>();
		EventDelegate.Add(_Checkbox_color_1.onChange,CheckboxColorOnChange_1);
		EventDelegate.Add(_Checkbox_color_2.onChange,CheckboxColorOnChange_2);
		EventDelegate.Add(_Checkbox_color_3.onChange,CheckboxColorOnChange_3);
		EventDelegate.Add(_Checkbox_color_4.onChange,CheckboxColorOnChange_4);
		EventDelegate.Add(_Checkbox_color_5.onChange,CheckboxColorOnChange_5);
		_Checkbox_color_Label_1.color = UICommon.UNIT_NAME_COLOR_0;
		_Checkbox_color_Label_2.color = UICommon.UNIT_NAME_COLOR_1;
		_Checkbox_color_Label_3.color = UICommon.UNIT_NAME_COLOR_2;
		_Checkbox_color_Label_4.color = UICommon.UNIT_NAME_COLOR_3;
		_Checkbox_color_Label_5.color = UICommon.UNIT_NAME_COLOR_4;

		_scrollview = _panelContainer.Find("heroList_Container/Scroll View").GetComponent<UIScrollView>();
		_grid = _panelContainer.Find("heroList_Container/Scroll View/Grid").GetComponent<UIGrid>();
	}
}
