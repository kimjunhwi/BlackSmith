using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour
{
	public float m_Scroll_speed = 3f;
	Material m_Material;

	void Start()
	{
		m_Material = GetComponent<Renderer> ().material;
	}

	void Update()
	{
		float m_PicOffset = Time.time * m_Scroll_speed;
		m_Material.mainTextureOffset = new Vector2 (m_PicOffset, 0);
	}
}
