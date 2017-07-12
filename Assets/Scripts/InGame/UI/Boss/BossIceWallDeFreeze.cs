using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceWallDeFreeze : MonoBehaviour {

	Animator animator;



	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		gameObject.SetActive (false);
	}

	public void StartDeFreeze()
	{
		StartCoroutine (IceWallDefreeze());
	}


	public IEnumerator IceWallDefreeze()
	{
		Debug.Log ("Active Defreeze");
		animator.SetBool ("isDefreeze", true);
		while (true) 
		{
			yield return new WaitForSeconds (0.5f);
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Arbait_Ice_Defreeze")) 
			{
				animator.SetBool ("isDefreeze", false);
				animator.Play ("Arbait_Ice_Defreeze_Idle");
				gameObject.SetActive (false);
				break;

			}
			yield return null;
		}
			
	}
}
