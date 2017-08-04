using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBoom : MonoBehaviour
{
	public RepairObject repairObj;
	public BossFire bossFire;
	private Animator FireBoomAnimator;					//불씨 Animator


	void Awake()
	{
		FireBoomAnimator = GetComponent<Animator> ();
		gameObject.SetActive (false);
	}

	public void StartBoolFireSmall()
	{
		Debug.Log ("Boom Active");
		gameObject.SetActive (true);
		StartCoroutine (BoomFireSmall ());
	}

	public IEnumerator BoomFireSmall()
	{	

		FireBoomAnimator.SetBool ("isBoom", true);
	

		int nRemoveCount = 0;

		//물 현재량 0
		repairObj.fCurrentWater = 0f;
		//온도 최대로
		repairObj.SetMaxTempuratrue();


		if (bossFire.smallFireRespawnPoint.childCount >= 10) 
		{
			nRemoveCount = 10;
			bossFire.nCurFireCount -= 10;
		} 
		else
		{
			nRemoveCount = bossFire.smallFireRespawnPoint.childCount;
			bossFire.nCurFireCount -= nRemoveCount;
		}
		while (nRemoveCount != 0) 
		{
			GameObject go = bossFire.smallFireRespawnPoint.GetChild (0).gameObject;
			bossFire.smallFirePool.ReturnObject (go);
			nRemoveCount--;

			//불씨 하나당 물 충전량 -3%
			repairObj.fSmallFireMinusWater -= (float)(GameManager.Instance.playerData.fWaterPlus * 0.03);
			//불씨 하나당 온도 증가량 10%
			repairObj.fSmallFirePlusTemperatrue -= 0.1f;
		}


		yield return new WaitForSeconds (0.6f);

	
		if (nRemoveCount == 0) 
		{
			Debug.Log ("DeActiveBoom");
			FireBoomAnimator.SetBool ("isBoom", false);

			FireBoomAnimator.Play ("BossIdle");

			gameObject.SetActive (false);
		}
			
	}


}


