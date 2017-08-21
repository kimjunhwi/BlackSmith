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
	public int nCurFireCount;							//작은 불 개수(현재)

	public BossFireBoom bossFireBoom;
	private bool isSmallFireActive;

	private bool isActivePassiveSkill01 = false;
	private bool isActivePassiveSkill02 = false;

	//tmpValue
	private GameObject smallFire;
	private float fTime = 0f;

	public int nCurLevel = 0;



	private void Start()
	{
		isSmallFireActive = false;
		nSmallFireMaxCount = 12;
		bossFireBoom.bossFire = this;
		bossFireBoom.repairObj = repairObj;
		animator = gameObject.GetComponent<Animator> ();
		fXPos = smallFireRespawnPoint.position.x;
		fYPos = smallFireRespawnPoint.position.y;

		gameObject.SetActive (false);
		smallFirePool.PreloadPool ();
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
					if (nCurLevel >= 2)
					{
						repairObj.GetBossWeapon (ObjectCashing.Instance.LoadSpriteFromCache (sBossWeaponSprite), bossInfo.fComplate +
							(bossInfo.fComplate  * 0.05f) * nCurLevel - 1, 0, 0, this);
					} 
					else 
					{
						repairObj.GetBossWeapon (ObjectCashing.Instance.LoadSpriteFromCache(sBossWeaponSprite), bossInfo.fComplate, 0, 0, this);
					}
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
		uiManager.AllDisable ();
		bossPanel.SetActive (true);
		isSmallFireActive = true;
		
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_BEGIN]);
		isStandardPhaseFailed = true;
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (smallFireRespawnPoint.sizeDelta.x/2), fXPos + (smallFireRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (smallFireRespawnPoint.sizeDelta.y/2), fYPos + (smallFireRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			if (fTime >= 2.0f && nCurFireCount < nSmallFireMaxCount && isSmallFireActive == true)
				CreateSmallFire ();
		
		

			if (fCurComplete < 0) {
				FailState ();
				yield break;
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

			if (fTime >= 1.5f  && nCurFireCount < nSmallFireMaxCount && isSmallFireActive == true )
				CreateSmallFire ();

		
			if (fCurComplete < 0) {
				FailState ();
				yield break;
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

			if (fTime >= 1.0f  && nCurFireCount < 20 && isSmallFireActive == true)
				CreateSmallFire ();

			//불씨 개수 10개 일시 터진다
			if (nCurFireCount >= 10)
			{
				bossFireBoom.StartBoolFireSmall ();
			}

			if (isActivePassiveSkill02 == false)
				GameManager.Instance.player.SetBasicWaterPlus (GameManager.Instance.player.GetBasicWaterPlus() * 0.5f);
			
				
		
			if (fCurComplete < 0) 
			{
				FailState ();
				yield break;
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
		isSmallFireActive = false;
		Debug.Log ("Boss Die");
		//화면에 남아있는 불씨들을 없앤다
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
				if (eCureentBossState == EBOSS_STATE.RESULT) 
				{
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



					Debug.Log ("Finish Boss");
					bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
					//Weapon 터지는 효과
					repairObj.ShowBreakWeapon ();
					repairObj.SetFinishBoss ();									//수리 패널 초기화


					break;
				}
			}
			else
				yield return null;
		}
		StartCoroutine (BossResult ());


	}

	protected override IEnumerator BossResult ()
	{
		//ChangeSound
		//SoundManager.instance.ChangeBGM(eSoundArray.BGM_BossBattle, eSoundArray.BGM_Main);
		ActiveTimer ();

		while (true) 
		{
			//확인버튼을 누르면 피니쉬로 넘어간다
			if (eCureentBossState == EBOSS_STATE.FINISH) 
			{
				StartCoroutine (BossFinish ());
				yield break;
			}

			if (isFailed == false && bossPopUpWindow.isRewardPanelOn_Success == false) {
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch_isSuccess ();
				bossPopUpWindow.GetBossInfo (this);
				bossPopUpWindow.PopUpWindow_Reward_YesButton.onClick.AddListener (bossPopUpWindow.PopUpWindowReward_Switch_isSuccess);
			} 
			//실패시
			if (isFailed == true && bossPopUpWindow.isRewardPanelOn_Fail == false) {
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch_isFail ();
				bossPopUpWindow.PopUpWindow_Reward_YesButton.onClick.AddListener (bossPopUpWindow.PopUpWindowReward_Switch_isFail);
			}

			yield return new WaitForSeconds (0.1f);
		}
	}	


	protected override IEnumerator BossFinish ()
	{
		yield return null;

		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());
		StopCoroutine (BossDie ());
		StopCoroutine (BossResult ());


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
            GuestPanel.SetActive(true);
            bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_FIRE);
		}
		else 
		{
            GuestPanel.SetActive(false);
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
		Debug.Log ("SmallFire Count = " + nCurFireCount);
		smallFire = smallFirePool.GetObject ();
		smallFire.transform.SetParent (smallFireRespawnPoint.transform,false);
		smallFire.transform.localScale = Vector3.one;
		smallFire.transform.position = new Vector3 (fRandomXPos, fRandomYPos, smallFire.transform.position.z);
		smallFire.name = "SmallFireTouch";

		//불씨 하나당 물 충전량 -3%
		repairObj.fSmallFireMinusWater += 0.03f;
		//불씨 하나당 온도 증가량 10%
		repairObj.fSmallFirePlusTemperatrue += 0.1f;

		BossSmallFireObject smallFireObj = smallFire.GetComponent<BossSmallFireObject> ();
		smallFireObj.smallFireObjPull = smallFirePool;
		smallFireObj.repairObj = repairObj;
		smallFireObj.nTouchCount = 3;
		smallFireObj.parentTransform = smallFireRespawnPoint;
		smallFireObj.bossfire = this;

		Debug.Log ("CurFireMinusWater : " + repairObj.fSmallFireMinusWater + "/ CurPlusTemperatrue : " +  repairObj.fSmallFirePlusTemperatrue);
		fTime = 0f;
		nCurFireCount++;
	}

}
