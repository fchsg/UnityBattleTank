using UnityEngine;
using System.Collections;

public interface InItem  {

	Transform GetTransform();
	GameObject GetGameObject();

	void SetOffset(float xoffset,float yoffset);

	void SetIndex(int dataIndex);

	int GetIndex();

	void SetData(object data);
	 
}