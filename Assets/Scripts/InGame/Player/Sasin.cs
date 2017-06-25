using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sasin : Character {

	void Awake()
	{
		base.Awake ();

		boxCollider = gameObject.GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void Start () {
		m_anim.SetBool ("bIsWalk", true);
	}
}
