using UnityEngine;
using System.Collections;

public class UDragDropData
{
	/// <summary>
	/// 类型
	/// </summary>
	public string type;

	/// <summary>
	/// 拖拽数据
	/// </summary>
	public object dragData;

	public UDragDropData(string type, object dragData)
	{
		this.type = type;
		this.dragData = dragData;
	}
}
