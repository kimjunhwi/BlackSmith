﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : BossCharacter 
{
	//SmallFire Pool

	private float fXPos;								//불 등장 포인트의 x좌표
	private float fYPos;								//불 등장 포인트의 y좌표
	private float fRandomXPos;							//불 등장 포인트의 랜덤 x좌표
	private float fRandomYPos;							//불 등장 포인트의 랜덤 y좌표

	private SimpleObjectPool smallFirePool;
	public RectTransform smallFireRespawnPoint;

	private int nSmallFireMaxCount;						//작은 불 개수(최대)
	private int nCurFireCount;							//작은 불 개수(현재)

	//tmpValue
	private GameObject smallFire;
	private float fTime = 0f;

	private void Start()
	{
		nSmallFireMaxCount = 5;
		animator = gameObject.GetComponent<Animator> ();
		smallFirePool = GameObject.Find ("SmallFirePool").GetComponent<SimpleObjectPool> ();
		fXPos = smallFireRespawnPoint.position.x;
		fYPos = smallFireRespawnPoint.position.y;

		gameObject.SetActive (false);
	}
	private void OnEnable()
	{
		if (isFirstActive == false) {
			isFirstActive = true;
		} 
		else 
		{
			eCureentBossState = EBOSS_STATE.CREATEBOSS;
			StartCoroutine (BossWait ());
		}

	}
	private void OnDisable()
	{
		StopCoroutine (BossWait ());
		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());
		StopCoroutine (BossDie ());
		StopCoroutine (BossResult ());
	} 

	private void Update()
	{
		if (eCureentBossState == EBOSS_STATE.FINISH) 
		{

			//Effect Off
			if(isStandardPhaseFailed == false)
				bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);

			//말풍선 off
			if (bossTalkPanel.bossTalkPanel.activeSelf == true)
				bossTalkPanel.bossTalkPanel.SetActive (false);


			StopCoroutine (repairObj.BossMusicWeaponMove ());
			StopCoroutine (BossSkillStandard ());
			StopCoroutine (BossSkill_01 ());
			StopCoroutine (BossSKill_02 ());
			StopCoroutine (BossDie ());
			StopCoroutine (BossResult ());
			Debug.Log ("Finish Boss");
			bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
			repairObj.SetFinishBoss ();		//수리 패널 초기화

			eCureentBossState = EBOSS_STATE.CREATEBOSS;
			isFailed = false;
			isStandardPhaseFailed = false;
			nSmallFireMaxCount = 5;

			if (bossBackGround.isBossBackGround == true) {
				SpawnManager.Instance.bIsBossCreate = false;
				bossBackGround.isBossBackGround = false;
				bossBackGround.isOriginBackGround = true;
			}
			bossUIDisable.SetActive (false);

			SpawnManager.Instance.ReliveArbaitBossRepair ();

			gameObject.SetActive (false);

		}		
	}

	protected override IEnumerator BossWait ()
	{

		while (true)
		{

			//무기 이미지 추가
			if (bossBackGround.isBossBackGround == true) {

				animator.SetBool ("isBackGroundChanged", true);

				if (animator.GetCurrentAnimatorStateInfo (0).IsName("FireAppear")) 
				{
					//yield return new WaitForSeconds (0.1f);
					animator.SetBool ("isAppear", true);
					eCureentBossState = EBOSS_STATE.PHASE_00;
				} 
				else
					yield return null;


				if (eCureentBossState == EBOSS_STATE.PHASE_00) {

					repairObj.GetBossWeapon (ObjectCashing.Instance.LoadSpriteFromCache(sBossWeaponSprite), bossInfo.fComplate, 0, 0, this);
					ActiveTimer ();
					break;
				}
			}
			else
				yield return null;


		}
		StartCoroutine (BossSkillStandard ());
		yield break;
	}

	protected override IEnumerator BossSkillStandard ()
	{
		
		bossTalkPanel.StartShowBossTalkWindow (2f, "Fire~~~");
		isStandardPhaseFailed = true;
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (smallFireRespawnPoint.sizeDelta.x/2), fXPos + (smallFireRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (smallFireRespawnPoint.sizeDelta.y/2), fYPos + (smallFireRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			if (fTime >= 2.0f && nCurFireCount < 5 )
				CreateSmallFire ();

			if (fCurComplete < 0) {
				isFailed = true;

				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();

				eCureentBossState = EBOSS_STATE.FINISH;
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 30) {
				eCureentBossState = EBOSS_STATE.PHASE_01;
				animator.SetBool ("isPhase00", true);
			}
			if (eCureentBossState == EBOSS_STATE.PHASE_01)
				break;
			else
				yield return null;

		}
		StartCoroutine (BossSkill_01 ());
		yield break;
	}


	protected override IEnumerator BossSkill_01 ()
	{
		nSmallFireMaxCount = 10;
		bossTalkPanel.StartShowBossTalkWindow (2f, "흐으음~~~");

		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		isStandardPhaseFailed = false;

		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (smallFireRespawnPoint.sizeDelta.x/2), fXPos + (smallFireRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (smallFireRespawnPoint.sizeDelta.y/2), fYPos + (smallFireRespawnPoint.sizeDelta.y/2));


			fTime += Time.deltaTime;

			//BossWeapon info
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			if (fTime >= 2.0f  && nCurFireCount < 10 )
				CreateSmallFire ();

			if (fCurComplete < 0) {
				isFailed = true;
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();

				eCureentBossState = EBOSS_STATE.FINISH;
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 60) {
				eCureentBossState = EBOSS_STATE.PHASE_02;
				animator.SetBool ("isPhase01", true);
			}
			if (eCureentBossState == EBOSS_STATE.PHASE_02)
				break;
			else
				yield return null;
		}
		StartCoroutine (BossSKill_02 ());
		yield break;

	}

	protected override IEnumerator BossSKill_02 ()
	{
		
		nSmallFireMaxCount = 20;

		bossTalkPanel.StartShowBossTalkWindow (2f,"파이어 ~~~!");
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (smallFireRespawnPoint.sizeDelta.x/2), fXPos + (smallFireRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (smallFireRespawnPoint.sizeDelta.y/2), fYPos + (smallFireRespawnPoint.sizeDelta.y/2));


			fTime += Time.deltaTime;

			//GetCompletion
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			if (fTime >= 2.0f  && nCurFireCount < 20 )
				CreateSmallFire ();
			

			if (fCurComplete < 0) 
			{
				isFailed = true;
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();
				eCureentBossState = EBOSS_STATE.FINISH;
			}

			if (fCurComplete >= fMaxComplete)
				eCureentBossState = EBOSS_STATE.DIE;

			if (eCureentBossState == EBOSS_STATE.DIE)
				break;
			else
				yield return null;

		}
		StartCoroutine (BossDie ());
		yield break;
	}



	protected override IEnumerator BossDie ()
	{

		bossTalkPanel.StartShowBossTalkWindow (2f, "Bye~~~!");
		while (true)
		{
			animator.SetBool ("isDisappear", true);

			yield return new WaitForSeconds (0.1f);

			eCureentBossState = EBOSS_STATE.RESULT;
			if (eCureentBossState == EBOSS_STATE.RESULT)
			{
				animator.SetBool ("isAppear", false);
				animator.SetBool ("isPhase00", false);
				animator.SetBool ("isPhase01", false);
				animator.SetBool ("isDisappear", false);
				animator.SetBool ("isBackGroundChanged", false);	
				break;
			}
			else
				yield return null;
		}



		StartCoroutine (BossResult ());

		yield break;
	}

	protected override IEnumerator BossResult ()
	{
		while (true)
		{
			Debug.Log ("BossResult Active!!");
			yield return new WaitForSeconds (1.0f);
			animator.Play ("BossIdle");
			ActiveTimer ();
			//실패가 아닐시
			if (isFailed == false)
			{
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();
				bossPopUpWindow.GetBossInfo (this);							//보상 정보 
			} 
			//실패시
			else 
			{
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();
			}
			eCureentBossState = EBOSS_STATE.FINISH;
			if (eCureentBossState == EBOSS_STATE.FINISH)
				break;
			else
				yield return null;

		}
		//Destroy (gameObject);
		yield break;
	}	


	public void ActiveTimer()
	{


		if (bossTimer_Obj.activeSelf == true)
		{
			bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_FIRE);
		}
		else 
		{
			bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_FIRE);
			bossTimer.bossFire = this;
		}

	}

	public void FailState()
	{
		isFailed = true;

		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());

		StartCoroutine (BossDie ());
	}

	public void CreateSmallFire()
	{
		smallFire = smallFirePool.GetObject ();
		smallFire.transform.SetParent (smallFireRespawnPoint.transform,false);
		smallFire.transform.localScale = Vector3.one;
		smallFire.transform.position = new Vector3 (fRandomXPos, fRandomYPos, smallFire.transform.position.z);
		smallFire.name = "SmallFire";


		BossSmallFireObject smallFireObj = smallFire.GetComponent<BossSmallFireObject> ();
		smallFireObj.smallFireObjPull = smallFirePool;
		smallFireObj.nTouchCount = 3;
		smallFireObj.parentTransform = smallFireRespawnPoint;
		smallFireObj.StartCheckSmallFire ();
		fTime = 0f;
		nCurFireCount++;
	}

}
