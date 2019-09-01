using UnityEngine;
using System.Collections;

public class FormationDragItemUI : UIDragDropItemFormation {

	public const string DUST_PARTICLE_PATH = "profab/ui/effect/TankDust_zhenxing";

	public enum STATE
	{
		SCROLLVIEW,   
		SLOT,
	}

	public STATE state = STATE.SCROLLVIEW;
	public FormationSlotUI attachSlotUI;

	public BattleFormationPanel battlePanel;


	public GameObject item_scrollview;
	public GameObject item_slot;

		
	void Start()
	{
		restriction = Restriction.Vertical;
		cloneOnDrag = true;

		item_scrollview = transform.Find ("formation_unit_scrollview").gameObject;
		item_slot 		= transform.Find ("formation_unit_slot").gameObject;
	}

	public void InitForSlot()
	{
		state = STATE.SLOT;
		restriction = Restriction.None;
		cloneOnDrag = false; 
	}


	protected virtual void OnPress (bool isPressed)
	{
		base.OnPress (isPressed);
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
			this.GetComponent<FormationSlotItemUI>().SetLabelVisible(false);
			this.GetComponent<FormationSlotItemUI>().IncreaseDepth();
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
				FormationSlotUI surfaceSlotUI = surface.GetComponent<FormationSlotUI>();
				if(surfaceSlotUI != null && surfaceSlotUI.model_UnitGroup.isEmpty)
				{
					Model_UnitGroup  model_UnitGroup = surfaceSlotUI.model_UnitGroup;
					
					//上阵
					Model_Formation.RESULT result = EnterBattleFormation(model_UnitGroup);
					if(result == Model_Formation.RESULT.SUCCESS)
					{
						this.transform.position = surface.transform.position;
						attachSlotUI = surfaceSlotUI;
						state = STATE.SLOT;
						surfaceSlotUI.dragItemUI = gameObject.GetComponent<FormationDragItemUI>();
						this.GetComponent<FormationSlotItemUI>().UpdateUI(model_UnitGroup);

						// 首次进入 上阵Unit 如果teamId = 0 强制修改为1
						int teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId();
						if (teamId == 0) 
						{
							InstancePlayer.instance.model_User.model_Formation.SetSelectTeamId(1);
						}
						
						// 更新scrollView列表
						UpdateUnitScrollView();	
						
						// 播放粒子特效
						StartCoroutine(PlayEffect (this.gameObject));
					}
					else
					{
						base.OnDragDropRelease(surface);
						UpdateEnterBattlePopupMsg(result);
					}
					return;
				}
				
				// scrollView 移动到包含 item slot
				FormationDragItemUI dragItemUI = surface.GetComponent<FormationDragItemUI>();
				if(dragItemUI != null && dragItemUI.attachSlotUI != null)
				{
					FormationSlotUI slotUI = dragItemUI.attachSlotUI;
					Model_UnitGroup model_UnitGroup = slotUI.model_UnitGroup;
					
					Model_Formation.RESULT result = EnterExchangeBattleFormation(model_UnitGroup);
					if(result == Model_Formation.RESULT.SUCCESS)
					{
						dragItemUI.GetComponent<FormationSlotItemUI>().UpdateUI(model_UnitGroup);

						// 替换spine tankicon
						dragItemUI.ReplaceTankIcon();
						
						// 更新scrollView列表
						UpdateUnitScrollView();

						// 播放粒子特效
						StartCoroutine(PlayEffect (dragItemUI.gameObject));
					}
					else
					{
						UpdateEnterBattlePopupMsg(result);
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
					FormationSlotUI surfaceSlotUI = surface.GetComponent<FormationSlotUI>();
					if(surfaceSlotUI != null && surfaceSlotUI.model_UnitGroup.isEmpty)
					{
						Model_Formation.RESULT result = ExchageFormation(surfaceSlotUI.model_UnitGroup);
						if(result == Model_Formation.RESULT.SUCCESS)
						{
							this.transform.position = surfaceSlotUI.transform.position;
							
							this.attachSlotUI.dragItemUI = null;
							this.attachSlotUI = surfaceSlotUI;
							this.attachSlotUI.dragItemUI = this.GetComponent<FormationDragItemUI>();

							this.GetComponent<FormationSlotItemUI>().UpdateUI(surfaceSlotUI.model_UnitGroup);

							// 播放粒子特效
							StartCoroutine(PlayEffect (this.gameObject));

						}
						else
						{
							base.OnDragDropRelease(surface);
						}
						UpdateUnitScrollView();

						return;
					}
				}

				// (2) slot区域有item
				{
					FormationDragItemUI surfaceDragItemUI = surface.GetComponent<FormationDragItemUI>();

					if(surfaceDragItemUI != null && surfaceDragItemUI.attachSlotUI != null)
					{
						FormationSlotUI surfaceSlotUI = surfaceDragItemUI.attachSlotUI;

						Model_Formation.RESULT result = ExchageFormation(surfaceSlotUI.model_UnitGroup);
						if(result == Model_Formation.RESULT.SUCCESS)
						{
							// 更显UI显示数据
							FormationSlotUI sourceSlotUI =  this.attachSlotUI;
							FormationDragItemUI sourceDragUI = this.GetComponent<FormationDragItemUI>();
							
							FormationSlotUI targetSlotUI = surfaceDragItemUI.attachSlotUI;
							FormationDragItemUI targetDragUI = surfaceDragItemUI;

							FormationSlotItemUI sourceSlotItemUI = sourceDragUI.gameObject.GetComponent<FormationSlotItemUI>();
							FormationSlotItemUI targetSlotItemUI = targetDragUI.gameObject.GetComponent<FormationSlotItemUI>();

							Model_UnitGroup source = attachSlotUI.model_UnitGroup;
							Model_UnitGroup target = targetSlotUI.model_UnitGroup;
							
							sourceSlotItemUI.UpdateUI(source);
							targetSlotItemUI.UpdateUI(target);
							
							this.transform.position = sourceSlotUI.transform.position;

							// 交换 spine tank icon
							surfaceDragItemUI.ReplaceTankIcon();
							sourceDragUI.ReplaceTankIcon ();

							// 播放粒子特效
							StartCoroutine(PlayEffect (sourceSlotItemUI.gameObject));
							StartCoroutine(PlayEffect (targetSlotItemUI.gameObject));
						}
						else
						{
							base.OnDragDropRelease(surface);
						}
						UpdateUnitScrollView();
						
						return;
					}

				}

				// 2 移动到scrollView 下阵
				{
					FormationDragItemUI surfaceDragItemUI = surface.GetComponent<FormationDragItemUI>();
					bool isScrllViewCollider = surface.gameObject.name.Equals("scrollView_bg_unit");

					if((surfaceDragItemUI != null && surfaceDragItemUI.attachSlotUI == null) ||
				  	   isScrllViewCollider)
					{
						ExitBattleFormation();
						
						attachSlotUI.dragItemUI = null;
						UpdateUnitScrollView();
						DestroyImmediate(this.gameObject);
						return;
					}
				}
			
				// 拖动到其他区域 
				if(attachSlotUI != null)
				{
					this.transform.position = attachSlotUI.transform.position;
					StartCoroutine(PlayEffect (this.gameObject));

					return;
				}

				break;
			}
		}
		base.OnDragDropRelease(surface);
	}


	// 从scrollView中 上阵 slot为空白
	public Model_Formation.RESULT EnterBattleFormation(Model_UnitGroup model_UnitGroup)
	{
		Model_Formation.RESULT result = Model_Formation.RESULT.SUCCESS;

		FormationScrollViewItemUI scViewItem = this.GetComponent<FormationScrollViewItemUI>();
		int unitId = scViewItem.unitId;
		int battleCount = 0;   // 上阵数量

		Model_Unit model_Unit; 
		InstancePlayer.instance.model_User.unlockUnits.TryGetValue(unitId, out model_Unit);
		if (model_Unit != null) 
		{
			battleCount =  Mathf.Min(model_Unit.num, model_UnitGroup.maxNum);
		}

		Model_UnitGroup new_UnitGroup = new Model_UnitGroup();
		new_UnitGroup.unitId = unitId;
		new_UnitGroup.num    = battleCount;       
		new_UnitGroup.teamId = model_UnitGroup.teamId;
		new_UnitGroup.posId  = model_UnitGroup.posId;

		result = battlePanel.model_Formation.Add(new_UnitGroup);
					
		return result;
	}

	// 从scrollView中 上阵 slot中包含item
	public Model_Formation.RESULT EnterExchangeBattleFormation(Model_UnitGroup model_UnitGroup)
	{
		Model_Formation.RESULT reslut = Model_Formation.RESULT.SUCCESS;

		FormationScrollViewItemUI scViewItem = this.GetComponent<FormationScrollViewItemUI>();
		int unitId = scViewItem.unitId;
		int battleCount = 0;

		Model_Unit model_Unit; 
		InstancePlayer.instance.model_User.unlockUnits.TryGetValue(unitId, out model_Unit);
		if (model_Unit != null) 
		{
			battleCount =  Mathf.Min(model_Unit.num, model_UnitGroup.maxNum);
		}

		Model_UnitGroup new_UnitGroup = new Model_UnitGroup();
		new_UnitGroup.unitId = unitId;
		new_UnitGroup.num    = battleCount;       
		new_UnitGroup.teamId = model_UnitGroup.teamId;
		new_UnitGroup.posId  = model_UnitGroup.posId;

		reslut = battlePanel.model_Formation.AddExchange(new_UnitGroup, model_UnitGroup);

		return reslut;
	}

	// 下阵
	public void ExitBattleFormation()
	{
		Model_UnitGroup model_UnitGroup = attachSlotUI.model_UnitGroup;
		battlePanel.model_Formation.Remove(model_UnitGroup);
	}


	public void UpdateEnterBattlePopupMsg(Model_Formation.RESULT result)
	{
		string path = "profab/ui/public/TextPromptPanel";

		string msg = "";
		switch (result)
		{
		case Model_Formation.RESULT.SUCCESS:
			msg = "成功"; 
			break;
		case Model_Formation.RESULT.SAME:
			msg = "队伍中已经包含该类型Unit"; 
			break;
		case Model_Formation.RESULT.LACK:
			msg = "可战斗unit不足"; 
			break;
		case Model_Formation.RESULT.MAX:
			msg = "达到最大战斗数";
			break;
		}

		// 弹出提示
		UIHelper.ShowTextPromptPanel (battlePanel.gameObject, msg);
	}


	// 交换Unit
	public Model_Formation.RESULT ExchageFormation(Model_UnitGroup target)
	{
		Model_Formation.RESULT result = Model_Formation.RESULT.SUCCESS;

		Model_UnitGroup source = attachSlotUI.model_UnitGroup;
		result = battlePanel.model_Formation.Exchange (source, target);


		return result;
	}
	
	public void SetBoxColliderEnabled()
	{
		this.gameObject.GetComponent<BoxCollider>().enabled = true;
	}

	public void ResetTeamSlotItemUI()
	{
		FormationSlotItemUI currentSlotItemUI = this.GetComponent<FormationSlotItemUI> ();
		if (currentSlotItemUI != null) 
		{
			currentSlotItemUI.ResetUI();
		}

		cloneFormationDragItemUI = null;

		battlePanel.ResetCurrentTeamSlotItemUI ();
	}

	public void UpdateUnitScrollView()
	{
		if (battlePanel._state == BattleFormationPanel.STATE.UNIT) 
		{
			battlePanel.UpdateCurrectUnitList();
		}
	}


	public void UpdateDraggingItemVisible()
	{
		// 从scrollView 拖拽
		if (cloneFormationDragItemUI != null) 
		{
			cloneFormationDragItemUI.item_scrollview.SetActive(false);
			cloneFormationDragItemUI.item_slot.SetActive(true);
			cloneFormationDragItemUI.GetComponent<FormationSlotItemUI>().IncreaseDepth();
			cloneFormationDragItemUI.GetComponent<FormationSlotItemUI>().SetLabelVisible(false);

		}
	}


	// 播放粒子效果 
	IEnumerator PlayEffect(GameObject go)
	{
		go = go.GetComponent<FormationDragItemUI> ().attachSlotUI.gameObject;
		GameObject prefab = Resources.Load(DUST_PARTICLE_PATH) as GameObject;
		GameObject instanceObj  = NGUITools.AddChild(go, prefab);

    	instanceObj.transform.localScale = AppConfig.UNITY_SCALE_UI;

		battlePanel.particleList.Add (instanceObj);

		float leftTime = ParticleHelper.GetlifeCycleSec (instanceObj.transform);
		yield return new WaitForSeconds (leftTime);

		if (instanceObj != null) 
		{
			battlePanel.particleList.Remove (instanceObj);
			DestroyImmediate (instanceObj);
		} 
	}

}
