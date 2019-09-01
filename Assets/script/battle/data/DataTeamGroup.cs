using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataTeamGroup {

	private Dictionary<int, DataTeam> dataTeams;
	
	private bool isLoad = false;
	
	public DataTeam GetTeam(int id)
	{
		if (dataTeams.ContainsKey (id)) {
			return dataTeams[id];
		}

		return null;
	}
	
	public void Load(string name)
	{
		if (isLoad) 
		{
			return;		
		}
		isLoad = true;
		
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		dataTeams = new Dictionary<int, DataTeam> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataTeam data = new DataTeam();
			data.Load(subNode);
			
			dataTeams.Add(data.id, data);
		}

		
	}
	
}
