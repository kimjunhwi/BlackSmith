using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParticle : MonoBehaviour {

	public void Play()
	{
		StartCoroutine(StartDisappearAfter(2f));
	}

	IEnumerator StartDisappearAfter(float time)
	{
		yield return new WaitForSeconds(time);

		gameObject.SetActive (false);
	}
}
