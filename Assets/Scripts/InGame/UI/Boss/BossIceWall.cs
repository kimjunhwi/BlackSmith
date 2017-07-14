using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossIceWall : MonoBehaviour , IPointerDownHandler 
{
	private GameObject getInfoGameObject;
	public int nCountBreakWall;
	public BossIce bossIce;
	private Animator animator;
	public int nCurrentArbaitIndex;

	void Start()
	{
		animator = GetComponent<Animator> ();
		gameObject.SetActive (false);
	}

	public void TapWall(int _hitDamage)
	{
		if (bossIce.isIceWallOn == true)
		{
			nCountBreakWall -= _hitDamage;
			if (nCountBreakWall == 0) 
			{
				bossIce.ActiveIceWall ();
			}
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;


		if (getInfoGameObject.gameObject.name == "bossIceWall") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 10) {
				animator.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 5) {
				animator.SetBool ("isBreak02", true);
			}


			if (nCountBreakWall == 0) {
				StartCoroutine (DeFreezeRepair ());

			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait1") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 7) {
				animator.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0) {
				
				SpawnManager.Instance.DeFreezeArbait (0);
				bossIce.isIceWall_ArbaitOn [0] = false;
				DeFreezeArbait ();
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait2") 
		{
			nCountBreakWall--;
			if (nCountBreakWall == 7) {
				animator.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0) {

				SpawnManager.Instance.DeFreezeArbait (0);
				bossIce.isIceWall_ArbaitOn [0] = false;

				DeFreezeArbait ();
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait3") 
		{
			nCountBreakWall--;
			if (nCountBreakWall == 7) {
				animator.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0) {

				SpawnManager.Instance.DeFreezeArbait (0);
				bossIce.isIceWall_ArbaitOn [0] = false;

				DeFreezeArbait ();
			}
		}
	}
	public void StartFreezeArbait()
	{
		StartCoroutine(FreezeArbait());
	}

	public void StartFreezeRepair()
	{
		StartCoroutine (FreezeRepair ());
	}

	public IEnumerator FreezeRepair()
	{
		animator.SetBool ("isFreeze", true); //Start Freeze Animation
		while (true) 
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName("Ice_Repair_Freeze")) 
			{
				//yield return new WaitForSeconds (0.1f);
				animator.SetBool ("isIced", true);
			} 
			yield return null;
		}
		yield break;
	}

	public IEnumerator FreezeArbait()
	{
		animator.SetBool ("isFreeze", true); //Start Freeze Animation
		while (true) 
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName("Arbait_Freeze")) 
			{
				//yield return new WaitForSeconds (0.1f);
				animator.SetBool ("isIced", true);
			} 
			yield return null;
		}
		yield break;
	}


	public void DeFreezeArbait()
	{
		//FreezeAnimation Init
		animator.SetBool ("isFreeze", false);
		animator.SetBool ("isIced", false);
		animator.SetBool ("isBreak01", false);
		animator.SetBool ("isBreak02", false);
		animator.Play ("Arbait_Ice_Idle");

		BossIceWallDeFreeze bossDefreeze = null;

		bossIce.iceWall_Arbait_Defreeze [nCurrentArbaitIndex].SetActive (true);
		bossDefreeze = bossIce.iceWall_Arbait_Defreeze [nCurrentArbaitIndex].GetComponent<BossIceWallDeFreeze> ();
		bossDefreeze.StartDeFreeze ();
		gameObject.SetActive (false);
	}

	public IEnumerator DeFreezeRepair()
	{
		animator.SetBool ("isDefreeze", true); //Start Freeze Animation
		while (true) 
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName("Ice_Repair_Defreeze")) 
			{
				yield return new WaitForSeconds (0.3f);
				animator.SetBool ("isFreeze", false);
				animator.SetBool ("isIced", false);
				animator.SetBool ("isBreak01", false);
				animator.SetBool ("isBreak02", false);
				animator.SetBool ("isDefreeze", false);

				animator.Play ("Arbait_Ice_Idle");

				bossIce.ActiveIceWall ();
			} 
			yield return null;
		}
	}

}
