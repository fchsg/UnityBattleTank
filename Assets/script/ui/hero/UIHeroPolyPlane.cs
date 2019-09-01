using UnityEngine;
using System.Collections;

public class UIHeroPolyPlane : MonoBehaviour
{
	public const int VERTEX_COUNT = 5;
	public const float RADIUS = 1;

	private float deltaDegree;
	private Mesh m_Mesh;

	public Vector3[] m_Vertices;

	private int[] m_Indices;
	private Vector3[] m_Normals;
	private Vector2[] m_UVs;

	void Start()
	{
		MeshRenderer meshRender = gameObject.GetComponent<MeshRenderer>();
		RenderHelper.SetUIPanelSortingOrder (meshRender , 1);
	}

	public void CreateMesh(Color color)
	{
		deltaDegree = 2 * Mathf.PI / VERTEX_COUNT;

		float curDegree = 0.0f;
		m_Vertices = new Vector3[VERTEX_COUNT + 1];
		m_Indices = new int[VERTEX_COUNT * 3];
		m_Normals = new Vector3[VERTEX_COUNT + 1];
		m_UVs = new Vector2[VERTEX_COUNT + 1];

		m_Vertices[VERTEX_COUNT - 1].x = 0;
		m_Vertices[VERTEX_COUNT - 1].y = 0;
		m_Vertices[VERTEX_COUNT - 1].z = 0;

		for (int i = 0; i < VERTEX_COUNT; i++)
		{
			m_Vertices[i].x = RADIUS * Mathf.Sin(curDegree);
			m_Vertices[i].y = RADIUS * Mathf.Cos(curDegree);
			m_Vertices[i].z = 0;

			float uv_x = Mathf.Abs (0.5f * Mathf.Sin (curDegree));
			float uv_y = Mathf.Abs(0.5f * Mathf.Cos (curDegree));

			if (i == 0 || i == 1 || i == 2) 
			{
				m_UVs[i].x = uv_x + 0.5f;
			}
			else 
			{
				m_UVs[i].x = 0.5f -  uv_x;
			}
		
			if (i == 0 || i == 1 || i == 4) 
			{
				m_UVs [i].y = uv_y + 0.5f;
			}
			else 
			{
				m_UVs [i].y = 0.5f - uv_y;
			}		

			curDegree += deltaDegree;
		}

	 	m_UVs[VERTEX_COUNT] = new Vector2(0.5f, 0.5f); 

		int a = 0;
		for (int i = 0; i < VERTEX_COUNT * 3; i+= 3)
		{
			m_Indices[i] = a;
			m_Indices[i + 1] = VERTEX_COUNT;
			a = (++a) % VERTEX_COUNT;
			m_Indices[i + 2] = a;
		}
			
		for (int i = 0; i < VERTEX_COUNT + 1; i++)
		{
			m_Normals[i].x = 0;
			m_Normals[i].y = 0;
			m_Normals[i].z = -1;
		}
			
		m_Mesh = new Mesh();
		m_Mesh.vertices = m_Vertices;
		m_Mesh.triangles = m_Indices;
		m_Mesh.normals = m_Normals;
		m_Mesh.uv = m_UVs;

		GetComponent<Renderer> ().material.color = color;
		GetComponent<MeshFilter>().mesh = m_Mesh;
	}

	// index 0 ~ 5 Value 0 ~ 1
	public void SetSectionValue(int index, float value)
	{
		float degree = index * deltaDegree;
		m_Vertices[index].x = RADIUS * value * Mathf.Sin(degree);
		m_Vertices[index].y = RADIUS * value * Mathf.Cos(degree);

		m_Mesh.vertices = m_Vertices;
		GetComponent<MeshFilter>().mesh = m_Mesh;
	}
}
