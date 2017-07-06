using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class BlueHair : ArbaitBatch {

	private Player playerData;

	private float fChangeRepair = 0.0f;

	private float fGetRepairPower = 0.0f;

	private float fMinusRepair = 0.0f;

	protected override void Awake ()
	{
		base.Awake ();

		nIndex = (int)E_ARBAIT.E_BLUEHAIR;

		playerData = GameManager.Instance.player;
	}

	// Update is called once per frame
	void Update () 
	{
		StartCoroutine(this.CharacterAction());
	}


	protected override void OnEnable()
	{
		if (m_CharacterChangeData == null)
			return;

		bIsComplate = false;

		string strPath = string.Format("ArbaitUI/{0}", m_CharacterChangeData.name);

		myCharacterSprite.sprite = ObjectCashing.Instance.LoadSpriteFromCache(strPath);

		nGrade = m_CharacterChangeData.grade;

		E_STATE = E_ArbaitState.E_WAIT;

        CheckCharacterState(E_STATE);

        ApplySkill();

		SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
	}

	protected override void OnDisable ()
	{
        ReliveSkill();

		base.OnDisable ();
	}

    protected override void ApplySkill()
    {
        if (fChangeRepair != 0)
            ReliveSkill();

		fGetRepairPower = playerData.GetRepairPower ();

		fChangeRepair = fGetRepairPower * (buff[0].fValue * 0.01f);

		fChangeRepair =  Mathf.Round (fChangeRepair);

		playerData.SetRepairPower(fGetRepairPower + fChangeRepair);
    }

    protected override void ReliveSkill()
    {
		fGetRepairPower = playerData.GetRepairPower ();

		fMinusRepair = fGetRepairPower - fChangeRepair;

		fMinusRepair =  Mathf.Round (fMinusRepair);

		playerData.SetRepairPower(fMinusRepair);
    }

	protected override void CheckCharacterState(E_ArbaitState _E_STATE)
	{
        if (E_STATE == _E_STATE)
           return;

        //액션 변경
        E_STATE = _E_STATE;

        //추후 사용 될 수 있을 부분이 있기에 만들어둠
        switch(E_STATE)
        {
            case E_ArbaitState.E_WAIT:
                {

                }
                break;

            case E_ArbaitState.E_REPAIR:
                {

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
                if (AfootOjbect != null)
                    CheckCharacterState(E_ArbaitState.E_REPAIR);

			break;
		case E_ArbaitState.E_REPAIR:

            //수리
			fTime += Time.deltaTime;

			//수리 시간이 되면 0으로 초기화 하고 수리해줌
			if(fTime >= m_fRepairTime)
			{
                fTime = 0.0f;

				animator.SetTrigger("bIsRepair");

				m_fComplate += m_CharacterChangeData.fRepairPower;

				//완성 됐을 경우
				if (m_fComplate >= weaponData.fComplate)
				{
					ScoreManager.ScoreInstance.GoldPlus(100);

					ComplateWeapon();
				}

				SpawnManager.Instance.CheckComplateWeapon (AfootOjbect, m_fComplate);
			}
			break;
		}
	}
}
