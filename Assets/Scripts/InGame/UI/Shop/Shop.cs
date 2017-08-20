using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class Shop : MonoBehaviour {

    public int nEquimentLength;

    private int nShopCount = 0;
    private int nShopMaxLength = 10;

    public Transform parentPanel;
    public GameObject shopButton;

    public Inventory inventory;
    public ShopShowPanel showPanel;

    ShopButton[] ShopList;

    List<CGameEquiment> EquimentList = new List<CGameEquiment>();

    System.DateTime StartDate = new System.DateTime();

    System.DateTime EndData;

    System.TimeSpan timeCal;

    private string strTime ="";

    void Awake()
    {
        ShopList = new ShopButton[nShopMaxLength];

        for(int nIndex = 0; nIndex < nShopMaxLength; nIndex++)
        {
            GameObject obj = Instantiate(shopButton) as GameObject;

            ShopButton shopScript = obj.GetComponent<ShopButton>();

			obj.transform.SetParent(parentPanel,false);

			obj.transform.localScale = Vector3.one;

            ShopList[nIndex] = shopScript;
        }

        nEquimentLength = GameManager.Instance.GetEquimentLength();
    }

    void OnEnable()
    {
        EquimentList = GameManager.Instance.GetEquimentShopData();

        if (EquimentList == null)
            EquimentList = new List<CGameEquiment>();

        if (PlayerPrefs.HasKey("NowTime"))
        {
            strTime = PlayerPrefs.GetString("NowTime");

            StartDate = System.Convert.ToDateTime(strTime);
        }

        EndData = System.DateTime.Now;

        timeCal = EndData - StartDate;

        int nStartTime = StartDate.Hour * 360 + StartDate.Minute * 60 + StartDate.Second;
        int nEndTime = EndData.Hour * 360 + EndData.Minute * 60 + EndData.Second;

		int nCheck = Mathf.Abs(nEndTime - nStartTime);

        //1시간이 지났거나 하루차이가 있을 경우
		if(timeCal.Days != 0 || nCheck >= 1)
        {
            PlayerPrefs.SetString("NowTime", EndData.ToString());

            EquimentList.Clear();

            for (int nIndex = 0; nIndex < 3; nIndex++)
            {
                CGameEquiment cGameEquiment = GetEquiment();

                ShopList[nIndex].GetEquiment(inventory, showPanel, cGameEquiment);

                EquimentList.Add(cGameEquiment);
            }

        }
        else
        {
			if (EquimentList != null)
            {
                foreach(CGameEquiment equit in EquimentList )
                {
                    ShopList[nShopCount++].GetEquiment(inventory, showPanel,equit);
                }
            }
            //완전 처음 일 경우 
            else
            {
                for(int nIndex = 0; nIndex < 3; nIndex++)
                {
                    CGameEquiment cGameEquiment = GetEquiment();

                    ShopList[nIndex].GetEquiment(inventory, showPanel,cGameEquiment);

                    EquimentList.Add(cGameEquiment);
                }
            }
        }

        GameManager.Instance.SaveShopList(EquimentList);
    }

    private CGameEquiment GetEquiment()
    {
        CGameEquiment resultEquiment = new CGameEquiment();


        CGameEquiment getEquiment = GameManager.Instance.GetEquimentData(Random.Range(0, nEquimentLength-1));

        if (getEquiment == null)
            return GetEquiment();

        resultEquiment.nIndex = getEquiment.nIndex;
		resultEquiment.sGrade = getEquiment.sGrade;
        resultEquiment.strName = getEquiment.strName;
        resultEquiment.nSlotIndex = getEquiment.nSlotIndex;
        resultEquiment.strResource = getEquiment.strResource;

		int nLength = GetGradeAmount( resultEquiment.sGrade);

        int nInsertIndex = 0;

        while(nLength > 0)
        {
            nInsertIndex = Random.Range((int)E_Equiment.E_REPAIR, (int)E_Equiment.E_MAX);

            if (CheckData(resultEquiment, nInsertIndex))
                nLength--;
        }

        return resultEquiment;
    }

    private bool CheckData(CGameEquiment _equiment, int nIndex)
    {
        switch(nIndex)
        {
            case (int)E_Equiment.E_REPAIR:
                if (_equiment.fReapirPower == 0)
                {
                    _equiment.fReapirPower = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_ARBAIT:
                if (_equiment.fArbaitRepair == 0)
                {
                    _equiment.fArbaitRepair = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_HONOR:
                if (_equiment.fHonorPlus == 0)
                {
                    _equiment.fHonorPlus = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_GOLD:
                if (_equiment.fGoldPlus == 0)
                {
                    _equiment.fGoldPlus = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_WATERCHARGE:
                if (_equiment.fWaterChargePlus == 0)
                {
                    _equiment.fWaterChargePlus = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_CRITICAL:
                if (_equiment.fCritical == 0)
                {
                    _equiment.fCritical = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_CRITICALD:
                if (_equiment.fCriticalDamage == 0)
                {
                    _equiment.fCriticalDamage = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_BIGCRITICAL:
                if (_equiment.fBigCritical == 0)
                {
                    _equiment.fBigCritical = 5;
                    return true;
                }
                break;
            case (int)E_Equiment.E_ACCURACY:
                if (_equiment.fAccuracyRate == 0)
                {
                    _equiment.fAccuracyRate = 5;
                    return true;
                }
                break;
        }

        return false;
    }

	public int GetGradeAmount(string strGrade)
	{
		switch (strGrade) 
		{
		case "C":
			return 1;
		case "B":
			return 2;
		case "A":
			return 3;
		case "S":
			return 3;
		}

		return 0;
	}

    //void OnDisable()
    //{
    //    if (EquimentList == null)
    //        return;

    //    GameManager.Instance.SaveEquiment(EquimentList);
    //}
}
