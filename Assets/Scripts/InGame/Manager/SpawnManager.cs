using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ReadOnlys;

public class SpawnManager : GenericMonoSingleton<SpawnManager>
{
    int m_nMaxArbaitAmount;                  //아르바이트 수

    const int m_nMaxBatchArbaitAmount = 3;  //배치될 아르바이트 최대 개수

    const int m_nMaxPollAmount = 5;         //오브젝트풀로 만들어둘 최대 개수
   
	public Transform contentsPanel;

	public GameObject ArbaitPanel;

	public Sprite[] arbaitSprite;

    public GameObject m_ArbaitData;

    public GameObject[] m_CharicPool;

    public ArrayList m_GuestList = new ArrayList();

    public List<GameObject> list_Character = new List<GameObject>();

	public List<GameObject> list_FreeazeCharacter = new List<GameObject> ();
	public int nRandomIndex;

    public Transform[] m_BatchPosition;

    public GameObject[] m_BatchArbait;      //배치 아르바이트

    public ArbaitBatch[] array_ArbaitData;  //아르바이트 데이터 캐싱

	public bool bIsBossCreate = false;

    private float m_fCreateTime;            //몬스터 생성시간에 도달하면 몬스터 생성되는시간
    private float m_fLevelTime;             //다음 레벨 시간에 도달하게 하는 시간

    private float m_nNextLevelTime = 10;    //다음 레벨로 진행되는시간

	private int m_nCreateArbaitAmount;

    private CGameWeaponInfo cLevelData;         //Level에 따른 데이터를 받아오기 위함

    private void Awake()
    {
        m_nMaxArbaitAmount = GameManager.Instance.ArbaitLength();

		array_ArbaitData = new ArbaitBatch[m_nMaxArbaitAmount];

        //몬스터 풀을 만듬
        CreateMonsterPool();
    }

    private void Update()
    {
        m_fCreateTime += Time.deltaTime;
        m_fLevelTime += Time.deltaTime;

        if (m_fCreateTime >= 4.5f && list_Character.Count < m_nMaxPollAmount &&
			bIsBossCreate == false)
        {
            CreateCharacter();
        }
    }

	public ArbaitBatch OverlapArbaitData(GameObject obj)
    {
		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
        {
            if (m_BatchArbait[nIndex].activeSelf)
                if (array_ArbaitData[nIndex].AfootOjbect == obj)
					return array_ArbaitData[nIndex];
        }
		return null;
    }
    
    //몬스터 오브젝트풀을 생성한다.
    private void CreateMonsterPool()
    {
        for (int i = 0; i < m_CharicPool.Length; i++)
        {
            m_CharicPool[i] = Instantiate(m_CharicPool[i]);
            m_CharicPool[i].SetActive(false);
        }
		 
		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
        {
            //화면에 보이는 배치 오브젝트
			m_BatchArbait[nIndex] = Instantiate(m_BatchArbait[nIndex]);

			//미리 ArbaitBatch를 캐싱해준후 아르바이트 데이터를 넣어줌
			array_ArbaitData [nIndex] = m_BatchArbait[nIndex].GetComponent<ArbaitBatch> ();

			array_ArbaitData[nIndex].m_CharacterChangeData = GameManager.Instance.GetArbaitData(nIndex);

			GameObject createArbaitUI = Instantiate (ArbaitPanel);

			createArbaitUI.GetComponent<ArbaitCharacter> ().SetUp (nIndex,arbaitSprite[nIndex]);

			createArbaitUI.transform.SetParent (contentsPanel, false);

			createArbaitUI.transform.localScale = Vector3.one;

			m_BatchArbait[nIndex].SetActive(false);
        }
    }

    //WeaponData
    #region

    public void DeleteObject(GameObject _obj)
    {
        int nSearchIndex = 0;

        nSearchIndex = list_Character.IndexOf(_obj);

        //캐릭터를 리스트에서 지움
        list_Character.Remove(_obj);

        //재이동시킴
        OrderMove(nSearchIndex);
    }

    public GameObject SearchCharacter(GameObject _obj)
    {
        foreach(GameObject obj in list_Character)
        {
            if (obj == _obj)
                return obj;
        }

        return null;
    }

    //게임 오브젝트를 찾아서 데이터를 넣어줌
	public void ReturnInsertData(GameObject obj,bool bIsRepair,bool bIsResearch, float _fComplate,float _fTemperator)
    {
        GameObject tempObject = null;

        tempObject = SearchCharacter(obj);

        if (tempObject)
			tempObject.GetComponent<NormalCharacter>().GetRepairData(bIsRepair,bIsResearch, _fComplate, _fTemperator);
    }

	public float GetActiveArbaitRepair()
	{
		float fResultValue = 0.0f;

		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++) {

			if (m_BatchArbait [nIndex].activeSelf) {
				fResultValue += array_ArbaitData [nIndex].m_CharacterChangeData.fRepairPower;
			}
		}

		return fResultValue;
	}

    public void ComplateCharacter(GameObject _obj,float fComplate)
    {
        GameObject tempObject = null;

        tempObject = SearchCharacter(_obj);

        if (tempObject)
			tempObject.GetComponent<NormalCharacter>().m_bIsBack = true;

    }

	public bool CheckComplateWeapon(GameObject _obj, float _fComplate,float _fTemperator)
	{
		GameObject tempObject = null;

		tempObject = SearchCharacter(_obj);

		if (tempObject) 
			return tempObject.GetComponent<NormalCharacter> ().CheckComplate (_fComplate,_fTemperator);

		return false;
	}


    //보스 소환시 호출 캐릭터를 이동속도를 3으로 한후 전부 되돌림
	public void AllCharacterComplate()
	{
		bIsBossCreate = true;

		if (list_Character.Count == 0)
			return;

		int nIndex = 0;

		while (true) {
			list_Character [nIndex++].GetComponent<NormalCharacter> ().RetreatCharacter (3.0f, true);

			if (nIndex >= list_Character.Count)
				break;
		}


	}

    //이동
    void OrderMove(int nIndex = 0)
    {
        for (int i = nIndex; i < list_Character.Count; i++)
        {
			list_Character[i].GetComponent<NormalCharacter>().Move(i);
        }
    }

    //캐릭터를 추가함
    void InsertCharacter(GameObject _obj)
    {
        list_Character.Add(_obj);

		_obj.GetComponent<NormalCharacter>().Move(list_Character.Count-1);
    }

    private void CreateCharacter()
    {
        int nSelectCharacter;

        while(true)
        { 
            //범위안에 랜덤으로 선택
            nSelectCharacter = Random.Range(0, m_CharicPool.Length);

            //만약 이미 활성화 돼있다면 뒤로 돌림
            if (m_CharicPool[nSelectCharacter].activeSelf)
                continue;

            //만약 비활성화 상태라면 활성화 시킨후
            m_CharicPool[nSelectCharacter].SetActive(true);

            //손님 리스트에 추가
            InsertCharacter(m_CharicPool[nSelectCharacter]);

            //루프 탈출
            break;
        }
      

        //몬스터가 생성됐을 경우에 m_fCreateTime을 0으로 해줌
        m_fCreateTime = 0;

    }

    #endregion 

    //ArbaitData
    #region

    ////////////////////////////////아르바이트 추가가 가능한지 부분 함수 

    //만약 넣었을 경우 실행하는 함수 한번에 하지 않은 이유는 만약 넣을 수 없는데
    //현재 이 함수의 인자 값을 전부 복사해서 확인하면 부하가 커질거 같기 때문이다.
	public int AddArbaitCheck()
    {
		bool bIsSearch = true;

		for (int nIndex = 0; nIndex < m_nMaxBatchArbaitAmount; nIndex++)
        {
			for (int nSearch = 0; nSearch < array_ArbaitData.Length; nSearch++) 
			{
				if (nIndex == array_ArbaitData [nSearch].nBatchIndex) {
					bIsSearch = false;
					break;
				}
			}

			if (bIsSearch) 
				return nIndex;
			
			else
				bIsSearch = true;
        }

		int nCount = 0;

		for (int nIndex = 0; nIndex <m_BatchArbait.Length; nIndex++) {
			if (m_BatchArbait [nIndex].activeSelf)
				nCount++;
		}

		return (nCount > m_nMaxBatchArbaitAmount) ? (int)E_CHECK.E_FAIL : nCount;
    }

    //아르바이트 추가
    //추가될 캐릭터 인덱스, 배치되는 인덱스,오브젝트,데이터를 추가해준다.
    public bool AddArbait(int _nCharacterIndex, int _nIndex,GameObject _obj, ArbaitData _data)
    {
        if (m_BatchArbait[_nCharacterIndex].activeSelf == false)
        {
			array_ArbaitData[_nCharacterIndex].GetArbaitData(_nIndex, _obj, _data);
            m_BatchArbait[_nCharacterIndex].transform.position = m_BatchPosition[_nIndex].position;
            m_BatchArbait[_nCharacterIndex].SetActive(true);
            return true;
        }

        return false;
    }
    
    //배치된 아르바이트의 인덱스를 가져옴
    public int GetArbaitBatchIndex(int _nIndex)
    {
        return array_ArbaitData[_nIndex].nIndex;
    }

    ///////// 무기에서 아르바이트 를 넣을 수 있는지 판별 하는 함수 부분
    //인자로 넣은 값을 넣을 수 있는지 없는지를 확인한다.
    public int InsertArbatiWeaponCheck(int _nGrade)
    {
		int nMinValue = 10;

		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++) {
			
			if (m_BatchArbait [nIndex].activeSelf) {

				if ((array_ArbaitData [nIndex].nGrade <= _nGrade)
				     && (array_ArbaitData [nIndex].bIsRepair == false)) {

					if (array_ArbaitData [nIndex].nBatchIndex < nMinValue)
						nMinValue = array_ArbaitData [nIndex].nBatchIndex;

					if (nMinValue == 0)
						return nMinValue;
				}

			}
		}

		return (nMinValue > m_nMaxBatchArbaitAmount) ? (int)E_CHECK.E_FAIL : nMinValue;
    }

    //만약 넣었을 경우 실행하는 함수 한번에 하지 않은 이유는 만약 넣을 수 없는데
    //현재 이 함수의 인자 값을 전부 복사해서 확인하면 부하가 커질거 같기 때문이다.
    public void InsertArbaitWeapon(int _nIndex, GameObject _obj, CGameWeaponInfo _data, float _fComplate, float _fTemperator)
    {
		int nIndex;

		for (nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++) 
		{
			if (array_ArbaitData [nIndex].nBatchIndex == _nIndex)
				break;
		}
        array_ArbaitData[nIndex].GetWeaponData(_obj, _data, _fComplate, _fTemperator);
    }
    ////////////////////////////////////////////////////////////////

	public void InsertWeaponArbait(int _nIndex,int _nBatchIndex, int _nGrade)
    {
		NormalCharacter charData;

        foreach(GameObject _obj in list_Character)
        {
			charData = _obj.GetComponent<NormalCharacter>();

			if (charData.weaponData.nGrade <= _nGrade && charData.m_bIsRepair == false && charData.E_STATE == Character.ENORMAL_STATE.WAIT)
            {
                charData.m_bIsRepair = true;

				charData.SpeechSelect (_nBatchIndex);

                array_ArbaitData[_nIndex].GetWeaponData(_obj, charData.weaponData, charData.m_fComplate, charData.m_fTemperator);
                break;
            }
        }
    }

    public void DeleteArbait(GameObject _obj)
    {
		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++) 
		{
			if (m_BatchArbait [nIndex].activeSelf) 
			{
				if (array_ArbaitData [nIndex].ArbaitPanelObject == _obj) {
					m_BatchArbait[nIndex].SetActive(false);
					break;
				}
			}
		}
    }

    public float ActtiveArbaitRepair()
    {
        float fResultValue = 0.0f;

        for(int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
        {
            if(m_BatchArbait[nIndex].activeSelf)
            {
				fResultValue += array_ArbaitData[nIndex].m_CharacterChangeData.fRepairPower;
            }
        }

        return fResultValue;
    }

	//물 사용시 아르바이트중에 물 사용시 버프가 있을 경우 적용
	public void UseWater()
	{
		//ArbaitData arbait;
		float fTime = 0.0f;
		float fValue = 0.0f;


		if (m_BatchArbait [(int)E_ARBAIT.E_CLEA].activeSelf) 
		{
			fTime = array_ArbaitData [(int)E_ARBAIT.E_CLEA].m_CharacterChangeData.fCurrentFloat;
			fValue = array_ArbaitData [(int)E_ARBAIT.E_CLEA].m_CharacterChangeData.fSkillPercent;

            for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
            {
                if(m_BatchArbait[nIndex].activeSelf)
					StartCoroutine(array_ArbaitData[nIndex].ApplyWaterBuffAttackSpeed(fValue,fTime));
            }
		}

		if (m_BatchArbait [(int)E_ARBAIT.E_ROSA].activeSelf) 
        {
			fTime = array_ArbaitData [(int)E_ARBAIT.E_ROSA].m_CharacterChangeData.fCurrentFloat;
			fValue = array_ArbaitData [(int)E_ARBAIT.E_ROSA].m_CharacterChangeData.fSkillPercent;

            for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++) 
				if(m_BatchArbait[nIndex].activeSelf)
					StartCoroutine(array_ArbaitData [nIndex].ApplyWaterBuffRepairPower (fValue,fTime));
        }

		if (m_BatchArbait [(int)E_ARBAIT.E_LUNA].activeSelf) 
        {
			fTime = array_ArbaitData [(int)E_ARBAIT.E_LUNA].m_CharacterChangeData.fCurrentFloat;
			fValue = array_ArbaitData [(int)E_ARBAIT.E_LUNA].m_CharacterChangeData.fSkillPercent;

            for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++) 
				if(m_BatchArbait[nIndex].activeSelf)
					StartCoroutine(array_ArbaitData [nIndex].ApplyWaterBuffCritical (fValue,fTime));
        }
	}

    public void PlayerCritical()
    {

		float fTime = 0.0f;
		float fValue = 0.0f;

		if (m_BatchArbait[(int)E_ARBAIT.E_GLAUS].activeSelf)
		{
			fTime = array_ArbaitData [(int)E_ARBAIT.E_GLAUS].m_CharacterChangeData.fCurrentFloat;
			fValue = array_ArbaitData [(int)E_ARBAIT.E_GLAUS].m_CharacterChangeData.fSkillPercent;

			for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++) 
				if(m_BatchArbait[nIndex].activeSelf)
					StartCoroutine(array_ArbaitData[nIndex].ApplySmithCriticalBuffAccuracy(fValue,fTime));

			array_ArbaitData [(int)E_ARBAIT.E_GLAUS].ApplySkill ();
		}

		if (m_BatchArbait[(int)E_ARBAIT.E_ELLIE].activeSelf)
        {
			fTime = array_ArbaitData [(int)E_ARBAIT.E_ELLIE].m_CharacterChangeData.fCurrentFloat;
			fValue = array_ArbaitData [(int)E_ARBAIT.E_ELLIE].m_CharacterChangeData.fSkillPercent;

			for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++) 
				if(m_BatchArbait[nIndex].activeSelf)
					array_ArbaitData[nIndex].ApplyCriticalArbaitBuffAttackSpeed(fValue,fTime);
        }

		if (m_BatchArbait[(int)E_ARBAIT.E_MICHEAL].activeSelf)
			array_ArbaitData[(int)E_ARBAIT.E_MICHEAL].ApplySkill();
		
    }

	public void ApplyArbaitBossRepair()
	{
		for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++)
			if (m_BatchArbait [nIndex].activeSelf)
				array_ArbaitData [nIndex].CheckCharacterState (E_ArbaitState.E_BOSSREPAIR);
	}

	public void ReliveArbaitBossRepair()
	{
		for (int nIndex = 0; nIndex < array_ArbaitData.Length; nIndex++)
			if (m_BatchArbait [nIndex].activeSelf)
				array_ArbaitData [nIndex].CheckCharacterState (E_ArbaitState.E_WAIT);
	}


	public int FreezeArbait()
	{
		
		list_FreeazeCharacter.Clear ();
		nRandomIndex = -1;
		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++) 
		{
			if (m_BatchArbait [nIndex].activeSelf && array_ArbaitData[nIndex].E_STATE != E_ArbaitState.E_FREEZE) {
				list_FreeazeCharacter.Add (m_BatchArbait [nIndex]);
			}
		}

		if (list_FreeazeCharacter.Count != 0) 
		{
			nRandomIndex = Random.Range (0, list_FreeazeCharacter.Count);
			array_ArbaitData[nRandomIndex].CheckCharacterState (E_ArbaitState.E_FREEZE);

		}
		return nRandomIndex;
	}

	public void DeFreezeArbait(int _nIndex)
	{
		array_ArbaitData[_nIndex].CheckCharacterState (E_ArbaitState.E_WAIT);
	}

    #endregion

}



 