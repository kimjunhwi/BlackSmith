using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIce : BossCharacter 
{
	//IceWall
	public GameObject iceWall;
	public bool isIceWallOn;					  		//IceWall이 켜졌는지 아닌지
	public float fIceWallGenerateTimer = 0f;			//현재 수리패널 얼음 타이머
	private float fBossIceWallGenerateTime = 15.0f;		//수리패널 얼음이 나타나는 타이머
	private int nBossIceWallCount = 15;					//수리패널에 어는 얼음 깨지는 횟수
	//IceWall Arbait
	int iceWallIndex = 0;
	public float fIceWallArbaitTimer =0f;				//현재 아르바이트들 빙결 타이머
	private float fIceWallArbaitGenerateTime = 10.0f;	//빙결시간
	public GameObject[] iceWall_Arbait_Freeze;			//어는 animation
	public GameObject[] iceWall_Arbait_Defreeze;		//풀리는 animation
	public bool[] isIceWall_ArbaitOn;

	public int nCurLevel = 0;

	private void Start()
	{
		animator = gameObject.GetComponent<Animator> ();

		for (int i = 0; i < 3; i++) 
			isIceWall_ArbaitOn [i] = false;
		
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

				if (animator.GetCurrentAnimatorStateInfo (0).IsName("Ice_Appear")) 
				{
					yield return new WaitForSeconds (0.8f);
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
	}

	protected override IEnumerator BossSkillStandard ()
	{
		uiManager.AllDisable ();
		bossPanel.SetActive (true);

		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_BEGIN]);
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_ICEBLLIZARD);
		isStandardPhaseFailed = true;
		while (true)
		{
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			//수리패널 얼음
			if (fIceWallGenerateTimer >= fBossIceWallGenerateTime)
				ActiveIceWall ();
			
			if (isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;


			if (fCurComplete < 0) 
			{
				FailState ();
				yield break;
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 30)
				eCureentBossState = EBOSS_STATE.PHASE_01;

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
		repairObj.bossWeaponAnimator.SetBool ("isPhase00", true);
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE01]);

		isStandardPhaseFailed = false;

		while (true)
		{
			//BossWeapon info
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;


			//수리패널 얼음
			if (fIceWallGenerateTimer >= fBossIceWallGenerateTime)
				ActiveIceWall ();
			
			if (isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;

			

			//모든 알바 빙결 해제(현재온도가 맥스 온도를 넘을 시에)
			//if (repairObj.isCurTemperatureOver () == true) 
			//{
			//	DefreezeAllArbait ();

			//}

			//Arbait Ice Wall Timer
			//얼지 않은 아르바이트들이 있다면 시간이 계속간다
			if (SpawnManager.Instance.FreezeArbaitCheck() == true) {
				fIceWallArbaitTimer += Time.deltaTime;
			}
			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
				FreezeArbait ();
	
			//Fail Condition
			if (fCurComplete < 0) {
				FailState ();
				yield break;
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
		SpawnManager.Instance.Active_IcePassive02 ();

		repairObj.bossWeaponAnimator.SetBool ("isPhase01", true);
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE02]);
		while (true)
		{
			//GetCompletion
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;
	


			//수리패널 얼음
			if (fIceWallGenerateTimer >= fBossIceWallGenerateTime)
				ActiveIceWall ();
			
			if (isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;





			//모든 알바 빙결 해제(현재온도가 맥스 온도를 넘을 시에)
			//if (repairObj.isCurTemperatureOver () == true) {
			//	DefreezeAllArbait ();
			//	fIceWallArbaitTimer = 0;
			//}

			//Arbait Ice Wall Timer
			if (SpawnManager.Instance.FreezeArbaitCheck() == true) {
				//Debug.Log (fIceWallArbaitTimer);
				fIceWallArbaitTimer += Time.deltaTime;
			}

			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
				FreezeArbait ();
			


			if (fCurComplete < 0) {
				FailState ();
				yield break;
			}

			if (fCurComplete >=	(fMaxComplete / 100) * 90)
				repairObj.bossWeaponAnimator.SetBool ("isPhase02", true);


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

		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_END]);
		animator.SetBool ("isDisappear", true);

	

		while (true)
		{
			yield return new WaitForSeconds (1.0f);

			eCureentBossState = EBOSS_STATE.RESULT;
			if (eCureentBossState == EBOSS_STATE.RESULT)
			{
				//Effect Off
				bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_ICEBLLIZARD);

				//RepairPanel IceWall Off
				if (iceWall.activeSelf == true)
					ActiveIceWall ();

				//Arbait IceWall off
				DefreezeAllArbait();

				//말풍선 off
				if (bossTalkPanel.bossTalkPanel.activeSelf == true)
					bossTalkPanel.bossTalkPanel.SetActive (false);

				animator.SetBool ("isAppear", false);
				animator.SetBool ("isDisappear", false);
				animator.SetBool ("isBackGroundChanged", false);	
				animator.Play ("BossIdle");


				repairObj.bossWeaponAnimator.SetBool ("isPhase00", false);
				repairObj.bossWeaponAnimator.SetBool ("isPhase01", false);
				repairObj.bossWeaponAnimator.SetBool ("isPhase02", false);
				repairObj.bossWeaponAnimator.Play ("IceWeapon");

				bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
				//Weapon 터지는 효과
				repairObj.ShowBreakWeapon ();
				repairObj.SetFinishBoss ();		//수리 패널 초기화

				break;
			}
			else
				yield return null;
		}
		StartCoroutine (BossResult ());
	}

	protected override IEnumerator BossResult ()
	{
		Debug.Log ("BossResult Active!!");
		//ChangeSound
		//SoundManager.instance.ChangeBGM(eSoundArray.BGM_BossBattle, eSoundArray.BGM_Main);

		ActiveTimer ();

		while (true) 
		{
			//확인버튼을 누르면 피니쉬로 넘어간다
			if (eCureentBossState == EBOSS_STATE.FINISH) {
				StartCoroutine (BossFinish ());
				yield break;
			}
			//실패가 아닐시
			if (isFailed == false && bossPopUpWindow.isRewardPanelOn_Success == false) 
			{
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch_isSuccess ();
				bossPopUpWindow.GetBossInfo (this);
				bossPopUpWindow.PopUpWindow_Reward_YesButton.onClick.AddListener (bossPopUpWindow.PopUpWindowReward_Switch_isSuccess);
			} 
			//실패시
			if(isFailed == true && bossPopUpWindow.isRewardPanelOn_Fail == false)
			{
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch_isFail ();
				bossPopUpWindow.PopUpWindow_Reward_YesButton.onClick.AddListener (bossPopUpWindow.PopUpWindowReward_Switch_isFail);
			}


			yield return new WaitForSeconds(0.1f);
		}
	}

	protected override IEnumerator BossFinish ()
	{
		yield return null;


		//예외 코루틴 모두 종료
		StopCoroutine (BossSkillStandard ());
		StopCoroutine (BossSkill_01 ());
		StopCoroutine (BossSKill_02 ());
		StopCoroutine (BossDie ());
		StopCoroutine (BossResult ());

		SpawnManager.Instance.m_nBatchArbaitCount = 0;

		isFailed = false;
		isStandardPhaseFailed = false;

		if (bossBackGround.isBossBackGround == true) {
			SpawnManager.Instance.bIsBossCreate = false;
			bossBackGround.isBossBackGround = false;
			bossBackGround.isOriginBackGround = true;
		}
		bossUIDisable.SetActive (false);

		SpawnManager.Instance.ReliveArbaitBossRepair ();

		gameObject.SetActive (false);
		eCureentBossState = EBOSS_STATE.CREATEBOSS;
		Debug.Log ("Finish Boss");


	}

	public void ActiveIceWall()
	{
		BossIceWall iceWall_instance = null;
		iceWall_instance = iceWall.GetComponent<BossIceWall> ();
		if (iceWall.activeSelf == true)
		{
			
			isIceWallOn = false;

			nBossIceWallCount = 15;
			//iceWall_instance.StartDeFreezeRepair ();
			iceWall.SetActive (false);
		}
		else 
		{
			Debug.Log ("Active Ice Wall");
			iceWall.SetActive (true);
			isIceWallOn = true;
			fIceWallGenerateTimer = 0f;
			iceWall_instance.nCountBreakWall = nBossIceWallCount;
			iceWall_instance.StartFreezeRepair ();	//수리창 어는 것 시작
		
		}
	}

	public void FreezeArbait()
	{
		//아르바이트가 얼지 않은 곳의 인덱스를 가져온다
		BossIceWall iceWall_instance = null;
		iceWallIndex = SpawnManager.Instance.FreezeArbait ();

		if (iceWallIndex == -1)
			return;
		
		Debug.Log ("Create Arbait Ice Wall");
		if (isIceWall_ArbaitOn [iceWallIndex] != true)
		{
			isIceWall_ArbaitOn [iceWallIndex] = true;

			iceWall_instance = iceWall_Arbait_Freeze [iceWallIndex].GetComponent<BossIceWall> ();
			iceWall_instance.nCountBreakWall = 10;
			iceWall_instance.nCurrentArbaitIndex = iceWallIndex;
			iceWall_Arbait_Freeze [iceWallIndex].SetActive (true);
			iceWall_instance.StartFreezeArbait ();

			fIceWallArbaitTimer = 0f;
		}
	}
		

	public void DefreezeAllArbait()
	{
		fIceWallArbaitTimer = 0;
		SpawnManager.Instance.GetFreezeArbait ();
		//얼어있는 아르바이트가 없다면 그냥 return;
		if (SpawnManager.Instance.checkList.Count == 0) 
		{
			Debug.Log ("No Freeze Arbait");
			return;
		}

		for (int i = 0; i < SpawnManager.Instance.checkList.Count ; i++) 
		{
			BossIceWall iceWall_Freeze = null;
			Debug.Log ("Max Temp DefreezeAll Arbait");
			iceWall_Freeze = iceWall_Arbait_Freeze [SpawnManager.Instance.checkList[i]].GetComponent<BossIceWall> ();
			iceWall_Freeze.DeFreezeArbaitAll ();
			iceWall_Arbait_Defreeze [SpawnManager.Instance.checkList[i]].SetActive (true);
			isIceWall_ArbaitOn [SpawnManager.Instance.checkList[i]] = false;

			BossArbaitDeFreeze bossDefreeze = null;
			bossDefreeze = iceWall_Arbait_Defreeze [SpawnManager.Instance.checkList[i]].GetComponent<BossArbaitDeFreeze> ();
			bossDefreeze.nIndex = SpawnManager.Instance.checkList[i];
			bossDefreeze.StartDeFreeze ();
		}
	}

	public void ActiveTimer()
	{
		if (bossTimer_Obj.activeSelf == true)
		{
            GuestPanel.SetActive(true);
			bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_ICE);
		}
		else 
		{
            GuestPanel.SetActive(false);
            bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_ICE);
			bossTimer.bossIce = this;
		}

	}

	public void FailState()
	{
		isFailed = true;

		StartCoroutine (BossDie ());
	}




}
