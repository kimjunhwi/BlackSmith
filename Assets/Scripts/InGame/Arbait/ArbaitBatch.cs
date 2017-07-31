using ReadOnlys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaitBatch : MonoBehaviour {

	public ArbaitData m_CharacterChangeData;

	protected Player playerData;

    string[] strBuffsIndex;

    //수리중인지
    public bool bIsRepair = false;

    //완료 됐는가
    public bool bIsComplate = false;

	public string strPath;

	public int nIndex = -1;

	public int nBatchIndex = -1;

    protected float fTime = 0.0f;

	protected float fBuffTime = 0.0f;
    
    //무기 완성도
    protected float m_fComplate;

    //무기 수리 시간
    protected float m_fRepairTime;

    // 무기 온도
    protected float m_fTemperator;

    //무기 완성 맥스치
    protected float m_fMaxComplate;

    //아르바이트 현재 상태
	public E_ArbaitState E_STATE;

    //진행중인 오브젝트
    public GameObject AfootOjbect;

    //아르바이트 패널을 저장
    public GameObject ArbaitPanelObject;

    public CGameWeaponInfo weaponData;

    //무기가 보일 말풍선(?) // 미정
	protected RepairObject RepairShowObject;

    //캐릭터 스프라이트
	protected SpriteRenderer myCharacterSprite;

	protected BoxCollider2D boxCollider;

	protected Animator animator;

    private bool m_bIsWaterAttackSpeed = false;
    private bool m_bIsWaterRepairPower = false;
    private bool m_bIsWaterCritical = false;

    private float m_fWaterAttackSpeedValue = 0.0f;
    private float m_fWaterRepairPowerValue = 0.0f;
    private float m_fWaterCriticalValue = 0.0f;

    //private float m_fWaterAttackSpeedTime = 0.0f;
    //private float m_fWaterRepairPowerTime = 0.0f;
    //private float m_fWaterCriticalTime = 0.0f;

    private float m_fWaterAttackSpeedPlusTime = 0.0f;
    private float m_fWaterRepairPowerPlusTime = 0.0f;
    private float m_fWaterCriticalPlusTime = 0.0f;

	//수리력 감소 체크 , 감소 되는 량 
	protected float fRepairDownPercent = 1;
	protected float fAttackSpeedDownPercent = 1;



    //무기 등급을 어디까지 받아올지를 정하기 위해 사용
    public int nGrade { get; set; }

	// Use this for initialization
	protected virtual void Awake()
    {
		animator = gameObject.GetComponent<Animator>();

		myCharacterSprite = gameObject.GetComponent<SpriteRenderer>();

		RepairShowObject = GameObject.Find("TouchPad").GetComponent<RepairObject>();

		animator = GetComponent<Animator> ();

		playerData = GameManager.Instance.player;
	}

	protected virtual void Update()
	{

	}

    protected virtual void OnEnable() { }

	protected virtual void OnDisable()
    {
        //만약 각 버프들이 활성화 중이라면 false로 바꾼후 수치를 원래대로 바꾼다.
        #region Disable Check Active Buff
        if (m_bIsWaterAttackSpeed)
        {
            m_bIsWaterAttackSpeed = false;
            m_CharacterChangeData.fAttackSpeed += m_fWaterAttackSpeedValue;
        }

        if (m_bIsWaterRepairPower)
        {
            m_bIsWaterRepairPower = false;
            m_CharacterChangeData.fRepairPower -= m_fWaterRepairPowerValue;
        }

        if (m_bIsWaterCritical)
        {
             m_bIsWaterCritical = false;
             m_CharacterChangeData.fCritical += m_fWaterCriticalValue;
        }

        if (m_bIsSmithBuffAccuracy)
        {
            m_bIsSmithBuffAccuracy = false;
			m_CharacterChangeData.fAccuracyRate += m_fArbaitAccuracyValue;
        }

        if (m_bIsSmithCriticalAttackSpeed)
        {
            m_bIsSmithCriticalAttackSpeed = false;
            m_CharacterChangeData.fAttackSpeed += m_fSmithCriticalAttackSpeedValue;
        }
        #endregion

        Init();

		nBatchIndex = -1;
    }

    //배치 될 경우 데이터를 넣어줌 (몇 번째 얘인지, 이 아르바이트에 원래 있던 위치, 아르바이트 데이터, 애니메이터)
	public void GetArbaitData(int _nIndex ,GameObject _obj,ArbaitData _data) 
    {
		nBatchIndex = _nIndex;

		m_CharacterChangeData = _data;

        ArbaitPanelObject = _obj;

        m_fRepairTime = m_CharacterChangeData.fAttackSpeed;
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

		m_fMaxComplate = _data.fMaxComplate;

        E_STATE = E_ArbaitState.E_REPAIR;
    }

    //캐릭터 스테이트가 바뀌었을때 초기화를 위함
	public virtual void CheckCharacterState(E_ArbaitState _E_STATE) { }

    //캐릭터 동작 부분
	protected virtual IEnumerator CharacterAction() { yield return null; }

    //스킬 적용
    public virtual void ApplySkill() { }

    //스킬 해제
    protected virtual void ReliveSkill() { }

    //물 사용 했을 때 버프를 적용시키기 위함
    //만약 버프가 활성화 중이라면 경과시간을 0초로 바꿔줌

    #region if Use Water, Apply and Relive Buff

    public IEnumerator ApplyWaterBuffAttackSpeed(float _fValue, float _fTime) 
	{
        //만약 활성화 중이라면 0초로 바꿔줌
        if (m_bIsWaterAttackSpeed)
            m_fWaterAttackSpeedPlusTime = 0.0f;

        //비활성화 중이라면 코룬틴으올 동작시킴
        else
            yield return StartCoroutine(ReliveWaterBuffAttackSpeed(_fValue, _fTime));
	}

    public IEnumerator ReliveWaterBuffAttackSpeed(float _fValue, float _fTime)
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsWaterAttackSpeed = true;

        m_fWaterAttackSpeedValue = m_CharacterChangeData.fAttackSpeed * (_fValue * 0.01f);

        m_CharacterChangeData.fAttackSpeed -= m_fWaterAttackSpeedValue;

        while(true)
        {
            yield return null;

            m_fWaterAttackSpeedPlusTime += Time.deltaTime;

            if (m_fWaterAttackSpeedPlusTime > _fTime)
                break;
        }

        Debug.Log("Remove");

		if (!m_bIsWaterAttackSpeed)
			yield break;

        m_bIsWaterAttackSpeed = false;
        m_CharacterChangeData.fAttackSpeed += m_fWaterAttackSpeedValue;
    }

    public IEnumerator ApplyWaterBuffRepairPower(float _fValue, float _fTime) 
	{
        if (m_bIsWaterRepairPower)
            m_fWaterRepairPowerPlusTime = 0.0f;

        else
            yield return StartCoroutine(ReliveWaterBuffRepairPower(_fValue, _fTime));
	}

    public IEnumerator ReliveWaterBuffRepairPower(float _fValue, float _fTime)
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsWaterRepairPower = true;

        m_fWaterRepairPowerValue = m_CharacterChangeData.fRepairPower * (_fValue * 0.01f);

        m_CharacterChangeData.fRepairPower += m_fWaterRepairPowerValue;

		Debug.Log ("RepairPower" + m_CharacterChangeData.fRepairPower);

        while (true)
        {
            yield return null;

            m_fWaterRepairPowerPlusTime += Time.deltaTime;

            if (m_fWaterRepairPowerPlusTime > _fTime)
                break;
        }

		if (!m_bIsWaterRepairPower)
			yield break;

        m_bIsWaterRepairPower = false;
        m_CharacterChangeData.fRepairPower -= m_fWaterRepairPowerValue;
    }

    public IEnumerator ApplyWaterBuffCritical(float _fValue, float _fTime) 
	{
        if (m_bIsWaterCritical)
            m_fWaterCriticalPlusTime = 0.0f;

        else
            yield return StartCoroutine(ReliveWaterBuffBuffCritical(_fValue, _fTime));
	}

    public IEnumerator ReliveWaterBuffBuffCritical(float _fValue, float _fTime)
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsWaterCritical = true;

        m_fWaterCriticalValue = m_CharacterChangeData.fCritical * (_fValue * 0.01f);

        m_CharacterChangeData.fCritical += m_fWaterCriticalValue;

        while (true)
        {
            yield return null;

            m_fWaterCriticalPlusTime += Time.deltaTime;

            if (m_fWaterCriticalPlusTime > _fTime)
                break;
        }

		if (!m_bIsWaterCritical)
			yield break;

        m_bIsWaterCritical = false;
        m_CharacterChangeData.fCritical += m_fWaterCriticalValue;
    }

    #endregion

	//플레이어 크리시 플레이어 
    #region if Smith Critical, Apply and Relive Arbait Accuracy Buff

    private bool m_bIsSmithBuffAccuracy = false;

    private float m_fSmithAccuracyPlusTime = 0.0f;
	private float m_fArbaitAccuracyValue = 0.0f;


    public IEnumerator ApplySmithCriticalBuffAccuracy(float _fValue, float _fTime)
    {
        //만약 활성화 중이라면 0초로 바꿔줌
        if (m_bIsSmithBuffAccuracy)
            m_fSmithAccuracyPlusTime = 0.0f;

        //비활성화 중이라면 코룬틴으올 동작시킴
        else
            yield return StartCoroutine(ReliveSmithCriticalAccuracy(_fValue, _fTime));
    }

    public IEnumerator ReliveSmithCriticalAccuracy(float _fValue, float _fTime)
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsSmithBuffAccuracy = true;

		m_fArbaitAccuracyValue = m_CharacterChangeData.fAccuracyRate * (_fValue * 0.01f);

		m_CharacterChangeData.fAccuracyRate += m_fArbaitAccuracyValue;

        while (true)
        {
            yield return null;

            m_fSmithAccuracyPlusTime += Time.deltaTime;

            if (m_fSmithAccuracyPlusTime > _fTime)
                break;
        }

		if (!m_bIsSmithBuffAccuracy)
			yield break;

        m_bIsSmithBuffAccuracy = false;
		m_CharacterChangeData.fAccuracyRate -= m_fArbaitAccuracyValue;
    }

    #endregion

	//플레이어 크리시 공격속도 증가 
    #region if Smith Critical, Apply and Relive AttackSpeed Buff

    private bool m_bIsSmithCriticalAttackSpeed = false;

    private float m_fSmithCriticalAttackSpeedPlusTime = 0.0f;
    private float m_fSmithCriticalAttackSpeedValue = 0.0f;


    public IEnumerator ApplySmithCriticalBuffAttackSpeed(float _fValue, float _fTime)
    {
        //만약 활성화 중이라면 0초로 바꿔줌
        if (m_bIsSmithCriticalAttackSpeed)
            m_fSmithCriticalAttackSpeedPlusTime = 0.0f;

        //비활성화 중이라면 코룬틴으올 동작시킴
        else
            yield return StartCoroutine(ReliveWaterBuffAttackSpeed(_fValue, _fTime));

        m_fSmithCriticalAttackSpeedPlusTime = 0.0f;
    }

    public IEnumerator ReliveSmithCriticalAttackSpeed(float _fValue, float _fTime)
    {
        yield return new WaitForSeconds(0.1f);

        m_bIsSmithCriticalAttackSpeed = true;


        m_fSmithCriticalAttackSpeedValue = m_CharacterChangeData.fAttackSpeed * (_fValue * 0.01f);

        m_CharacterChangeData.fAttackSpeed -= m_fSmithCriticalAttackSpeedValue;

        while (true)
        {
            yield return null;

            m_fSmithCriticalAttackSpeedPlusTime += Time.deltaTime;

            if (m_fSmithCriticalAttackSpeedPlusTime > _fTime)
                break;
        }

		if (!m_bIsSmithCriticalAttackSpeed)
			yield break;

        m_bIsSmithCriticalAttackSpeed = false;
        m_CharacterChangeData.fAttackSpeed += m_fSmithCriticalAttackSpeedValue;
    }

    #endregion

	//플레이어 크리시 아르바이트들의 공격속도 증가 
	#region if Smith Critical, Apply and Relive Arbait AttackSpeed Buff

	private bool m_bIsCriticalArbaitAttackSpeed = false;

	private float m_fCriticalArbaitAttackSpeedPlusTime = 0.0f;
	private float m_fCriticalArbaitAttackSpeedValue = 0.0f;


	public IEnumerator ApplyCriticalArbaitBuffAttackSpeed(float _fValue, float _fTime)
	{
		//만약 활성화 중이라면 0초로 바꿔줌
		if (m_bIsCriticalArbaitAttackSpeed)
			m_fCriticalArbaitAttackSpeedPlusTime = 0.0f;

		//비활성화 중이라면 코룬틴으올 동작시킴
		else
			yield return StartCoroutine(ReliveWaterBuffAttackSpeed(_fValue, _fTime));

		m_fCriticalArbaitAttackSpeedPlusTime = 0.0f;
	}

	public IEnumerator ReliveCriticalArbaitAttackSpeed(float _fValue, float _fTime)
	{
		yield return new WaitForSeconds(0.1f);

		m_bIsCriticalArbaitAttackSpeed = true;


		m_fCriticalArbaitAttackSpeedValue = m_CharacterChangeData.fAttackSpeed * (_fValue * 0.01f);

		m_CharacterChangeData.fAttackSpeed -= m_fCriticalArbaitAttackSpeedValue;

		while (true)
		{
			yield return null;

			m_fCriticalArbaitAttackSpeedPlusTime += Time.deltaTime;

			if (m_fCriticalArbaitAttackSpeedPlusTime > _fTime)
				break;
		}

		if (!m_bIsCriticalArbaitAttackSpeed)
			yield break;

		m_bIsCriticalArbaitAttackSpeed = false;
		m_CharacterChangeData.fAttackSpeed += m_fCriticalArbaitAttackSpeedValue;
	}

	#endregion


    //무기 수리 완료시 호출
	protected void ComplateWeapon()
    {
        //현재 수리중인 오브젝트와 무기의 완성도를 보내 수리한다.
		SpawnManager.Instance.CheckComplateWeapon (AfootOjbect, m_fComplate, m_fTemperator);

        //초기화
		fTime = 0.0f;

		bIsRepair = false;

		bIsComplate = false;

		weaponData = null;

		AfootOjbect = null;

		m_fComplate = 0.0f;

		m_fMaxComplate = 0.0f;

		m_fTemperator = 0.0f;

        //수리중인 무기가없을것이므로 무기를 찾아 넣어준다.
		SpawnManager.Instance.InsertWeaponArbait(m_CharacterChangeData.index,nBatchIndex);
    }

    //현재 수리중인 무기를 되돌려준다.
    public void ResetWeaponData()
    {
		fTime = 0.0f;

		bIsRepair = false;

		bIsComplate = false;

		weaponData = null;
			
		SpawnManager.Instance.CheckComplateWeapon (AfootOjbect, m_fComplate, m_fTemperator);
		
		m_fComplate = 0.0f;

		m_fMaxComplate = 0.0f;

		m_fTemperator = 0.0f;

		AfootOjbect = null;

        //AfootOjbect = _obj;
		SpawnManager.Instance.InsertWeaponArbait(m_CharacterChangeData.index,nBatchIndex);
    }

	public void Init()
    {
		fTime = 0.0f;

		bIsRepair = false;

		bIsComplate = false;

		weaponData = null;

		if (AfootOjbect != null) 
			SpawnManager.Instance.ReturnInsertData (AfootOjbect,false,true, m_fComplate, m_fTemperator);
		
		m_fComplate = 0.0f;

		m_fMaxComplate = 0.0f;

		m_fTemperator = 0.0f;
    }

	public void SetAttackSpeed(float _fValue)
	{
		m_fRepairTime = m_CharacterChangeData.fAttackSpeed * _fValue;
	}
}
