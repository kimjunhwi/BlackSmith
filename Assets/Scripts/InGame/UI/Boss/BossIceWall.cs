using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossIceWall : MonoBehaviour , IPointerDownHandler 
{
	private GameObject getInfoGameObject;
	public int nCountBreakWall;
	public BossIce bossIce;
	public Animator animator_IceWallRepair;
	public Animator animator_IceWallArbait;

	public int nCurrentArbaitIndex = -1;

	void Start()
	{
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
	public void StartDeFreezeRepair()
	{
		StartCoroutine (DeFreezeRepair ());
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
				animator_IceWallRepair.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 1) {
				animator_IceWallRepair.SetBool ("isBreak02", true);
			}


			if (nCountBreakWall == 0)
			{
				StartCoroutine (DeFreezeRepair ());

			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait1") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 7) {
				animator_IceWallArbait.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator_IceWallArbait.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0) {
				bossIce.isIceWall_ArbaitOn [0] = false;
				DeFreezeArbait ();
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait2") 
		{
			nCountBreakWall--;
			if (nCountBreakWall == 7) {
				animator_IceWallArbait.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator_IceWallArbait.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0) {
				bossIce.isIceWall_ArbaitOn [1] = false;
				DeFreezeArbait ();
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait3") 
		{
			nCountBreakWall--;
			if (nCountBreakWall == 7) {
				animator_IceWallArbait.SetBool ("isBreak01", true);
			}

			if (nCountBreakWall == 4) {
				animator_IceWallArbait.SetBool ("isBreak02", true);
			}

			if (nCountBreakWall == 0)
			{
				bossIce.isIceWall_ArbaitOn [2] = false;
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
		animator_IceWallRepair.SetBool ("isFreeze", true); //Start Freeze Animation
		while (true) 
		{
			Debug.Log ("While FreezeRepair");
			if (animator_IceWallRepair.GetCurrentAnimatorStateInfo (0).IsName("Ice_Repair_Freeze")) 
			{
				Debug.Log ("Finish FreezeRepair");
				//yield return new WaitForSeconds (0.1f);
				animator_IceWallRepair.SetBool ("isIced", true);
				yield break;
			} 
			yield return null;
		}
	
	}

	public IEnumerator DeFreezeRepair()
	{
		animator_IceWallRepair.SetBool ("isDefreeze", true); //Start Freeze Animation
		while (true) 
		{
			if (animator_IceWallRepair.GetCurrentAnimatorStateInfo (0).IsName("Ice_Repair_Defreeze")) 
			{
				yield return new WaitForSeconds (0.3f);
				animator_IceWallRepair.SetBool ("isFreeze", false);
				animator_IceWallRepair.SetBool ("isIced", false);
				animator_IceWallRepair.SetBool ("isBreak01", false);
				animator_IceWallRepair.SetBool ("isBreak02", false);
				animator_IceWallRepair.SetBool ("isDefreeze", false);

				animator_IceWallRepair.Play ("Arbait_Ice_Idle");


				bossIce.ActiveIceWall ();
			} 
			yield return null;
		}
	}
		
	public IEnumerator FreezeArbait()
	{
		animator_IceWallArbait = gameObject.GetComponent<Animator> ();
		animator_IceWallArbait.SetBool ("isFreeze", true); //Start Freeze Animation
		while (true) 
		{
			if (animator_IceWallArbait.GetCurrentAnimatorStateInfo (0).IsName("Arbait_Freeze")) 
			{
				//yield return new WaitForSeconds (0.1f);
				animator_IceWallArbait.SetBool ("isIced", true);
			} 
			yield return null;
		}
		yield break;
	}


	public void DeFreezeArbait()
	{
		//FreezeAnimation Init
		animator_IceWallArbait.SetBool ("isFreeze", false);
		animator_IceWallArbait.SetBool ("isIced", false);
		animator_IceWallArbait.SetBool ("isBreak01", false);
		animator_IceWallArbait.SetBool ("isBreak02", false);
		animator_IceWallArbait.Play ("Arbait_Ice_Idle");

		BossArbaitDeFreeze bossDefreeze = null;

		bossIce.iceWall_Arbait_Defreeze [nCurrentArbaitIndex].SetActive (true);
		bossIce.isIceWall_ArbaitOn [nCurrentArbaitIndex] = false;
		bossDefreeze = bossIce.iceWall_Arbait_Defreeze [nCurrentArbaitIndex].GetComponent<BossArbaitDeFreeze> ();
		bossDefreeze.nIndex = nCurrentArbaitIndex;
		bossDefreeze.StartDeFreeze ();
		nCurrentArbaitIndex = -1;
		gameObject.SetActive (false);
	}

	public void DeFreezeArbaitAll()
	{
		Debug.Log ("Arbait Ice Wall DeActive");
		//FreezeAnimation Init
		animator_IceWallArbait.SetBool ("isFreeze", false);
		animator_IceWallArbait.SetBool ("isIced", false);
		animator_IceWallArbait.SetBool ("isBreak01", false);
		animator_IceWallArbait.SetBool ("isBreak02", false);
		animator_IceWallArbait.Play ("Arbait_Ice_Idle");

		nCurrentArbaitIndex = -1;
		nCountBreakWall = 0;
		gameObject.SetActive (false);

	}
}
