using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : BossCharacter 
{
	public RectTransform bossNoteRespawnPoint;
	private float fXPos;
	private float fYPos;
	private float fRandomXPos;
	private float fRandomYPos;
	public SimpleObjectPool noteObjectPool;
	public int nNoteCount = 0;

	private int nNoteMaxCount = 7;
	private float nBossGenerateTime = 2.0f;
	private float nContinueTime = 10f;
	private float nBossSpeedIncreaseValue =0f;    //보스 무기 속도 증가량
	private float nBossSpeedIncreaseRate = 0.1f;  //보스 무기 속도 증가비율
	//임시 변수
	private float fTime = 0f;					  //보스 리젠 시간
	GameObject Note;							  //노트 변수


	private void Start()
	{
		noteObjectPool = GameObject.Find ("NotePool").GetComponent<SimpleObjectPool> ();

		//bossNoteRespawnPoint = GameObject.Find ("BossNoteCreateArea").GetComponent<RectTransform>();
		fXPos = bossNoteRespawnPoint.position.x;
		fYPos = bossNoteRespawnPoint.position.y;
		animator = gameObject.GetComponent<Animator> ();
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
			//BossFx off
			if(isStandardPhaseFailed == false)
				bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);
		
			//말풍선 off
			if (bossTalkPanel.bossTalkPanel.activeSelf == true)
				bossTalkPanel.bossTalkPanel.SetActive (false);
			
			//예외 코루틴 모두 종료
			StopCoroutine (repairObj.BossMusicWeaponMove ());
			StopCoroutine (BossSkillStandard ());
			StopCoroutine (BossSkill_01 ());
			StopCoroutine (BossSKill_02 ());
			StopCoroutine (BossDie ());
			StopCoroutine (BossResult ());
		
			bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
			repairObj.SetFinishBoss ();									//수리 패널 초기화
			eCureentBossState = EBOSS_STATE.CREATEBOSS;					//현재 보스 상태 초기화

			//변수 초기화  
			isStandardPhaseFailed = false;
			isFailed = false;

			//배경이 원래대로 돌아가면 다시 손님들이 나오게 한다.
			if (bossBackGround.isBossBackGround == true) 
				SpawnManager.Instance.bIsBossCreate = false;
			
			//UiBlock Disable
			bossUIDisable.SetActive (false);	

			//아르바이트들 대기상태로 전환
			SpawnManager.Instance.ReliveArbaitBossRepair ();

			//노트 개수 초기화 
			nNoteCount = 0;
			//게임화면에 있는 모든 음악 노트 제거
			while (bossNoteRespawnPoint.childCount != 0) 
			{
				GameObject go = bossNoteRespawnPoint.GetChild (0).gameObject;
				noteObjectPool.ReturnObject(go);
			}
			gameObject.SetActive (false);
			Debug.Log ("Finish Boss");

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
		isStandardPhaseFailed = true;
		bossTalkPanel.StartShowBossTalkWindow (2f, "Hey Yo! 내 무기 고쳐줘!");
		while (true)
		{
			//fRandomXPos = Random.Range (fXPos  - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			//fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));
			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;

			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;



			if (fTime >= nBossGenerateTime && nNoteCount < nNoteMaxCount) 
			{
				
				CreateNote ();
			}

			if (fCurComplete < 0) {

				FailState ();
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
		
		bossTalkPanel.StartShowBossTalkWindow (2f, "허리 업 ~~~!");
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);

		isStandardPhaseFailed = false;
		while (true)
		{
			
			//fRandomXPos = Random.Range (fXPos - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			//fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));
			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;
			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			//Note 생성 
			if (fTime >= nBossGenerateTime && nNoteCount < nNoteMaxCount) 
			{
				CreateNote ();
			}
		

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
		bossTalkPanel.StartShowBossTalkWindow (2f, "Drop the Beat!!");
		while (true)
		{
			//fRandomXPos = Random.Range (fXPos - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			//fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));
			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;
			fTime += Time.deltaTime;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;
			//Note 생성 
			if (fTime >= nBossGenerateTime && nNoteCount < nNoteMaxCount) 
			{
				CreateNote ();
			}


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
		bossTalkPanel.StartShowBossTalkWindow (2f, "Bye~!");
		while (true)
		{
			animator.SetBool ("isDisappear", true);

			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("RucioDisappear")) 
			{
				eCureentBossState = EBOSS_STATE.RESULT;
				if (eCureentBossState == EBOSS_STATE.RESULT)
				{
					animator.SetBool ("isAppear", false);
					animator.SetBool ("isDisappear", false);
					animator.SetBool ("isBackGroundChanged", false);	
					break;
				}
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
		//Destroy (gameObject);
		yield break;
	}	

	public void CreateNote()
	{
		Note = noteObjectPool.GetObject ();
		Note.transform.SetParent (bossNoteRespawnPoint.transform, false);
		Note.transform.localScale = Vector3.one;
		Note.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Note.transform.position.z);
		Note.name = "Note";

		NoteObject noteObj = Note.GetComponent<NoteObject> ();
		noteObj.noteObjPull = noteObjectPool;
		noteObj.parentTransform = bossNoteRespawnPoint;
		noteObj.fTime = nContinueTime;
		noteObj.repairObj = repairObj;
		nBossSpeedIncreaseValue = 5.0f;
		repairObj.AddBossWeaponSpeed (nBossSpeedIncreaseValue * nBossSpeedIncreaseRate);
		nBossSpeedIncreaseValue = 0f;
		fTime = 0f;

		nNoteCount++;

	}

	public void ActiveTimer()
	{
		if (bossTimer_Obj.activeSelf == true)
		{
			bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_MUSIC);
		}
		else 
		{
			bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_MUSIC);
			bossTimer.bossMusic = this;
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
