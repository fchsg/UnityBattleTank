using UnityEngine;
using System.Collections;

public class SkillAnimation : MonoBehaviour {

	private Vector3 _worldPosition;
	private Vector3 _mousePos;

	private Vector3 _startPosition;
	private Vector3 _endPosition;

	private const float SPEED = 120.0f;
	private const float OFFSET = 10.0f;

	private bool _isOver = false;
	public bool isOver
	{
		get { return _isOver; }
	}

	private bool _flyOver = false;
	public bool flyOver
	{
		get { return _flyOver; }
	}

	public void Init (Vector3 worldPosition, Vector3 mousePos) {
	
		_worldPosition = worldPosition;
		_mousePos = mousePos;

		MapCamera mapCamera = BattleGame.instance.mapCamera;
		float expectScreenWidth = mapCamera.cameraControl.sizeControl.expectScreenWidth2 * 2;
		float ratio = _mousePos.x / Screen.width;

		float offsetX1 = -(expectScreenWidth * ratio) - OFFSET;
		float offsetX2 = expectScreenWidth * (1 - ratio) + OFFSET;

		_startPosition = worldPosition + new Vector3 (offsetX1, SkillBombBase.HEIGHT, 0);
		_endPosition = worldPosition + new Vector3 (offsetX2, SkillBombBase.HEIGHT, 0);

		transform.position = _startPosition;

		AudioGroup.Play (GetComponent<AudioGroup> ().move, gameObject);
	}

	void Update () 
	{
		transform.Translate (SPEED * TimeHelper.deltaTime, 0, 0);

		if (transform.position.x > _endPosition.x) 
		{
			_isOver = true;	
			MonoBehaviour.DestroyImmediate(this.gameObject);
			return;
		}
		if (transform.position.x > _worldPosition.x) 
		{
			_flyOver = true;	
		}
	}
}
