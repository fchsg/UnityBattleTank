using UnityEngine;
using System.Collections;

public class LocationMapping : MonoBehaviour {

	private Transform _source;
	private float _sourceStartX;
	private float _sourceStartY;
	
	private int _sourceWidth;
	private int _sourceHeight;

	private int _targetWidth;
	private int _targetHeight;

	private float _scaleX;
	private float _scaleY;

	public enum ALIGN
	{
		NONE,

		TOP_LEFT,
		TOP_CENTER,
		TOP_RIGHT,

		BOTTOM_LEFT,
		BOTTOM_CENTER,
		BOTTOM_RIGHT,

	}
	private ALIGN _align = ALIGN.NONE;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		Follow ();
	}

	public void Init(Transform s, int sw, int sh, int tw, int th, ALIGN align)
	{
		_source = s;
		_sourceStartX = _source.position.x;
		_sourceStartY = _source.position.y;

		_sourceWidth = sw;
		_sourceHeight = sh;
		_targetWidth = tw;
		_targetHeight = th;

		_scaleX = tw / sw;
		_scaleY = th / sh;

		_align = align;

	}

	void Follow()
	{
		transform.localRotation = _source.localRotation;
		transform.localScale = _source.localScale;

		float sourceX = _source.position.x;
		float sourceY = _source.position.y;

		if (_align == ALIGN.NONE) {
			transform.position = new Vector3 (sourceX * _scaleX, sourceY * _scaleY, _source.position.z);
		} else {
			float sourceBaseX = 0;
			float sourceBaseY = 0;
			float targetBaseX = 0;
			float targetBaseY = 0;
			
			switch(_align)
			{
			case ALIGN.TOP_LEFT:
				sourceBaseX = 0;
				sourceBaseY = 0;
				targetBaseX = 0;
				targetBaseY = 0;
				break;
				
			case ALIGN.TOP_CENTER:
				sourceBaseX = _sourceWidth / 2;
				sourceBaseY = 0;
				targetBaseX = _targetWidth / 2;
				targetBaseY = 0;
				break;
				
			case ALIGN.TOP_RIGHT:
				sourceBaseX = _sourceWidth;
				sourceBaseY = 0;
				targetBaseX = _targetWidth;
				targetBaseY = 0;
				break;
				
				
			case ALIGN.BOTTOM_LEFT:
				sourceBaseX = 0;
				sourceBaseY = _sourceHeight;
				targetBaseX = 0;
				targetBaseY = _targetHeight;
				break;
				
			case ALIGN.BOTTOM_CENTER:
				sourceBaseX = _sourceWidth / 2;
				sourceBaseY = _sourceHeight;
				targetBaseX = _targetWidth / 2;
				targetBaseY = _targetHeight;
				break;
				
			case ALIGN.BOTTOM_RIGHT:
				sourceBaseX = _sourceWidth;
				sourceBaseY = _sourceHeight;
				targetBaseX = _targetWidth;
				targetBaseY = _targetHeight;
				break;

			default:
				Assert.assert(false);
				break;

			}
			
			float targetX = (sourceX - _sourceStartX) * _scaleX + targetBaseX + (_sourceStartX - sourceBaseX);
			float targetY = (sourceY - _sourceStartY) * _scaleY + targetBaseY + (_sourceStartY - sourceBaseY);
			float targetZ = _source.position.z;
			transform.position = new Vector3(targetX, targetY, targetZ);
		}

	}

}
