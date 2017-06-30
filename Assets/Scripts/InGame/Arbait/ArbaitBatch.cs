﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class ArbaitBatch : MonoBehaviour {

	public ArbaitData arbaitData;

    public List<Buff> buff = new List<Buff>();

    string[] strBuffsIndex;

    public bool bIsRepair = false;

    public bool bIsComplate = false;

	protected int nIndex = 0;

    protected float fTime = 0.0f;

	protected float fBuffTime = 0.0f;
    
    //무기 완성도
    protected float m_fComplate;

    //무기 수리 시간
    protected float m_fRepairTime = 2.0f;

    // 무기 온도
    protected float m_fTemperator;

    //무기 완성 맥스치
    protected float m_fMaxComplate;

    //아르바이트 현재 상태
	protected E_ArbaitState E_STATE;

    //진행중인 오브젝트
    public GameObject AfootOjbect;

    //아르바이트 패널을 저장
    public GameObject ArbaitPanelObject;

    public CGameWeaponInfo weaponData;

    //무기가 보일 말풍선(?) // 미정
	protected RepairObject RepairShowObject;

	protected SpriteRenderer myCharacterSprite;

	protected BoxCollider2D boxCollider;

	protected Animator animator;

    //무기 등급을 어디까지 받아올지를 정하기 위해 사용
    public int nGrade { get; set; }

	// Use this for initialization
	protected virtual void Awake()
    {
		animator = gameObject.GetComponent<Animator>();

		myCharacterSprite = gameObject.GetComponent<SpriteRenderer>();

        RepairShowObject = GameObject.Find("RepairPanel").GetComponent<RepairObject>();       
	}

    protected virtual void OnEnable() { }

	protected virtual void OnDisable()
    {
        Init();
    }

    //배치 될 경우 데이터를 넣어줌 (몇 번째 얘인지, 이 아르바이트에 원래 있던 위치, 아르바이트 데이터, 애니메이터)
    public void GetArbaitData(int _nIndex ,GameObject _obj,ArbaitData _data) 
    {
        buff.Clear();

        nIndex = _nIndex;

        arbaitData = _data;

        ArbaitPanelObject = _obj;

        strBuffsIndex = arbaitData.strBuffIndexs.Split(',');

        for (int nBuffIndex = 0; nBuffIndex < strBuffsIndex.Length; nBuffIndex++)
        {
            Buff stBuff;

            stBuff.nIndex       = System.Convert.ToInt32(strBuffsIndex[nBuffIndex]);
            stBuff.fValue       = arbaitData.fSkillPercent;
			stBuff.fCurrentFloat = arbaitData.fCurrentFloat;
            stBuff.strBuffExplain = arbaitData.strExplains;

            buff.Add(stBuff);
        }


        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/" + _data.strAnimation);
    }

    //무기 받음
    public void GetWeaponData(GameObject _obj, CGameWeaponInfo _data, float _fComplate, float _fTemperator)
    {
        //무기를 받았기에 수리로 변경
        bIsRepair = true;

        //현재진행중인 오브젝트에 받은 캐릭터 오브젝트를 넣음 나중에 비교를 위함
        AfootOjbect = _obj;

        //무기 데이터를 넣어줌
        weaponData = _data;

        //완성도를 대입
        m_fComplate = _fComplate;

        m_fTemperator = _fTemperator;

        m_fMaxComplate = _data.fComplate;

        E_STATE = E_ArbaitState.E_REPAIR;
    }

    protected virtual void CheckCharacterState(E_ArbaitState _E_STATE) { }

	protected virtual IEnumerator CharacterAction() { yield return null; }

    //스킬 적용
    protected virtual void ApplySkill() { }

    //스킬 해제
    protected virtual void ReliveSkill() { }


    //무기 수리 완료시 호출
	protected void ComplateWeapon()
    {
        //현재 수리중인 오브젝트와 무기의 완성도를 보내 수리한다.
        SpawnManager.Instance.ComplateCharacter(AfootOjbect,weaponData.fComplate);

        //초기화
		Init();

        //수리중인 무기가없을것이므로 무기를 찾아 넣어준다.
        SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
    }

    //현재 수리중인 무기를 되돌려준다.
    public void ResetWeaponData()
    {

        Init(false);
        //AfootOjbect = _obj;
        SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
    }

	public void Init(bool bIsReturn = false)
    {
		if (bIsReturn == false)
			SpawnManager.Instance.ReturnInsertData (AfootOjbect,true, m_fComplate, m_fTemperator);

        fTime = 0.0f;

        m_fComplate = 0.0f;

        m_fMaxComplate = 0.0f;

        bIsRepair = false;

        bIsComplate = false;

        weaponData = null;
        
        AfootOjbect = null;

        CheckCharacterState(E_ArbaitState.E_WAIT);
    }
}
