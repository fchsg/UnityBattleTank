using UnityEngine;
using System.Collections;

public class UIHeroDragItem : UIDragDropItem {

	public UIHeroPanel _heroPanel;
	public UIHeroPanel heroPanel
	{
		set { _heroPanel = value; }
		get{ return _heroPanel; }
	}

	public FormationSlotUIManager _slotUIManager;
	public FormationSlotUIManager slotUIManager
	{
		set { _slotUIManager = value; }
	}
	 

	UIHeroDragItem cloneDragItem;

	public enum STATE
	{
		SCROLLVIEW,   
		SLOT,
	}

	public STATE state = STATE.SCROLLVIEW;
	public UIHeroSlot attachSlotUI;

	public GameObject item_head;
	public GameObject item_body;


	void Start()
	{
		restriction = Restriction.Vertical;
		cloneOnDrag = true;

		item_head = transform.Find ("Hero_Head_Sprite").gameObject;
		item_body = transform.Find ("Hero_Body_Sprite").gameObject;

	}

	public void InitForSlot()
	{
		state = STATE.SLOT;
		restriction = Restriction.None;
		cloneOnDrag = false; 
	}

	protected override void OnDragStart ()
	{
		switch (state) 
		{
		case STATE.SCROLLVIEW:
			restriction = Restriction.Vertical;
			cloneOnDrag = true; 
			break;

		case STATE.SLOT:
			restriction = Restriction.None;
			cloneOnDrag = false;
			break;
		}
		base.OnDragStart ();

		if (!isHorizontalSlide) 
		{
			UpdateDraggingItemVisible ();
		}

		// 从slot上 拖拽
		if (state == STATE.SLOT) 
		{
			this.GetComponent<UIHeroHeadItem>().IncreaseDepth();
		}
	}

	protected override void OnDragDropRelease (GameObject surface)
	{
		SetBoxColliderEnabled ();
		ResetTeamSlotItemUI ();

		if (surface != null) 
		{
			switch(state)
			{
			case STATE.SCROLLVIEW:
				{
					// scrollView 移动空slot
					UIHeroSlot surfaceSlotUI = surface.GetComponent<UIHeroSlot>();
					if(surfaceSlotUI != null && !surfaceSlotUI.model_UnitGroup.HasHero())
					{
						Model_UnitGroup  model_UnitGroup = surfaceSlotUI.model_UnitGroup;

					//上阵
						Model_Formation.RESULT result = EnterBattleHero(model_UnitGroup);

						if(result == Model_Formation.RESULT.SUCCESS)
						{
							this.transform.position = surface.transform.position;
							attachSlotUI = surfaceSlotUI;
							state = STATE.SLOT;
							surfaceSlotUI.dragItemUI = this;

							// 更新上阵Unit显示数量
							_slotUIManager.UpdateBattleUnitNum (model_UnitGroup);

							// 更新scrollView列表
							UpdateHeroScrollView();	

							// 更新Unit列表
							_heroPanel._battleFormationPanel.UpdateCurrectUnitList ();
						}
						else
						{
							base.OnDragDropRelease(surface);
							UpdateErrorPopupMsg(result);
						}
						return;
					}

//					// scrollView 移动到包含 item slot
					UIHeroDragItem dragItemUI = surface.GetComponent<UIHeroDragItem>();
					if(dragItemUI != null && dragItemUI.attachSlotUI != null)
					{
						UIHeroSlot slotUI = dragItemUI.attachSlotUI;
						Model_UnitGroup model_UnitGroup = slotUI.model_UnitGroup;

						Model_Formation.RESULT result = EnterBattleHero(model_UnitGroup);
						if(result == Model_Formation.RESULT.SUCCESS)
						{
							dragItemUI.GetComponent<UIHeroHeadItem>().UpdateUI(model_UnitGroup.heroId);

							// 更新上阵Unit显示数量
							_slotUIManager.UpdateBattleUnitNum (model_UnitGroup);

							// 更新scrollView列表
							UpdateHeroScrollView();

							// 更新Unit列表
							_heroPanel._battleFormationPanel.UpdateCurrectUnitList ();
						}
						else
						{
							UpdateErrorPopupMsg(result);
						}

						// 删除当前拖动 GameObject
						base.OnDragDropRelease(surface);	
						return;
					}
				}
				break;

			case STATE.SLOT:

				/*
				 * 1 移动另一个slot
				 *   (1) slot 区域没有item
				 *   (2) slot 区域有item
				 * 2 移动到scrollView
				 * 3 移动到空白区
				 */

				// 1 移动另一个slot 
				// (1)slot区域没有item
				{
					UIHeroSlot surfaceSlotUI = surface.GetComponent<UIHeroSlot>();
					if(surfaceSlotUI != null && !surfaceSlotUI.model_UnitGroup.HasHero())
					{
						Model_Formation.RESULT result = ExchangeBattleHero(surfaceSlotUI.model_UnitGroup);
						if(result == Model_Formation.RESULT.SUCCESS)
						{
							// 更新上阵Unit显示数量 交换之前更新
							_slotUIManager.UpdateBattleUnitNum (surfaceSlotUI.model_UnitGroup);
							_slotUIManager.UpdateBattleUnitNum (attachSlotUI.model_UnitGroup);

							this.transform.position = surfaceSlotUI.transform.position;

							this.attachSlotUI.dragItemUI = null;
							this.attachSlotUI = surfaceSlotUI;
							this.attachSlotUI.dragItemUI = this;

							this.GetComponent<UIHeroHeadItem>().UpdateUI(surfaceSlotUI.model_UnitGroup.heroId);

							// 更新Unit列表
							_heroPanel._battleFormationPanel.UpdateCurrectUnitList ();
						}
						else
						{
							base.OnDragDropRelease(surface);
						}

						UpdateHeroScrollView();
						return;
					}
				}

				// (2) slot区域有item
				{
					UIHeroDragItem surfaceDragItemUI = surface.GetComponent<UIHeroDragItem>();

					if(surfaceDragItemUI != null && surfaceDragItemUI.attachSlotUI != null)
					{
						UIHeroSlot surfaceSlotUI = surfaceDragItemUI.attachSlotUI;

						Model_Formation.RESULT result = ExchangeBattleHero(surfaceSlotUI.model_UnitGroup);
						if(result == Model_Formation.RESULT.SUCCESS)
						{
							// 更显UI显示数据
							UIHeroSlot sourceSlotUI =  this.attachSlotUI;
							UIHeroDragItem sourceDragUI = this;

							UIHeroSlot targetSlotUI = surfaceDragItemUI.attachSlotUI;
							UIHeroDragItem targetDragUI = surfaceDragItemUI;

							UIHeroHeadItem sourceSlotItemUI = sourceDragUI.gameObject.GetComponent<UIHeroHeadItem>();
							UIHeroHeadItem targetSlotItemUI = targetDragUI.gameObject.GetComponent<UIHeroHeadItem>();

							Model_UnitGroup source = attachSlotUI.model_UnitGroup;
							Model_UnitGroup target = targetSlotUI.model_UnitGroup;

							sourceSlotItemUI.UpdateUI(source.heroId);
							targetSlotItemUI.UpdateUI(target.heroId);

							this.transform.position = sourceSlotUI.transform.position;

							// 更新上阵Unit显示数量
							_slotUIManager.UpdateBattleUnitNum (source);
							_slotUIManager.UpdateBattleUnitNum (target);

							// 更新Unit列表
							_heroPanel._battleFormationPanel.UpdateCurrectUnitList ();
						}
						else
						{
							base.OnDragDropRelease(surface);
						}

						UpdateHeroScrollView();

						return;
					}

				}

				// 2 移动到scrollView 下阵
				{
					UIHeroDragItem surfaceDragItemUI = surface.GetComponent<UIHeroDragItem>();
					bool isScrllViewCollider = surface.gameObject.name.Equals("scrollView_bg_hero");

					if((surfaceDragItemUI != null && surfaceDragItemUI.attachSlotUI == null) ||
						isScrllViewCollider)
					{
						ExitBattleHero();

						attachSlotUI.dragItemUI = null;

						UpdateHeroScrollView();

						_slotUIManager.UpdateBattleUnitNum (attachSlotUI.model_UnitGroup);

						DestroyImmediate(this.gameObject);
						return;
					}
				}

				// 拖动到其他区域 
				if(attachSlotUI != null)
				{
					this.transform.position = attachSlotUI.transform.position;
					return;
				}

				break;
			}
		}
		base.OnDragDropRelease(surface);
	}


	public void UpdateHeroScrollView()
	{
		if (_heroPanel._battleFormationPanel._state == BattleFormationPanel.STATE.HERO) 
		{
			heroPanel.UpdateScrollView ();
		}
	}

	// 从scrollView中 上阵Hero
	public Model_Formation.RESULT EnterBattleHero(Model_UnitGroup model_UnitGroup)
	{
		Model_Formation.RESULT result = Model_Formation.RESULT.SUCCESS;

		UIHeroBodyItem bodyItem = this.GetComponent<UIHeroBodyItem>();
		int heroId = bodyItem.heroId;

		Model_UnitGroup new_UnitGroup = new Model_UnitGroup();
		new_UnitGroup.teamId = model_UnitGroup.teamId;
		new_UnitGroup.posId  = model_UnitGroup.posId;
		new_UnitGroup.heroId = heroId;

		result = _heroPanel._battleFormationPanel.model_Formation.AddHero(new_UnitGroup);

		return result;
	}

	// 交换Hero 
	public Model_Formation.RESULT ExchangeBattleHero(Model_UnitGroup target)
	{
		Model_Formation.RESULT result = Model_Formation.RESULT.SUCCESS;

		Model_UnitGroup source = attachSlotUI.model_UnitGroup;
		result = _heroPanel._battleFormationPanel.model_Formation.ExchangeHero (source, target);

		return result;
	}

	// 下阵Hero
	public Model_Formation.RESULT ExitBattleHero()
	{
		Model_Formation.RESULT result = Model_Formation.RESULT.SUCCESS;

		Model_UnitGroup model_UnitGroup = attachSlotUI.model_UnitGroup;
		_heroPanel._battleFormationPanel.model_Formation.RemoveHero(model_UnitGroup);

		return result;
	}


	public void UpdateErrorPopupMsg(Model_Formation.RESULT result)
	{
		string path = "profab/ui/public/TextPromptPanel";

		string msg = "";
		switch (result)
		{
		case Model_Formation.RESULT.SUCCESS:
			msg = "成功"; 
			break;
		case Model_Formation.RESULT.SAME_HERO:
			msg = "队伍中已经包含该类型Hero"; 
			break;
		}

		// 弹出提示
		UIHelper.ShowTextPromptPanel (_heroPanel.gameObject, msg);
	}


	public void ResetTeamSlotItemUI()
	{
		UIHeroHeadItem headItemUI = this.GetComponent<UIHeroHeadItem> ();
		if (headItemUI != null) 
		{
			headItemUI.ResetDepth();
		}

		cloneDragItem = null;
	}

	public void SetBoxColliderEnabled()
	{
		this.gameObject.GetComponent<BoxCollider>().enabled = true;
	}

	public void UpdateDraggingItemVisible()
	{
	// 从scrollView 拖拽
		if (cloneObject != null) 
		{
			cloneDragItem = cloneObject.GetComponent<UIHeroDragItem> ();
			if (cloneDragItem != null) 
			{
				cloneDragItem.item_body.SetActive(false);
				cloneDragItem.item_head.SetActive(true);
				cloneDragItem.GetComponent<UIHeroHeadItem>().IncreaseDepth();
			}
		}
	
	}

}
