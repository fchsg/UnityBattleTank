using UnityEngine;
using System.Collections;

public class UIDragDropItemFormation : UIDragDropItem {

	public FormationDragItemUI cloneFormationDragItemUI = null;
	public int unitId;

	private GameObject _spineTankIcon = null;
	public GameObject spineTankIcon
	{
		set { _spineTankIcon = value; }
		get { return _spineTankIcon; }
	}

	override public void StartDragging ()
	{
		base.StartDragging ();

		if (cloneObject != null) {
			cloneFormationDragItemUI = cloneObject.GetComponent<FormationDragItemUI>();

			if (cloneFormationDragItemUI != null) {
				cloneFormationDragItemUI.CreateSpineTank ();
			}
		}


	}

	public void CreateSpineTank()
	{
		if (_spineTankIcon == null) 
		{
				DataUnit unit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
				string primitive = "tankIconPrimitiveUI";

				_spineTankIcon = TankIconSpineAttach.Create (unit, Vector3.zero, 7f, 90, 90, primitive);
				_spineTankIcon.name = "SpineTankIconUI";

				_spineTankIcon.transform.parent = transform;
				_spineTankIcon.transform.localPosition = Vector3.zero;
				_spineTankIcon.transform.localScale = Vector3.one;// new Vector3 (0.1f, 0.1f, 0.1f);

				SetSortingOrder (1);

				RenderHelper.ChangeTreeLayer (_spineTankIcon, gameObject.layer);

		}
	}

	private void FreeSpineTank()
	{
		if (_spineTankIcon != null) {
			MonoBehaviour.DestroyImmediate (_spineTankIcon);
			_spineTankIcon = null;
		}
	}

	public void SetSortingOrder(int sortingOrder)
	{
		if (_spineTankIcon != null)
		{
			TankIconSpineAttach spineAttach = _spineTankIcon.GetComponent<TankIconSpineAttach> ();
			SkeletonAnimation skeletonAnimation = spineAttach.skeletonAnim;
			RenderHelper.SetSpineSortingOrder (skeletonAnimation, sortingOrder);
		}
	}

	protected void ReplaceTankIcon()
	{
		FreeSpineTank ();
		CreateSpineTank ();
	}

	/*
	protected override void OnDragDropRelease (GameObject surface)
	{
		Debug.Log ("OnDragDropRelease");
		base.OnDragDropRelease (surface);

		if (_spineTank != null) {
			MonoBehaviour.DestroyImmediate (_spineTank);
			_spineTank = null;
		}
	}
	*/


	void OnDestroy() {
		FreeSpineTank ();
	}

}
