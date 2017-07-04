using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossSasin : BossCharacter 
{
	private RectTransform bossSkullRespawnPoint;
	private float fXPos;
	private float fYPos;
	private float fRandomXPos;
	private float fRandomYPos;

	public SimpleObjectPool skullObjectPool;
	public SimpleObjectPool bossAppearEffectPool;
	public SimpleObjectPool bossDisappearEffectPool;
	public int nSkullCount = 0;
	public BossBackGround bossBackGround;

	public SpriteRenderer bossImage;
	public BossPopUpWindow bossPopUpWindow;
	public BossEffect bossEffect;

	private bool isFailed = false;



	Animator animator;

	bool isFirstActive = false;

	private void Start()
	{
		bossImage = GetComponent<SpriteRenderer> ();
		//bossImage.enabled = false;
		//bossBackGround = GameObject.Find ("BackGround").GetComponent<BossBackGround> ();
		skullObjectPool = GameObject.Find ("SkullPool").GetComponent<SimpleObjectPool> ();
	
		bossSkullRespawnPoint = GameObject.Find ("BossSkullCreateArea2").GetComponent<RectTransform>();
		fXPos = bossSkullRespawnPoint.position.x;
		fYPos = bossSkullRespawnPoint.position.y;

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
			bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
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
			while (bossSkullRespawnPoint.childCount != 0) 
			{
				GameObject go = bossSkullRespawnPoint.GetChild (0).gameObject;
				skullObjectPool.ReturnObject(go);
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
		
		while (true)
		{
			
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = bossInfo.fComplate;

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
		GameObject Skull;
		bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
		while (true)
		{
			



			fRandomXPos = Random.Range (fXPos - (bossSkullRespawnPoint.sizeDelta.x/2), fXPos + (bossSkullRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossSkullRespawnPoint.sizeDelta.y/2), fYPos + (bossSkullRespawnPoint.sizeDelta.y/2));



			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= 2.0f && bossSkullRespawnPoint.childCount != 4) 
			{
					
				Skull = skullObjectPool.GetObject ();
				Skull.transform.SetParent (bossSkullRespawnPoint.transform);
				Skull.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Skull.transform.position.z);
				Skull.name = "Skull";

				SkullObject skullObj = Skull.GetComponent<SkullObject> ();
				skullObj.skullObjPull = skullObjectPool;
				skullObj.parentTransform = bossSkullRespawnPoint;
				skullObj.fTime = 7f;
				skullObj.repairObj = repairObj;
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
		GameObject Skull;
		while (true)
		{
			fRandomXPos = Random.Range (fXPos - (bossSkullRespawnPoint.sizeDelta.x/2), fXPos + (bossSkullRespawnPoint.sizeDelta.x/2));
			fRandomYPos = Random.Range (fYPos - (bossSkullRespawnPoint.sizeDelta.y/2), fYPos + (bossSkullRespawnPoint.sizeDelta.y/2));

			fTime += Time.deltaTime;
			//해골 생성 
			if (fTime >= 2.0f && bossSkullRespawnPoint.childCount != 4) 
			{

				Skull = skullObjectPool.GetObject ();
				Skull.transform.SetParent (bossSkullRespawnPoint.transform);
				Skull.transform.position = new Vector3 (fRandomXPos, fRandomYPos, Skull.transform.position.z);
				Skull.name = "Skull";

				SkullObject skullObj = Skull.GetComponent<SkullObject> ();
				skullObj.skullObjPull = skullObjectPool;
				skullObj.parentTransform = bossSkullRespawnPoint;
				skullObj.fTime = 7f;
				skullObj.repairObj = repairObj;
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
		//bossEffect.ActiveEffect (BOSSEFFECT.BOSSEFFECT_SASINANGRY);
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
