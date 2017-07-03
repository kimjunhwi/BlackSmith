using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : BossCharacter 
{
	private RectTransform bossNoteRespawnPoint;
	private float fXPos;
	private float fYPos;
	private float fRandomXPos;
	private float fRandomYPos;
	public SimpleObjectPool noteObjectPool;
	public int nNoteCount = 0;
	public BossBackGround bossBackGround;

	public SpriteRenderer bossImage;
	public BossPopUpWindow bossPopUpWindow;
	public BossEffect bossEffect;

	private bool isFailed = false;
	private int nNoteMaxCount = 7;
	private float nBossGenerateTime = 2.0f;
	private float nContinueTime = 10f;



	Animator animator;

	bool isFirstActive = false;

	private void Start()
	{
		bossImage = GetComponent<SpriteRenderer> ();
		//bossImage.enabled = false;
		//bossBackGround = GameObject.Find ("BackGround").GetComponent<BossBackGround> ();
		noteObjectPool = GameObject.Find ("NotePool").GetComponent<SimpleObjectPool> ();

		bossNoteRespawnPoint = GameObject.Find ("BossNoteCreateArea").GetComponent<RectTransform>();
		fXPos = bossNoteRespawnPoint.position.x + 60;
		fYPos = bossNoteRespawnPoint.position.y + 60;

		//Debug.Log (fXPos + "," + fYPos);
		bossPopUpWindow = GameObject.Find("BossPopUpWindow").GetComponent<BossPopUpWindow>();
		bossEffect = GameObject.Find ("BossEffect").GetComponent<BossEffect> ();

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

	private void Update()
	{
		if (eCureentBossState == EBOSS_STATE.FINISH) 
		{
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
			while (bossNoteRespawnPoint.childCount != 0) 
			{
				GameObject go = bossNoteRespawnPoint.GetChild (0).gameObject;
				noteObjectPool.ReturnObject(go);
			}

		}		
	}

	protected override IEnumerator BossWait ()
	{

		while (true)
		{

			//무기 이미지 추가
			if (bossBackGround.isBossBackGround == true) {

				animator.SetBool ("isBackGroundChanged", true);

				if (animator.GetCurrentAnimatorStateInfo (0).length > 1.0f) {
					yield return new WaitForSeconds (0.5f);
					animator.SetBool ("isAppear", true);
					eCureentBossState = EBOSS_STATE.PHASE_00;

				} 
				else
					yield return null;


				if (eCureentBossState == EBOSS_STATE.PHASE_00) {
					repairObj.GetBossWeapon (GameManager.Instance.cWeaponInfo [0], bossInfo.fComplate, 0, 0, this);

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
		float fTime = 0f;
		GameObject Note;


		float fCurComplete = repairObj.GetCurCompletion ();
		float fMaxComplete = bossInfo.fComplate;

		while (true)
		{


			fRandomXPos = Random.Range (fXPos  - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;

			if (fTime >= nBossGenerateTime && bossNoteRespawnPoint.childCount != nNoteMaxCount) 
			{

				Note = noteObjectPool.GetObject ();
				Note.transform.SetParent (bossNoteRespawnPoint.transform);
				Note.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Note.transform.position.z);
				Note.name = "Music";

				NoteObject noteObj = Note.GetComponent<NoteObject> ();
				noteObj.skullObjPull = noteObjectPool;
				noteObj.parentTransform = bossNoteRespawnPoint;
				noteObj.fTime = nContinueTime;
				noteObj.repairObj = repairObj;
				fTime = 0f;
			}

			if (fCurComplete < 0) {
				isFailed = true;

				bossPopUpWindow.SetBossRewardBackGroundImage (isFailed);
				bossPopUpWindow.PopUpWindowReward_Switch ();

				eCureentBossState = EBOSS_STATE.FINISH;
			}

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
		float fTime = 0f;
		GameObject Note;
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		while (true)
		{




			fRandomXPos = Random.Range (fXPos - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= nBossGenerateTime && bossNoteRespawnPoint.childCount != nNoteMaxCount) 
			{

				Note = noteObjectPool.GetObject ();
				Note.transform.SetParent (bossNoteRespawnPoint.transform);
				Note.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Note.transform.position.z);
				Note.name = "Music";

				NoteObject noteObj = Note.GetComponent<NoteObject> ();
				noteObj.skullObjPull = noteObjectPool;
				noteObj.parentTransform = bossNoteRespawnPoint;
				noteObj.fTime = nContinueTime;
				noteObj.repairObj = repairObj;
				fTime = 0f;
			}
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

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
		float fTime = 0f;
		GameObject Note;
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (bossNoteRespawnPoint.sizeDelta.x/2), fXPos + (bossNoteRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossNoteRespawnPoint.sizeDelta.y/2), fYPos + (bossNoteRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= nBossGenerateTime && bossNoteRespawnPoint.childCount != nNoteMaxCount) 
			{

				Note = noteObjectPool.GetObject ();
				Note.transform.SetParent (bossNoteRespawnPoint.transform);
				Note.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Note.transform.position.z);
				Note.name = "Music";

				NoteObject noteObj = Note.GetComponent<NoteObject> ();
				noteObj.skullObjPull = noteObjectPool;
				noteObj.parentTransform = bossNoteRespawnPoint;
				noteObj.fTime = nContinueTime;
				noteObj.repairObj = repairObj;
				fTime = 0f;
			}
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

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
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		while (true)
		{

			/*
			bossDisappear_AnimationObject = bossDisappearEffectPool.GetObject();
			bossDisappear_AnimationObject.transform.SetParent (bossAppearAndDisappearPos.transform);
			bossDisappear_AnimationObject.transform.position = bossAppearAndDisappearPos.transform.transform.position;
			animator = bossDisappear_AnimationObject.GetComponent<Animator> ();
			animator.Play ("SasinDisAppear");
			yield return new WaitForSeconds (0.5f);
			bossImage.enabled = false;
			yield return new WaitForSeconds (0.6f);
			animator.Play ("SasinAppearIdle");
			yield return new WaitForSeconds (0.1f);
			bossDisappearEffectPool.ReturnObject (bossDisappear_AnimationObject);
			*/

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
			animator.Play ("BossSasinIdle");
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
}
