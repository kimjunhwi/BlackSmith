using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArbaitDeFreeze : MonoBehaviour {

	Animator animator;
	// Use this for initialization
	public int nIndex;
	void Start () 
	{
		animator = GetComponent<Animator> ();
		gameObject.SetActive (false);
	}

	public void StartDeFreeze()
	{
		StartCoroutine (ArbaitIceWallDefreeze());
	}



	public IEnumerator ArbaitIceWallDefreeze()
	{
		Debug.Log ("IceWall : " + nIndex + " : Active Defreeze");
		animator.SetBool ("isDefreeze", true);
		while (true) {
			yield return new WaitForSeconds (0.5f);
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Arbait_Ice_Defreeze")) 
			{
				animator.SetBool ("isDefreeze", false);
				animator.Play ("Arbait_Ice_Defreeze_Idle");
				SpawnManager.Instance.DeFreezeArbait (nIndex);
				gameObject.SetActive (false);
				yield break;
			}
			yield return null;
		}
	}

}
