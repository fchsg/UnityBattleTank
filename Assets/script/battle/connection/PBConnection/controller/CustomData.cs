using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomData {

	public List<int> guideData = new List<int>();

	public const int VERSION = 1;
	public const int MASK_GUIDE = 0xcdcd01;

	public void SetGuideData(List<int> data)
	{
		guideData = data;
	}


	public List<int> GetCombineData()
	{
		List<int> stream = new List<int> ();

		stream.Add (VERSION);
		PushGuideData (stream);

		return stream;
	}

	private void PushGuideData(List<int> stream)
	{
		stream.Add (MASK_GUIDE);
		stream.Add (guideData.Count);
		ListHelper.Push<int> (stream, guideData);
	}


	public void SetGuideComplete(int id)
	{
		int index = guideData.IndexOf (id);
		if (index < 0) {
			guideData.Add (id);
		}
	}

	public bool IsGuideComplete(int id)
	{
		int index = guideData.IndexOf (id);
		return index >= 0;
	}

	public void Parse(List<int> stream)
	{
		Reset ();

		if (stream.Count == 0) {
			return;
		}

		int pos = 0;

		int version = stream [pos++];
		if (version != VERSION) {
			return;
		}


		while (pos < stream.Count) {
			int mask = stream [pos++];
			int length = stream [pos++];

			List<int> target = null;
			switch (mask) {
			case MASK_GUIDE:
				target = guideData;
				break;
			}

			ListHelper.Push<int> (target, stream, pos, length);
			pos += length;

		}
	}

	private void Reset()
	{
		guideData = new List<int> ();

	}

}
