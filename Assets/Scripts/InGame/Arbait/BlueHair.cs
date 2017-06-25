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

		nGrade = arbaitData.nowGrade;

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
				m_fComplate += arbaitData.repairPower;

				m_fCurComplateX = m_fComplate / m_fMaxComplate;

				ComplateGauge.localScale = new Vector3(m_fCurComplateX,  ComplateGauge.transform.localScale.y, ComplateGauge.transform.localScale.z);

				//완성 됐을 경우
				if (m_fCurComplateX >= 1.0f)
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

	//만약 클릭 했을 경우
	void OnMouseDown()
	{
		if (Input.GetMouseButtonDown(0) && E_STATE == E_ArbaitState.E_REPAIR)
		{
			bIsComplate = true;

			AfootOjbect.GetComponent<Character>().m_bIsRepair = true;

			RepairShowObject.GetWeapon(AfootOjbect, weaponData, m_fComplate, m_fTemperator);

			ResetWeaponData();
		}
	}
}
