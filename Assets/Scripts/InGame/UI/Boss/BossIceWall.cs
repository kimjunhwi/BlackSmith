using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossIceWall : MonoBehaviour , IPointerDownHandler 
{
	private GameObject getInfoGameObject;
	public int nCountBreakWall;
	public BossIce bossIce;

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

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;


		if (getInfoGameObject.gameObject.name == "bossIceWall") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 0) {
				bossIce.ActiveIceWall ();
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait1") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 0) {
				gameObject.SetActive (false);
				SpawnManager.Instance.DeFreezeArbait (0);
				bossIce.isIceWall_ArbaitOn [0] = false;
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait2") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 0) {
				gameObject.SetActive (false);
				SpawnManager.Instance.DeFreezeArbait (1);
				bossIce.isIceWall_ArbaitOn [1] = false;
			}
		}

		if (getInfoGameObject.gameObject.name == "bossIceWall_Arbait3") 
		{
			nCountBreakWall--;
			Debug.Log(getInfoGameObject.gameObject.name + " = " + nCountBreakWall);
			if (nCountBreakWall == 0) {
				gameObject.SetActive (false);
				SpawnManager.Instance.DeFreezeArbait (2);
				bossIce.isIceWall_ArbaitOn [2] = false;
			}
		}
	}

}
