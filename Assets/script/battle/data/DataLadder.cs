using UnityEngine;
using System.Collections;
using System;


public class DataLadder : IComparable<DataLadder> {

	public int rank;
	public int honor;
	public int cash;
	public int combat;

	public int itemId;
	public int itemNum;
	public int dropGroup;

	public void Load(LitJson.JSONNode json)
	{
		rank = int.Parse(json["Rank"]);
		honor = int.Parse(json["Honor"]);
		cash = int.Parse(json["Cash"]);
		combat = int.Parse(json["Combat"]);

		itemId = JsonReader.Int(json,"Item");
		itemNum = JsonReader.Int(json,"ItemNum");
		dropGroup = int.Parse(json["DropGroup"]);

	}


	public int CompareTo(DataLadder other)
	{
		if (rank > other.rank) {
			return 1;
		} else if (rank < other.rank) {
			return -1;
		} else {
			return 0;
		}


	}

	/*
	public int Compare(DataLadder a, DataLadder b)
	{
		if (a.rank > b.rank) {
			return 1;
		} else if (a.rank < b.rank) {
			return -1;
		} else {
			return 0;
		}

	}
	*/

}
