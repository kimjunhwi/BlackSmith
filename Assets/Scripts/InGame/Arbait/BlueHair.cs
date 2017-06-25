using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class BlueHair : ArbaitBatch {



	protected override void Awake ()
	{
		base.Awake ();

		nIndex = (int)E_ARBAIT.E_BLUEHAIR;


	}

	// Update is called once per frame
	void Update () 
	{
		StartCoroutine(this.CheckCharacterState());
		StartCoroutine(this.CharacterAction());
	}


	void OnEnable()
	{
		if (arbaitData == null)
			return;

		bIsComplate = false;

		string strPath = string.Format("ArbaitUI/{0}", arbaitData.name);

		myCharacterSprite.sprite = ObjectCashing.Instance.LoadSpriteFromCache(strPath);

		nGrade = arbaitData.grade;

		E_STATE = E_ArbaitState.E_WAIT;

		SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
	}

	protected override IEnumerator CheckCharacterState()
	{
		yield return new WaitForSeconds(0.3f);

		if (weaponData == null)
			E_STATE = E_ArbaitState.E_WAIT;
		else
			E_STATE = E_ArbaitState.E_REPAIR;
	}

	protected override IEnumerator CharacterAction()
	{

		switch(E_STATE)
		{
		case E_ArbaitState.E_WAIT:
			//대기중일경우 어떠한 애니메이션을 취하게 함
			break;

		case E_ArbaitState.E_REPAIR:
			//수리

			fTime += Time.deltaTime;

			//수리 시간이 되면 0으로 초기화 하고 수리해줌
			if(fTime >= m_fRepairTime)
			{
				animator.SetTrigger ("bIsRepair");

				m_fComplate += arbaitData.fRepairPower;

				//완성 됐을 경우
				if (m_fComplate >= weaponData.fComplate)
				{
					ScoreManager.ScoreInstance.GoldPlus(100);

					ComplateWeapon();
				}

				fTime = 0.0f;
			}


			break;
		}

		yield return null;
	}
}
