using UnityEngine;
using System.Collections;

public class DataGuide {


	public int id;
	public string command;
	public float A;
	public float B;


	public string title;
	public string stepDesc;
	public string stepImage;
	public int task;


	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int(json, "Id");

		command = json["Command"];
		A = JsonReader.Float(json, "A");
		B = JsonReader.Float(json, "B");

		title = json["Title"];
		stepDesc = json["StepDescript"];
		stepImage = json["StepImage"];

		task = JsonReader.Int(json, "Task");

	}

}
