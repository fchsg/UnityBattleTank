using UnityEngine;
using System.Collections;
using SlgPB;
/// <summary>
/// 坦克回收
/// </summary>
public class RecyclingTankPanel : MonoBehaviour {

	private UIButton _RecyclingNumBtn;
	private UIButton _DelBtn;
	private UIButton _AddBtn;
	private UIButton _close;

	private UILabel _AddTankNumLabel; 
	private UISlider _TankItem_Slider; 
	private int _CurrentTankNum;
	private float _canRecyclingUnitCount;
	//data 
	DataUnit _dataUnit;
	Model_Unit _model_Unit;

	public void Init(Model_Unit model_Unit)
	{
		_model_Unit = model_Unit;
	}
	void Start () {
	
	}

	void Update () {
		if(_model_Unit != null)
		{
			_canRecyclingUnitCount = (float)_model_Unit.num;
			if (_TankItem_Slider != null && _AddTankNumLabel != null)
				_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * _canRecyclingUnitCount).ToString();
			_CurrentTankNum = Mathf.RoundToInt(_TankItem_Slider.value * _canRecyclingUnitCount);
		}
	}

	public void OnDel()
	{
//		Trace.trace("OnDel ",Trace.CHANNEL.UI);
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value - (1 /_canRecyclingUnitCount);
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * _canRecyclingUnitCount).ToString();
		}
	}
	
	public void	OnAdd()
	{
//		Trace.trace("OnAdd ",Trace.CHANNEL.UI);
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value + (1 /_canRecyclingUnitCount);
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * _canRecyclingUnitCount).ToString();
		}
	}

	public void OnRecycling()
	{	
		if(_CurrentTankNum <= _model_Unit.num)
		{
			DismissUnit();

		}
		else
		{
			Trace.trace(" 战车不足 ",Trace.CHANNEL.UI);
		}

	}

	void OnHidePanel()
	{
		this.gameObject.SetActive(false);
	}


	void Awake()
	{
		_RecyclingNumBtn = transform.Find("RecyclingNumBtn").GetComponent<UIButton>();
		_DelBtn = transform.Find("DelBtn").GetComponent<UIButton>();
		_AddBtn = transform.Find("AddBtn").GetComponent<UIButton>();
		_AddTankNumLabel = transform.Find("AddTankNumLabel").GetComponent<UILabel>();
		
		_TankItem_Slider = transform.Find("TankItem_Slider").GetComponent<UISlider>();  
		_close = transform.Find("btnClose").GetComponent<UIButton>();

		EventDelegate evClose = new EventDelegate (OnHidePanel);
		_close.onClick.Add (evClose);

		EventDelegate evRecyclin = new EventDelegate (OnRecycling);
		_RecyclingNumBtn.onClick.Add (evRecyclin);

		EventDelegate evDel = new EventDelegate (OnDel);
		_DelBtn.onClick.Add (evDel);
		EventDelegate evAdd = new EventDelegate (OnAdd);
		_AddBtn.onClick.Add (evAdd);
	}
	//=========================================================================

	void DismissUnit()  //解散Unit
	{
		UIHelper.LoadingPanelIsOpen(true);

		PBConnect_dismissUnit.Dismiss (_model_Unit.unitId, _CurrentTankNum, OnDismissUnit);

		/*
		ProcessUnitRequest request = new ProcessUnitRequest ();
		
		request.api = new Model_ApiRequest ().api;
		request.unitId = _model_Unit.unitId;
		request.num = _CurrentTankNum;
		
		(new PBConnect_dismissUnit ()).Send (request, OnDismissUnit);
		*/

	}
	
	void OnDismissUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("OnDismissUnit success",Trace.CHANNEL.INTEGRATION);
			OnHidePanel();
		} else {
			Trace.trace("OnDismissUnit failed",Trace.CHANNEL.INTEGRATION);
		}
	}
}
