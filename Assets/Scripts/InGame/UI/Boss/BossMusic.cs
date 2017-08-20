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
	public int nNoteCount = 0;					  //현재 노트 개수

	private int nNoteMaxCount = 4;				  //노트 최대 개수
	private float nBossGenerateTime = 2.0f;		  //노트 생성 주기(X초마다)
	private float nContinueTime = 10f;			  //노트 지속 시간
	private float nBossSpeedIncreaseValue =0f;    //보스 무기 속도 증가량
	private float nBossSpeedIncreaseRate = 0.1f;  //보스 무기 속도 증가비율
	//임시 변수
	private float fTime = 0f;					  //보스 리젠 시간
	GameObject Note;							  //노트 변수


	//Reflect 관련
	public bool isReflect 	= false;			  //루시우 반사 여부 
	public bool isSwitch 	= false;

	public float fCurReflectTime = 0f;			  //현재 시간
	public float fReflectRoutineTime = 5f;	  	  //반사 주기 시간 (5초마다 바뀐다)
	private float fReflectMaxTime = 10f;

	public int nCurLevel = 0;



	private void Start()
	{
		noteObjectPool = GameObject.Find ("NotePool").GetComponent<SimpleObjectPool> ();

		fXPos = bossNoteRespawnPoint.position.x;
		fYPos = bossNoteRespawnPoint.position.y;
		animator = gameObject.GetComponent<Animator> ();
		noteObjectPool.PreloadPool ();
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
					repairObj.bossWeaponAnimator.SetBool ("isBackGroundChanged", true);
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

		isStandardPhaseFailed = true;
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_BEGIN]);
		while (true)
		{
			//노트가 보스무기 위치에서 생성
			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;

			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			if (fReflectRoutineTime <= 1.0f)
				fReflectRoutineTime = 1.0f;
			

			//반사 타이머
			fCurReflectTime += Time.deltaTime;
			//Debug.Log ("fCurReflectTime = " + fCurReflectTime); 

			//최대 시간에 도달하면 초기화
			if (fCurReflectTime >= fReflectMaxTime) 
			{
				isSwitch = false;
				isReflect = false;
				fCurReflectTime = 0f;
				repairObj.bossWeaponAnimator.SetBool ("isPhase00_Reflect", false);
				Debug.Log ("상태 변환 : 반사 -> 노말");
			}
				
			//NonReflect
			if (fCurReflectTime < fReflectRoutineTime && isSwitch == false && isReflect == false) 
			{
				//Debug.Log ("노말 상태");
			}

			if (fCurReflectTime >= fReflectRoutineTime && isSwitch == false && isReflect == false ) 
			{
				isReflect = true;
				isSwitch = true;
				repairObj.bossWeaponAnimator.SetBool ("isPhase00_Reflect", true);
				Debug.Log ("상태 변환 : 노말 -> 반사");
			}

			//Reflect
			if (fCurReflectTime >= fReflectRoutineTime && isSwitch == true && isReflect == true) 
			{
				//Debug.Log ("반사 상태");
			}
				

			//현재 완성도 (실패조건)
			if (fCurComplete < 0) {
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
		//Music01 Passive
		SpawnManager.Instance.Active_MusicPassive01 ();
		nNoteMaxCount = 4;
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE01]);
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);
		repairObj.bossWeaponAnimator.SetBool ("isPhase00_Reflect", false);
		repairObj.bossWeaponAnimator.SetBool ("isPhase00", true);
		isStandardPhaseFailed = false;

		//if(repairObj.isMoveWeapon == false)
		//	repairObj.StartBossMusiceWeaponMove();
		
		while (true)
		{
			if (fReflectRoutineTime <= 1.0f)
				fReflectRoutineTime = 1.0f;
			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;
		
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

			fTime += Time.deltaTime;
			//Note 생성 
			if (fTime >= nBossGenerateTime && nNoteCount < nNoteMaxCount) 
				CreateNote ();

			//반사 타이머
			fCurReflectTime += Time.deltaTime;

			//최대 시간에 도달하면 초기화
			if (fCurReflectTime >= fReflectMaxTime) 
			{
				isSwitch = false;
				isReflect = false;
				fCurReflectTime = 0f;
				repairObj.bossWeaponAnimator.SetBool ("isPhase01_Reflect", false);
				Debug.Log ("상태 변환 : 반사 -> 노말");
			}

			//NonReflect
			if (fCurReflectTime < fReflectRoutineTime && isSwitch == false && isReflect == false) 
			{
				//Debug.Log ("노말 상태!");
			}

			if (fCurReflectTime >= fReflectRoutineTime && isSwitch == false && isReflect == false ) 
			{
				isReflect = true;
				isSwitch = true;
				repairObj.bossWeaponAnimator.SetBool ("isPhase01_Reflect", true);
				Debug.Log ("상태 변환 : 노말 -> 반사");
			}

			//Reflect
			if (fCurReflectTime >= fReflectRoutineTime && isSwitch == true && isReflect == true) 
			{
				//Debug.Log ("반사 상태!");
			}

		
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
		nNoteMaxCount = 8;
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_PHASE02]);
		while (true)
		{

			if (fReflectRoutineTime <= 1.0f)
				fReflectRoutineTime = 1.0f;


			fRandomXPos = bossWeapon.transform.position.x;
			fRandomYPos = bossWeapon.transform.position.y;


			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete =  bossInfo.fComplate;

			fTime += Time.deltaTime;
			//Note 생성 
			if (fTime >= nBossGenerateTime && nNoteCount < nNoteMaxCount) 
				CreateNote ();
			

			//반사 타이머
			fCurReflectTime += Time.deltaTime;

			//최대 시간에 도달하면 초기화
			if (fCurReflectTime >= fReflectMaxTime) 
			{
				isSwitch = false;
				isReflect = false;
				fCurReflectTime = 0f;
				repairObj.bossWeaponAnimator.SetBool ("isPhase01_Reflect", false);
				Debug.Log ("상태 변환 : 반사 -> 노말");
			}

			//NonReflect
			if (fCurReflectTime < fReflectRoutineTime && isSwitch == false && isReflect == false) 
			{
				//Debug.Log ("노말 상태!");
			}

			if (fCurReflectTime >= fReflectRoutineTime && isSwitch == false && isReflect == false ) 
			{
				isReflect = true;
				isSwitch = true;
				repairObj.bossWeaponAnimator.SetBool ("isPhase01_Reflect", true);
				Debug.Log ("상태 변환 : 노말 -> 반사");
			}

			//Reflect
			if (fCurReflectTime <= fReflectRoutineTime && isSwitch == true && isReflect == true) 
			{
				//Debug.Log ("반사 상태!");
			}



			if (fCurComplete < 0) {
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
		bossTalkPanel.StartShowBossTalkWindow (2f, bossWord[(int)E_BOSSWORD.E_BOSSWORD_END]);
		animator.SetBool ("isDisappear", true);


		repairObj.bossWeaponAnimator.SetBool ("isPhase00_Reflect", false);
		repairObj.bossWeaponAnimator.SetBool ("isPhase01_Reflect", false);
		repairObj.bossWeaponAnimator.SetBool ("isBackGroundChanged", false);
		repairObj.bossWeaponAnimator.SetBool ("isPhase00", false);
		repairObj.bossWeaponAnimator.Play ("BossIdle");



		while (true)
		{
			
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("RucioDisappear")) 
			{


				yield return new WaitForSeconds (0.8f);
				eCureentBossState = EBOSS_STATE.RESULT;
				if (eCureentBossState == EBOSS_STATE.RESULT)
				{	
					//BossFx off
					if(isStandardPhaseFailed == false)
						bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_RUCIOVOLUMEUP);

					//말풍선 off
					if (bossTalkPanel.bossTalkPanel.activeSelf == true)
						bossTalkPanel.bossTalkPanel.SetActive (false);


					animator.SetBool ("isAppear", false);
					animator.SetBool ("isDisappear", false);
					animator.SetBool ("isBackGroundChanged", false);	
					animator.Play ("BossIdle");



					bossBackGround.StartReturnBossBackGroundToBackGround ();	//배경 초기화
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
		Debug.Log ("BossResult Active!!");
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

		//반사 상태 초기화
		isReflect = false;
		isSwitch = false;
		fCurReflectTime = 0f;
		fReflectRoutineTime = 5f;

		eCureentBossState = EBOSS_STATE.CREATEBOSS;					//현재 보스 상태 초기화
		gameObject.SetActive (false);
		Debug.Log ("Finish Boss");

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
		noteObj.bossMusic = this;
		noteObj.StartNoteObjMove ();
		fTime = 0f;

		IncreaseRefectionTime (0.5f);

		nNoteCount++;

	}

	public void ActiveTimer()
	{
		if (bossTimer_Obj.activeSelf == true)
		{
            GuestPanel.SetActive(true);
            bossTimer_Obj.SetActive (false);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StopTimer(0f,0f,(int)E_BOSSNAME.E_BOSSNAME_MUSIC);
		}
		else 
		{
            GuestPanel.SetActive(false);
            bossTimer_Obj.SetActive (true);
			bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
			bossTimer.StartTimer (1f, 30f , (int)E_BOSSNAME.E_BOSSNAME_MUSIC);
			bossTimer.bossMusic = this;
		}
	}


	public void FailState()
	{
		isFailed = true;
		StartCoroutine (BossDie ());
	}


	public void IncreaseRefectionTime(float _time)
	{
		fReflectRoutineTime -= _time;
	}

	public void DecreaseRefectionTime(float _time)
	{
		fReflectRoutineTime += _time;
	}
}
