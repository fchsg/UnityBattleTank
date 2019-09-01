using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitTrack {

	class Node
	{
		public int tileIndex;
		public long timestamp;
		public float alpha = 1;
	}

	private List<Node> _nodes = new List<Node> ();

	private Unit _unit;

	private bool _firstCheck = true;
	private Vector3 _lastCheckPos;

	public const float TRACK_DURATION_MS = 3f * 1000;
	public const int TRACK_RESERVE_COUNT = 2;


	public UnitTrack(Unit unit)
	{
		_unit = unit;
	}

	public void Update () {

		UpdateNodes ();
		TryAddNode ();
	}

	private void UpdateNodes()
	{
		long ct = TimeHelper.GetCurrentTimestampScaled ();

		int n = _nodes.Count;
		for (int i = n - 1; i >= 0; --i) {
			Node node = _nodes[i];

			bool fadeout = _unit.isDead || (i < n - TRACK_RESERVE_COUNT);

			if(!fadeout)
			{
				node.timestamp = ct;
			}
			else
			{
				float dt = ct - node.timestamp;
				float alpha = 1 - dt / TRACK_DURATION_MS;
				if(alpha <= 0)
				{
					UnitTrackControl.ReturnTrackTile(node.tileIndex);
					_nodes.RemoveAt(i);
				}
				else
				{
					if(alpha - node.alpha < -0.05f)
					{
						UnitTrackControl.SetTileAlpha(node.tileIndex, alpha);
						node.alpha = alpha;
					}
				}
			}
		}
	}

	private void TryAddNode()
	{
		if (_firstCheck) {
			_firstCheck = false;

			Vector3 v = UnitHelper.GetOrientation(_unit.transform);
			CreateNode(v);
		} else {
			Vector3 v = _unit.transform.position - _lastCheckPos;
			float l = 0.13f * MapGrid.GRID_SIZE;
			if(v.sqrMagnitude >= l * l)
			{
				CreateNode(v);
			}
		}
	}

	private void CreateNode(Vector3 v)
	{
		_lastCheckPos = _unit.transform.position;

		Vector3 p = new Vector3 (v.z, 0, -v.x);
		VectorHelper.ResizeVector (ref p, _unit.unit.dataUnit.GetRadius () - 0.7f);

		VectorHelper.ResizeVector (ref v, -(_unit.unit.dataUnit.GetRadius () - 1f));
		Vector3 p1 = _unit.transform.position - p + v;
		Vector3 p2 = _unit.transform.position + p + v;

		float rotationY = -Mathf.Atan2(v.z, v.x) / Mathf.PI * 180;

		int tileIndex1 = UnitTrackControl.QueryTrackTile ();
		int tileIndex2 = UnitTrackControl.QueryTrackTile ();

		UnitTrackControl.SetTrackTile (tileIndex1, p1, rotationY);
		UnitTrackControl.SetTrackTile (tileIndex2, p2, rotationY);

		Node node1 = new Node ();
		node1.tileIndex = tileIndex1;
		node1.timestamp = TimeHelper.GetCurrentTimestampScaled ();
		_nodes.Add (node1);

		Node node2 = new Node ();
		node2.tileIndex = tileIndex2;
		node2.timestamp = TimeHelper.GetCurrentTimestampScaled ();
		_nodes.Add (node2);
	}



}
