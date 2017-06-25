using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public struct stArbait
{
    public int nId;
    public int nLevel;
    public string strName;
    public int nMaxGrade;
    public int nNowGrade;
    public int nBatch;
    public float dRepairPower;
    public float dRepairLevelPlus;

    public stArbait(int _id,int _level, string _strName, int _nNowGrade,int _nMaxGrade, int _nBatch,
        float _dRepairPower, float _dRepairLevelPlus)
    {
        this.nId = _id;
        this.nLevel = _level;
        this.strName = _strName;
        this.nNowGrade = _nNowGrade;
        this.nMaxGrade = _nMaxGrade;
        this.nBatch = _nBatch;
        this.dRepairPower = _dRepairPower;
        this.dRepairLevelPlus = _dRepairLevelPlus;
    }
}

public class GameManager : GenericMonoSingleton<GameManager> {

    public CGameWeaponInfo[] cWeaponInfo = null;                //무기 정보들

    public CGameCharacterInfo[] cCharacterInfo = null;          //캐릭터 정보들

    public CGameEquiment[] cEquimentInfo = null;                //장비 정보들

    public CGameQuestInfo[] cQusetInfo = null;                  //퀘스트 정보들
	public List<int> cQuestSaveIndex = new List<int>();			//남아 있는 퀘스트 저장 
	public float curLeftQuestTime_Minute = 0f;
	public float curLeftQuestTime_Second = 0f;					//남아 있는 시간 저장
	
    public List<CGameEquiment> cInvetoryInfo = null;            //인벤토리 정보들

	public Boss[] bossInfo = null;

	public BossWeapon[] bossWeaponInfo = null;

    //private List<WeaponsData> weaponDataBase = new List<WeaponsData>();
    private List<ArbaitData> arbaitDataBase = new List<ArbaitData>();

    private List<CGameEquiment> equimnetData = new List<CGameEquiment>();

    private JsonData itemData;
    private JsonData arbaitData;

    private LogoManager logoManager;

	private string strArbaitPath = "ArbaitData.json";
    private string strEquiementPath = "Equiment.json";
    private string strInvetoryPath = "Inventory.json";


    private string strWeaponPath;

    int[] ArrGradeCount = new int[5];

    public Player player;

    public void DataLoad()
    {
		
        logoManager = GameObject.Find("LogoManager").GetComponent<LogoManager>();

		string ArbaitFilePath = Path.Combine(Application.persistentDataPath, strArbaitPath);

		string EquimentFilePath = Path.Combine(Application.persistentDataPath, strEquiementPath);

		string InventoryFilePath = Path.Combine(Application.persistentDataPath, strInvetoryPath);

		Load_TableInfo_Weapon();

		Load_TableInfo_Quest();

		Load_TableInfo_Equiment();

		Load_TableInfo_Boss ();

		Load_TableInfo_BossWeapon ();


#if UNITY_EDITOR

        itemData = JsonMapper.ToObject(File.ReadAllText(
            Application.dataPath + "/StreamingAssets/WeaponsData.json"));


		arbaitDataBase = ConstructString<ArbaitData>(strArbaitPath);


        equimnetData = ConstructString<CGameEquiment>(strEquiementPath);

        cInvetoryInfo = ConstructString<CGameEquiment>(strInvetoryPath);

        //ConstructEquimentDatabase();

        //ConstructWeaponDatabase();

        //arbaitData = JsonMapper.ToObject(File.ReadAllText(
        //    Application.dataPath + "/StreamingAssets/ArbaitData.json"));

        //ConstructArbaitDatabase();

#elif UNITY_ANDROID

		if(Directory.Exists(ArbaitFilePath)) StartCoroutine (LinkedArbaitAccess (ArbaitFilePath));

		else 
		{
			ArbaitFilePath = Path.Combine(Application.streamingAssetsPath, strArbaitPath);
			StartCoroutine(LinkedArbaitAccess (ArbaitFilePath));
		}
		Debug.Log("1");

		if(Directory.Exists(InventoryFilePath)) StartCoroutine (LinkedInventoryAccess (InventoryFilePath));

		else 
		{
		InventoryFilePath = Path.Combine(Application.streamingAssetsPath, strInvetoryPath);
		StartCoroutine(LinkedArbaitAccess (InventoryFilePath));
		}

		Debug.Log("4");

		if(Directory.Exists(EquimentFilePath)) StartCoroutine (LinkedShopAccess (EquimentFilePath));

		else 
		{
		EquimentFilePath = Path.Combine(Application.streamingAssetsPath, strEquiementPath);
		StartCoroutine(LinkedArbaitAccess (EquimentFilePath));
		}
		Debug.Log("7");


#endif

        player = new Player();

		player.Init(cInvetoryInfo);


		logoManager.bIsSuccessed = true;

		Debug.Log ("10");
    }

    IEnumerator LinkedArbaitAccess(string filePath)
    {
		Debug.Log("2");

        WWW www = new WWW(filePath);

        yield return www;

        string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

        arbaitDataBase = JsonHelper.ListFromJson<ArbaitData>(dataAsJson);

		Debug.Log ("arbaitData Count" + arbaitDataBase.Count);

		Debug.Log("3");
    }

	IEnumerator LinkedInventoryAccess(string filePath)
	{
		Debug.Log("5");

		WWW www = new WWW(filePath);

		yield return www;

		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		cInvetoryInfo = JsonHelper.ListFromJson<CGameEquiment>(dataAsJson);

		Debug.Log ("cInvetoryInfo Count" + cInvetoryInfo.Count);

		Debug.Log("6");
	}

	IEnumerator LinkedShopAccess(string filePath)
	{
		Debug.Log("8");

		WWW www = new WWW(filePath);

		yield return www;
	
		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		equimnetData = JsonHelper.ListFromJson<CGameEquiment>(dataAsJson);

		Debug.Log ("equimnetData Count" + equimnetData.Count);

		Debug.Log("9");
	}

    private List<T> ConstructString<T>(string _strPath)
    {
        List<T> getList = new List<T>();

        string filePath = Path.Combine(Application.streamingAssetsPath, _strPath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            getList = JsonHelper.ListFromJson<T>(dataAsJson);

            return getList;
        }

        return null;
    }

    void OnApplicationQuit()
    {
        SaveEquiment();
    }

    public void SaveEquiment()
    {
		if (player == null)
			return;

		#if UNITY_EDITOR
		string filePath = Path.Combine(Application.streamingAssetsPath, strInvetoryPath);

		#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strInvetoryPath);

		#endif

		if (player.GetItemListCount () != 0)
			cInvetoryInfo = player.inventory.GetItemList ();
		else
			return;


        string dataAsJson = JsonHelper.ListToJson<CGameEquiment>(cInvetoryInfo);

        File.WriteAllText(filePath, dataAsJson);
    }

    public void SaveShopList(List<CGameEquiment> _equimnet)
    {
		if (_equimnet == null)
			return;

		#if UNITY_EDITOR
		string filePath = Path.Combine(Application.streamingAssetsPath, strEquiementPath);

		#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strEquiementPath);

		#endif

        equimnetData = _equimnet;

        string dataAsJson = JsonHelper.ListToJson<CGameEquiment>(equimnetData);

        File.WriteAllText(filePath, dataAsJson);
    }

    public Player GetPlayer()
    {
        return player;
    }

    void Load_TableInfo_Weapon()
    {
        if (cWeaponInfo.Length != 0) return;

        string txtFilePath = "Weapon";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        CGameWeaponInfo[] kInfo = new CGameWeaponInfo[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameWeaponInfo();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
            kInfo[i - 1].nGrade = int.Parse(Cells[1]);
            kInfo[i - 1].strName = Cells[2];
            kInfo[i - 1].strPath = Cells[3];
            kInfo[i - 1].fComplate = float.Parse(Cells[4]);
            kInfo[i - 1].fPlusTemperature = float.Parse(Cells[5]);
            kInfo[i - 1].fLimitedTime = float.Parse(Cells[6]);
            kInfo[i - 1].fGold = float.Parse(Cells[7]);
            kInfo[i - 1].WeaponSprite = ObjectCashing.Instance.LoadSpriteFromCache(kInfo[i - 1].strPath);
        }

        cWeaponInfo = kInfo;
    }

    void Load_TableInfo_charic()
    {
        if (cCharacterInfo != null) return;

        string txtFilePath = "Character";
        TextAsset ta = LoadTextAsset(txtFilePath);
        List<string> line = LineSplit(ta.text);

        CGameCharacterInfo[] kInfo = new CGameCharacterInfo[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameCharacterInfo();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
            kInfo[i - 1].nGrade = int.Parse(Cells[1]);
            kInfo[i - 1].strName = Cells[2];
            kInfo[i - 1].strResourcePath = Cells[3];
            kInfo[i - 1].fRepair = float.Parse(Cells[4]);
            kInfo[i - 1].fPlusRepair = float.Parse(Cells[5]);
            kInfo[i - 1].fArbaitRepair = float.Parse(Cells[6]);
            kInfo[i - 1].fHonor = float.Parse(Cells[7]);
            kInfo[i - 1].fGetGoldPercent = float.Parse(Cells[8]);
            kInfo[i - 1].fWaterPlusTime = float.Parse(Cells[9]);
            kInfo[i - 1].fWater = float.Parse(Cells[10]);
            kInfo[i - 1].fCreaticalPercent = float.Parse(Cells[11]);
            kInfo[i - 1].fCreaticlaDamage = float.Parse(Cells[12]);
            kInfo[i - 1].fSuccessedPercent = float.Parse(Cells[13]);
            kInfo[i - 1].fAccuracyRate = float.Parse(Cells[14]);
            kInfo[i - 1].fGuestWaitTimePlus = float.Parse(Cells[15]);
            kInfo[i - 1].fGuestTime = float.Parse(Cells[16]);
            kInfo[i - 1].fSpecialGuest = float.Parse(Cells[17]);
            kInfo[i - 1].fRaidGuest = float.Parse(Cells[18]);
        }

        cCharacterInfo = kInfo;
    }

    void Load_TableInfo_Quest()
    {
        if (cQusetInfo.Length != 0) return;

        string txtFilePath = "Quest";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameQuestInfo[] kInfo = new CGameQuestInfo[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameQuestInfo();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
            kInfo[i - 1].nGrade = int.Parse(Cells[1]);
			kInfo[i - 1].strExplain = Cells[2];
			kInfo[i - 1].nCompleteCondition = int.Parse(Cells[3]);
			kInfo[i - 1].nRewardGold = int.Parse(Cells[4]);
			kInfo[i - 1].nRewardHonor = int.Parse(Cells[5]);
			kInfo[i - 1].nRewardBossPotion = int.Parse(Cells[6]);


        }


        cQusetInfo = kInfo;
    }


    void Load_TableInfo_Equiment()
    {
        if (cEquimentInfo.Length != 0) return;

        string txtFilePath = "Shop";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameEquiment[] kInfo = new CGameEquiment[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameEquiment();
            kInfo[i - 1].nIndex = int.Parse(Cells[0]);
            kInfo[i - 1].strResource = Cells[1];
            kInfo[i - 1].strName = Cells[2];
            kInfo[i - 1].bIsBuy = bool.Parse(Cells[3]);
            kInfo[i - 1].nSlotIndex = int.Parse(Cells[4]);
            kInfo[i - 1].nGrade = int.Parse(Cells[5]);
            kInfo[i - 1].nReapirPower = int.Parse(Cells[6]);
            kInfo[i - 1].nTemperaPlus = int.Parse(Cells[7]);
            kInfo[i - 1].nTemperaDown = int.Parse(Cells[8]);
            kInfo[i - 1].nArbaitRepair = int.Parse(Cells[9]);
            kInfo[i - 1].nHonorPlus = int.Parse(Cells[10]);
            kInfo[i - 1].nGoldPlus = int.Parse(Cells[11]);
            kInfo[i - 1].nWaterMaxPlus = int.Parse(Cells[12]);
            kInfo[i - 1].nWaterChargePlus = int.Parse(Cells[13]);
            kInfo[i - 1].nWaterUse = int.Parse(Cells[14]);
            kInfo[i - 1].nCritical = int.Parse(Cells[15]);
            kInfo[i - 1].nCriticalDamage = int.Parse(Cells[16]);
            kInfo[i - 1].nBigCritical = int.Parse(Cells[17]);
            kInfo[i - 1].nAccuracyRate = int.Parse(Cells[18]);
        }

        cEquimentInfo = kInfo;
    }


	void Load_TableInfo_Boss()
	{
		if (bossInfo.Length != 0) return;

		string txtFilePath = "Boss";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		Boss[] kInfo = new Boss[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new Boss();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo [i - 1].name = Cells [1];
			kInfo[i - 1].skillExplainOne =Cells[2];
			kInfo[i - 1].skillExplainTwo = Cells[3];
			kInfo[i - 1].GetWeaponsIndex = Cells[4];
			kInfo[i - 1].fComplate = float.Parse(Cells[5]);
			kInfo[i - 1].fWaitSecond = float.Parse(Cells[6]);
			kInfo[i - 1].nGold = int.Parse(Cells[7]);
			kInfo[i - 1].nHonor = int.Parse(Cells[8]);
			kInfo[i - 1].nDia = int.Parse(Cells[9]);
			kInfo[i - 1].fWaitSecond = float.Parse(Cells[10]);
		}

		bossInfo = kInfo;
	}

	void Load_TableInfo_BossWeapon()
	{
		if (bossWeaponInfo.Length != 0) return;

		string txtFilePath = "BossWeapon";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		BossWeapon[] kInfo = new BossWeapon[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new BossWeapon();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].strResouce = Cells[1];
			kInfo[i - 1].strName = Cells[2];
			kInfo[i - 1].nSlot = int.Parse(Cells[3]);
			kInfo[i - 1].strGrade = Cells[4];
			kInfo[i - 1].explain = Cells[5];
		}

		bossWeaponInfo = kInfo;
	}
    public int GetEquimentLength()
    {
		if (cEquimentInfo == null)
			return 0;

        if (cEquimentInfo.Length != 0)
            return cEquimentInfo.Length;

        else
            return 0;
    }

    public int GetInvetoryListCount()
    {
        if (cInvetoryInfo.Count != 0)
            return cInvetoryInfo.Count;

        else
            return 0;
    }

    TextAsset LoadTextAsset(string _txtFile)
    {
        TextAsset ta;
        ta = Resources.Load("Data/" + _txtFile) as TextAsset;
        return ta;
    }

    public List<string> LineSplit(string text)
    {
        //Console.WriteLine("LineSplit " + text.Length);

        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];
            //int value = Convert.ToInt32(c); Console.WriteLine(String.Format("{0:x4}", value) + " " + c.ToString());

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )			    
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n')	//file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }

    /*
    public WeaponsData GetWeaponData(int _nGrade)
    {
        int nRandom;
        int nLength = 0;

        for (int i = 0; i <= _nGrade; i++)
            nLength += ArrGradeCount[i];

        nRandom = Random.Range(0, nLength);

        return weaponDataBase[nRandom];
    }
    */

    public CGameWeaponInfo GetWeaponData(int _nGrade)
    {
        int nRandom;

        nRandom = Random.Range(0, 7);

        return cWeaponInfo[nRandom];
    }
    public ArbaitData GetArbaitData(int _id)
    {
        if (_id < 0 || arbaitDataBase.Count <= _id)
            return null;

        return arbaitDataBase[_id];
    }

    public int EquimentShopLength()
    {
        return equimnetData.Count;
    }

    public List<CGameEquiment> GetEquimentShopData()
    {
        return equimnetData;
    }

    public CGameEquiment GetEquimentData(int nIndex)
    {
        if (cEquimentInfo == null)
            return null;

        return cEquimentInfo[nIndex];
    }

    public int ArbaitLength()
    {
        return arbaitDataBase.Count;
    }
}

enum E_Equiment
{
	E_REPAIR = 6,
	E_TEMPPLUS =7,
	E_TEMPDOWN =8,
	E_ARBAIT,
	E_HONOR,
	E_GOLD,
	E_WATERMAX,
	E_WATERCHARGE,
	E_WATERUSE,
	E_CRITICAL,
	E_CRITICALD,
	E_BIGCRITICAL,
	E_ACCURACY,
	E_MAX,
}

[System.Serializable]
public class AllArbaitData
{
	public ArbaitData[] allArbaitData;
}

[System.Serializable]
public class CGameEquiment
{
    public int nIndex = 0;
    public string strResource = "";
    public string strName = "";
    public bool bIsBuy = false;
    public int nSlotIndex = 0;
    public int nGrade = 0;
    public int nReapirPower = 0;
    public int nTemperaPlus = 0;
    public int nTemperaDown = 0;
    public int nArbaitRepair = 0;
    public int nHonorPlus = 0;
    public int nGoldPlus = 0;
    public int nWaterMaxPlus = 0;
    public int nWaterChargePlus = 0;
    public int nWaterUse = 0;
    public int nCritical = 0;
    public int nCriticalDamage = 0;
    public int nBigCritical = 0;
    public int nAccuracyRate = 0;
}



[System.Serializable]
public class ArbaitData
{
    //id값
    public int index;
    //레벨
    public int level;
    //이름
    public string name;

	public string strAnimation;
    //현재 등급
	public int grade;

    //배치 위치 (-1 = 배치안함) 0, 1, 2
    public int batch;

    //수리 량
	public float fRepairPower;

	//공격속도
	public float fAttackSpeed;

	//크리티컬 
	public float fCritical;

	//명중률
	public float fAccuracyRate;

	//특수 능력 인덱스
	public string strBuffIndexs;

	//특수능력 설명 
	public string strExplains;

	//스킬 지속시간 
	public float fCurrentFloat;

	//영입 카운트 
	public int nScoutCount;

	//영입 골드 
	public int nScoutGold;

	//영입 명예
	public int nScoutHonor;

	//영입 다이아 
	public int nScoutDia;

	//다이아로 구매 가능한지
	public int nIsDia;

	//구매 했는지
	public bool bIsBuyCheck;
}

[System.Serializable]
public class CGameWeaponInfo
{
    public int nIndex = 0;
    public int nGrade = 0;
    public string strName = string.Empty;
    public string strPath = string.Empty;
    public float fComplate = 0.0f;
    public float fPlusTemperature = 0.0f;
    public float fLimitedTime = 0.0f;
    public float fGold = 0.0f;
    public Sprite WeaponSprite = null;
}

[System.Serializable]
public class CGameQuestInfo
{
    public int nIndex = 0;
    public int nGrade = 0;
	public string strExplain = "";
	public int nCompleteCondition = 0;
    
	public int nRewardGold = 0;
	public int nRewardHonor=0;
	public int nRewardBossPotion =0;
    
}



[System.Serializable]
public class CGameCharacterInfo
{
    public int nIndex = 0;
    public int nGrade = 0;                          //캐릭터 타입(근,원)
    public string strName = string.Empty;           //캐릭터 이름
    public string strResourcePath = string.Empty;   //캐릭터 경로
    public float fRepair = 0.0f;                    //캐릭터 수리력
    public float fPlusRepair = 0.0f;                //온도 증감량
    public float fArbaitRepair = 0.0f;              //알바 수리력
    public float fHonor = 0.0f;                     //명예 획득량
    public float fGetGoldPercent = 0.0f;            //골드 획득량
    public float fWaterPlusTime = 0.0f;             //물 충전시간
    public float fWater = 0.0f;                     //물 수치
    public float fCreaticalPercent = 0.0f;          //크리 확률
    public float fCreaticlaDamage = 0.0f;           //크리 데미지
    public float fSuccessedPercent = 0.0f;          //대 성공 확률
    public float fAccuracyRate = 0.0f;              //명중률
    public float fGuestWaitTimePlus = 0.0f;         //손님 대기시간
    public float fGuestTime = 0.0f;                 //손님 등장시간
    public float fSpecialGuest = 0.0f;              //특수 손님 등장확률
    public float fRaidGuest = 0.0f;                 //레이드 손님 등장확률
}

[System.Serializable]
public class Boss
{
	//id값
	public int nIndex;
	//이름
	public string name;

	public string skillExplainOne;

	public string skillExplainTwo;

	public string GetWeaponsIndex;

	public float fComplate;

	public float fWaitSecond;

	public int nGold;

	public int nHonor;

	public int nDia;

	public float fDropPercent;
}

[System.Serializable]
public class BossWeapon
{
	public int nIndex = 0;
	public string strResouce = string.Empty;
	public string strName = string.Empty;
	public int nSlot = 0;
	public string strGrade;
	public string explain;
}
