﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class BlueHair : ArbaitBatch {


    private float fChangeRepair = 0.0f;

    private float fGetRepairPower = 0.0f;

    private float fMinusRepair = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        nIndex = (int)E_ARBAIT.E_ROY;


    }

    protected override void Update()
    {
        StartCoroutine(this.CharacterAction());
    }

    protected override void OnEnable()
    {
        if (m_CharacterChangeData == null || nBatchIndex == -1)
            return;

        bIsComplate = false;

        nGrade = m_CharacterChangeData.grade;

        E_STATE = E_ArbaitState.E_WAIT;

        CheckCharacterState(E_STATE);

        ApplySkill();

        SpawnManager.Instance.InsertWeaponArbait(m_CharacterChangeData.index, nBatchIndex);
    }

    protected override void OnDisable()
    {
        ReliveSkill();

        base.OnDisable();

        fChangeRepair = 0.0f;

        fGetRepairPower = 0.0f;

        fMinusRepair = 0.0f;
    }

    public override void ApplySkill()
    {
        if (fChangeRepair >= 1)
            ReliveSkill();

        fGetRepairPower = playerData.GetBasicRepairPower();

        fChangeRepair = fGetRepairPower * (m_CharacterChangeData.fSkillPercent * 0.01f);

        fChangeRepair = Mathf.Round(fChangeRepair);

		playerData.SetBasicRepairPower(fGetRepairPower + fChangeRepair);
    }

    protected override void ReliveSkill()
    {
        if (fChangeRepair <= 1)
            return;

		fGetRepairPower = playerData.GetBasicRepairPower();

        fMinusRepair = fGetRepairPower - fChangeRepair;

        fMinusRepair = Mathf.Round(fMinusRepair);

		playerData.SetBasicRepairPower(fMinusRepair);
    }

    public override void RelivePauseSkill()
    {
        base.RelivePauseSkill();

        ReliveSkill();
    }

    public override void ApplyPauseSkill()
    {
        base.ApplyPauseSkill();

        ApplySkill();
    }

    public override void CheckCharacterState(E_ArbaitState _E_STATE)
	{
        if (E_STATE == _E_STATE)
           return;

        //액션 변경
        E_STATE = _E_STATE;
		animator.speed = 1.0f;
        //추후 사용 될 수 있을 부분이 있기에 만들어둠
        switch(E_STATE)
        {
            case E_ArbaitState.E_WAIT:
                {
					animator.speed = 1.0f;
                }
                break;

		case E_ArbaitState.E_REPAIR:
			{
				
			}
            break;
		case E_ArbaitState.E_FREEZE:
			{
				animator.speed = 0.0f;
			}
			break;

		case E_ArbaitState.E_BOSSREPAIR:
			{
				fTime = 0.0F;
			}
			break;
        }
	}

	protected override IEnumerator CharacterAction()
	{
        yield return new WaitForSeconds(0.1f);

		switch(E_STATE)
		{
		case E_ArbaitState.E_WAIT:

			//대기중 수리 아이템이 있을 경우 수리로 바꿈
			if (AfootOjbect != null && bIsRepair == true)
				CheckCharacterState(E_ArbaitState.E_REPAIR);

			break;
		case E_ArbaitState.E_REPAIR:

			//수리
			fTime += Time.deltaTime;

			if(AfootOjbect == null || bIsRepair == false)
				CheckCharacterState(E_ArbaitState.E_WAIT);

			//수리 시간이 되면 0으로 초기화 하고 수리해줌
			if(fTime >= m_fRepairTime)
			{
                fTime = 0.0f;

				animator.SetTrigger("bIsRepair");

				//크리티컬 확률 
				if (Random.Range (1, 100) <= Mathf.Round (m_CharacterChangeData.fAccuracyRate)) 
					m_fComplate += m_CharacterChangeData.fRepairPower * 1.5f * fRepairDownPercent;
				else 
					m_fComplate += m_CharacterChangeData.fRepairPower *fRepairDownPercent;
				
				//완성 됐을 경우
				if (m_fComplate >= weaponData.fMaxComplate)
				{
					ScoreManager.ScoreInstance.GoldPlus(100);

					ComplateWeapon();
				}

				SpawnManager.Instance.CheckComplateWeapon (AfootOjbect, m_fComplate,m_fTemperator);
			}
			break;

		case E_ArbaitState.E_BOSSREPAIR:

			//수리
			fTime += Time.deltaTime;

			//수리 시간이 되면 0으로 초기화 하고 수리해줌
			if(fTime >= m_fRepairTime)
			{
				fTime = 0.0f;

				animator.SetTrigger("bIsRepair");

				RepairShowObject.SetCurCompletion(m_CharacterChangeData.fRepairPower );
			}

			break;
		}

	
	}
}
