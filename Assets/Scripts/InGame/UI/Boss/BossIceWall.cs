using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossIceWall : MonoBehaviour , IPointerDownHandler 
{
	private GameObject getInfoGameObject;
	public int nCountBreakWall;
	public BossIce bossIce;


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

		if (getInfoGameObject.gameObject.name == "BossIceWall_Arbait1") 
		{
			//float fCurComplete = repairObj.GetCurCompletion ();
			//float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

			/*
			nCountBreakWall_Arbait--;
			if (nCountBreakWall_Arbait == 0) {
				nCountBreakWall_Arbait = 10;
				gameObject.SetActive (false);
			}
			*/
		}
	}

}
