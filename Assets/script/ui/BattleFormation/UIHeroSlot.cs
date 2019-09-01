using UnityEngine;
using System.Collections;

public class UIHeroSlot : MonoBehaviour {
	
	private const string HERO_TIEM_PATH = "profab/ui/BattleFormation/formation_hero_item";

	public UIHeroDragItem dragItemUI;
	public UIHeroPanel _heroPanel;

	public Model_UnitGroup model_UnitGroup;

	public int i;
	public int j;

	public void UpdateUI(Model_UnitGroup unitGroup, UIHeroPanel heroPanel)
	{
		model_UnitGroup = unitGroup;
		_heroPanel = heroPanel;

		// 更新阵型数据
		if (unitGroup.HasHero())
		{
			InitHeroSlotUI();
		}
	}

	public void InitHeroSlotUI()
	{
		GameObject prefab = Resources.Load (HERO_TIEM_PATH) as GameObject;
		GameObject obj = NGUITools.AddChild (this.gameObject, prefab);
		obj.transform.parent = _heroPanel.dragDropRoot.transform;

		obj.transform.Find("Hero_Head_Sprite").gameObject.SetActive(true);
		obj.transform.Find("Hero_Body_Sprite").gameObject.SetActive(false);

		dragItemUI = obj.GetComponent<UIHeroDragItem>();
		dragItemUI.InitForSlot();
		dragItemUI.attachSlotUI = this;

		UIHeroHeadItem slotItem = obj.GetComponent<UIHeroHeadItem>();
		SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (model_UnitGroup.heroId);
		slotItem.UpdateUI(hero);

		UIHeroDragItem dragItem = obj.GetComponent<UIHeroDragItem>();
		dragItem.attachSlotUI = this;
		dragItem.heroPanel = _heroPanel;
		dragItem.slotUIManager = _heroPanel._battleFormationPanel.slotUIManager;

	}


}
