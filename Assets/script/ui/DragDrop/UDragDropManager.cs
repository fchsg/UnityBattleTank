using UnityEngine;
using System.Collections;

public class UDragDropManager : MonoBehaviour 
{
	// 拖放代理
	public delegate void Callback(UDragDropItem gameObject);
	
	/// <summary>
	/// 是否正在拖拽
	/// </summary>
	public static bool isDraging = false;

	/// <summary>
	/// 拖放的对象
	/// </summary>
	private static GameObject dragTarget;

	/// <summary>
	/// 代理图片
	/// </summary>
	private static UISprite proxyTarget;

	/// <summary>
	/// 数据对象
	/// </summary>
	private static UDragDropData dragDropData;

	/// <summary>
	/// 偏移位置
	/// </summary>
	private static Vector3 offset;

	/// <summary>
	/// 代理图片范围
	/// </summary>
	private static Bounds bounds;
	//回调函数
	private static Callback callback;

	/// <summary>
	/// 判断移动距离是否开始拖拽
	/// </summary>
	private static Vector3 dragOffset;

	/// <summary>
	/// 拖拽移动距离
	/// </summary>
	private static float dragDistance;

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="proxyTarget">Proxy target.</param>
	public static void Init(UISprite proxyTarget)
	{
		UDragDropManager.proxyTarget = proxyTarget;
	}

	/// <summary>
	/// 开始拖拽
	/// </summary>
	/// <param name="dragTarget">Drag target.</param>
	/// <param name="proxyAtlas">Proxy atlas.</param>
	/// <param name="proxySpriteName">Proxy sprite name.</param>
	/// <param name="dragDropData">Drag drop data.</param>
	/// <param name="callback">Callback.</param>
	public static void starDrag(GameObject dragTarget, string proxyIconName, float proxyWidth, float proxyHeight, UDragDropData dragDropData, Callback callback)
	{
		if (dragTarget == null || UDragDropManager.proxyTarget == null) return;

		UDragDropManager.dragTarget = dragTarget;
		UDragDropManager.dragDropData = dragDropData;
		UDragDropManager.callback = callback;

		// 设置宽高
		UDragDropManager.proxyTarget.width = (int)proxyWidth;
		UDragDropManager.proxyTarget.height = (int)proxyHeight;
		UDragDropManager.proxyTarget.transform.localScale = new Vector3(1f, 1f, 1f);
		UDragDropManager.proxyTarget.spriteName = proxyIconName;
		
		NGUITools.SetActive (UDragDropManager.proxyTarget.gameObject, false);
		
		UIEventListener.Get(dragTarget).onPress += OnPressOperater;
		UIEventListener.Get(dragTarget).onDrag += OnDragOperater;
	}

	/// <summary>
	/// 按住鼠标事件
	/// </summary>
	/// <param name="o">O.</param>
	/// <param name="pressed">If set to <c>true</c> pressed.</param>
	private static void OnPressOperater(GameObject o, bool pressed)
	{
		if (pressed) 
		{
			dragOffset = Input.mousePosition;
		} 
		else 
		{
			// 要在拖拽的情况下
			if(UDragDropManager.isDraging)
			{
				// 限制角色视角控制
				UDragDropManager.isDraging = false;

				if(UICamera.lastHit.collider != null)
				{
					GameObject gameObject = UICamera.lastHit.collider.gameObject;

					UDragDropItem uDragDropItem = gameObject.GetComponent<UDragDropItem>();
					if(uDragDropItem != null)
					{
						callback.Invoke(uDragDropItem);
					}
					else
					{
						callback.Invoke(null);
					}
				}else{
					callback.Invoke(null);
				}
			}

			NGUITools.SetActive (proxyTarget.gameObject, false);
			UIEventListener.Get(dragTarget).onPress -= OnPressOperater;
			UIEventListener.Get(dragTarget).onDrag -= OnDragOperater;
		}
	}

	/// <summary>
	/// 拖拽事件
	/// </summary>
	/// <param name="o">O.</param>
	/// <param name="delta">Delta.</param>
	private static void OnDragOperater(GameObject o, Vector2 delta)
	{
		dragDistance = Vector3.Distance (Input.mousePosition, dragOffset);
		// 移动一定距离才开始拖拽
		if (dragDistance >= 1f && !UDragDropManager.isDraging)
		{
			UDragDropManager.isDraging = true;

			NGUITools.SetActive (proxyTarget.gameObject, true);
			proxyTarget.gameObject.transform.position = UICamera.currentCamera.ScreenToWorldPoint (Input.mousePosition);
			
			bounds = NGUIMath.CalculateRelativeWidgetBounds(proxyTarget.gameObject.transform);
			Vector3 position = UICamera.currentCamera.WorldToScreenPoint(proxyTarget.gameObject.transform.position);
			offset = new Vector3(Input.mousePosition.x - (position.x - bounds.size.x / 2), Input.mousePosition.y - (position.y - bounds.size.y / 2),0f);
		}
		if (UDragDropManager.isDraging)
		{
			Vector3 currentPoint = new Vector3 (Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y, 0f);
		
			//如果坐标小于0
			if (currentPoint.x < 0)
			{
				currentPoint.x = 0;
			}
			//如果坐标大于屏幕宽
			if (currentPoint.x + bounds.size.x > Screen.width)
			{
				currentPoint.x = Screen.width - bounds.size.x;
			}
			//如果坐标小于0
			if (currentPoint.y < 0)
			{
				currentPoint.y = 0;
			}
			//如果坐标大于屏幕高
			if (currentPoint.y + bounds.size.y > Screen.height)
			{
				currentPoint.y = Screen.height - bounds.size.y;
			}
		
			currentPoint.x += bounds.size.x / 2;
			currentPoint.y += bounds.size.y / 2;
		
			proxyTarget.gameObject.transform.position = UICamera.currentCamera.ScreenToWorldPoint (currentPoint);
		}
	}
}
