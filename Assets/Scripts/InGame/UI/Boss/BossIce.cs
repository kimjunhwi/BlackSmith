using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIce : BossCharacter 
{
	//IceWall
	private BossIceWall iceWall_instance;
	public GameObject iceWall;
	public bool isIceWallOn;					  //IceWall이 켜졌는지 아닌지
	public float fIceWallGenerateTimer = 0f;
	private float nBossIceWallGenerateTime = 5.0f;
	//IceWall Arbait
	int iceWallIndex = 0;
	public float fIceWallArbaitTimer =0f;
	private float fIceWallArbaitGenerateTime = 5.0f;
	public GameObject[] iceWall_Arbait_Freeze;
	public GameObject[] iceWall_Arbait_Defreeze;
	public bool[] isIceWall_ArbaitOn;

	private void Start()
	{
		animator = gameObject.GetComponent<Animator> ();
		iceWall.SetActive (false);

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

	private void Update()
	{
		if (eCureentBossState == EBOSS_STATE.FINISH) 
		{

			//Effect Off
			if(isStandardPhaseFailed == false)
				bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_ICEBLLIZARD);

			//RepairPanel IceWall Off
			if (iceWall.activeSelf == true)
				ActiveIceWall ();

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

			SpawnManager.Instance.m_nBatchArbaitCount = 0;
			eCureentBossState = EBOSS_STATE.CREATEBOSS;
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

		}		
	}

	protected override IEnumerator BossWait ()
	{

		while (true)
		{

			//무기 이미지 추가
			if (bossBackGround.isBossBackGround == true) {

				animator.SetBool ("isBackGroundChanged", true);

				if (animator.GetCurrentAnimatorStateInfo (0).IsName("Ice_Appear")) 
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
		bossTalkPanel.StartShowBossTalkWindow (2f, "저... 무기좀... 고쳐주세요");
		isStandardPhaseFailed = true;
		while (true)
		{
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			//Boss Ice Wall Timer
			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;
			
			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
				ActiveIceWall ();
			
			if (fCurComplete < 0) {
				isFailed = true;

				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();

				eCureentBossState = EBOSS_STATE.FINISH;
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
		bossTalkPanel.StartShowBossTalkWindow (2f, "흐으음~~~");

		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_ICEBLLIZARD);
		isStandardPhaseFailed = false;

		while (true)
		{

			//BossWeapon info
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			//Boss Ice Wall Timer
			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;

			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
				ActiveIceWall ();
			
		

			//Arbait Ice Wall Timer
			if (SpawnManager.Instance.FreezeArbaitCheck() == true) {
				Debug.Log (fIceWallArbaitTimer);
				fIceWallArbaitTimer += Time.deltaTime;
			}

			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
				FreezeArbait ();
			
			if (fCurComplete < 0) {
				isFailed = true;
				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();

				eCureentBossState = EBOSS_STATE.FINISH;
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

		bossTalkPanel.StartShowBossTalkWindow (2f, "눈보라 ~~~!");
		while (true)
		{
			iceWallIndex = SpawnManager.Instance.FreezeArbait ();

			//GetCompletion
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;
	
			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;

			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
				ActiveIceWall ();
			
			//Arbait Ice Wall Timer
			if (SpawnManager.Instance.FreezeArbaitCheck() == true) {
				Debug.Log (fIceWallArbaitTimer);
				fIceWallArbaitTimer += Time.deltaTime;
			}

			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
				FreezeArbait ();
			
			if (fCurComplete < 0) {
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

		bossTalkPanel.StartShowBossTalkWindow (2f, "그럼 이만!");
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

	public void ActiveIceWall()
	{
		if (iceWall.activeSelf == true)
		{
			iceWall.SetActive (false);
			isIceWallOn = false;
			fIceWallGenerateTimer = 0f;
		}
		else 
		{
			isIceWallOn = true;
			iceWall_instance = iceWall.GetComponent<BossIceWall> ();
			iceWall_instance.nCountBreakWall = 15;

			iceWall.SetActive (true);
			iceWall_instance.StartFreezeRepair ();	//수리창 어는 것 시작
		
		}
	}

	public void FreezeArbait()
	{
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

	public void ActiveTimer()
	{
		

		if (bossTimer_Obj.activeSelf == true)
		{
			bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_ICE);
		}
		else 
		{
			bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_ICE);
			bossTimer.bossIce = this;
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




}
