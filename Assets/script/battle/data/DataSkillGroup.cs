using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataSkillGroup {

	private Dictionary<int, DataSkill> _hashMap;

	public static int GetHash(DataSkill.TYPE type, int level)
	{
		int typeMask = (int)type;
		Assert.assert(typeMask >= 0 && typeMask < 256);
		Assert.assert(level >= 0 && level < 4096);
		int hash = (typeMask << 8 | level);
		return hash;
	}

	public DataSkill GetSkill(DataSkill.TYPE type, int level)
	{
		int hash = GetHash (type, level);
		return _hashMap [hash];
	}

	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		_hashMap = new Dictionary<int, DataSkill> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataSkill skill = new DataSkill();
			skill.Load(subNode);

			int hash = GetHash(skill.type, skill.level);
			_hashMap.Add(hash, skill);
		}	
	}

}
