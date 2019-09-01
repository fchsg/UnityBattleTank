using UnityEngine;
using System.Collections;
using SlgPB;
public class UIHelper {
	
	public static GameObject FindChildInObject(GameObject parent, string name)
	{
		Transform t = parent.transform.Find(name);
		if(t != null)
			return t.gameObject;
		
		for(int i = 0; i < parent.transform.childCount; i++)
		{
			GameObject o = FindChildInObject(parent.transform.GetChild(i).gameObject, name);
			if(o != null)
				return o;
		}
		return null;
	}

	public static void AddBtnClick(GameObject parent ,string buttonName, EventDelegate.Callback cb)
	{
		GameObject findObj = FindChildInObject (parent, buttonName);
		if (findObj != null) {
		
			UIButton btn = findObj.GetComponent<UIButton>();

			Assert.assert (btn != null, parent.gameObject + " " + buttonName + " not add button script");

			EventDelegate ev = new EventDelegate (cb);
			btn.onClick.Add (ev);
		}
	}

	public static void AddBtnClick(UIButton btn, EventDelegate.Callback cb)
	{
		EventDelegate ev = new EventDelegate (cb);
		btn.onClick.Add (ev);
	}
	
	public static void AddBtnClick(UIButton btn, UIEventListener.VoidDelegate cb)
	{
		UIEventListener listener = UIEventListener.Get(btn.gameObject);
		listener.onClick = cb;
	}

	public static void PlayPanelAlpha(PanelBase panel, bool openOrClosed, EventDelegate.Callback callBack = null)
	{
		TweenAlpha ta = panel.gameObject.GetComponent<TweenAlpha>();
		if (ta == null) 
		{
			ta = panel.gameObject.AddComponent<TweenAlpha>();	
		}

		ta.from = 0.0f;
		ta.to = 1.0f;	
		ta.duration = 0.2f;
		ta.method = UITweener.Method.EaseInOut;
		ta.SetOnFinished (callBack);

		if (openOrClosed) 
		{
			ta.PlayForward();
		}
		else 
		{
			ta.PlayReverse();
		}

	}

	public static void PlayPanelScale(PanelBase panel, bool openOrClosed, EventDelegate.Callback callBack = null)
	{
		TweenScale ts = panel.gameObject.GetComponent<TweenScale>();
		if (ts == null) 
		{
			ts = panel.gameObject.AddComponent<TweenScale> ();
		}

		ts.from = Vector3.zero;
		ts.to = Vector3.one;
		ts.duration = 0.2f;
		ts.method = UITweener.Method.EaseInOut;
		ts.SetOnFinished (callBack);

		if (openOrClosed) 
		{
			ts.PlayForward();
		}
		else 
		{
			ts.PlayReverse();
		}

	}

	public static string GetItemSuffix(int id)
	{
		if(id < 10)
		{
			return "0" + id;
		}
		else
		{
			return id + "";
		}
	}

	/// <summary>
	/// 设置时间格式
	/// </summary>
	/// <returns>The time DHM.</returns>
	/// <param name="timeSec">Time sec.</param>
	public static string setTimeDHMS(int timeSec)
	{
		if(timeSec != null){
			int day = timeSec / (3600 * 24);
			int lastDay = timeSec % (3600 * 24);
			int hour = lastDay / 3600;
			int lastHour = lastDay % 3600;
			
			int minute = lastHour /  60;
			int second = lastHour % 60;
			
			string days = Mathf.Round(day).ToString();
			if(days.Length < 2){
				days = days + "d ";
			}
			string hours = Mathf.Round(hour).ToString();
			if(hours.Length < 2){
				hours = "0" + hours;
			}
			string minutes = Mathf.Round(minute).ToString();
			if(minutes.Length < 2){
				minutes = "0" + minutes;
			}
			string seconds = Mathf.Round(second).ToString();
			if(seconds.Length < 2){
				seconds = "0" + seconds;
			}
			if(day != 0){
				return days + hours + ":" + minutes + ":" + seconds;
			}else if(hour != 0){
				return hours + ":" + minutes + ":" + seconds;
			}else{
				return	minutes + ":" + seconds;
			}
		}
		return null;		
	}

	/// <summary>
	/// 购买资源面板
	/// </summary>
	/// <returns>The resources U.</returns>
	/// <param name="resArr">Res arr.</param>
	/// <param name="buyType">Buy type.</param>
	public static void BuyResourcesUI(int[] resArr,ResourcesBuyType buyType,object data)
	{
		UIController.instance.CreatePanel(UICommon.UI_TIPS_BUYRES, resArr,buyType,data);
	}

	/// <summary>
	/// 金币不够请充值
	/// </summary>
	/// <returns>The cash U.</returns>
	public static void BuyCashUI()
	{
		Trace.trace(" -------- 金币不够请充值 --------- ",Trace.CHANNEL.UI);
	}

	/// <summary>
	/// 设置字符串颜色
	/// </summary>
	/// <returns>The string color.</returns>
	/// <param name="stringData">String data.</param>
	public static string SetStringColor(string stringData)
	{
		if(stringData != null)
		{
			return UICommon.SIXTEEN_RED + stringData + "[-]";
		}
		return null;
	}
	public static string SetStringSixteenColor(string stringData,string color)
	{
		if(stringData != null)
		{
			return color + stringData + "[-]";
		}
		return null;
	}


	public static Color SetFontARGB(Color color)
	{
		if(color != null)
		{

			return new Color(color.r/255,color.g/255,color.b/255,color.a/255);
		}
		return new Color(1f,1f,1f,1f);
	}
	/// <summary>
	/// 设置资源显示格式
	/// </summary>
	/// <returns>The resources show format.</returns>
	/// <param name="resData">Res data.</param>
	public static string SetResourcesShowFormat(int resData)
	{
		if(resData != null )
		{
			float resNum = (float)resData;
			if(resNum >= 10000.0f)
			{
				int res = Mathf.FloorToInt(resNum / 1000.0f);
				return res + "K";
			}
			else if(resNum >= 100000000.0f)
			{
				int res = Mathf.FloorToInt(resNum / 10000.0f);
				return res + "M";
			}
			else if(resNum < 10000.0f)
			{
				return (int)resNum + "";
			}


		}
		return null;
	}
	/// <summary>
	/// 弹出提示
	/// </summary>
	/// <param name="parent">Parent.</param>
	/// <param name="str">String.</param>
	public static void ShowTextPromptPanel(GameObject parent,string str)
	{
		string path = "profab/ui/public/TextPromptPanel";
		// 弹出提示
		GameObject prefab = Resources.Load(path) as GameObject;
		GameObject go = NGUITools.AddChild(parent, prefab);

		TextPromptPanel text = go.GetComponent<TextPromptPanel>();
		text.InitText(str);
	}

	/// <summary>
	/// 打开或者关闭loding
	/// </summary>
	/// <param name="isOpen">If set to <c>true</c> is open.</param>
	public static void LoadingPanelIsOpen(bool isOpen)
	{
		if(isOpen)
		{
			UIController.instance.CreatePanel (UICommon.UI_PANEL_LOADING);
		}
		else
		{
			UIController.instance.DeletePanel (UICommon.UI_PANEL_LOADING);
		}
	}
	public static void PlayCoinsEffect(GameObject parent,GameObject positionTarget,Model_Building model_building)
	{
		if(model_building != null && parent != null)
		{
			string iconName = "";
			int resNum = 0;
			switch (model_building.buildingType) 
			{
			case Model_Building.Building_Type.FoodFactory:
				iconName = "Food";
				break;
			case Model_Building.Building_Type.OilFactory:
				iconName = "Accel";
				break;
			case Model_Building.Building_Type.MetalFactory:
				iconName = "Metal";
				break;
			case Model_Building.Building_Type.RareFactory:
				iconName = "Rare";
				break;
			}
			if(model_building.model_Production.num != null)
			{
				resNum = model_building.model_Production.num;
				if(resNum > 100)
				{
					
					int coinCount =	resNum / 100;
					for(int coinNum = 0; coinNum < coinCount;coinNum ++)
					{
						UIHelper.CreateTankItem(parent,positionTarget,iconName,model_building);
					}
				}
			}
			
		}
	}
	
	public static void CreateTankItem (GameObject parent,GameObject positionTarget,string iconName,Model_Building model_building)
	{
		if(parent != null){
			GameObject Item = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "main/CoinsBezierItem");
			GameObject item = NGUITools.AddChild(parent,Item);
			
			UISprite coin = item.GetComponent<UISprite>();
			coin.spriteName = iconName;
			item.GetComponent<CoinsBezierItem>().Init(positionTarget,model_building);
			
		}
	}
 

	public static UIDropItem GetUIDropItemByPrizeItem(SlgPB.PrizeItem prizeItem)
	{
		UIDropItem drop = new UIDropItem (prizeItem);

		switch(drop.type)
		{
		case (int)DataConfig.DATA_TYPE.Unit:
			{
				DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (drop.id);
				drop.name = dataUnit.name;
				drop.icon = UICommon.UNIT_SMALL_PATH_PREFIX + drop.id;
			}
			break;

		case (int)DataConfig.DATA_TYPE.UnitPart:
			{
				DataUnitPart dataUnitPart = DataManager.instance.dataUnitPartGroup.GetPart (drop.id, 1);
				drop.name = dataUnitPart.name;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Item:
			{
				DataItem dataItem = DataManager.instance.dataItemGroup.GetItem (drop.id);
				drop.name = dataItem.name;
				drop.icon = UICommon.ITEM_PATH_PREFIX + drop.id;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Hero:
			{
				DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive(drop.id);
				drop.name = dataHero.name;
				drop.icon = UICommon.HERO_SMALL_PATH_PREFIX + drop.id;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Combat:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Combat);
				drop.icon = UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Honor:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Honor);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Food:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Food);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX +(int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Oil:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Oil);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Metal:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Metal);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX +(int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Rare:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Rare);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Cash:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Cash);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Exp:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Exp);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Energy:
			{
				drop.name = DataConfig.GetDataTypeName (DataConfig.DATA_TYPE.Energy);
				drop.icon =	UICommon.DROP_ITEM_PATH_PREFIX + (int)drop.type;
			}
			break;

		case (int)DataConfig.DATA_TYPE.Building:
			break;
		case (int)DataConfig.DATA_TYPE.Mission:
			break;
		case (int)DataConfig.DATA_TYPE.Battle:
			break;
		case (int)DataConfig.DATA_TYPE.DropGroup:
			break;
		case (int)DataConfig.DATA_TYPE.Equipment:
			break;
		}

		return drop;
	}
		
	// render ------------------------------------
	public static void IncreasePanelRender(UIPanel panel)
	{
		RenderHelper.SetUIPanelRenderQueue (panel, 3100, 1);
	}


}
