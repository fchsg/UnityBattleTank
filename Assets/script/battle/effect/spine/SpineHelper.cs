using UnityEngine;
using System.Collections;

public class SpineHelper {

	public static bool IsPointInsideBounding(Vector3 worldPoint, Spine.Slot boundingSlot, Transform transform)
	{
		worldPoint = transform.InverseTransformPoint (worldPoint);
		float mx;
		float my;
		boundingSlot.Bone.worldToLocal (worldPoint.x, worldPoint.y, out mx, out my);

		Spine.BoundingBoxAttachment bounding = boundingSlot.Attachment as Spine.BoundingBoxAttachment;
		float[] vertices = (float[])bounding.Vertices.Clone();
	
		/*
		Vector3 p = new Vector3 ();

		int n = vertices.Length / 2;
		for(int i = 0; i < n; ++i)
		{
			float localX = vertices[i * 2 + 0];
			float localY = vertices[i * 2 + 1];
			float worldX;
			float worldY;
			boundingSlot.Bone.localToWorld(localX, localY, out worldX, out worldY);

			p.Set(worldX, worldY, 0);
			p = transform.TransformPoint(p);
			
			vertices[i * 2 + 0] = p.x;
			vertices[i * 2 + 1] = p.y;
		}
		*/
		
		bool inside = GeometryHelper.IsPointInsidePolygon(mx, my, vertices);
		return inside;
	}

	public static void ShowBounding(Spine.Slot boundingSlot, Transform transform, LineRenderer lineRender)
	{
		Spine.BoundingBoxAttachment bounding = boundingSlot.Attachment as Spine.BoundingBoxAttachment;
		float[] vertices = bounding.Vertices;
		
		int n = vertices.Length / 2;
		lineRender.SetVertexCount (n);
		for(int i = 0; i < n; ++i)
		{
			float localX = vertices[i * 2 + 0];
			float localY = vertices[i * 2 + 1];
			float worldX;
			float worldY;
			boundingSlot.Bone.localToWorld(localX, localY, out worldX, out worldY);
			
//			Vector3 p = new Vector3(worldX, worldY, 0);
//			p = transform.TransformPoint(p);
			
			lineRender.SetPosition(i, new Vector3(worldX, worldY, -1));
		}

		lineRender.transform.position = transform.position;
		lineRender.transform.rotation = transform.rotation;
		lineRender.transform.localScale = transform.localScale;

	}


	public static Vector3 CalcTileArrowOffset(Spine.Slot boundingSlot, Transform transform, float rotation,
	                                          float innerRate = 0.7f)
	{
		float radius = AngleHelper.AngleToRadius (rotation - 90);
		Vector3 v = new Vector3(Mathf.Cos(radius), Mathf.Sin(radius)) * 10000;

		Spine.BoundingBoxAttachment bounding = boundingSlot.Attachment as Spine.BoundingBoxAttachment;
		float[] vertices = (float[])bounding.Vertices.Clone();

		Vector2 p1 = new Vector2();
		Vector2 p2 = new Vector2();

		int n = vertices.Length / 2;
		for(int i = 0; i <= n; ++i)
		{
			int m = i % n;
			float localX = vertices[m * 2 + 0];
			float localY = vertices[m * 2 + 1];
			p2.Set(localX, localY);

			if(i >= 1)
			{
				float intersectDist;
				Vector2 intersectPoint;
				if(GeometryHelper.CalcIntersectPoint(Vector2.zero, v, p1, p2, out intersectPoint, out intersectDist))
				{
					intersectPoint *= innerRate;

					float worldX;
					float worldY;
					boundingSlot.Bone.localToWorld(intersectPoint.x, intersectPoint.y, out worldX, out worldY);

					Vector3 offset = new Vector3(worldX, worldY, 0);
					offset = transform.TransformPoint(offset);
					return offset;
				}

			}

			p1 = p2;
		}

		Vector3 boneCenter = new Vector3 (boundingSlot.Bone.WorldX, boundingSlot.Bone.WorldY, 0);
		boneCenter = transform.TransformPoint(boneCenter);
		return boneCenter;

	}
	
	
	public static Vector3 GetBoneCenter(Transform transform, Spine.Bone bone)
	{
		Vector3 wPoint = transform.TransformPoint(bone.WorldX, bone.WorldY, 0);
		return wPoint;
	}


}
