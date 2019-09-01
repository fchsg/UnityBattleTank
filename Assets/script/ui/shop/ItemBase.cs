using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour ,InItem{

	public virtual Transform GetTransform(){
		return this.transform;
	}

	public virtual GameObject GetGameObject(){
		return this.gameObject;
	}
	public virtual void SetOffset(float xoffset,float yoffset){
		this.transform.localPosition += new Vector3 (xoffset,yoffset,0f);
	}
	
	private int dataIndex;

	public virtual	void SetIndex(int dataIndex){
		this.dataIndex = dataIndex;
	}

	public virtual int  GetIndex(){
		return this.dataIndex;
	}

	
	public virtual void SetData(object data){

	}
}
