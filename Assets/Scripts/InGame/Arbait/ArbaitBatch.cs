using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class ArbaitBatch : MonoBehaviour {

    public bool bIsRepair = false;

    public bool bIsComplate = false;

    private int nIndex = 0;

    protected float fTime = 0.0f;
    
    //무기 완성도
    protected float m_fComplate;

    //무기 수리 시간
    protected float m_fRepairTime = 2.0f;

    // 무기 온도
    protected float m_fTemperator;

    //무기 완성 맥스치
    protected float m_fMaxComplate;

    //무기 남은 시간
    protected float m_fCurComplateX;

    //아르바이트 현재 상태
    private E_ArbaitState E_STATE;

    //진행중인 오브젝트
    public GameObject AfootOjbect;

    //아르바이트 패널을 저장
    public GameObject ArbaitPanelObject;

    private Transform ComplateGauge;

    //무기가 보일 말풍선(?) // 미정
    private GameObject WeaponBackground;

    public ArbaitData arbaitData;

    public CGameWeaponInfo weaponData;

    //무기가 보일 말풍선(?) // 미정
    private RepairObject RepairShowObject;

    //무기 이미지
    private SpriteRenderer weaponsSprite;

    private SpriteRenderer myCharacterSprite;

    private BoxCollider2D boxCollider;

    public Animator animator;

    //무기 등급을 어디까지 받아올지를 정하기 위해 사용
    public int nGrade { get; set; }

	// Use this for initialization
    private void Awake()
    {
        RepairShowObject = GameObject.Find("RepairPanel").GetComponent<RepairObject>();

        animator = gameObject.GetComponent<Animator>();

        myCharacterSprite = gameObject.GetComponent<SpriteRenderer>();

        WeaponBackground = transform.FindChild("Case").gameObject;

        weaponsSprite = WeaponBackground.transform.FindChild("WeaponSprite").GetComponent<SpriteRenderer>();

        boxCollider = WeaponBackground.GetComponent<BoxCollider2D>();

        ComplateGauge = transform.FindChild("GaugeParent").transform;

        ComplateGauge.transform.localScale = new Vector3(0, ComplateGauge.transform.localScale.y, ComplateGauge.transform.localScale.z);
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

    void OnDisable()
    {
        Init();
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


    //배치 될 경우 데이터를 넣어줌 (몇 번째 얘인지, 이 아르바이트에 원래 있던 위치, 아르바이트 데이터, 애니메이터)
    public void GetArbaitData(int _nIndex ,GameObject _obj,ArbaitData _data, Animator _animator) 
    {
        nIndex = _nIndex;

        arbaitData = _data;

        animator = _animator;

        ArbaitPanelObject = _obj;
    }

    //무기 받음
    public void GetWeaponData(GameObject _obj, CGameWeaponInfo _data, float _fComplate, float _fTemperator)
    {
        //무기를 받았기에 수리로 변경
        bIsRepair = true;

        //초기화
        m_fCurComplateX = 0.0f;

        //현재진행중인 오브젝트에 받은 캐릭터 오브젝트를 넣음 나중에 비교를 위함
        AfootOjbect = _obj;

        //무기 데이터를 넣어줌
        weaponData = _data;

        //완성도를 대입
        m_fComplate = _fComplate;

        m_fTemperator = _fTemperator;

        m_fMaxComplate = _data.fComplate;

        E_STATE = E_ArbaitState.E_REPAIR;

        weaponsSprite.sprite = _data.WeaponSprite;

        m_fCurComplateX = m_fComplate / m_fMaxComplate;

        ComplateGauge.localScale = new Vector3(m_fCurComplateX, ComplateGauge.transform.localScale.y, ComplateGauge.transform.localScale.z);

    }
	
	// Update is called once per frame
	void Update () 
    {
        StartCoroutine(this.CheckCharacterState());
        StartCoroutine(this.CharacterAction());
	}

    IEnumerator CheckCharacterState()
    {
        yield return new WaitForSeconds(0.3f);

        if (weaponData == null)
            E_STATE = E_ArbaitState.E_WAIT;
        else
            E_STATE = E_ArbaitState.E_REPAIR;
    }

    IEnumerator CharacterAction()
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

    private void ComplateWeapon()
    {
        SpawnManager.Instance.ComplateCharacter(AfootOjbect,weaponData.fComplate);

        Init();

        SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
    }

    public void ResetWeaponData()
    {
        Init();
        //AfootOjbect = _obj;
        SpawnManager.Instance.InsertWeaponArbait(nIndex, nGrade);
    }

    public void Init()
    {
        if (bIsComplate == false && AfootOjbect != null)
			AfootOjbect.GetComponent<NormalCharacter>().GetRepairData(false, m_fComplate, m_fTemperator);

        bIsRepair = false;

        bIsComplate = false;

        AfootOjbect = null;

        ArbaitPanelObject = null;

        weaponData = null;

        fTime = 0.0f;

        m_fComplate = 0.0f;

        m_fMaxComplate = 0.0f;

        m_fCurComplateX = 0.0f;

        weaponsSprite.sprite = null;

        ComplateGauge.localScale = new Vector3(0, ComplateGauge.transform.localScale.y, ComplateGauge.transform.localScale.z);   
    }
}
