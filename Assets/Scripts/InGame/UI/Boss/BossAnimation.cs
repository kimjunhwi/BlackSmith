using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAnimation : MonoBehaviour {

	Animator bossAnimator;
	Image bossImage;
	// Use this for initialization
	void Start () {
		bossAnimator = GetComponent<Animator> ();
		bossImage = GetComponent<Image> ();
	}


	void Update()
	{
		if (Input.GetMouseButtonDown (0))
			bossAnimator.Play ("SasinAppear");
		else if (Input.GetMouseButtonDown (1))
			bossAnimator.Play ("SasinAppearIdle");
	}

}
