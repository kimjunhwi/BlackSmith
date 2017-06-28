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

    public Transform[] m_BatchPosition;

    public GameObject[] m_BatchArbait;      //배치 아르바이트

    public ArbaitBatch[] array_ArbaitData;  //아르바이트 데이터 캐싱

    public ArrayList array_Charic;

	public bool bIsBossCreate = false;

    private float m_fCreateTime;            //몬스터 생성시간에 도달하면 몬스터 생성되는시간
    private float m_fLevelTime;             //다음 레벨 시간에 도달하게 하는 시간

    private float m_nNextLevelTime = 10;    //다음 레벨로 진행되는시간

    private CGameWeaponInfo cLevelData;         //Level에 따른 데이터를 받아오기 위함

    private void Start()
    {
        m_nMaxArbaitAmount = GameManager.Instance.ArbaitLength();

		array_ArbaitData = new ArbaitBatch[m_nMaxArbaitAmount];
		m_BatchArbait = new GameObject[m_nMaxArbaitAmount];

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
        for (int i = 0; i < m_nMaxPollAmount; i++)
        {
            m_CharicPool[i] = Instantiate(m_CharicPool[i]);
            m_CharicPool[i].SetActive(false);
        }
		 
		for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
        {
            //화면에 보이는 배치 오브젝트
			m_BatchArbait[nIndex] = Instantiate(m_ArbaitData);

			Factory (nIndex, m_BatchArbait [nIndex]);

			m_BatchArbait[nIndex].SetActive(false);
        }
    }

	//배치 되는 아르바이트의 컴포넌트를 추가하고 그 캐릭터를 생성하는 패널을 추가한다.
	public void Factory(int nIndex, GameObject _obj)
	{
		switch (nIndex) {
		case (int)E_ARBAIT.E_BLUEHAIR: 	_obj.AddComponent<BlueHair> (); break;
		case (int)E_ARBAIT.E_REDHAIR:	_obj.AddComponent<BlueHair> (); break;
		case (int)E_ARBAIT.E_NURSE: _obj.AddComponent<BlueHair> (); break; 
		}

		array_ArbaitData [nIndex] = _obj.GetComponent<ArbaitBatch> ();

		_obj.GetComponent<ArbaitBatch> ().arbaitData = GameManager.Instance.GetArbaitData (nIndex);

		GameObject createArbaitUI = Instantiate (ArbaitPanel);

		createArbaitUI.GetComponent<ArbaitCharacter> ().SetUp (nIndex,arbaitSprite[nIndex]);

		createArbaitUI.transform.SetParent (contentsPanel, false);
	}

    //WeaponData
    #region

    public void DeleteObject(GameObject _obj)
    {
        int nSearchIndex = 0;

		//for (int nIndex = 0; nIndex < m_BatchArbait.Length; nIndex++)
        //{
            //만약 배치된 아르바이트 중에 같은 무기가 있을경우 그 무기를 초기화 시켜줌
        //    if (array_ArbaitData[nIndex].AfootOjbect == _obj)
        //    {
        //        nSearchIndex = nIndex;
        //        array_ArbaitData[nIndex].ResetWeaponData();
        //        break;
        //    }
        //}

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
    public void ReturnInsertData(GameObject obj,bool bIsRepair, float _fComplate,float _fTemperator)
    {
        GameObject tempObject = null;

        tempObject = SearchCharacter(obj);

        if (tempObject)
            tempObject.GetComponent<NormalCharacter>().GetRepairData(bIsRepair, _fComplate, _fTemperator);
    }

    public void ComplateCharacter(GameObject _obj,float fComplate)
    {
        GameObject tempObject = null;

        tempObject = SearchCharacter(_obj);

        if (tempObject)
			tempObject.GetComponent<NormalCharacter>().Complate(fComplate);

    }

    void InsertSortObject()
    {
        bool bIsNull = false;
        int nNullIndex = 0;
        GameObject tempObject = null;

        for(int i = 0; i< m_nMaxPollAmount; i++)
        {
            if(list_Character[i] != null && bIsNull == true)
            {
                tempObject = list_Character[i];
                list_Character[i] = list_Character[nNullIndex];
                list_Character[nNullIndex] = tempObject;
                bIsNull = false;
                i = nNullIndex;
            }

            if(list_Character[i] == null && bIsNull == false)
            {
                nNullIndex = i;
                bIsNull = true;
            }
        }
    }

	public void AllCharacterComplate()
	{
		bIsBossCreate = true;

		for(int nIndex = 0; nIndex < list_Character.Count; nIndex++)
			list_Character[nIndex].GetComponent<NormalCharacter>().m_bIsBack = true;
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

        //어떤 몬스터를 생성할지에 대한 범위를 위한 변수
        int nCreateRange = 1;

        bool bIsCreateCharacter = false;

        ////만약 LFMonster나 Lerp몬스터가 있을경우 ++해줌 
        //nCreateRange = (m_nCreateLFAmount > 0) ? nCreateRange + 1 : nCreateRange;
        //nCreateRange = (m_nCreateLerpAmount > 0) ? ++nCreateRange + 1 : nCreateRange;

        //0이상 nCreateRange미만
        nSelectCharacter = Random.Range(0, nCreateRange);

        switch (nSelectCharacter)
        {
            case (int)CharacterType.StreightMonster:
                bIsCreateCharacter = IsCreateCharacter(5, m_CharicPool);
                break;
            //case (int)CharacterType.LFMonster:
            //    bIsCreateMonster = IsCreateMonster(m_nCreateLFAmount, m_LFPool);
            //    break;
            //case (int)CharacterType.LerpMonster:
            //    bIsCreateMonster = IsCreateMonster(m_nCreateLerpAmount, m_LerpPool);
            //    break;

            default: Debug.Log("캐릭터 생성이 잘못됨!!"); break;
        }

        //몬스터가 생성됐을 경우에 m_fCreateTime을 0으로 해줌
        if (bIsCreateCharacter)
            m_fCreateTime = 0;

    }

    private bool IsCreateCharacter(int _nMaxPool, GameObject[] objData)
    {
        for (int i = 0; i < _nMaxPool; i++)
        {
            if (objData[i].activeSelf == false)
            {
                objData[i].SetActive(true);
                
                InsertCharacter(objData[i]);
                return true;
            }
        }

        return false;
    }

    #endregion 

    //ArbaitData
    #region

    ////////////////////////////////아르바이트 추가가 가능한지 부분 함수 

    //만약 넣었을 경우 실행하는 함수 한번에 하지 않은 이유는 만약 넣을 수 없는데
    //현재 이 함수의 인자 값을 전부 복사해서 확인하면 부하가 커질거 같기 때문이다.
    public int AddArbaitCheck()
    {
		for (int nIndex = 0; nIndex < m_nMaxArbaitAmount; nIndex++)
        {
            if (m_BatchArbait[nIndex].activeSelf == false)
            {
                return nIndex;
            }
        }
        return (int)E_CHECK.E_FAIL;
    }

    //아르바이트 추가
    public bool AddArbait(int _nIndex,GameObject _obj, ArbaitData _data)
    {
		for (int nIndex = 0; nIndex < m_nMaxArbaitAmount; nIndex++)
        {
            if (m_BatchArbait[nIndex].activeSelf == false)
            {
				
                array_ArbaitData[nIndex].GetArbaitData(nIndex,_obj, _data);
                m_BatchArbait[nIndex].transform.position = m_BatchPosition[nIndex].position;
                m_BatchArbait[nIndex].SetActive(true);
                return true;
            }
        }

        return false;
    }

    ///////// 무기에서 아르바이트 를 넣을 수 있는지 판별 하는 함수 부분

    //인자로 넣은 값을 넣을 수 있는지 없는지를 확인한다.
    public int InsertArbatiWeaponCheck(int _nGrade)
    {
		for (int nIndex = 0; nIndex <  m_nMaxArbaitAmount; nIndex++)
        {
            if (m_BatchArbait[nIndex].activeSelf)
            {
                if ((array_ArbaitData[nIndex].nGrade <= _nGrade) 
                    && (array_ArbaitData[nIndex].bIsRepair == false))
                    return nIndex;
                
            }
        }
            return (int)E_CHECK.E_FAIL;
    }

    //만약 넣었을 경우 실행하는 함수 한번에 하지 않은 이유는 만약 넣을 수 없는데
    //현재 이 함수의 인자 값을 전부 복사해서 확인하면 부하가 커질거 같기 때문이다.
    public void InsertArbaitWeapon(int nIndex, GameObject _obj, CGameWeaponInfo _data, float _fComplate, float _fTemperator)
    {
        array_ArbaitData[nIndex].GetWeaponData(_obj, _data, _fComplate, _fTemperator);
    }
    ////////////////////////////////////////////////////////////////

    public void InsertWeaponArbait(int _nIndex, int _nGrade)
    {
		NormalCharacter charData;

        foreach(GameObject _obj in list_Character)
        {
			charData = _obj.GetComponent<NormalCharacter>();

            if (charData.weaponData.nGrade <= _nGrade && charData.m_bIsRepair == false && charData.E_STATE == Character.ENORMAL_STATE.WAIT)
            {
                charData.m_bIsRepair = true;

				charData.SpeechSelect (_nIndex);

                array_ArbaitData[_nIndex].GetWeaponData(_obj, charData.weaponData, charData.m_fComplate, charData.m_fTemperator);
                break;
            }
        }
    }

    public void DeleteArbait(GameObject _obj)
    {
        int nIndex = 0;

        foreach(GameObject obj in m_BatchArbait)
        {
            if(obj.activeSelf)
            {
                if(array_ArbaitData[nIndex].ArbaitPanelObject == _obj)
                {
                    array_ArbaitData[nIndex].Init();
                    obj.SetActive(false);
                    break;
                }
            }

            nIndex++;
        }
    }

    #endregion
}



 