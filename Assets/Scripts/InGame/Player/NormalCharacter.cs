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
	public int m_nIndex = -1;

	//도착 지점
	private Vector3 m_VecEndPos;

	//활성화 됐을때 위치
	private Vector3 m_VecStartPos;
   
	//무기가 보일 말풍선(?) // 미정
	private GameObject WeaponBackground;
	private GameObject TimerGameobject;

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
	public bool m_bIsArrival = false;

	public Sprite[] TimerSprites;

	public SpriteRenderer Timer;

	private int nTimerCount = 0;
	private float fNextTimer = 0.0f;
	private const int nMaxTimerCount = 10;

	private int nDay = 1;

	public override void Awake()
    {
        base.Awake();


		boxCollider = gameObject.GetComponent<BoxCollider2D>();

		WeaponBackground = transform.FindChild("Case").gameObject;

		TimerGameobject = transform.FindChild ("Timer").gameObject;

		backGround = WeaponBackground.GetComponent<SpriteRenderer> ();

		m_VecEndPos = GameObject.Find("EndPoint").transform.position;

		spawnTransform = GameObject.Find("SpawnPoint").gameObject.transform;

		RepairShowObject = GameObject.Find("RepairPanel").GetComponentInChildren<RepairObject>();

		weaponsSprite = WeaponBackground.transform.FindChild("WeaponSprite").GetComponent<SpriteRenderer>();

		ComplateScale = WeaponBackground.transform.FindChild ("ComplateGaugeParent").GetComponent<Transform> ();

        m_VecStartPos = spawnTransform.position;

		ComplateScale.localScale = new Vector3 (1.0f, 0, 1.0f);
    }

    //활성화 됐을 때 초기화
	void OnEnable()
    {
		backGround.sprite = NoneSpeech;

        mySprite.flipX = true;

        m_bIsRepair = false;

		m_bIsFirstBack = false;

        mySprite.sortingOrder = (int)E_SortingSprite.E_WALK;

		fSpeed = 1.0f;

        m_nIndex = -1;

        m_fCharacterTime = 0.0f;

        E_STATE = ENORMAL_STATE.WALK;

        transform.position = m_VecStartPos;

        weaponData = GameManager.Instance.GetWeaponData((int)E_GRADE);

		if (weaponData == null)
			return;

		//무기 데이터가 있을 경우 날짜를 받아와 적용시켜 준다
		//수리력 3% 나머지 1%
		else 
		{
			nDay = cPlayerData.GetDay ();

			weaponData.fMaxComplate = weaponData.fMaxComplate + (weaponData.fMaxComplate * nDay * 0.03f);
			weaponData.fMinusRepair = weaponData.fMinusRepair + (weaponData.fMinusRepair * nDay * 0.01f);
			weaponData.fMinusChargingWater = weaponData.fMinusChargingWater + (weaponData.fMinusChargingWater * nDay * 0.01f);
			weaponData.fMinusCriticalDamage = weaponData.fMinusCriticalDamage + (weaponData.fMinusCriticalDamage * nDay * 0.01f);
			weaponData.fMinusUseWater = weaponData.fMinusUseWater + (weaponData.fMinusUseWater * nDay * 0.01f);
			weaponData.fMinusCriticalChance = weaponData.fMinusCriticalChance + (weaponData.fMinusCriticalChance * nDay * 0.01f);
			weaponData.fMinusAccuracy = weaponData.fMinusAccuracy + (weaponData.fMinusAccuracy * nDay * 0.01f);
			weaponData.fGold = weaponData.fGold + (weaponData.fGold * nDay * 0.01f);
			weaponData.fHonor = weaponData.fHonor + (weaponData.fHonor * nDay * 0.01f);
		}

		//다음 시간 저장 
		fNextTimer += (float)(weaponData.fLimitedTime * 0.125);

		//이미지를 넣어줌
		Timer.sprite = TimerSprites [nTimerCount++];


        m_fComplate = 0;

        m_fTemperator = 0;

        m_fCharacterWaitTime = weaponData.fLimitedTime;

        weaponsSprite.sprite = weaponData.WeaponSprite;

        boxCollider.isTrigger = false;
        
        WeaponBackground.SetActive(false);

		TimerGameobject.SetActive (false);

		ComplateScale.localScale = new Vector3(1.0f, 0.0f , 1.0f);
    }

    void OnDisable()
    {
        m_bIsBack = false;

        m_bIsFirst = false;

		m_bIsRepair = false;

        m_bIsFirstBack = false;

        m_bIsArrival = false;

        m_fComplate = 0;

        m_fTemperator = 0;

		fNextTimer = 0.0f;

		nTimerCount = 0;

        m_fCharacterTime = 0.0f;

        weaponsSprite.sprite = null;

        WeaponBackground.SetActive(false);

		TimerGameobject.SetActive (false);

        m_VecMoveDistance = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        m_fCharacterTime += Time.deltaTime;

		if (m_fCharacterTime > fNextTimer && nTimerCount < nMaxTimerCount) 
		{
			//다음 시간 저장 
			fNextTimer += (float)(weaponData.fLimitedTime * 0.125);

			//이미지를 넣어줌
			Timer.sprite  = TimerSprites [nTimerCount++];
		}

        StartCoroutine(this.CheckCharacterState());

        StartCoroutine(this.CharacterAction());
    }

    IEnumerator CheckCharacterState()
    {
        yield return new WaitForSeconds(0.3f);

        //시간이 지났거나 m_bIsBack이 트루일경우 돌려보냄
		if (m_fCharacterTime >= m_fCharacterWaitTime || m_bIsBack == true)
			E_STATE = ENORMAL_STATE.BACK;
		
            //시간이 지나지 않았거나 도착하지 않았다면 Walk
		else if(m_fCharacterTime < m_fCharacterWaitTime && m_bIsArrival == false)
			E_STATE = ENORMAL_STATE.WALK;
		
            //그 외에는 대기
		else
			E_STATE = ENORMAL_STATE.WAIT;
    }

    IEnumerator CharacterAction()
    {
		switch (E_STATE) {
		case ENORMAL_STATE.WALK:

			m_anim.SetBool ("bIsWalk", true);

                //Move 함수를 통해 지정된 위치로 자연스럽게 이동하기 위해 MoveTowards 함수를 사용
			transform.position = Vector3.MoveTowards (transform.position, m_VecMoveDistance, fSpeed * Time.deltaTime);

                //만약 도착했다면
			if ((transform.position.x == m_VecMoveDistance.x)) {

                //처음일 경우 true로 바꿔주고 수리 할 수 있는 무기를 보여줌
				if (m_bIsFirst == false) {
					m_bIsFirst = true;

					WeaponBackground.SetActive (true);
					TimerGameobject.SetActive (true);
				}

                //만약 수리중이라면 도착했다는것으로 간주하고 리턴
				if (m_bIsRepair) {
					m_bIsArrival = true;
					yield break;
				}

				//만약 현재 수리중인 오브젝트가 없을 경우  
                //수리 중 및 도착한것으로 바꾸고 현재 무기를 넣어준다.
				if (RepairShowObject.AfootObject == null) {
					m_bIsRepair = true;
					m_bIsArrival = true;

					RepairShowObject.GetWeapon (gameObject, weaponData, m_fComplate, m_fTemperator);

					SpeechSelect ((int)E_SPEECH.E_PLAYER);
					yield break;
				}

                //지정된 위치로 도착했다면
				if (m_bIsArrival == false) {
					
					m_bIsArrival = true;

                    //수리할 수 있는 아르바이트가 있는지 체크한다.
					m_nCheck = SpawnManager.Instance.InsertArbatiWeaponCheck (weaponData.nGrade);

					if (m_nCheck != (int)E_CHECK.E_FAIL) {
						m_bIsRepair = true;

						SpeechSelect (m_nCheck);

						SpawnManager.Instance.InsertArbaitWeapon (m_nCheck, gameObject, weaponData, m_fComplate, m_fTemperator);
					}
				}
			}
			break;

		case ENORMAL_STATE.WAIT:
			m_anim.SetBool ("bIsWalk", false);
			break;

		case ENORMAL_STATE.BACK:

                //딱 한번만 호출 되야 하는 부분
			if (!m_bIsFirstBack) 
			{
				m_bIsFirstBack = true;

				boxCollider.isTrigger = true;

				m_anim.SetBool ("bIsWalk", true);

				//이미지 변경이나 효과 애니메이션 변경등을 진행
				mySprite.flipX = false;

				mySprite.sortingOrder = (int)E_SortingSprite.E_BACK;

                //현재 오브젝트를 보내서 있는지 확인
				RepairShowObject.CheckMyObject (gameObject);

				if(RepairShowObject.AfootObject == null)
					SpawnManager.Instance.AutoInputWeaponData ();

                //현재 캐릭터를 지움
				SpawnManager.Instance.DeleteObject(gameObject);

                //결과값 호출
				Complate (m_fComplate);

				//현재 아르바이트가 수리중인지 확인 
				ArbaitBatch arbait =  SpawnManager.Instance.OverlapArbaitData (gameObject);
                
                //수리 중 이였다면 아르바이트 초기화
				if (arbait != null)
					arbait.ResetWeaponData();
				
				WeaponBackground.SetActive (false);
				TimerGameobject.SetActive (false);

			}

			transform.position = Vector3.MoveTowards (transform.position, m_VecStartPos, fSpeed * Time.deltaTime);

			if (Vector3.Distance (transform.position, m_VecStartPos) < 0.5f) 
			{
				gameObject.SetActive (false);
			}
			break;

		default:
			break;
		}

        yield return null;
    }

    //손님의 이동속도와 뒤로 가는지에 대한 선택을 위함
	public void RetreatCharacter(float _fSpeed,bool _bIsBack)
	{
		fSpeed = _fSpeed;

        m_bIsBack = _bIsBack;
	}

    //지정한 인덱스로 손님을 이동시키기 위함
	public void Move(int _nIndex)
	{
		m_nIndex = _nIndex;

		m_bIsArrival = false;

		float fDistance = 0.8f;

		fDistance *= _nIndex;

		m_VecMoveDistance = new Vector3(m_VecEndPos.x + fDistance, transform.position.y, 0);
	}

	public void GetRepairData(bool _bIsRepair,bool _bIsResearch, float _fComplate, float _fTemperator)
	{
		m_bIsRepair = _bIsRepair;
		m_fComplate = _fComplate;
		m_fTemperator = m_fTemperator;

		if (_bIsResearch) 
		{
			m_nCheck = SpawnManager.Instance.InsertArbatiWeaponCheck (weaponData.nGrade);

			if (m_nCheck != (int)E_CHECK.E_FAIL) {
				m_bIsRepair = true;

				SpeechSelect (m_nCheck);

				SpawnManager.Instance.InsertArbaitWeapon (m_nCheck, gameObject, weaponData, m_fComplate, m_fTemperator);

				return;
			}
		}

		SpeechSelect ((int)E_SPEECH.E_NONE);
	}

    //현재 무기가 누구에게 수리중인지를 지정하기 위함
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
		if(Input.GetMouseButtonDown(0) && (E_STATE == ENORMAL_STATE.WAIT || WeaponBackground.activeSelf))
		{
			//onPointerDown 보다 먼저 호출
			if (!EventSystem.current.IsPointerOverGameObject ()) {

				m_bIsRepair = true;

				//현재 아르바이트가 수리중인지 확인 
				ArbaitBatch arbait =  SpawnManager.Instance.OverlapArbaitData (gameObject);

                if (arbait != null)
                    arbait.ResetWeaponData();

				RepairShowObject.GetWeapon (gameObject, weaponData, m_fComplate, m_fTemperator);

				backGround.sprite = PlayerRepairSpeech;
			}
		}
	}

    //완성도를 체크하고 온도를 저장하기 위한 함수이다.
	public override bool CheckComplate (float _fComplate,float _fTemperator)
	{
		float fCurCompletY;

		m_fComplate = _fComplate;
		m_fTemperator = _fTemperator;

		fCurCompletY = m_fComplate / weaponData.fMaxComplate;

        //만약 완성도가100% 라면
        //뒤로 이동 후 true 반환
		if (fCurCompletY >= 1.0f) {
			m_bIsBack = true;

			ComplateScale.localScale = new Vector3 (ComplateScale.localScale.x, fCurCompletY, ComplateScale.localScale.z);

			return true;
		}

		ComplateScale.localScale = new Vector3 (ComplateScale.localScale.x, fCurCompletY, ComplateScale.localScale.z);
	
		return false;
	}

	public override void Complate(float _fComplate = 0.0f)
	{
		//70%이상
		if ((weaponData.fMaxComplate * 0.7) < _fComplate) {
			fGold = weaponData.fGold + (weaponData.fGold * _fComplate / weaponData.fMaxComplate);

			ScoreManager.ScoreInstance.GoldPlus (fGold);

			cPlayerData.SetSuccessedGuestCount (cPlayerData.GetSuccessedGuestCount () + 1);
            ScoreManager.ScoreInstance.SetSuccessedGuestCount(cPlayerData.GetSuccessedGuestCount());


			//셩공 손님이 30명 이상 이라면 
			if (cPlayerData.GetSuccessedGuestCount () >= 30) 
			{
                //날짜 초기화
                cPlayerData.SetSuccessedGuestCount(0);
                cPlayerData.SetFaieldGuestCount(0);

                ScoreManager.ScoreInstance.SetSuccessedGuestCount(0);
                ScoreManager.ScoreInstance.SetFaieldGuestCount(0);

                //날짜를 1일 추가
                SpawnManager.Instance.SetDayInitInfo (cPlayerData.GetDay () + 1);	
			}
		}

		m_bIsRepair = true;

		//이미지 변경이나 효과 애니메이션 변경등을 진행
		mySprite.flipX = false; 

		mySprite.sortingOrder = (int)E_SortingSprite.E_BACK;

		WeaponBackground.SetActive(false);
		TimerGameobject.SetActive (false);
	}

	public void RepairObjectInputWeapon()
	{
		m_bIsRepair = true;
		m_bIsArrival = true;

		RepairShowObject.GetWeapon (gameObject, weaponData, m_fComplate, m_fTemperator);

		SpeechSelect ((int)E_SPEECH.E_PLAYER);
	}
}
