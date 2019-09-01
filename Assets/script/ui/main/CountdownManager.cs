using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

/// <summary>
/// 倒计时管理器
/// </summary>
public class CountdownManager : MonoBehaviour {
	private UIScrollView _scrollview;
	private UIGrid _grid;

	private List<CountdownData.Countdown> _countdownList;
	private int _countdownCount = 0;
	void Awake()
	{
		_scrollview = transform.Find("ClockScrollView").GetComponent<UIScrollView>();
		_grid = transform.Find("ClockScrollView/ClockGrid").GetComponent<UIGrid>();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI();
	}
	void UpdateUI () {
		CountdownData  _countdown = new CountdownData();
		_countdownList = _countdown.GetCountdownData();
		int count = _countdownList.Count;
		if(count != _countdownCount)
		{
			_grid.DestoryAllChildren();

			foreach(CountdownData.Countdown countdown in _countdownList)
			{
				Create(countdown);
			}
			_grid.Reposition();
			_countdownCount = count;
		}
		
	}

	void Create(CountdownData.Countdown data)
	{
		if(_grid.gameObject != null){
			GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "main/ClockItem");
			GameObject item = NGUITools.AddChild(_grid.gameObject,tankItem);
			CountdownItem countdownItem = item.GetComponent<CountdownItem>();
			countdownItem.Init(data);
			_grid.repositionNow = true;
			_grid.Reposition();
		}
	}
}
