using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAnimation : MonoBehaviour {

	Animator bossAnimator;
	Image bossImage;
	int i =0;
	private string[] bossAnimationboolList;

	// Use this for initialization
	void Start () {
		bossAnimationboolList = new string[3];

		bossAnimationboolList [0] = "isBackGroundChanged";
		bossAnimationboolList [1] = "isAppear";
		bossAnimationboolList [2] = "isDisappear";
		bossAnimator = GetComponent<Animator> ();
		bossImage = GetComponent<Image> ();
	}


	void Update()
	{
		if (bossAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f)
		{
			bossAnimator.SetBool (bossAnimationboolList[i], true);
			i++;
		}
	}

}
