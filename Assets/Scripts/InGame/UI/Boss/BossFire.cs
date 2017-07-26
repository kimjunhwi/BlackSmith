using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : BossCharacter 
{
	//SmallFire Pool

	private float fXPos;								//불 등장 포인트의 x좌표
	private float fYPos;								//불 등장 포인트의 y좌표
	private float fRandomXPos;							//불 등장 포인트의 랜덤 x좌표
	private float fRandomYPos;							//불 등장 포인트의 랜덤 y좌표

	public SimpleObjectPool smallFirePool;				//불시 ObjPool
	public RectTransform smallFireRespawnPoint;			//불씨 생성지점

	private int nSmallFireMaxCount;						//작은 불 개수(최대)
	private int nCurFireCount;							//작은 불 개수(현재)

	public GameObject FireBoom;							//불씨 터질때의 Obj
	private Animator FireBoomAnimator;					//불씨 Animator


	//tmpValue
	private GameObject smallFire;
	private float fTime = 0f;



	private void Start()
	{
		nSmallFireMaxCount = 12;
		animator = gameObject.GetComponent<Animator> ();
		fXPos = smallFireRespawnPoint.position.x;
		fYPos = smallFireRespawnPoint.position.y;
		FireBoomAnimator = FireBoom.GetComponent<Animator> ();
		FireBoom.SetActive (false);

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
		
	protected override IEnumerator BossWait ()
	{
		while (true)
		{
			//무기 이미지 추가
			if (bossBackGround.isBossBackGround == true) 
			{

				animator.SetBool ("isBackGroundChanged", true);

				if (animator.GetCurrentAnimatorStateInfo (0).IsName("FireAppear")) 
				{
					//yield return new WaitForSeconds (0.1f);
					animator.SetBool ("isAppear", true);
					eCureentBossState = EBOSS_STATE.PHASE_00;
				} 
				else
					yield return null;


				if (eCureentBossState == EBOSS_STATE.PHASE_00) 
				{
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
		
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_BEGIN]);
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
				FailState ();
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 30) {
				eCureentBossState = EBOSS_STATE.PHASE_01;
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
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE01]);

		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_FIREANGRY);
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
				FailState ();
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 60) {
				eCureentBossState = EBOSS_STATE.PHASE_02;
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
		bossTalkPanel.StartShowBossTalkWindow (2f,bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE02]);
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

			//불씨 개수 10개 일시 터진다
			if (nCurFireCount >= 10)
			{
				FireBoom.SetActive (true);
				FireBoomAnimator.SetBool ("isBoom", true);
				int nRemoveCount = 0;

				//물 현재량 0
				repairObj.fCurrentWater = 0f;
				//온도 최대로
				repairObj.SetMaxTempuratrue();

				if (smallFireRespawnPoint.childCount >= 10) 
				{
					nRemoveCount = 10;
					nCurFireCount -= 10;
				} 
				else
				{
					nRemoveCount = smallFireRespawnPoint.childCount;
					nCurFireCount -= nRemoveCount;
				}
				while (nRemoveCount != 0) 
				{
					GameObject go = smallFireRespawnPoint.GetChild (0).gameObject;
					smallFirePool.ReturnObject (go);
					nRemoveCount--;

					//불씨 하나당 물 충전량 -3%
					repairObj.fSmallFireMinusWater -= (float)(GameManager.Instance.playerData.fWaterPlus * 0.03);
					//불씨 하나당 온도 증가량 10%
					repairObj.fSmallFirePlusTemperatrue -= 0.1f;
				}
				yield return new WaitForSeconds (0.6f);
				FireBoomAnimator.SetBool ("isBoom", false);
				FireBoomAnimator.Play ("BossIdle");
				FireBoom.SetActive (false);
			}
				
		
			if (fCurComplete < 0) 
			{
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
		yield return null;
		Debug.Log ("Boss Die");
		while (smallFireRespawnPoint.childCount != 0) 
		{
			GameObject go = smallFireRespawnPoint.GetChild (0).gameObject;
			smallFirePool.ReturnObject(go);
		}

		//사라질때의 말풍선
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_END]);

		//Animator Bool change
		animator.SetBool ("isDisappear", true);

		//사라지는 애니메이션이 끝날때 까지 기달인다.
		while (true) 
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("FireDisappear"))
			{
				yield return new WaitForSeconds (0.8f);
				eCureentBossState = EBOSS_STATE.RESULT;
				if (eCureentBossState == EBOSS_STATE.RESULT) {

					break;
				}
			} else
				yield return null;
		}
		StartCoroutine (BossResult ());


	}

	protected override IEnumerator BossResult ()
	{
		yield return null;

		ActiveTimer ();
		//실패가 아닐시
		if (isFailed == false) {
			bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
			bossPopUpWindow.PopUpWindowReward_Switch ();
			bossPopUpWindow.GetBossInfo (this);							//보상 정보 
		} 
		//실패시
		else {
			bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
			bossPopUpWindow.PopUpWindowReward_Switch ();
		}
		eCureentBossState = EBOSS_STATE.FINISH;


		StartCoroutine (BossFinish ());
	}	


	protected override IEnumerator BossFinish ()
	{
		yield return null;
		//Effect Off
		if(isStandardPhaseFailed == false)
			bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_FIREANGRY);
		
		//말풍선 off
		if (bossTalkPanel.bossTalkPanel.activeSelf == true)
			bossTalkPanel.bossTalkPanel.SetActive (false);
		
		animator.SetBool ("isAppear", false);
		animator.SetBool ("isDisappear", false);
		animator.SetBool ("isBackGroundChanged", false);	
		animator.Play ("BossIdle");


		StopCoroutine (repairObj.BossMusicWeaponMove ());
		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());
		StopCoroutine (BossDie ());
		StopCoroutine (BossResult ());

		Debug.Log ("Finish Boss");
		bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
		repairObj.SetFinishBoss ();									//수리 패널 초기화


		isFailed = false;
		isStandardPhaseFailed = false;
		nSmallFireMaxCount = 12;
		nCurFireCount = 0;

		if (bossBackGround.isBossBackGround == true)
		{
			SpawnManager.Instance.bIsBossCreate = false;
			bossBackGround.isBossBackGround = false;
			bossBackGround.isOriginBackGround = true;
		}
		bossUIDisable.SetActive (false);

		SpawnManager.Instance.ReliveArbaitBossRepair ();

	

		eCureentBossState = EBOSS_STATE.CREATEBOSS;

		gameObject.SetActive (false);


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

		//StopCoroutine (BossSkillStandard ());
		//StopCoroutine (BossSkill_01 ());
		//StopCoroutine (BossSKill_02 ());

		StartCoroutine (BossDie ());
	}

	public void CreateSmallFire()
	{
		smallFire = smallFirePool.GetObject ();
		smallFire.transform.SetParent (smallFireRespawnPoint.transform,false);
		smallFire.transform.localScale = Vector3.one;
		smallFire.transform.position = new Vector3 (fRandomXPos, fRandomYPos, smallFire.transform.position.z);
		smallFire.name = "SmallFireTouch";

		//불씨 하나당 물 충전량 -3%
		repairObj.fSmallFireMinusWater += (float)(GameManager.Instance.playerData.fWaterPlus * 0.03);
		//불씨 하나당 온도 증가량 10%
		repairObj.fSmallFirePlusTemperatrue += 0.1f;

		BossSmallFireObject smallFireObj = smallFire.GetComponent<BossSmallFireObject> ();
		smallFireObj.smallFireObjPull = smallFirePool;
		smallFireObj.nTouchCount = 3;
		smallFireObj.parentTransform = smallFireRespawnPoint;
		smallFireObj.StartCheckSmallFire ();
		fTime = 0f;
		nCurFireCount++;
	}

}
