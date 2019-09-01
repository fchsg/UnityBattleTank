using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class UIHeroPanel : MonoBehaviour {

	public const string HERO_ITEM_PATH = "profab/ui/BattleFormation/formation_hero_item";

	//------------------------------------------data
	private bool isInitScrllView = false;
	public BattleFormationPanel _battleFormationPanel;

	public UIHeroCategory _formationHeroCategory;

	//---------------------------------------------UI
	public UIScrollView _scrollView;
	public UIGrid _grid;

	public UIDragDropRoot dragDropRoot;


	public void Init(BattleFormationPanel battlePanel)
	{
		_formationHeroCategory = new UIHeroCategory (InstancePlayer.instance.model_User.model_heroGroup.heroesMap);

		_battleFormationPanel = battlePanel;

		GameObject container = UIHelper.FindChildInObject (gameObject, "HeroContainer");
		_scrollView = UIHelper.FindChildInObject (container, "hero_scView_0").GetComponent<UIScrollView>();
		_grid = UIHelper.FindChildInObject (container, "ui_grid").GetComponent<UIGrid> ();

		dragDropRoot = _battleFormationPanel.dragDropRoot;
	}


	public void UpdateScrollView()
	{
		List <UIHeroCategory.Hero> herosCategory= _formationHeroCategory.GetSortHero ();

		if (!isInitScrllView)
		{
			int i = 0;
			isInitScrllView = true;

			foreach (UIHeroCategory.Hero heroCategory in herosCategory)
			{
				GameObject prefab = Resources.Load (HERO_ITEM_PATH) as GameObject;
				GameObject obj = NGUITools.AddChild (_grid.gameObject, prefab);
				_grid.AddChild (obj.transform);

				obj.name = UIHelper.GetItemSuffix(++i);

				UIHeroBodyItem  heroBodyItem = obj.GetComponent<UIHeroBodyItem> ();
				heroBodyItem.UpdateUI (heroCategory);

				UIHeroHeadItem  heroHeadItem = obj.GetComponent<UIHeroHeadItem> ();
				heroHeadItem.UpdateUI (heroCategory.pb_Hero);

				UIHeroDragItem heroDragItem = obj.GetComponent<UIHeroDragItem> ();
				heroDragItem.heroPanel = this;
				heroDragItem.slotUIManager = _battleFormationPanel.slotUIManager;

				obj.transform.Find("Hero_Head_Sprite").gameObject.SetActive(false);
				obj.transform.Find("Hero_Body_Sprite").gameObject.SetActive(true);
			}

			// scrollView cell 排序
			_grid.animateSmoothly = false;
			_grid.repositionNow = true;

			// 小于单行最多显示 停止滑动
			if(herosCategory.Count < BattleFormationPanel.UNIT_SHOW_MAX)
			{
				_scrollView.enabled = false;
			}
			else
			{
				_scrollView.enabled = true;
			}
		}
		else
		{
			int id = 0;
			foreach (UIHeroCategory.Hero heroCategory in herosCategory)
			{
				string cellPath = UIHelper.GetItemSuffix(++id);
				GameObject cell = _grid.transform.Find(cellPath).gameObject;

				UIHeroBodyItem  heroBodyItem = cell.GetComponent<UIHeroBodyItem> ();
				heroBodyItem.UpdateUI (heroCategory);
			}
		}
	}

}
