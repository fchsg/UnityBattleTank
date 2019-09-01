using UnityEngine;
using System.Collections;

public class PopupNumber {

	public enum COLOR
	{
		RED,
		GREEN,
	}

	private const float CHAR_WIDTH = 1;

	public PopupNumber(string s, COLOR c, Vector3 position, float scale = 1)
	{
		int n = s.Length;

		float width = n * CHAR_WIDTH * scale;
		float x = -width / 2;

		for(int i = 0; i < n; ++i)
		{
			string ch = s.Substring(i, 1);
			if(ch == "+")
			{
				ch = "add";
			}
			else if(ch == "-")
			{
				ch = "sub";
			}

			string name = "";
			if(c == COLOR.GREEN)
			{
				name = "green_" + ch;
			}
			else if(c == COLOR.RED)
			{
				name = "red_" + ch;
			}
			else
			{
				Assert.assert(false);
			}

			try
			{
				GameObject o = ResourceHelper.Load(AppConfig.FOLDER_PROFAB_EFFECT + "Number");
				o.transform.position = position + new Vector3(x, 0, 0);
				o.transform.localScale = new Vector3(scale, scale, scale);

				SkeletonAnimation spineAnim = o.GetComponentInChildren<SkeletonAnimation>();
				spineAnim.Skeleton.SetSkin(name);
				spineAnim.Skeleton.SetSlotsToSetupPose();
			}
			catch(System.Exception)
			{
				Trace.trace("error in PopupNumber, char = " + ch);
			}

			x += CHAR_WIDTH * scale;
		}
	}

}
