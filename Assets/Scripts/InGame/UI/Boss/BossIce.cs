using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIce : BossCharacter 
{
	public int nNoteCount = 0;
	public BossBackGround bossBackGround;

	public SpriteRenderer bossImage;
	public BossPopUpWindow bossPopUpWindow;
	public BossEffect bossEffect;

	private bool isFailed = false;
	private int nNoteMaxCount = 7;

	private float nContinueTime = 10f;
	private float nBossSpeedIncreaseValue =0f;    //보스 무기 속도 증가량
	private float nBossSpeedIncreaseRate = 0.1f;  //보스 무기 속도 증가비율
	private string sBossSprite = "Weapons/Boss/deathnote";

	Animator animator;							  //BossAnimator
	bool isFirstActive = false;					  //처음 실행시 체크 변수

	//IceWall
	private BossIceWall iceWall_instance;
	public GameObject iceWall;
	public bool isIceWallOn;					  //IceWall이 켜졌는지 아닌지
	public float fIceWallGenerateTimer =0f;
	private float nBossIceWallGenerateTime = 15.0f;
	//IceWall Arbait
	int iceWallIndex = 0;
	private float fIceWallArbaitTimer =0f;
	private float fIceWallArbaitGenerateTime = 10.0f;
	public GameObject[] iceWall_Arbait;
	public bool[] isIceWall_ArbaitOn;

	private void Start()
	{
		bossImage = GetComponent<SpriteRenderer> ();
		bossEffect = GameObject.Find ("BossEffect").GetComponent<BossEffect> ();

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

	private void Update()
	{
		if (eCureentBossState == EBOSS_STATE.FINISH) 
		{
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

			if (bossBackGround.isBossBackGround == true) {
				SpawnManager.Instance.bIsBossCreate = false;
				bossBackGround.isBossBackGround = false;
				bossBackGround.isOriginBackGround = true;
			}


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

				if (animator.GetCurrentAnimatorStateInfo (0).IsName("RucioAppear")) 
				{
					//yield return new WaitForSeconds (0.1f);
					animator.SetBool ("isAppear", true);
					eCureentBossState = EBOSS_STATE.PHASE_00;
				} 
				else
					yield return null;


				if (eCureentBossState == EBOSS_STATE.PHASE_00) {

					repairObj.GetBossWeapon (ObjectCashing.Instance.LoadSpriteFromCache(sBossSprite), bossInfo.fComplate, 0, 0, this);

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
		
		while (true)
		{

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;


			//Boss Ice Wall Timer
			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;
		
		
			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
			{
				ActiveIceWall ();
			}

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


		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);


		while (true)
		{
			//BossWeapon info
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			//Boss Ice Wall Timer
			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;

			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
			{
				ActiveIceWall ();
			}


			//Arbait Ice Wall Timer
			if (isIceWall_ArbaitOn [0] == false || isIceWall_ArbaitOn [1] == false || isIceWall_ArbaitOn [2] == false)
				fIceWallArbaitTimer += Time.deltaTime;
			

			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
			{
				iceWallIndex = SpawnManager.Instance.FreezeArbait ();
				if (isIceWall_ArbaitOn [iceWallIndex] != true) {
					isIceWall_ArbaitOn [iceWallIndex] = true;
					iceWall_Arbait [iceWallIndex].SetActive (true);
					iceWall_instance = iceWall_Arbait [iceWallIndex].GetComponent<BossIceWall> ();
					iceWall_instance.nCountBreakWall = 10;
					fIceWallArbaitTimer = 0f;
				}
			}
		

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

		while (true)
		{

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;
	



			if(isIceWallOn == false)
				fIceWallGenerateTimer += Time.deltaTime;
			if (fIceWallGenerateTimer >= nBossIceWallGenerateTime && isIceWallOn == false) 
			{
				ActiveIceWall ();
			}
				
			//Arbait Ice Wall Timer
			if (isIceWall_ArbaitOn [0] == false || isIceWall_ArbaitOn [1] == false || isIceWall_ArbaitOn [2] == false)
				fIceWallArbaitTimer += Time.deltaTime;


			if (fIceWallArbaitTimer >= fIceWallArbaitGenerateTime) 
			{
				iceWallIndex = SpawnManager.Instance.FreezeArbait ();
				if (isIceWall_ArbaitOn [iceWallIndex] != true) {
					isIceWall_ArbaitOn [iceWallIndex] = true;
					iceWall_Arbait [iceWallIndex].SetActive (true);
					iceWall_instance = iceWall_Arbait [iceWallIndex].GetComponent<BossIceWall> ();
					iceWall_instance.nCountBreakWall = 10;
				}
			}


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
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);
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
			bossPopUpWindow.PopUpWindowReward_Switch();
			bossPopUpWindow.GetBossInfo (this);
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
			iceWall.SetActive (true);
			isIceWallOn = true;
			iceWall_instance = iceWall.GetComponent<BossIceWall> ();
			iceWall_instance.nCountBreakWall = 15;
		
		}
	}


}
