using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionClearResultPanel : PanelBase {

	// path
	public const string NAME_PATH = "profab/ui/mission/mission_clear_name";
	public const string ITEM_PATH   = "profab/ui/mission/mission_clear_item";
	public const string HERO_PATH  = "profab/ui/mission/mission_clear_hero";

	public const int ITME_PROPS_COUNT = 6;

	// description
	public string TITLE_TEXT = "第{0}次扫荡";
	public string CLEAR_TEXT = "扫荡{0}次";


	// ui
	public UITable Content_Table;
	public UIScrollView ui_scrollview;

	public UILabel left_label;
	public UILabel clear_label;

	// data
	public float ui_name_height;
	public float ui_item_height;
	public float ui_hero_height;
	public float ui_scrollview_height;

	public float time_interval_item = 0.4f;
	public float time_interval_props = 0.15f;

	public int all_item_count = 0;


	private int magicId;
	private int clear_count;

	void Awake()
	{
		// test
		//Init ();
		//StartCoroutine (CreateContentAll ());
	}

	public override void Init ()
	{
		base.Init ();

		// 关闭
		UIButton closedBtn = UIHelper.FindChildInObject (this.gameObject, "Bg").GetComponent<UIButton> ();
		UIHelper.AddBtnClick (closedBtn, OnClosed);

		Content_Table = UIHelper.FindChildInObject (this.gameObject, "Content_Table").GetComponent<UITable>();
		ui_scrollview = UIHelper.FindChildInObject (this.gameObject, "Scroll_View").GetComponent<UIScrollView>();

		left_label = UIHelper.FindChildInObject (this.gameObject, "Left_Label").GetComponent<UILabel>();
		clear_label = UIHelper.FindChildInObject (this.gameObject, "Btn_Clear/Label").GetComponent<UILabel>();

		UIHelper.AddBtnClick (this.gameObject, "Btn_Clear", OnClearClicked);
		UIHelper.AddBtnClick (this.gameObject, "Btn_Confirm", OnConfirmClicked);


		// 计算UI高度
		CalcUIHeight ();

		// 奖励条目
		all_item_count = InstancePlayer.instance.multiFightPrizeItems.Count;
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if (parameters != null) 
		{
			magicId = (int)parameters [0];
			clear_count = (int)parameters [1];
		}

		Model_Mission model_Mission = InstancePlayer.instance.model_User.model_level.GetMission (magicId);
		left_label.text = model_Mission.remainFightNum + "";
		clear_label.text = string.Format(CLEAR_TEXT, clear_count);


		ui_scrollview.enabled = false;

		StartCoroutine (CreateContentAll ());
	}

	IEnumerator CreateContentAll()
	{
		float total_height = 0;

		for (int i = 0; i < all_item_count; ++i) 
		{
			CreateTitle (i);
			total_height += ui_name_height;
			MoveScrollView (total_height, ui_name_height);
			yield return new WaitForSeconds (time_interval_item);

			CreateItem (i);
			total_height += ui_item_height;
			MoveScrollView (total_height, ui_item_height);
			yield return new WaitForSeconds (time_interval_item);

			int hero_line_count = 0;
			CreateHero (ref hero_line_count, i);
			total_height += (hero_line_count * ui_hero_height);
			MoveScrollView (total_height, hero_line_count * ui_hero_height);
			yield return new WaitForSeconds (time_interval_item);
		}

		ui_scrollview.enabled = true;
	}

	public void MoveScrollView(float height, float itemHeight)
	{
		if (height >= ui_scrollview_height) 
		{
			ui_scrollview.MoveRelative (new Vector3(0, itemHeight, 0));
		}
	}

	private void CreateTitle(int index)
	{
		GameObject cell_prefab = Resources.Load (NAME_PATH) as GameObject;
		GameObject cell = NGUITools.AddChild (Content_Table.gameObject, cell_prefab);
		Content_Table.Reposition ();

		UpdateTitleUI (cell, index);
	}

	private void UpdateTitleUI(GameObject cell, int index)
	{
		UILabel name_Label = cell.transform.Find ("Sprite/Label").GetComponent<UILabel> ();
		name_Label.text =  string.Format(TITLE_TEXT, index + 1);
	}

	private void CreateItem(int index)
	{
		GameObject cell_prefab = Resources.Load (ITEM_PATH) as GameObject;
		GameObject cell = NGUITools.AddChild (Content_Table.gameObject, cell_prefab);
		Content_Table.Reposition ();

		StartCoroutine (PlayPropsAnimation(cell, index));
	}
		
	IEnumerator PlayPropsAnimation(GameObject item, int index)
	{
		SlgPB.MultiPrizeItem multiPrizeItem = InstancePlayer.instance.multiFightPrizeItems[index];
		List<SlgPB.PrizeItem> prizeItems = multiPrizeItem.prizeItems;

		int itemCount = prizeItems.Count; 

		for (int i = 0; i < ITME_PROPS_COUNT; ++i) 
		{
			GameObject props = item.transform.Find ("Bg_Sprite_" + i).gameObject;
			if (i < itemCount)
			{
				props.SetActive (true);
				props.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
				iTween.ScaleTo (props, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", time_interval_props));

				SlgPB.PrizeItem prizeItem = prizeItems [i];
				UpdateItemUI (props, prizeItem);

				yield return new WaitForSeconds (time_interval_props);
			}
			else
			{
				props.SetActive (false);
			}
		}
	}

	public void UpdateItemUI(GameObject cell, SlgPB.PrizeItem prizeItem)
	{
		UITexture texture = cell.transform.Find ("Texture").GetComponent<UITexture>();
		UILabel label = cell.transform.Find ("Label").GetComponent<UILabel> ();

		label.text = prizeItem.num + "";
	}

	private void CreateHero(ref int count, int index)
	{
		List<Model_HeroGroup.ExpChangeResult> expChangeResults = InstancePlayer.instance.multiFightHeroGotExp[index];

		int data_hero_count = expChangeResults.Count;

		int ui_hero_count = 0;
		if (data_hero_count % 2 == 0)
		{
			ui_hero_count = data_hero_count / 2;
		}
		else 
		{
			ui_hero_count = data_hero_count / 2 + 1;
		}

		count = ui_hero_count;
			
		for (int i = 0; i < ui_hero_count; ++i) 
		{
			GameObject cell_prefab = Resources.Load (HERO_PATH) as GameObject;
			GameObject cell = NGUITools.AddChild (Content_Table.gameObject, cell_prefab);
			Content_Table.Reposition ();

			GameObject lab_0 = cell.transform.Find ("Label_0").gameObject;
			GameObject lab_1 = cell.transform.Find ("Label_1").gameObject;

			int data_index_0 = i * 2;
			int data_index_1 = i * 2 + 1;

			if (data_hero_count < ((i + 1) * 2)) 
			{
				lab_1.SetActive (false);
				UpdateHeroUI (lab_0, expChangeResults[data_index_0]);
			} 
			else
			{
				UpdateHeroUI (lab_0, expChangeResults[data_index_0]);
				UpdateHeroUI (lab_1, expChangeResults[data_index_1]);
			}
		}
	}

	private void UpdateHeroUI(GameObject cell, Model_HeroGroup.ExpChangeResult expHero)
	{
		UILabel name = cell.transform.Find ("Name").GetComponent <UILabel> ();
		UILabel level = cell.transform.Find ("Level").GetComponent <UILabel> ();

		DataHero dataHero = DataManager.instance.dataHeroGroup.GetHeroPrimitive (expHero.heroId);

		name.text = dataHero.name;
		level.text = expHero.level + "";
	}
		
	private	void CalcUIHeight()
	{
		UIWidget name_widget = ResourceHelper.Load (NAME_PATH).GetComponent<UIWidget>();
		ui_name_height = name_widget.height;
		Destroy (name_widget.gameObject);

		UIWidget item_widget = ResourceHelper.Load (ITEM_PATH).GetComponent<UIWidget>();
		ui_item_height = item_widget.height;
		Destroy (item_widget.gameObject);

		UIWidget hero_widget = ResourceHelper.Load (HERO_PATH).GetComponent<UIWidget>();
		ui_hero_height = hero_widget.height;
		Destroy (hero_widget.gameObject);

		ui_scrollview_height = ui_scrollview.panel.GetViewSize().y;
	}
		
	public void OnConfirmClicked()
	{
		OnClosed ();
	}
		
	public void OnClearClicked()
	{
		BattleConnection.instance.SrartClearBattle (magicId, clear_count, MultiFightCallback, this.gameObject);
	}

	public  void MultiFightCallback(bool success, System.Object content)
	{
		if (success) 
		{
			base.DeleteImmediately ();
			UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION_ClEAR_RESULT, magicId, clear_count);
		}
		else
		{
			Trace.trace ("扫荡请求失败", Trace.CHANNEL.UI);
		}
	}

	public void OnClosed()
	{
		base.Delete ();
	}
}
