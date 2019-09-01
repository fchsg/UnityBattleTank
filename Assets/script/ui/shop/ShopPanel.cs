//using UnityEngine;
//using System.Collections;
//using UICommonNameSpace;
//
//public class ShopPanel : PanelBase 
//{
//
//	public static readonly string PANEL_PATH = "profab/ui/system/Shop/ShopPanel";
//	public static readonly UI_PANEL_TYPE PANEL_NAME = UI_PANEL_TYPE.ShopPanel;
//
//	public UIGrid grid;
//	public GameObject parent ;
//	public GitfItem GiftItem;
//	public ArrayList itemList = new ArrayList();
//	private static int count = 0;
//
//	override public void Create(Transform parent){
//		base.Init (PANEL_PATH,PANEL_NAME,parent);
//		base.showStyle = PANEL_SHOW_STYLE.UpToSlide;
//		InitData ();
//	}
//
//	private void InitData(){
//
//	}
//
//	protected override void OnClick (GameObject target)
//	{
//		base.OnClick (target);
//		ButtonClick (target);
//	}
//
//	void ButtonClick(GameObject click){
//		if (click.name.Equals ("BtnClose")) {
//			this.Hide();
//		} else if (click.name.Equals ("Btn_add")){
//			addItem();
//		}else if (click.name.Equals ("Btn_del")){
//			delItem();
//		}
//
//	}
//
//	void Start(){
//		grid = GameObject.Find("shop_Grid").GetComponent<UIGrid>();
//		parent = GameObject.Find ("shop_Grid");
//
//	}
//
//	void addItem(){
//		Debug.Log ("add");
//
//
////		itmefb = GameObject.Instantiate(itemfb_.GetTransform ().gameObject))as GameObject;
////		itmefb = GameObject.Instantiate(itemfb_.GetGameObject ())as GameObject;
////		GiftItem.InitData (parent);
//		GiftItem = gameObject.AddComponent ("GitfItem") as GitfItem;
//		GiftItem.SetData("234");
//		GameObject item_1 = GiftItem.GetGameObject ();
//
//		count ++;
////		item_1.name = "test_" + count; 
//		GameObject _item = NGUITools.AddChild (parent,item_1);
//		 
//		grid.AddChild(_item.transform);
//
//
//		itemList.Add (item_1);
//
////		Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
////		foreach (Transform child in allChildren)
////		{
////			print("11111");
////			if(child.gameObject.name.Equals("name_Label"))
////			{
////				print("22222");
////				print("star 44");
////				UILabel itemName_ = child.gameObject.GetComponent<UILabel>();
////				count++;
////				itemName_.text = "礼包" + count;
////			}else
////			{
////				print("test");
////			}
////			print("333333");
////		}
//
//
//
//		grid.repositionNow = true;   
//	}
//
//	void delItem(){
//		Debug.Log ("del");
////		UnityEngine.Object.Destroy(child.gameObject);
//	/*	GameObject item = GameObject.Find ("giftItem");
//		if (item != null) {
//			Destory(item);
//			grid.repositionNow = true; 
//		}
//	*/
//		Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
//		if (allChildren != null) 
//		{
//			foreach (Transform child in allChildren)
//			{
//				NGUITools.DestroyChildren(child);
//				print("dddddddd");
//			}
//
//
//		}
//
//	}
//
//	protected override void Destory ()
//	{
//		base.Destory ();
//
//	}
//
//
//
//
//
//
//}
