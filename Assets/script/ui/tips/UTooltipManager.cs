using UnityEngine;
using System.Collections;

// 设置鼠标长按提示数据

public class UTooltipManager : MonoBehaviour
{
	private static GameObject _tooltipTarget;
	private static GameObject _tooltip;

	public static void setTooltip(GameObject tooltipTarget, GameObject tooltip, object tooltipData)
	{
		if (UTooltipManager._tooltip != null) 
		{
			UTooltipManager._tooltip.SetActive (false);
		}

		if (tooltipTarget == null || tooltip == null) 
		{
			return;
		}

		UTooltip uTooltip = tooltip.GetComponent<UTooltip> ();
		if (uTooltip != null) 
		{
			uTooltip.setTipData (tooltipData);
		}
		
		UTooltipManager._tooltipTarget = tooltipTarget;
		UTooltipManager._tooltip = tooltip;

		UIEventListener.Get(tooltipTarget).onPress += OnPressOperater;
		UTooltipManager._tooltip.SetActive (true);

		OnPressOperater (null, true);
	}
	 
	public static void Hidden()
	{
		if (UTooltipManager._tooltip != null) 
		{
			UTooltipManager._tooltip.SetActive (false);
		}
	}

	private static void OnPressOperater(GameObject o, bool isOver)
	{
		if (isOver) 
		{
			//格子大小
			Bounds targetAbsoluteBounds = NGUIMath.CalculateAbsoluteWidgetBounds(_tooltipTarget.transform);
			Bounds targetRelativeBounds = NGUIMath.CalculateRelativeWidgetBounds(_tooltipTarget.transform);

			//提示大小
			Bounds tooltipAbsoluteBounds = NGUIMath.CalculateAbsoluteWidgetBounds(_tooltip.transform);
			Bounds tooltipRelativeBounds = NGUIMath.CalculateRelativeWidgetBounds(_tooltip.transform);

			Vector3 targetPosition = _tooltipTarget.transform.position;

			//提示起始位置
			targetPosition.x = targetPosition.x + targetAbsoluteBounds.size.x / 2;
			targetPosition.y = targetPosition.y + targetAbsoluteBounds.size.y / 2;

			Vector3 screenPoint = UICamera.currentCamera.WorldToScreenPoint(targetPosition);

			if(screenPoint.x + tooltipRelativeBounds.size.x > Screen.width)
			{
				screenPoint.x = screenPoint.x - tooltipRelativeBounds.size.x - targetRelativeBounds.size.x;
			}
			//因为坐标从左下角开始，所以只要判断高度是否大于坐标就可以了
			if(screenPoint.y - tooltipRelativeBounds.size.y < 0)
			{
				screenPoint.y = Screen.height - tooltipRelativeBounds.size.y;
			}

			_tooltip.transform.position = UICamera.currentCamera.ScreenToWorldPoint(screenPoint);
		} 
		else
		{
			_tooltip.SetActive(false);
			UIEventListener.Get(_tooltipTarget).onPress -= OnPressOperater;
		}
	}
}
