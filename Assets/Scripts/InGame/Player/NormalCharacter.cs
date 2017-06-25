using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;
using UnityEngine.EventSystems;


[System.Serializable]
public class NormalCharacter : Character {
    public Transform spawnTransform;

	private int m_nCheck = -1;

	//캐릭터 인덱스
	private int m_nIndex = -1;

	//도착 지점
	private Vector3 m_VecEndPos;

	//활성화 됐을때 위치
	private Vector3 m_VecStartPos;
   
	//무기가 보일 말풍선(?) // 미정
	private GameObject WeaponBackground;

	private Transform ComplateScale;

	private SpriteRenderer backGround;

	//움직일 이동 거리
	private Vector3 m_VecMoveDistance;

	//돌아가는지
	public bool m_bIsBack = false;

	//뒤로 가는 부분에 처음 부분만 실행하기 위함
	private bool m_bIsFirstBack = false;

	//처음 목표지점에 도달했을 경우에만 아르바이트에게 맞는게 있는지 검사함
	private bool m_bIsFirst = false;

	//캐릭터가 지정한 위치에 도달했는가
	private bool m_bIsArrival = false;

	public override void Awake()
    {
        base.Awake();


		boxCollider = gameObject.GetComponent<BoxCollider2D>();

		WeaponBackground = transform.FindChild("Case").gameObject;

		backGround = WeaponBackground.GetComponent<SpriteRenderer> ();

		m_VecEndPos = GameObject.Find("EndPoint").transform.position;

		spawnTransform = GameObject.Find("SpawnPoint").gameObject.transform;

		RepairShowObject = GameObject.Find("RepairPanel").GetComponent<RepairObject>();

		weaponsSprite = WeaponBackground.transform.FindChild("WeaponSprite").GetComponent<SpriteRenderer>();

		ComplateScale = WeaponBackground.transform.FindChild ("ComplateGaugeParent").GetComponent<Transform> ();

        m_VecStartPos = spawnTransform.position;

		ComplateScale.localScale = new Vector3 (1.0f, 0, 1.0f);
    }

	void OnEnable()
    {
		backGround.sprite = NoneSpeech;

        mySprite.flipX = true;

        m_bIsRepair = false;

        mySprite.sortingOrder = (int)E_SortingSprite.E_WALK;

        m_nIndex = -1;

        m_fCharacterTime = 0.0f;

        E_STATE = ENORMAL_STATE.WALK;

        transform.position = m_VecStartPos;

        weaponData = GameManager.Instance.GetWeaponData((int)E_GRADE);

        m_fComplate = 0;

        m_fTemperator = 0;

        m_fCharacterWaitTime = weaponData.fLimitedTime;

        weaponsSprite.sprite = weaponData.WeaponSprite;

        boxCollider.isTrigger = false;
        
        WeaponBackground.SetActive(false);
    }

    void OnDisable()
    {
        m_bIsBack = false;

        m_bIsFirst = false;

        m_bIsFirstBack = false;

        m_bIsArrival = false;

        m_fComplate = 0;

        m_fTemperator = 0;

        m_fCharacterTime = 0.0f;

        weaponsSprite.sprite = null;

        WeaponBackground.SetActive(false);

        m_VecMoveDistance = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        m_fCharacterTime += Time.deltaTime;

        StartCoroutine(this.CheckCharacterState());

        StartCoroutine(this.CharacterAction());
    }

    IEnumerator CheckCharacterState()
    {
        yield return new WaitForSeconds(0.3f);

        if (m_fCharacterTime < m_fCharacterWaitTime && m_bIsArrival == false)
            E_STATE = ENORMAL_STATE.WALK;

        else if (m_fCharacterTime >= m_fCharacterWaitTime || m_bIsBack == true)
            E_STATE = ENORMAL_STATE.BACK;

        else
            E_STATE = ENORMAL_STATE.WAIT;
    }

    IEnumerator CharacterAction()
    {
		switch (E_STATE) {
		case ENORMAL_STATE.WALK:

			m_anim.SetBool ("bIsWalk", true);

			transform.position = Vector3.MoveTowards (transform.position, m_VecMoveDistance, fSpeed * Time.deltaTime);

			if ((transform.position.x == m_VecMoveDistance.x)) {

				if (m_bIsArrival == false) {
					m_bIsArrival = true;

					m_nCheck = SpawnManager.Instance.InsertArbatiWeaponCheck (weaponData.nGrade);

					if (m_nCheck != (int)E_CHECK.E_FAIL) {
						m_bIsRepair = true;

						SpeechSelect (m_nCheck);

						SpawnManager.Instance.InsertArbaitWeapon (m_nCheck, gameObject, weaponData, m_fComplate, m_fTemperator);
					}
				}

				if (m_bIsFirst == false) {
					m_bIsFirst = true;

					WeaponBackground.SetActive (true);
				}
			}
			break;

		case ENORMAL_STATE.WAIT:
			m_anim.SetBool ("bIsWalk", false);
			break;

		case ENORMAL_STATE.BACK:

			if (!m_bIsFirstBack) {
				m_bIsFirstBack = true;

				boxCollider.isTrigger = true;

				m_anim.SetBool ("bIsWalk", true);

				//이미지 변경이나 효과 애니메이션 변경등을 진행
				mySprite.flipX = false;

				mySprite.sortingOrder = (int)E_SortingSprite.E_BACK;

				RepairShowObject.CheckMyObject (gameObject);

				WeaponBackground.SetActive (false);

				//무기수리 했거나 
				if (m_bIsRepair)
					Complate (m_fComplate);

				//시간이 지났을 경우 
				else if (m_fCharacterTime >= m_fCharacterWaitTime)
					Complate(m_fComplate);
			}
                

			transform.position = Vector3.MoveTowards (transform.position, m_VecStartPos, fSpeed * Time.deltaTime);

			if (Vector3.Distance (transform.position, m_VecStartPos) < 0.5f)
				gameObject.SetActive (false);


			break;

		default:
			break;
		}

        yield return null;
    }

	public void Move(int _nIndex)
	{
		m_nIndex = _nIndex;

		m_bIsArrival = false;

		float fDistance = 0.8f;

		fDistance *= _nIndex;

		m_VecMoveDistance = new Vector3(m_VecEndPos.x + fDistance, transform.position.y, 0);
	}

	public void GetRepairData(bool _bIsRepair, float _fComplate, float _fTemperator)
	{
		m_bIsRepair = _bIsRepair;
		m_fComplate = _fComplate;
		m_fTemperator = m_fTemperator;

		if (m_bIsRepair == false)
			SpeechSelect ((int)E_SPEECH.E_NONE);
	}

	public void SpeechSelect(int _nIndex)
	{
		switch (_nIndex) {
		case (int)E_SPEECH.E_NONE: backGround.sprite = NoneSpeech; break;
		case (int)E_SPEECH.E_ARBAITONE: backGround.sprite = ArbaitOneSpeech; break;
		case (int)E_SPEECH.E_ARBAITTWO: backGround.sprite = ArbaitTwoSpeech; break;
		case (int)E_SPEECH.E_ARBAITTHREE: backGround.sprite = ArbaitThreeSpeech;break;
		case (int)E_SPEECH.E_PLAYER: backGround.sprite = PlayerRepairSpeech; break;
		}
	}

	void OnMouseDown()
	{
		if(Input.GetMouseButtonDown(0) && E_STATE == ENORMAL_STATE.WAIT)
		{
			//onPointerDown 보다 먼저 호출
			if (!EventSystem.current.IsPointerOverGameObject ()) {

				//현재 아르바이트가 수리중인지 확인 
				ArbaitBatch arbait =  SpawnManager.Instance.OverlapArbaitData (gameObject);

				RepairShowObject.GetWeapon (gameObject, weaponData, m_fComplate, m_fTemperator);

				m_bIsRepair = true;

				if (arbait != null) 
					arbait.ResetWeaponData ();
				
				backGround.sprite = PlayerRepairSpeech;
			}
		}
	}

	public override void Complate(float _fComplate = 0.0f)
	{
		//50%이상
		if((weaponData.fComplate * 0.5) < _fComplate)
		{
			fGold = weaponData.fGold + (weaponData.fGold * _fComplate / weaponData.fComplate);

			ScoreManager.ScoreInstance.GoldPlus(fGold);
		}
		//50%이하
		else
		{
			fGold = weaponData.fGold + (weaponData.fGold * _fComplate / weaponData.fComplate);

			ScoreManager.ScoreInstance.GoldPlus(-fGold);
		}

		m_bIsBack = true;

		m_bIsRepair = false;

		//이미지 변경이나 효과 애니메이션 변경등을 진행
		mySprite.flipX = false; 

		mySprite.sortingOrder = (int)E_SortingSprite.E_BACK;

		RepairShowObject.CheckMyObject(gameObject);

		WeaponBackground.SetActive(false);

		SpawnManager.Instance.DeleteObject(gameObject);
	}
}
