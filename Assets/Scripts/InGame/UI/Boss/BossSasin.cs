using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSasin : BossCharacter 
{
	private RectTransform bossSkullRespawnPoint;
	private float fXPos;
	private float fYPos;
	private float fRandomXPos;
	private float fRandomYPos;
	public SimpleObjectPool skullObjectPool;
	public int nSkullCount = 0;

	private void Start()
	{
		skullObjectPool = GameObject.Find ("SkullPool").GetComponent<SimpleObjectPool> ();
		bossSkullRespawnPoint = GameObject.Find ("BossSkullCreateArea2").GetComponent<RectTransform>();
		fXPos = bossSkullRespawnPoint.position.x;
		fYPos = bossSkullRespawnPoint.position.y;

		//Debug.Log (fXPos + "," + fYPos);

	
		eCureentBossState = EBOSS_STATE.CREATEBOSS;
		StartCoroutine (BossWait ());
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
			Destroy (gameObject);

			repairObj.SetFinishBoss ();
			SpawnManager.Instance.bIsBossCreate = false;
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
			repairObj.GetBossWeapon(GameManager.Instance.cWeaponInfo[0],GameManager.Instance.bossInfo[1].fComplate,0,0 , boss  , this);

			//yield return new WaitForSeconds (1.0f);

			//Debug.Log ("BossWait Active!!");
			//eCureentBossState = EBOSS_STATE.PHASE_00;

			eCureentBossState = EBOSS_STATE.PHASE_00;

			if (eCureentBossState == EBOSS_STATE.PHASE_00)
				break;
			
		}
		StartCoroutine (BossSkillStandard ());
		yield break;
	}

	protected override IEnumerator BossSkillStandard ()
	{
		while (true)
		{
			
			float fCurComplete = repairObj.GetCurCompletion ();
			float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;

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
			float fMaxComplete = GameManager.Instance.bossInfo[1].fComplate;



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
		while (true)
		{
			//yield return new WaitForSeconds (2.0f);

			Debug.Log ("BossDie Active!!");
			eCureentBossState = EBOSS_STATE.RESULT;
			if (eCureentBossState == EBOSS_STATE.RESULT)
				break;
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
			//yield return new WaitForSeconds (2.0f);
			Debug.Log ("BossResult Active!!");
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
