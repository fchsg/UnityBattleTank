using UnityEngine;
using System.Collections;

public class CoinsBezierItem : MonoBehaviour 
{
	private TweenRotation _rotation;
	private GameObject _posTarget;

	//data
	Model_Building _model_Building;

	Vector3[] resultList = new Vector3[]{};
	int length = 0;
	int index = 0;
	float MoveSpeed = 1f;
	private Vector3 velocity = new Vector3(1f,1f,0f);
	bool isMove = false;
	void Awake()
	{
		_rotation = transform.GetComponent<TweenRotation>();
	}
 
	void Start () {


	}

	public void Init(GameObject posTarget,Model_Building model_building)
	{
		_posTarget = posTarget;
		_model_Building = model_building;
		 
		float randX = UnityEngine.Random.Range(-0.8f, 0.8f);  
		float randY = UnityEngine.Random.Range(0.5f, 1f);
		
		Vector3 contr1 = new Vector3(randX,randY,0f);
		_rotation.to = new Vector3(0f,UnityEngine.Random.Range(90f, 360f),0f); 
		_rotation.Play();
//		Vector3 pos = transform.InverseTransformPoint(_posTarget.transform.position); 
//		Trace.trace(" pos x = " + pos.x + " y = " + pos.y + "_posTarget  x = " + _posTarget.transform.position.x + " y  = " + _posTarget.transform.position.y, Trace.CHANNEL.UI);
		MyBezier bezier = new MyBezier (this.transform.position, contr1, new Vector3(1f,-1f,0f), _posTarget.transform.position);
		
		int pointSize = 180 + UnityEngine.Random.Range(0, 50);
		
		resultList = new Vector3[pointSize];
		for(int index = 1; index <= pointSize; index ++)
		{
			resultList[index - 1] = bezier.GetPointAtTime((float)index / (float)pointSize);
		}
		length = resultList.Length;
		// 随机 
		float randDelay = UnityEngine.Random.Range(0.1f,1.3f);
		StartCoroutine(Move(randDelay));
	}
	IEnumerator Move(float delay)
	{
		yield return new WaitForSeconds(delay);
		isMove = true;
	}

	void Update () 
	{
		if(isMove == true && _model_Building != null)
		{
			if(index <= (length - 2))
			{
				MoveSpeed =  MoveSpeed - Time.deltaTime - 0.01f;
				if(MoveSpeed <= 0)
				{
					MoveSpeed = 0.05f;
				}
				transform.position = Vector3.SmoothDamp(transform.position,resultList[index + 1],ref velocity,MoveSpeed,2.0f,MoveSpeed);
				index ++;
			}else
			{
				DeleteGameObject();
			}
		}
	}

	void DeleteGameObject()
	{
		switch (_model_Building.buildingType) 
		{
		case Model_Building.Building_Type.FoodFactory:
			NotificationCenter.instance.DispatchEvent (Notification_Type.RefreshFood, new Notification("100"));
			
			break;
		case Model_Building.Building_Type.OilFactory:
			NotificationCenter.instance.DispatchEvent (Notification_Type.RefreshOil, new Notification("100"));
			break;
		case Model_Building.Building_Type.MetalFactory:
			NotificationCenter.instance.DispatchEvent (Notification_Type.RefreshMetal, new Notification("100"));
			break;
		case Model_Building.Building_Type.RareFactory:
			NotificationCenter.instance.DispatchEvent (Notification_Type.RefreshRare, new Notification("100"));
			break;
		}
		Destroy(this.gameObject);
	}
}