using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSasin : BossCharacter 
{

	private void Start()
	{
		eCureentBossState = EBOSS_STATE.CREATEBOSS;
		StartCoroutine (BossWait());
	}

	private void Update()
	{
		if (eCureentBossState == EBOSS_STATE.PHASE_00)
			StartCoroutine (BossSkillStandard ());
		else if (eCureentBossState == EBOSS_STATE.PHASE_01)
			StartCoroutine (BossSkill_01 ());
		else if (eCureentBossState == EBOSS_STATE.PHASE_02)
			StartCoroutine (BossSKill_02 ());
		else if (eCureentBossState == EBOSS_STATE.DIE)
			StartCoroutine (BossDie ());
		else if (eCureentBossState == EBOSS_STATE.RESULT)
			StartCoroutine (BossResult ());
		else 
		{
			Debug.Log ("Finsh!!");
			Destroy (gameObject);
		}
		
	}


	protected override IEnumerator BossWait ()
	{
		while (true)
		{
			Debug.Log ("BossWait Active!!");
			if (eCureentBossState == EBOSS_STATE.PHASE_00)
				yield break;
			
			eCureentBossState = EBOSS_STATE.PHASE_00;
			yield return new WaitForSeconds (2.0f);


		}
		yield return null;
	}

	protected override IEnumerator BossSkillStandard ()
	{
		while (eCureentBossState != EBOSS_STATE.PHASE_01)
		{
			Debug.Log ("BossPhase00 Active!!");
			yield return new WaitForSeconds (2.0f);
			eCureentBossState = EBOSS_STATE.PHASE_01;
			yield break;

		}
		yield return null;
	}


	protected override IEnumerator BossSkill_01 ()
	{

		while (eCureentBossState != EBOSS_STATE.PHASE_02)
		{
			Debug.Log ("BossPhase01 Active!!");
			yield return new WaitForSeconds (2.0f);
			eCureentBossState = EBOSS_STATE.PHASE_02;
				
			yield break;
		}
		yield return null;

	}

	protected override IEnumerator BossSKill_02 ()
	{
		while (eCureentBossState != EBOSS_STATE.DIE)
		{
			Debug.Log ("BossPhase02 Active!!");
			yield return new WaitForSeconds (2.0f);
			eCureentBossState = EBOSS_STATE.DIE;
			yield break;
		}
		yield return null;
	}



	protected override IEnumerator BossDie ()
	{
		while (eCureentBossState != EBOSS_STATE.RESULT)
		{
			Debug.Log ("BossDie Active!!");
			yield return new WaitForSeconds (2.0f);
			eCureentBossState = EBOSS_STATE.RESULT;
			yield break;
		}
		yield return null;
	}

	protected override IEnumerator BossResult ()
	{
		while (eCureentBossState != EBOSS_STATE.FINISH)
		{
			Debug.Log ("BossSkill2 Active!!");
			yield return new WaitForSeconds (2.0f);
			eCureentBossState = EBOSS_STATE.FINISH;
			yield break;
		}
		yield return null;
	}



}
