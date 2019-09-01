using UnityEngine;
using System.Collections;

public class UISceneLoading : SceneBase
{
	private UILabel mSliderLabel;
	private UISlider mSlider;
	
	override public void Init()
	{
		base.Init ();

		mSliderLabel = rootTransform.Find("SliderLabel").GetComponent<UILabel>();
		mSlider = rootTransform.Find("Slider").GetComponent<UISlider>();

		RenderHelper.SetUIPanelSortingOrder (GetComponent<UIPanel> (), AppConfig.SORTINGORDER_UI_POPUP); 
	}

	public void UpdateUI(float value)
	{
		mSlider.value = value;
		if (mSlider.value >= 1) 
		{
			mSlider.value = 1;
		} 
		mSliderLabel.text = (mSlider.value * 100).ToString("0.00") + "%";
	}
}
