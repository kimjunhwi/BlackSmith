 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Text;
using ReadOnlys;
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


//모든 데이터 및 로드, 세이브를 관리하는 클래스 
//어디서든 사용해야 하기 때문에 제네릭싱글톤을 통해 구현
public class GameManager : GenericMonoSingleton<GameManager> {


    //읽어들이기만 하면 되기 때문에 유니코드 텍스트 방식 저장 후 읽어들인다.

    public CGameWeaponInfo[] cWeaponInfo = null;                //무기 정보들

    public CGameCharacterInfo[] cCharacterInfo = null;          //캐릭터 정보들

    public CGameEquiment[] cEquimentInfo = null;                //장비 정보들

    public CGameQuestInfo[] cQusetInfo = null;                  //퀘스트 정보들

	public BossPanelInfo cBossPanelInfo = null;

	public CGamePlayerEnhance[] cSmithEnhaceInfo = null;          //대장간 강화 정보

	public CGamePlayerEnhance[] cRepairEnhanceInfo = null;      //수리 강화 정보

	public CGamePlayerEnhance[] cMaxWaterEnhanceInfo = null;  //물 최대치 강화 정보

	public CGamePlayerEnhance[] cWaterPlusEnhanceInfo = null;//물 증가량 강화 정보

	public CGamePlayerEnhance[] cAccuracyRateInfo = null;        //명중률 강화 정보

	public CGamePlayerEnhance[] cCriticalEnhance = null;      //크리티컬 강화 정보

	public CGamePlayerEnhance[] cPlayerArbaitEnhace = null;		//아르바이트 강화 

    public CGameEnhanceData[] cOneGradeEnhance = null;          //1등급 강화 데이터 모음

    public CGameEnhanceData[] cTwoGradeEnhance = null;          //2등급 강화 데이터 모음

    public CGameEnhanceData[] cThreeGradeEnhance = null;          //3등급 강화 데이터 모음

    public CGameEnhanceData[] cFourGradeEnhance = null;          //4등급 강화 데이터 모음

	public CGameSoundData[] cSoundsData = null;					//사운드 데이

	public CGameArbaitGrade[] cArbaitCgrade = null;				//C등급 아르바이트
	public CGameArbaitGrade[] cArbaitBgrade = null;				//B등급 아르바이트
	public CGameArbaitGrade[] cArbaitAgrade = null;				//S등급 아르바이트
	public CGameArbaitGrade[] cArbaitSgrade = null;				//A등급 아르바이트

	public Boss[] bossInfo = null;

	public BossWeapon[] bossWeaponInfo = null;


	List<CGamePlayerData> playerSave = new List<CGamePlayerData>();
    //세이브가 필요한 부분들은 LitJson을 사용함
    //
    private List<ArbaitData> ArbaitDataBase = new List<ArbaitData>();

    private List<CGameEquiment> equimnetData = new List<CGameEquiment>();

    public List<CGameEquiment> cInvetoryInfo = null;            //인벤토리 정보들

	public List<CGameQuestInfo> cQuestSaveListInfo = null;				//퀘스트 저장


	public List<BossPanelInfo> cBossPanelListInfo = new List<BossPanelInfo>();

	public List<CGameMainWeaponOption> cMainWeaponOption = null;


    private JsonData itemData;
    private JsonData ArbaitData;

    private LogoManager logoManager;

    private const string strPlayerPath = "PlayerData.json";
    private const string strArbaitPath = "ArbaitData.json";
    private const string strEquiementPath = "Equiment.json";
    private const string strInvetoryPath = "Inventory.json";
	private const string strQuestPath = "Quest.json";
	private const string strBossPanelInfoPath = "BossPanelInfo.json";

    private string strWeaponPath;

    public Player player;

    public CGamePlayerData playerData;
	public GameObject Root_ui;

	public IEnumerator DataLoad()
    {
		PlayerPrefs.DeleteKey ("BossRegenTime");
		PlayerPrefs.DeleteKey ("BossInvitementSaveTime");


        logoManager = GameObject.Find("LogoManager").GetComponent<LogoManager>();

		Load_TableInfo_Sound ();

		Load_TableInfo_Weapon();

		Load_TableInfo_Quest();

		Load_TableInfo_Equiment();

		Load_TableInfo_Boss ();

		Load_TableInfo_BossWeapon ();

		Load_TableInfo_SmithEnhance ();

		Load_TableInfo_RepairEnhance ();

		Load_TableInfo_MaxWaterEnhance ();

		Load_TableInfo_WaterPlusEnhance ();

		Load_TableInfo_AccuracyEnhance ();

		Load_TableInfo_CriticalEnhance ();

        Load_TableInfo_OneGradeEnhance();

        Load_TableInfo_TwoGradeEnhance();

        Load_TableInfo_ThreeGradeEnhance();

        Load_TableInfo_FourGradeEnhance();

		Load_TableInfo_CgradeEnhance ();

		Load_TableInfo_BgradeEnhance();

		Load_TableInfo_AgradeEnhance();

		Load_TableInfo_SgradeEnhance();

		Load_TableInfo_PlayerArbaitEnhance ();

#if UNITY_EDITOR

        ArbaitDataBase = ConstructString<ArbaitData>(strArbaitPath);

        equimnetData = ConstructString<CGameEquiment>(strEquiementPath);

        cInvetoryInfo = ConstructString<CGameEquiment>(strInvetoryPath);

        playerData = ConstructString<CGamePlayerData>(strPlayerPath)[0];

		if(ConstructString<BossPanelInfo> (strBossPanelInfoPath) == null)
			cBossPanelListInfo.Add(cBossPanelInfo);
		else
			cBossPanelListInfo = ConstructString<BossPanelInfo> (strBossPanelInfoPath);
		
        //ConstructEquimentDatabase();

        //ConstructWeaponDatabase();

        //ArbaitData = JsonMapper.ToObject(File.ReadAllText(
        //    Application.dataPath + "/StreamingAssets/ArbaitData.json"));

        //ConstructArbaitDatabase();

#elif UNITY_ANDROID

        string ArbaitFilePath = Path.Combine(Application.persistentDataPath, strArbaitPath);

		string EquimentFilePath = Path.Combine(Application.persistentDataPath, strEquiementPath);

		string InventoryFilePath = Path.Combine(Application.persistentDataPath, strInvetoryPath);

		string PlayerFilePath = Path.Combine (Application.persistentDataPath, strPlayerPath);

		string QuestFilePath = Path.Combine (Application.persistentDataPath, strQuestPath);

		string BossFilePath =  Path.Combine (Application.persistentDataPath, strBossPanelInfoPath);

		if(Directory.Exists(ArbaitFilePath)) 
		yield return StartCoroutine (LinkedArbaitAccess (ArbaitFilePath));

		else 
		{
			ArbaitFilePath = Path.Combine(Application.streamingAssetsPath, strArbaitPath);
		yield return StartCoroutine(LinkedArbaitAccess (ArbaitFilePath));
		}
		Debug.Log("1");


		if(Directory.Exists(InventoryFilePath)) 
		 yield return StartCoroutine (LinkedInventoryAccess (InventoryFilePath));


		Debug.Log("4");

		if(Directory.Exists(EquimentFilePath)) 
		yield return StartCoroutine (LinkedShopAccess (EquimentFilePath));

//		else 
//		{
//		EquimentFilePath = Path.Combine(Application.streamingAssetsPath, strEquiementPath);
//		yield return StartCoroutine(LinkedShopAccess (EquimentFilePath));
//		}

		if(Directory.Exists(PlayerFilePath)) 
		{
		Debug.Log("Search PlayerData");
		yield return StartCoroutine (LinkedPlayerAccess (PlayerFilePath));
		}
		else 
		{
		Debug.Log("No SearchPlayerData");
		PlayerFilePath = Path.Combine(Application.streamingAssetsPath, strPlayerPath);
		yield return StartCoroutine(LinkedPlayerAccess (PlayerFilePath));
		}
		Debug.Log("7");

		if(Directory.Exists(QuestFilePath)) 
			yield return StartCoroutine (LinkedQuestAccess (QuestFilePath));

		if(Directory.Exists(BossFilePath)) 
			yield return StartCoroutine (LinkedBossPanelInfoAccess (BossFilePath));
		else
			cBossPanelListInfo.Add(cBossPanelInfo);

#endif

        Debug.Log(playerData.strName);

        player = new Player();

        player.Init(cInvetoryInfo, playerData);

        SoundManager.instance.LoadSource();

		Debug.Log (player.fRepairPower);

        SoundManager.instance.PlayBGM(eSound.bgm_main);

        logoManager.bIsSuccessed = true;

        yield break;
    }

    IEnumerator LinkedArbaitAccess(string filePath)
    {
		Debug.Log("2");

        WWW www = new WWW(filePath);

        yield return www;

        string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

        ArbaitDataBase = JsonHelper.ListFromJson<ArbaitData>(dataAsJson);

		Debug.Log ("ArbaitData Count" + ArbaitDataBase.Count);

		Debug.Log("3");
    }

	IEnumerator LinkedInventoryAccess(string filePath)
	{
		Debug.Log("5");

		WWW www = new WWW(filePath);

		yield return www;

		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		if (dataAsJson == "")
			yield break;

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

		if (dataAsJson == null)
			yield break;

		equimnetData = JsonHelper.ListFromJson<CGameEquiment>(dataAsJson);

		Debug.Log ("equimnetData Count" + equimnetData.Count);

		Debug.Log("9");
	}

	IEnumerator LinkedPlayerAccess(string filePath)
	{
		WWW www = new WWW(filePath);

		yield return www;

		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		playerData = JsonHelper.ListFromJson<CGamePlayerData>(dataAsJson)[0];
	}


	IEnumerator LinkedQuestAccess(string filePath)
	{
		Debug.Log ("Quest Loaded");
		
		WWW www = new WWW(filePath);

		yield return www;

		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		cQuestSaveListInfo = JsonHelper.ListFromJson<CGameQuestInfo>(dataAsJson);
	}

	IEnumerator LinkedBossPanelInfoAccess(string filePath)
	{
		Debug.Log ("BossPanel Loaded");

		WWW www = new WWW(filePath);

		yield return www;

		string dataAsJson = www.text.ToString();

		Debug.Log (dataAsJson);

		cBossPanelListInfo = JsonHelper.ListFromJson<BossPanelInfo>(dataAsJson);
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




    //void OnApplicationQuit()
    //{

    //    DataSave();

    //    StopAllCoroutines();
    //}

    //데이터 저장시 호출된다.
    //저장이 되는 부분은 OnApplicationPuase가 TRUE 이고 플레이어가 존재할시 호출
    void DataSave()
    {
        Debug.Log("Quit");

        if (player == null)
            return;

        PlayerPrefs.SetFloat("Gold", ScoreManager.ScoreInstance.GetGold());
        PlayerPrefs.SetFloat("Honor", ScoreManager.ScoreInstance.GetHonor());

        SpawnManager.Instance.ReleliveArbait();

        playerData = player.changeStats;

        SaveArbaitData();

        SavePlayerData();

        SaveEquiment();

        SaveQuestList();

		SaveBossPanelInfoList ();

        SpawnManager.Instance.ApplyArbait();

        PlayerPrefs.Save();

        StopAllCoroutines();
    }

    public void OnApplicationPause(bool bIsPause)
    {
        if (bIsPause)
        {
            if (player != null)
            {
                Debug.Log("Puase");
                DataSave();
            }
        }
    }


    public void SavePlayerData()
	{
		#if UNITY_EDITOR
		string filePath = Path.Combine(Application.streamingAssetsPath, strPlayerPath);

		#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strPlayerPath);

		#endif

		playerSave.Add (playerData);

		string dataAsJson = JsonHelper.ListToJson<CGamePlayerData>(playerSave);

		Debug.Log (dataAsJson);

		File.WriteAllText(filePath, dataAsJson);
	}

    public void SaveArbaitData()
    {
#if UNITY_EDITOR
        string filePath = Path.Combine(Application.streamingAssetsPath, strArbaitPath);

#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strArbaitPath);

#endif
        string dataAsJson = JsonHelper.ListToJson<ArbaitData>(ArbaitDataBase);

        File.WriteAllText(filePath, dataAsJson);
    }

    public void SaveEquiment()
    {

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

	public void SetQuestData(List<CGameQuestInfo> _QuestData)
	{
		cQuestSaveListInfo = _QuestData;
	}

	public void SaveQuestList()
	{
		if (cQuestSaveListInfo == null)
			return;

		#if UNITY_EDITOR
		string filePath = Path.Combine(Application.streamingAssetsPath, strQuestPath);

		#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strQuestPath);

		#endif


		string dataAsJson = JsonHelper.ListToJson<CGameQuestInfo>(cQuestSaveListInfo);

		File.WriteAllText(filePath, dataAsJson);
	}


	public void SaveBossPanelInfoList()
	{
		if (cQuestSaveListInfo == null)
			return;

		#if UNITY_EDITOR
		string filePath = Path.Combine(Application.streamingAssetsPath, strBossPanelInfoPath);

		#elif UNITY_ANDROID
		string filePath = Path.Combine(Application.persistentDataPath, strBossPanelInfoPath);

		#endif


		string dataAsJson = JsonHelper.ListToJson<BossPanelInfo>(cBossPanelListInfo);

		File.WriteAllText(filePath, dataAsJson);
	}


    public Player GetPlayer()
    {
        return player;
    }

	#region LoadTableInfo

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
			kInfo[i - 1].strPath = Cells[1];
			kInfo[i - 1].strName = Cells[2];
			kInfo [i - 1].fMaxComplate = float.Parse (Cells [3]);
			kInfo [i - 1].fMinusRepair= float.Parse(Cells[4]);
			kInfo [i - 1].fPlusTemperature= float.Parse(Cells[5]);
			kInfo [i - 1].fMinusTemperature= float.Parse(Cells[6]);
			kInfo [i - 1].fMinusUseWater= float.Parse(Cells[7]);
			kInfo [i - 1].fMinusCritical= float.Parse(Cells[8]);
			kInfo [i - 1].fMinusAccuracy= float.Parse(Cells[9]);
            kInfo[i - 1].fGold = float.Parse(Cells[10]);
			kInfo[i - 1].fHonor = float.Parse(Cells[11]);
			kInfo[i - 1].fLimitedTime = float.Parse(Cells[12]);
			kInfo[i - 1].nGrade = int.Parse(Cells[13]);
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
			kInfo [i - 1].bIsActive = bool.Parse (Cells [7]);
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
            kInfo[i - 1].sGrade = Cells[5];
			kInfo[i - 1].fReapirPower = float.Parse(Cells[6]);
			kInfo[i - 1].fTemperaPlus = float.Parse(Cells[7]);
			kInfo[i - 1].fTemperaDown = float.Parse(Cells[8]);
			kInfo[i - 1].fArbaitRepair = float.Parse(Cells[9]);
			kInfo[i - 1].fHonorPlus = float.Parse(Cells[10]);
			kInfo[i - 1].fGoldPlus = float.Parse(Cells[11]);
			kInfo[i - 1].fWaterMaxPlus = float.Parse(Cells[12]);
			kInfo[i - 1].fWaterChargePlus = float.Parse(Cells[13]);
			kInfo[i - 1].fWaterUse = float.Parse(Cells[14]);
			kInfo[i - 1].fCritical = float.Parse(Cells[15]);
			kInfo[i - 1].fCriticalDamage = float.Parse(Cells[16]);
			kInfo[i - 1].fBigCritical = float.Parse(Cells[17]);
			kInfo[i - 1].fAccuracyRate = float.Parse(Cells[18]);
			kInfo [i - 1].nStrenthCount = int.Parse (Cells [19]);
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
			kInfo [i - 1].bossName = Cells [1];
			kInfo[i - 1].skillExplainOne =Cells[2];
			kInfo[i - 1].skillExplainTwo = Cells[3];
			kInfo[i - 1].bossWeaponName = Cells[4];
			kInfo[i - 1].GetWeaponsIndex = Cells[5];
			kInfo[i - 1].fComplate = float.Parse(Cells[6]);
			kInfo[i - 1].nGold = int.Parse(Cells[7]);
			kInfo[i - 1].nHonor = int.Parse(Cells[8]);
			kInfo[i - 1].nDia = int.Parse(Cells[9]);
			kInfo[i - 1].fDropPercent =float.Parse( Cells[10]);
			kInfo[i - 1].bossWord02 = Cells[11];
			kInfo[i - 1].bossWord03 = Cells[12];
			kInfo[i - 1].bossWord04 = Cells[13];
			kInfo[i - 1].bossWord04 = Cells[14];
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
			kInfo[i - 1].strResource = Cells[1];
			kInfo[i - 1].strName = Cells[2];
			kInfo[i - 1].bIsBuy = bool.Parse(Cells[3]);
			kInfo[i - 1].nSlotIndex = int.Parse(Cells[4]);
			kInfo[i - 1].sGrade = Cells[5];
			kInfo[i - 1].fReapirPower = float.Parse(Cells[6]);
			kInfo[i - 1].fTemperaPlus = float.Parse(Cells[7]);
			kInfo[i - 1].fTemperaDown = float.Parse(Cells[8]);
			kInfo[i - 1].fArbaitRepair = float.Parse(Cells[9]);
			kInfo[i - 1].fHonorPlus = float.Parse(Cells[10]);
			kInfo[i - 1].fGoldPlus = float.Parse(Cells[11]);
			kInfo[i - 1].fWaterMaxPlus = float.Parse(Cells[12]);
			kInfo[i - 1].fWaterChargePlus = float.Parse(Cells[13]);
			kInfo[i - 1].fWaterUse = float.Parse(Cells[14]);
			kInfo[i - 1].fCritical = float.Parse(Cells[15]);
			kInfo[i - 1].fCriticalDamage = float.Parse(Cells[16]);
			kInfo[i - 1].fBigCritical = float.Parse(Cells[17]);
			kInfo[i - 1].fAccuracyRate = float.Parse(Cells[18]);
			kInfo [i - 1].nStrenthCount = int.Parse (Cells [19]);
			kInfo [i - 1].explain = Cells [20];
		}

		bossWeaponInfo = kInfo;
	}

	void Load_TableInfo_Sound()
	{
		if (cSoundsData.Length != 0) return;

		string txtFilePath = "sound";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGameSoundData[] kInfo = new CGameSoundData[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new CGameSoundData();
			kInfo[i - 1].nIndex = int.Parse(Cells[0]);
			kInfo[i - 1].strName = Cells[1];
			kInfo[i - 1].strResource = Cells[2];
			kInfo[i - 1].nType = int.Parse(Cells[3]);
			kInfo[i - 1].nVolume = int.Parse(Cells[4]);
			kInfo[i - 1].nLoop = int.Parse(Cells[5]);
		}

		cSoundsData = kInfo;
	}

    void Load_TableInfo_SmithEnhance()
    {
		if (cSmithEnhaceInfo.Length != 0) return;

        string txtFilePath = "SmithEnhance";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
        }

        cSmithEnhaceInfo = kInfo;
    }

	void Load_TableInfo_RepairEnhance()
	{
		if (cRepairEnhanceInfo.Length != 0) return;

		string txtFilePath = "RepairEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cRepairEnhanceInfo = kInfo;
	}

	void Load_TableInfo_MaxWaterEnhance()
	{
		if (cMaxWaterEnhanceInfo.Length != 0) return;

		string txtFilePath = "MaxWaterEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cMaxWaterEnhanceInfo = kInfo;
	}

	void Load_TableInfo_WaterPlusEnhance()
	{
		if (cWaterPlusEnhanceInfo.Length != 0) return;

		string txtFilePath = "WaterPlusEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cWaterPlusEnhanceInfo = kInfo;
	}

	void Load_TableInfo_AccuracyEnhance()
	{
		if (cAccuracyRateInfo.Length != 0) return;

		string txtFilePath = "AccuracyEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cAccuracyRateInfo = kInfo;
	}

	void Load_TableInfo_CriticalEnhance()
	{
		if (cCriticalEnhance.Length != 0) return;

		string txtFilePath = "CriticalEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cCriticalEnhance = kInfo;
	}

	void Load_TableInfo_PlayerArbaitEnhance()
	{
		if (cPlayerArbaitEnhace.Length != 0) return;

		string txtFilePath = "ArbaitEnhance";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGamePlayerEnhance[] kInfo = new CGamePlayerEnhance[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1]            = new CGamePlayerEnhance();
			kInfo[i - 1].nIndex     = int.Parse(Cells[0]);
			kInfo[i - 1].strName  = Cells[1];
			kInfo [i - 1].fPlusPercentValue = float.Parse (Cells [2]);
			kInfo[i - 1].nGoldCost     = int.Parse(Cells[3]);
			kInfo[i - 1].nHonorCost     = int.Parse(Cells[4]);
		}

		cPlayerArbaitEnhace = kInfo;
	}

    void Load_TableInfo_OneGradeEnhance()
    {
		if (cOneGradeEnhance.Length != 0) return;

        string txtFilePath = "OneGradeEnhance";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameEnhanceData[] kInfo = new CGameEnhanceData[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameEnhanceData();
            kInfo[i - 1].nLevel = int.Parse(Cells[0]);
            kInfo[i - 1].nPercent = int.Parse(Cells[1]);
            kInfo[i - 1].nGoldCost = int.Parse(Cells[2]);
            kInfo[i - 1].nHonorCost = int.Parse(Cells[2]);
        }

        cOneGradeEnhance = kInfo;
    }

    void Load_TableInfo_TwoGradeEnhance()
    {
		if (cTwoGradeEnhance.Length != 0) return;

        string txtFilePath = "TwoGradeEnhance";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameEnhanceData[] kInfo = new CGameEnhanceData[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameEnhanceData();
            kInfo[i - 1].nLevel = int.Parse(Cells[0]);
            kInfo[i - 1].nPercent = int.Parse(Cells[1]);
            kInfo[i - 1].nGoldCost = int.Parse(Cells[2]);
            kInfo[i - 1].nHonorCost = int.Parse(Cells[2]);
        }

        cTwoGradeEnhance = kInfo;
    }

    void Load_TableInfo_ThreeGradeEnhance()
    {
		if (cThreeGradeEnhance.Length != 0) return;

        string txtFilePath = "ThreeGradeEnhance";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameEnhanceData[] kInfo = new CGameEnhanceData[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameEnhanceData();
            kInfo[i - 1].nLevel = int.Parse(Cells[0]);
            kInfo[i - 1].nPercent = int.Parse(Cells[1]);
            kInfo[i - 1].nGoldCost = int.Parse(Cells[2]);
            kInfo[i - 1].nHonorCost = int.Parse(Cells[2]);
        }

        cThreeGradeEnhance = kInfo;
    }

    void Load_TableInfo_FourGradeEnhance()
    {
		if (cFourGradeEnhance.Length != 0) return;

        string txtFilePath = "FourGradeEnhance";

        TextAsset ta = LoadTextAsset(txtFilePath);

        List<string> line = LineSplit(ta.text);

        CGameEnhanceData[] kInfo = new CGameEnhanceData[line.Count - 1];

        for (int i = 0; i < line.Count; i++)
        {
            //Console.WriteLine("line : " + line[i]);
            if (line[i] == null) continue;
            if (i == 0) continue; 	// Title skip

            string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
            if (Cells[0] == "") continue;

            kInfo[i - 1] = new CGameEnhanceData();
            kInfo[i - 1].nLevel = int.Parse(Cells[0]);
            kInfo[i - 1].nPercent = int.Parse(Cells[1]);
            kInfo[i - 1].nGoldCost = int.Parse(Cells[2]);
            kInfo[i - 1].nHonorCost = int.Parse(Cells[2]);
        }

        cFourGradeEnhance = kInfo;
    }

	void Load_TableInfo_CgradeEnhance()
	{
		if (cArbaitCgrade.Length != 0) return;

		string txtFilePath = "CgradeArbait";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGameArbaitGrade[] kInfo = new CGameArbaitGrade[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new CGameArbaitGrade();

			kInfo[i - 1].nNextLevel = int.Parse(Cells[0]);
			kInfo[i - 1].nPercentPlusRepair = int.Parse(Cells[1]);
			kInfo[i - 1].nPercentPlusAccuracy = int.Parse(Cells[2]);
			kInfo[i - 1].nPercentPlusCritical = int.Parse(Cells[3]);
			kInfo[i - 1].nPercentPlusSkill = int.Parse(Cells[4]);
			kInfo[i - 1].nGoldCost = int.Parse(Cells[5]);
			kInfo[i - 1].nHonorCost = int.Parse(Cells[6]);
		}

		cArbaitCgrade = kInfo;
	}

	void Load_TableInfo_BgradeEnhance()
	{
		if (cArbaitBgrade.Length != 0) return;

		string txtFilePath = "BgradeArbait";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGameArbaitGrade[] kInfo = new CGameArbaitGrade[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new CGameArbaitGrade();

			kInfo[i - 1].nNextLevel = int.Parse(Cells[0]);
			kInfo[i - 1].nPercentPlusRepair = int.Parse(Cells[1]);
			kInfo[i - 1].nPercentPlusAccuracy = int.Parse(Cells[2]);
			kInfo[i - 1].nPercentPlusCritical = int.Parse(Cells[3]);
			kInfo[i - 1].nPercentPlusSkill = int.Parse(Cells[4]);
			kInfo[i - 1].nGoldCost = int.Parse(Cells[5]);
			kInfo[i - 1].nHonorCost = int.Parse(Cells[6]);
		}

		cArbaitBgrade = kInfo;
	}

	void Load_TableInfo_AgradeEnhance()
	{
		if (cArbaitAgrade.Length != 0) return;

		string txtFilePath = "AgradeArbait";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGameArbaitGrade[] kInfo = new CGameArbaitGrade[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new CGameArbaitGrade();

			kInfo[i - 1].nNextLevel = int.Parse(Cells[0]);
			kInfo[i - 1].nPercentPlusRepair = int.Parse(Cells[1]);
			kInfo[i - 1].nPercentPlusAccuracy = int.Parse(Cells[2]);
			kInfo[i - 1].nPercentPlusCritical = int.Parse(Cells[3]);
			kInfo[i - 1].nPercentPlusSkill = int.Parse(Cells[4]);
			kInfo[i - 1].nGoldCost = int.Parse(Cells[5]);
			kInfo[i - 1].nHonorCost = int.Parse(Cells[6]);
		}

		cArbaitAgrade = kInfo;
	}

	void Load_TableInfo_SgradeEnhance()
	{
		if (cArbaitSgrade.Length != 0) return;

		string txtFilePath = "SgradeArbait";

		TextAsset ta = LoadTextAsset(txtFilePath);

		List<string> line = LineSplit(ta.text);

		CGameArbaitGrade[] kInfo = new CGameArbaitGrade[line.Count - 1];

		for (int i = 0; i < line.Count; i++)
		{
			//Console.WriteLine("line : " + line[i]);
			if (line[i] == null) continue;
			if (i == 0) continue; 	// Title skip

			string[] Cells = line[i].Split("\t"[0]);	// cell split, tab
			if (Cells[0] == "") continue;

			kInfo[i - 1] = new CGameArbaitGrade();

			kInfo[i - 1].nNextLevel = int.Parse(Cells[0]);
			kInfo[i - 1].nPercentPlusRepair = int.Parse(Cells[1]);
			kInfo[i - 1].nPercentPlusAccuracy = int.Parse(Cells[2]);
			kInfo[i - 1].nPercentPlusCritical = int.Parse(Cells[3]);
			kInfo[i - 1].nPercentPlusSkill = int.Parse(Cells[4]);
			kInfo[i - 1].nGoldCost = int.Parse(Cells[5]);
			kInfo[i - 1].nHonorCost = int.Parse(Cells[6]);
		}

		cArbaitSgrade = kInfo;
	}

	#endregion

	#region SplitText

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
	#endregion

	public CGameEnhanceData[] GetEnhanceArbaitData(string _nIndex)
	{
		int nIndex = 0;
		if (_nIndex == "C")
			nIndex = 1;
		else if (_nIndex == "B")
			nIndex = 2;
		else if (_nIndex == "A")
			nIndex = 3;
		else 
			nIndex = 4;


		switch (nIndex) {
		case 1:
			return cOneGradeEnhance;

		case 2:
			return cTwoGradeEnhance;
		case 3:
			return cThreeGradeEnhance;
		case 4:
			return cFourGradeEnhance;
		default:
			Debug.Log ("Range Error");
			return null;

		}
	}

	public CGameArbaitGrade[] GetArbaitGradeEnhanceData(int _nGradeIndex)
	{
		switch (_nGradeIndex) {

		case (int)E_ArbaitGrade.E_Cgrade: return cArbaitCgrade;
		case (int)E_ArbaitGrade.E_Bgrade: return cArbaitBgrade;
		case (int)E_ArbaitGrade.E_Agrade: return cArbaitAgrade;
		case (int)E_ArbaitGrade.E_Sgrade: return cArbaitSgrade;
		
		default:	Debug.Log ("Range Error"); return null;
		}
	}

    public CGameWeaponInfo GetWeaponData(int _nGrade)
    {
        int nRandom;

        nRandom = Random.Range(0, 7);

		CGameWeaponInfo ResultWeapon = new CGameWeaponInfo (cWeaponInfo [nRandom]);

		return ResultWeapon;
    }

    public int EquimentShopLength()
    {
        

        return equimnetData.Count;
    }

    public List<CGameEquiment> GetEquimentShopData()
    {
        if (equimnetData == null)
            return null;

        return equimnetData;
    }

    public CGameEquiment GetEquimentData(int nIndex)
    {
        if (cEquimentInfo == null)
            return null;

        return cEquimentInfo[nIndex];
    }

    #region window popup

    // 윈도우 팝업 ---------------------------------------------------------------------------------------
	//CGame.Instance.Window_notice("213123 213123 ", rt => { if (rt == "0") print("notice");  });
	public void Window_notice(string _msg, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Window_notice"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowNotice w = go.GetComponent<CWindowNotice>();
		w.Show(_msg, _callback);
	}

	public void Window_yesno(string _title, string _msg, System.Action<string> _callback)
	{
		//GameObject Root_ui = GameObject.Find("root_window)"); //ui attach
		GameObject go = GameObject.Instantiate(Resources.Load("prefabs/Window_yesno"), Vector3.zero, Quaternion.identity) as GameObject;
		go.transform.parent = Root_ui.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		CWindowYesNo w = go.GetComponent<CWindowYesNo>();
		w.Show(_title, _msg, _callback);
	}
    #endregion

    #region Arbait
    public int ArbaitLength()
    {
        return ArbaitDataBase.Count;
    }

    public ArbaitData GetArbaitData(int _id)
    {
        if (_id < 0 || ArbaitDataBase.Count <= _id)
            return null;

        return ArbaitDataBase[_id];
    }
    #endregion

	#region Inventory
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
    #endregion 

	public CGameSoundData Get_TableInfo_sound(int _nIndex)
	{
		for (int nIndex = 0; nIndex < cSoundsData.Length; nIndex++)
			if (cSoundsData [nIndex].nIndex == _nIndex)
				return cSoundsData [nIndex];

		return null;
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

enum E_CREATOR
{
	E_ARBAIT = 0,
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

#region Classese

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
	public string sGrade = "";
    public float fReapirPower = 0;
	public float fTemperaPlus = 0;
    public float fTemperaDown = 0;
	public float fArbaitRepair = 0;
	public float fHonorPlus = 0;
	public float fGoldPlus = 0;
	public float fWaterMaxPlus = 0;
	public float fWaterChargePlus = 0;
	public float fWaterUse = 0;
	public float fCritical = 0;
	public float fCriticalDamage = 0;
	public float fBigCritical = 0;
	public float fAccuracyRate = 0;
	public int nStrenthCount = 0;
    public bool bIsEquip = false;

	public CGameEquiment(){}

	public CGameEquiment(CGameEquiment _equimentData)
	{
		nIndex = _equimentData.nIndex;
		strResource = _equimentData.strResource;
		strName = _equimentData.strName;
		bIsBuy = _equimentData.bIsBuy;
		nSlotIndex = _equimentData.nSlotIndex;
		sGrade = _equimentData.sGrade;
		fReapirPower = _equimentData.fReapirPower;
		fTemperaPlus = _equimentData.fTemperaPlus;
		fTemperaDown = _equimentData.fTemperaDown;;
		fArbaitRepair = _equimentData.fArbaitRepair;
		fHonorPlus = _equimentData.fHonorPlus;
		fGoldPlus = _equimentData.fGoldPlus;
		fWaterMaxPlus = _equimentData.fWaterMaxPlus;
		fWaterChargePlus = _equimentData.fWaterChargePlus;
		fWaterUse = _equimentData.fWaterUse;
		fCriticalDamage = _equimentData.fCriticalDamage;
		fBigCritical = _equimentData.fBigCritical;
		fAccuracyRate = _equimentData.fAccuracyRate;
		nStrenthCount = _equimentData.nStrenthCount;
		bIsEquip = _equimentData.bIsEquip;

	}

	public virtual string GetExplain(){
		return "";
	}
}


[System.Serializable]
public class CreatorWeapon
{
	public string strResource = "";
	public int nGrade = 0;				//옵션 등급

	public float fRepair = 0.0f;		//수리력
	public float fArbaitRepair = 0.0f;	//아르바이트 수리력
	public float fPlusHonorPercent = 0.0f ;//명예 추가 증가량
	public float fPlusGoldPercent = 0.0f;	//골드 추가 증가량
	public float fMaxWaterPlus = 0.0f;	//물 최대치 증가
	public float fWaterPlus = 0.0f;		//물 추가 수치
	public float fActiveWater = 0.0f;		//물 사용시 추가 증가
	public float fCriticalChance = 0.0f;	//크리티컬 찬스
	public float fCriticalDamage = 0.0f;	//크리티컬 데미지
	public float fBigSuccessed = 0.0f;	//대 성공 확률 
	public float fAccuracyRate = 0.0f;	//명중률

	public float fIceBossValue = 0.0f;
	public float fRusiuBossValue = 0.0f;
	public float fSasinBossValue = 0.0f;
	public float fFireBossValue = 0.0f;

	public int nOneLockSlotIndex = 0;
	public int nTwoLockSlotIndex = 0;
	public int nThreeLockSlotIndex = 0;
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


	//특수능력 설명 
	public string strExplains;

    //특스 능력들 증가량
    public float fSkillPercent;

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


	public ArbaitData(ArbaitData _data)
	{
		index = _data.index;

		level= _data.level;
		name = _data.name;

		grade = _data.grade;

		batch = _data.batch;

		fRepairPower = _data.fRepairPower;

		fAttackSpeed = _data.fAttackSpeed;

		fCritical = _data.fCritical;

		fAccuracyRate = _data.fAccuracyRate;

		strExplains= _data.strExplains;

		fSkillPercent= _data.fSkillPercent;

		fCurrentFloat= _data.fCurrentFloat;

		nScoutCount= _data.nScoutCount;

		nScoutGold= _data.nScoutGold;

		nScoutHonor= _data.nScoutHonor;

		nScoutDia= _data.nScoutDia;

		nIsDia= _data.nIsDia;

		bIsBuyCheck= _data.bIsBuyCheck;
	}
}

[System.Serializable]

public class CGameWeaponInfo
{
    public int nIndex = 0;
    public string strPath = string.Empty;
	public string strName = string.Empty;
	public float fMaxComplate = 0;
	public float fMinusRepair = 0;
	public float fPlusTemperature = 0;
	public float fMinusTemperature = 0;
	public float fMinusUseWater = 0f;
	public float fMinusCritical = 0f;
	public float fMinusAccuracy = 0f;
    
    public float fGold = 0.0f;
	public float fHonor = 0.0f;
	public float fLimitedTime = 0.0f;
	public int nGrade = 0;
    public Sprite WeaponSprite = null;

	public CGameWeaponInfo()
	{
	}

	public CGameWeaponInfo(CGameWeaponInfo weaponData)
	{
		nIndex = weaponData.nIndex;
		strName = weaponData.strName;
		strPath = weaponData.strPath;
		fMaxComplate = weaponData.fMaxComplate;
		fMinusRepair = weaponData.fMinusRepair;
		fPlusTemperature = weaponData.fPlusTemperature;
		fMinusTemperature = weaponData.fMinusTemperature;
		fMinusUseWater = weaponData.fMinusUseWater;
		fMinusCritical = weaponData.fMinusCritical;
		fMinusAccuracy = weaponData.fMinusAccuracy;
		fGold = weaponData.fGold;
		fHonor = weaponData.fHonor;
		fLimitedTime = weaponData.fLimitedTime;
		nGrade = weaponData.nGrade;
		WeaponSprite = weaponData.WeaponSprite;
	}
}

[System.Serializable]
public class CGameArbaitGrade
{
	public int nNextLevel;
	public int nPercentPlusRepair;
	public int nPercentPlusAccuracy;
	public int nPercentPlusCritical;
	public int nPercentPlusSkill;
	public int nGoldCost;
	public int nHonorCost;
}


[System.Serializable]
public class CGamePlayerEnhance
{
    public int nIndex;
	public string strName;
	public float fPlusPercentValue;
    public int nGoldCost;
	public int nHonorCost;
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
	public bool bIsActive = false;
    
}

[System.Serializable]
public class CGameEnhanceData
{
    public int nLevel;
    public int nPercent;
    public int nGoldCost;
    public int nHonorCost;
}

[System.Serializable]
public class CGamePlayerData
{
    public string strName;			//닉네임			
    public float fRepairPower;		//수리력 
    public float fTemperatureMinus;	//온도 감소 수치량 
    public float fArbaitsPower;		//알바수리력 
    public float fHornorPlusPercent;		//명예추가 증가량
    public float fGold;
    public float fGoldPlusPercent;		//골드추가 증가량
    public float fWaterPlus;		//물증가량
    public float fMaxWaterPlus;		//물최대치 증가량 
    public float fCriticalChance;		//크리티컬 확률
    public float fCriticalDamage;		//크리데미지
    public float fBigSuccessed;		//대성공
    public float fAccuracyRate;		//정확도
    public int nBlackSmithLevel;			//대장간 레벨
    public int nEnhanceRepaireLevel;		//수리력 레벨
    public int nEnhanceMaxWaterLevel;		//물최대치 레벨
	public int nEnhanceWaterPlusLevel;		//물 추가 증가치 레벨
	public int nEnhanceAccuracyRateLevel;	//명중률 증가 레벨
	public int nEnhanceCriticalLevel;		//크리티컬 확률 레벨
	public int nEnhanceArbaitLevel;			//아르바이트 강화 레벨
    public int nSasinMaterial;
    public int nRusiuMaterial;
    public int nIceMaterial;
    public int nFireMaterial;
    public int nDay;
	public int nMaxDay;
	public int nFaieldGuest;
	public int nSuccessedGuest;
    public int nGuestCount;

	public CGamePlayerData(CGamePlayerData playerData)
	{
		strName = playerData.strName;
		fRepairPower = playerData.fRepairPower;
		fTemperatureMinus = playerData.fTemperatureMinus;
		fArbaitsPower = playerData.fArbaitsPower;		 
		fHornorPlusPercent = playerData.fHornorPlusPercent;		
		fGold = playerData.fGold;
		fGoldPlusPercent= playerData.fGoldPlusPercent;		
		fWaterPlus= playerData.fWaterPlus;		
		fMaxWaterPlus = playerData.fMaxWaterPlus;		 
		fCriticalChance= playerData.fCriticalChance;		
		fCriticalDamage = playerData.fCriticalDamage;		
		fBigSuccessed = playerData.fBigSuccessed;		
		fAccuracyRate= playerData.fAccuracyRate;		
		nBlackSmithLevel= playerData.nBlackSmithLevel;
		nEnhanceRepaireLevel= playerData.nEnhanceRepaireLevel;
		nEnhanceMaxWaterLevel= playerData.nEnhanceMaxWaterLevel;
		nEnhanceWaterPlusLevel= playerData.nEnhanceWaterPlusLevel;
		nEnhanceAccuracyRateLevel= playerData.nEnhanceAccuracyRateLevel;
		nEnhanceCriticalLevel= playerData.nEnhanceCriticalLevel;
		nEnhanceArbaitLevel = playerData.nEnhanceArbaitLevel;
        nSasinMaterial = playerData.nSasinMaterial;
        nRusiuMaterial = playerData.nRusiuMaterial;
        nIceMaterial = playerData.nIceMaterial;
        nFireMaterial = playerData.nFireMaterial;
        nDay = playerData.nDay;
		nMaxDay = playerData.nMaxDay;
		nFaieldGuest = playerData.nFaieldGuest;
		nSuccessedGuest = playerData.nSuccessedGuest;
        nGuestCount = playerData.nGuestCount;
	}
}


[System.Serializable]
public class CGameMainWeaponOption
{
	public int nIndex = 0;
	public string strOptionName = "";
	public int nValue = 0;
	public bool bIsLock = false;
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
	public string bossName;
	//Passive 01
	public string skillExplainOne;
	//Passive 02
	public string skillExplainTwo;
	//BossWeaponName
	public string bossWeaponName;
	//Weapon reward index
	public string GetWeaponsIndex;
	//완성도
	public float fComplate;
	//골드
	public int nGold;
	//명예
	public int nHonor;
	//보석
	public int nDia;
	//장비 드랍 확률
	public float fDropPercent;
	//보스 대사 01
	public string bossWord01;
	//보스 대사 02
	public string bossWord02;
	//보스 대사 03
	public string bossWord03;
	//보스 대사 04
	public string bossWord04;
}

[System.Serializable]
public class BossWeapon : CGameEquiment
{
	public string explain;

	public override string GetExplain ()
	{
		return explain;
	}
}

[System.Serializable]
public class BossPanelInfo
{
	public bool isSaved;

	public int nBossInviteMentCount;
	public int nBossPotionCount;

	public int nBossSasinLeftCount;
	public int nBossMusicLeftCount;
	public int nBossIceLeftCount;
	public int nBossFireLeftCount;

	public int nBossInviteMentCurMin;
	public float fBossInviteMentCurSec;
	public int nBossRegenCurMin;
	public float fBossRegenCurSec;

	public int nBossSasinCurLevel;
	public int nBossMusicCurLevel;
	public int nBossIceCurLevel;
	public int nBossFireCurLevel;
}

[System.Serializable]
public class CGameSoundData
{
	public int nIndex= 0;
	public string strName ="";
	public string strResource = "";
	public int nType = 0;
	public int nVolume = 100;
	public int nLoop = 0;
}

#endregion