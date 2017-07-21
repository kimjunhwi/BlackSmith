using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Druid : ArbaitBatch
{
    private bool m_bIsApplyBuff = false;
    private float m_fBuffTime = 0.0f;

    private float fChangeRepair = 0.0f;

    private float fChangeCritical = 0.0f;

    protected override void Awake()
    {
        base.Awake();

		nIndex = (int)E_ARBAIT.E_ELLIE;
    }

    // Update is called once per frame
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

		SpawnManager.Instance.InsertWeaponArbait(m_CharacterChangeData.index, nBatchIndex, nGrade);
    }

    protected override void OnDisable()
    {
        if (m_bIsApplyBuff)
        {
            m_bIsApplyBuff = false;

            playerData.SetRepairPower(playerData.GetRepairPower() - fChangeRepair);

            playerData.SetCriticalChance(playerData.GetCriticalChance() - fChangeCritical);
        }

        base.OnDisable();

		m_bIsApplyBuff = false;

		m_fBuffTime = 0.0f;

		fChangeRepair = 0.0f;

		fChangeCritical = 0.0f;
    }

    public override void ApplySkill()
    {
        if (m_bIsApplyBuff)
			m_fBuffTime = 0.0f;

        else
            StartCoroutine(ApplyDruidSkill());
    }

    protected override void ReliveSkill() { }

    private IEnumerator ApplyDruidSkill()
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsApplyBuff = true;

		fChangeRepair = playerData.GetRepairPower() * (m_CharacterChangeData.fSkillPercent * 0.01f);

		fChangeCritical = playerData.GetCriticalChance() * (m_CharacterChangeData.fSkillPercent * 0.01f);

        fChangeRepair = Mathf.Round(fChangeRepair);

        fChangeCritical = Mathf.Round(fChangeCritical);

        playerData.SetRepairPower(playerData.GetRepairPower() + fChangeRepair);

        playerData.SetCriticalChance(playerData.GetCriticalChance() + fChangeCritical);

        while (true)
        {
            yield return null;

			m_fBuffTime += Time.deltaTime;

			if (m_fBuffTime > 3.0f)
                break;
        }

        m_bIsApplyBuff = false;

        playerData.SetRepairPower(playerData.GetRepairPower() - fChangeRepair);

        playerData.SetCriticalChance(playerData.GetCriticalChance() - fChangeCritical);
    }

    public override void CheckCharacterState(E_ArbaitState _E_STATE)
    {
        if (E_STATE == _E_STATE)
            return;

        //액션 변경
        E_STATE = _E_STATE;
		animator.speed = 1.0f;

        //추후 사용 될 수 있을 부분이 있기에 만들어둠
        switch (E_STATE)
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

        switch (E_STATE)
        {
            case E_ArbaitState.E_WAIT:

                //대기중 수리 아이템이 있을 경우 수리로 바꿈
                if (AfootOjbect != null)
                    CheckCharacterState(E_ArbaitState.E_REPAIR);

                break;
            case E_ArbaitState.E_REPAIR:

                //수리
                fTime += Time.deltaTime;

                //수리 시간이 되면 0으로 초기화 하고 수리해줌
                if (fTime >= m_fRepairTime)
                {
                    fTime = 0.0f;

                    animator.SetTrigger("bIsRepair");

				//크리티컬 확률 
				if (Random.Range (1, 100) <= Mathf.Round (m_CharacterChangeData.fAccuracyRate)) 
					m_fComplate += m_CharacterChangeData.fRepairPower * 1.5f;
				else 
					m_fComplate += m_CharacterChangeData.fRepairPower;

                    //완성 됐을 경우
                    if (m_fComplate >= weaponData.fComplate)
                    {
                        ScoreManager.ScoreInstance.GoldPlus(100);

                        ComplateWeapon();
                    }

				SpawnManager.Instance.CheckComplateWeapon(AfootOjbect, m_fComplate,m_fTemperator);
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