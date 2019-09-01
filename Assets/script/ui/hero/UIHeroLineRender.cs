using UnityEngine;
using System.Collections;

public class UIHeroLineRender : MonoBehaviour {
	
	protected Vector3[] _points;

	protected LineRenderer _lr;
	public Material _mt;

	public void SetPoints(Vector3[] points)
	{
		_points = points;

		if(_lr == null)
		{
			_lr = (LineRenderer)this.gameObject.AddComponent(typeof(LineRenderer));
			_lr.GetComponent<Renderer>().material = _mt;
			_lr.SetWidth(.1f,.1f);
			_lr.SetVertexCount(_points.Length);

			_lr.GetComponent<Renderer> ().sortingOrder = 1;
		}
	}


	void Update () {
		int i = 0 ;
		while(i < _points.Length){
			_lr.SetPosition(i,_points[i]);
			i++;
		}
	}
}
