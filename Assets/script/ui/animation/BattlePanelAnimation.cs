using UnityEngine;
using System.Collections;

public class BattlePanelAnimation : MonoBehaviour {
	
	void Start () 
	{
		StartCoroutine(PlayTopAnimation ());
	}
	
	IEnumerator PlayTopAnimation()
	{
		Transform lead_root = transform.Find ("top_animation_lead");
		Transform follow_root = transform.Find ("top_animation_follow");

		lead_root.GetComponent<Animator>().enabled = false;
		follow_root.GetComponent<Animator>().enabled = false;

		Transform[] lead_transforms = new Transform[3];
		lead_transforms[0] = lead_root.Find ("top_left_Container");
		lead_transforms[1] = lead_root.Find ("top_Container");
		lead_transforms[2] = lead_root.Find ("top_right_Container");

		Transform[] follow_transforms = new Transform[3];
		follow_transforms[0] = follow_root.Find ("top_left_Container");
		follow_transforms[1] = follow_root.Find ("top_Container");
		follow_transforms[2] = follow_root.Find ("top_right_Container");

		for (int i = 0; i < lead_transforms.Length; ++i) 
		{
			follow_transforms[i].gameObject.SetActive(false);
			
			LocationMappingNGUI location = follow_transforms[i].GetComponent<LocationMappingNGUI> ();
			if (location == null) 
			{
				location = follow_transforms[i].gameObject.AddComponent<LocationMappingNGUI> ();
				location.Init (lead_transforms[i]);
			}
		}
		
		yield return new WaitForSeconds(0.5f);

		for (int i = 0; i < follow_transforms.Length; ++i) 
		{
			follow_transforms[i].gameObject.SetActive(true);
		}
		
		lead_root.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds(1.0f);	

	}

}
