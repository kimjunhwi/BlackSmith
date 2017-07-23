﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossSasin : BossCharacter 
{
	//1,2 페이즈시 등장하는 해골 관련 변수들
	private RectTransform bossSkullRespawnPoint;				//해골 등장 포인트
	private float fXPos;										//해골 등장 포인트의 x좌표
	private float fYPos;										//해골 등장 포인트의 y좌표
	private float fRandomXPos;									//해골 등장 포인트의 랜덤 x좌표
	private float fRandomYPos;									//해골 등장 포인트의 랜덤 y좌표

	public SimpleObjectPool skullObjectPool;					//1,2페이즈 시에 등장 하는 해골들
	public int nSkullCount = 0;									//현재 나와있는 해골들의 개수

	//임시 해골 변수 
	private GameObject Skull;
	private float fTime = 0f;

	private void Start()
	{
		skullObjectPool = GameObject.Find ("SkullPool").GetComponent<SimpleObjectPool> ();
		bossSkullRespawnPoint = GameObject.Find ("BossSkullCreateArea2").GetComponent<RectTransform>();
		fXPos = bossSkullRespawnPoint.position.x;
		fYPos = bossSkullRespawnPoint.position.y;

		gameObject.SetActive (false);
		animator = gameObject.GetComponent<Animator> ();
	}
	private void OnEnable()
	{
		if (isFirstActive == false) {
			isFirstActive = true;
		} 
		else {
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

	protected override IEnumerator BossWait ()
	{

		while (true)
		{
			//무기 이미지 추가
			if (this.bossBackGround.isBossBackGround == true) 
			{
				animator.SetBool ("isBackGroundChanged", true);

				if (animator.GetCurrentAnimatorStateInfo (0).length > 1.0f) {
					yield return new WaitForSeconds (0.5f);
					animator.SetBool ("isAppear", true);
					eCureentBossState = EBOSS_STATE.PHASE_00;

				} 
				else
					yield return null;


				if (eCureentBossState == EBOSS_STATE.PHASE_00) {
					repairObj.GetBossWeapon (ObjectCashing.Instance.LoadSpriteFromCache(sBossWeaponSprite), bossInfo.fComplate, 0, 0, this);
					ActiveTimer ();
					uiDisable.isBossSummon = false;
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

		isStandardPhaseFailed = true;
		bossTalkPanel.StartShowBossTalkWindow (2f, "내가 사신이지롱");
		while (true)
		{
			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			if (fCurComplete < 0) {
				FailState ();
			}

		
			//해골 생성 
			if (fTime >= 2.0f && bossSkullRespawnPoint.childCount != 4) 
				CreateSkull ();
			

			if (fCurComplete >=	(fMaxComplete / 100) * 30)
				eCureentBossState = EBOSS_STATE.PHASE_01;
			
			//yield return new WaitForSeconds (2.0f);
			//Debug.Log ("BossPhase00 Active!!");

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

		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		isStandardPhaseFailed = false;
		bossTalkPanel.StartShowBossTalkWindow (2f, "나 화났어 ㅡ,ㅡ");
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (bossSkullRespawnPoint.sizeDelta.x/2), fXPos + (bossSkullRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossSkullRespawnPoint.sizeDelta.y/2), fYPos + (bossSkullRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= 2.0f && bossSkullRespawnPoint.childCount != 4) 
				CreateSkull ();
			
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

			if (fCurComplete < 0) {
				FailState ();
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 60)
				eCureentBossState = EBOSS_STATE.PHASE_02;

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
		bossTalkPanel.StartShowBossTalkWindow (2f, "뿌우우우우우!!!");
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (bossSkullRespawnPoint.sizeDelta.x/2), fXPos + (bossSkullRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossSkullRespawnPoint.sizeDelta.y/2), fYPos + (bossSkullRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= 2.0f && bossSkullRespawnPoint.childCount != 4) 
				CreateSkull ();
			
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			if (fCurComplete < 0) {
				FailState ();
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
		bossTalkPanel.StartShowBossTalkWindow (2f, "꾸앙 ㅇㅁㅇ...");
		while (true)
		{
			animator.SetBool ("isDisappear", true);

			yield return new WaitForSeconds (0.1f);

			eCureentBossState = EBOSS_STATE.RESULT;
			if (eCureentBossState == EBOSS_STATE.RESULT)
			{
				animator.SetBool ("isAppear", false);
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
				bossPopUpWindow.GetBossInfo (this);
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
		StartCoroutine (BossFinish ());
		yield break;
	}

	protected override IEnumerator BossFinish ()
	{
		yield return null;

		//효과 off
		if(isStandardPhaseFailed == false)
			bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		//말풍선 off
		if (bossTalkPanel.bossTalkPanel.activeSelf == true)
			bossTalkPanel.bossTalkPanel.SetActive (false);

		//혹시나 도는 코루틴들 종료
		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());
		StopCoroutine (BossDie ());
		StopCoroutine (BossResult ());

		repairObj.SetFinishBoss ();									//수리 패널 초기화

		//상태 변수 초기화
		isFailed = false;
		isStandardPhaseFailed = false;

		//배경 초기화 
		bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
		if (bossBackGround.isBossBackGround == true) {
			SpawnManager.Instance.bIsBossCreate = false;			//손님들 재등장
		}
		//UIBloack off
		bossUIDisable.SetActive (false);						

		SpawnManager.Instance.ReliveArbaitBossRepair ();

		//남아 있는 해골 제거
		while (bossSkullRespawnPoint.childCount != 0) 
		{
			GameObject go = bossSkullRespawnPoint.GetChild (0).gameObject;
			skullObjectPool.ReturnObject(go);
		}
		eCureentBossState = EBOSS_STATE.CREATEBOSS;
		gameObject.SetActive (false);
		Debug.Log ("Finish Boss");
	}


	public void ActiveTimer()
	{
		if (bossTimer_Obj.activeSelf == true)
		{
			bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_SASIN);
		}
		else 
		{
			bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_SASIN);
			bossTimer.bossSasin = this;
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

	public void CreateSkull()
	{
		Skull = skullObjectPool.GetObject ();
		Skull.transform.SetParent (bossSkullRespawnPoint.transform,false);
		Skull.transform.localScale = Vector3.one;
		Skull.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Skull.transform.position.z);
		Skull.name = "Skull";

		SkullObject skullObj = Skull.GetComponent<SkullObject> ();
		skullObj.skullObjPull = skullObjectPool;
		skullObj.parentTransform = bossSkullRespawnPoint;
		skullObj.fTime = 7f;
		skullObj.repairObj = repairObj;
		fTime = 0f;
	}
}
