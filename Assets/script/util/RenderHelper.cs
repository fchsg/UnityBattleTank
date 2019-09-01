using UnityEngine;
using System.Collections;

public class RenderHelper {

	public static void ChangeWholeModelColor(GameObject model, Color color)
	{
		Renderer[] renderers = model.GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers)
		{
			foreach (Material m in renderer.materials)
			{
				m.color = color;
			}
		}
	}

	public static void ChangeWholeModelAlpha(GameObject model, float alpha)
	{
		Renderer[] renderers = model.GetComponentsInChildren<Renderer> ();
		foreach (Renderer renderer in renderers)
		{
			foreach (Material m in renderer.materials)
			{
				Color color = m.color;
				color.a = alpha;
				m.color = color;
			}
		}
	}
	
	public static void ResetLocalTransform(GameObject target)
	{
		target.transform.localPosition = Vector3.zero;
		target.transform.localScale = Vector3.one;
		target.transform.localRotation = Quaternion.identity;
	}

	public static void ChangeTreeLayer(GameObject root, int layer)
	{
		root.layer = layer;
		int n = root.transform.childCount;
		for (int i = 0; i < n; ++i) {
			ChangeTreeLayer (root.transform.GetChild (i).gameObject, layer);
		}
	}

	public static void SetSpineSortingOrder(SkeletonAnimation skeletonAnimation, int sortingOrder)
	{
		MeshRenderer meshRenderer = skeletonAnimation.GetComponent<MeshRenderer> ();
		meshRenderer.sortingOrder = sortingOrder;
	}

	public static void SetUIPanelSortingOrder(MeshRenderer meshRenderer, int sortingOrder)
	{
		meshRenderer.sortingOrder = sortingOrder;
	}

	public static void SetObjectRenderQuene(GameObject obj, int renderQueue, int sortingOrder)
	{
		MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer> ();
		if (meshRenderer != null) 
		{
			meshRenderer.material.renderQueue = renderQueue;
			meshRenderer.sortingOrder = sortingOrder;
		}
	}

	public static void SetUIPanelSortingOrder(UIPanel panel, int sortingOrder)
	{
		panel.sortingOrder = sortingOrder;
	}

	public static void SetUIPanelRenderQueue(UIPanel panel, int srartRenderQueue, int sortingOrder)
	{
		panel.renderQueue = UIPanel.RenderQueue.StartAt;
		panel.startingRenderQueue = srartRenderQueue;
		panel.sortingOrder = sortingOrder;
	}
}
