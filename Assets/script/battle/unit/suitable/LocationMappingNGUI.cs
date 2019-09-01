using UnityEngine;
using System.Collections;

public class LocationMappingNGUI : MonoBehaviour {
	
	private Transform _leadTransform;
	private float _leadStartY;

	public void Init(Transform s)
	{
		_leadTransform = s;
		_leadStartY = s.position.y;
	}
	
	void LateUpdate () 
	{
		Follow ();
	}

	void Follow()
	{
		transform.localRotation = _leadTransform.localRotation;
		transform.localScale = _leadTransform.localScale;
		
		float leadX = _leadTransform.localPosition.x;
		float leadY = _leadTransform.localPosition.y;
		float leadZ = _leadTransform.localPosition.z;

		float scaleY = (Screen.height / AppConfig.DESIGN_HEIGHT) / (Screen.width / AppConfig.DESIGN_WIDTH);
		float targetY = (leadY - _leadStartY) * scaleY + _leadStartY;
		transform.localPosition = new Vector3(leadX, targetY, leadZ);
	}
}