
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_BOSSWORD
{
	E_BOSSWORD_BEGIN = 0,
	E_BOSSWORD_PHASE01,
	E_BOSSWORD_PHASE02,
	E_BOSSWORD_END,
}

public enum E_BOSSNAME 
{
	E_BOSSNAME_ICE = 0,
	E_BOSSNAME_SASIN = 1,
	E_BOSSNAME_FIRE = 2,
	E_BOSSNAME_MUSIC = 3,
}

public class BossCreator : MonoBehaviour
{
	
	private UIManager uiManager;					//보스 소환시 모든 UIDisalbe시 사용
	public GameObject bossRespawnPoint;				//보스 리스폰 지점
	public BossConsumeItemInfo bossConsumeItemInfo; //보스가 소비하는 아이템 정보
	public BossRegenTimer bossRegenTimer;			//보스 도전 횟수 리젠되는 시간
	public BossBackGround bossBackGround;			//보스등장시 바뀌는 배경
	public BossPopUpWindow bossPopUpWindow;			//보스 결과창
	public GameObject bossUIDisable;				//보스 등장시 아래 UI를 못쓰게 하는 패널
	public GameObject bossTimer_Obj;				//보스 시간 Obj
	public GameObject bossPanel;					//보스 리스트
	public BossTalkPanel bossTalkPanel;				//보스 말풍선
	public GameObject bossWeapon_Obj;				//보스 무기 Obj
	public UIDisable uiDisable;
	public GameObject GuestPanel; 					//손님정보 

	private BossTimer bossTimer;

	public GameObject[] bossList;					//보스 리스트
	public BossElement[] bossElementList;			//보스 리스트 원소
	public BossEffect bossEffect;					//보스 이펙트

	//각각의 보스 남은 도전 횟수
	public int nBossSasinLeftCount = 3;
	public int nBossIceLeftCount = 3;
	public int nBossFireLeftCount = 3;
	public int nBossMusicLeftCount = 3;

	public int nBossMaxLeftCount = 3;

	private int curLevel = 1;	//현재래벨
	private int maxLevel = 100;	//최대래벨
	private int minLevel = 1;	//최소래벨



	private int nBossIndex =0; 

	void Awake()
	{
		uiManager = FindObjectOfType<UIManager> ();
		bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
		bossUIDisable.SetActive (false);
	}

	public void BossPanelSetUp()
	{
		
		if (bossConsumeItemInfo.bossCreator == null)
			bossConsumeItemInfo.bossCreator = this;
		if (bossRegenTimer.bossCreator == null)
			bossRegenTimer.bossCreator = this;
		if (bossBackGround.bossCreator == null)
			bossBackGround.bossCreator = this;

		//결과창에 쓰임
		if (bossPopUpWindow.bossIce == null)
			bossPopUpWindow.bossIce = bossList[0].GetComponent<BossIce>() ;
	
		if (bossPopUpWindow.bossSasin == null)
			bossPopUpWindow.bossSasin =  bossList[1].GetComponent<BossSasin>();

	
		if (bossPopUpWindow.bossFire == null)
			bossPopUpWindow.bossFire =bossList[2].GetComponent<BossFire>();

		if (bossPopUpWindow.bossMusic == null)
			bossPopUpWindow.bossMusic = bossList[3].GetComponent<BossMusic>();

		for (int i = 0; i < 4; i++) {
			bossElementList[i].ReloadButton.onClick.AddListener(() => bossPopUpWindow.Active_YesNoWindow_BossReChargeCount("도전 횟수를 충전하시겠습니까?", i));
		}

		bossPopUpWindow.PopUpWindow_YesNo_NoButton.onClick.RemoveListener (bossPopUpWindow.PopUpWindowYesNo_Switch);
		bossPopUpWindow.PopUpWindow_YesNo_YesButton.onClick.RemoveListener (bossPopUpWindow.PopUpWindowYesNo_Switch);


		bossPopUpWindow.PopUpWindow_YesNo_NoButton.onClick.AddListener (bossPopUpWindow.PopUpWindowYesNo_Switch);
		bossPopUpWindow.PopUpWindow_YesNo_YesButton.onClick.AddListener (bossPopUpWindow.PopUpWindowYesNo_Switch);


		if (GameManager.Instance.cBossPanelListInfo[0].isSaved == true) 
		{
			Debug.Log ("Load Saved Info");

			bossElementList[0].BossLeftCount_Text.text = string.Format("{0} / {1}", GameManager.Instance.cBossPanelListInfo[0].nBossIceLeftCount, nBossMaxLeftCount);
			bossElementList[0].bossLevel_Text.text = string.Format ("Lv {0}", GameManager.Instance.cBossPanelListInfo[0].nBossIceCurLevel);
			bossElementList[1].BossLeftCount_Text.text = string.Format("{0} / {1}",  GameManager.Instance.cBossPanelListInfo[0].nBossSasinLeftCount, nBossMaxLeftCount);
			bossElementList[1].bossLevel_Text.text = string.Format ("Lv {0}", GameManager.Instance.cBossPanelListInfo[0].nBossSasinCurLevel);
			bossElementList[2].BossLeftCount_Text.text = string.Format("{0} / {1}",  GameManager.Instance.cBossPanelListInfo[0].nBossFireLeftCount, nBossMaxLeftCount);
			bossElementList[2].bossLevel_Text.text = string.Format ("Lv {0}", GameManager.Instance.cBossPanelListInfo[0].nBossFireCurLevel);
			bossElementList[3].BossLeftCount_Text.text = string.Format("{0} / {1}",  GameManager.Instance.cBossPanelListInfo[0].nBossMusicLeftCount, nBossMaxLeftCount);
			bossElementList[3].bossLevel_Text.text = string.Format ("Lv {0}", GameManager.Instance.cBossPanelListInfo[0].nBossMusicCurLevel);
			bossConsumeItemInfo.nInviteMentCurCount = GameManager.Instance.cBossPanelListInfo [0].nBossInviteMentCount;

			bossConsumeItemInfo.positionCount_Text.text = string.Format ("{0}", bossConsumeItemInfo.nPotionCount);

			//보스 도전 횟수(개개인) 이 다 됬을시 충전 버튼 활성화
			if (GameManager.Instance.cBossPanelListInfo [0].nBossIceLeftCount <= 0)
				bossElementList [0].ReloadButton_Obj.SetActive (true);
			if (GameManager.Instance.cBossPanelListInfo [0].nBossSasinLeftCount <= 0)
				bossElementList [1].ReloadButton_Obj.SetActive (true);
			if (GameManager.Instance.cBossPanelListInfo [0].nBossFireLeftCount <= 0)
				bossElementList [2].ReloadButton_Obj.SetActive (true);
			if (GameManager.Instance.cBossPanelListInfo [0].nBossMusicLeftCount <= 0)
				bossElementList [3].ReloadButton_Obj.SetActive (true);
			
			nBossSasinLeftCount = GameManager.Instance.cBossPanelListInfo [0].nBossSasinLeftCount;
			nBossIceLeftCount = GameManager.Instance.cBossPanelListInfo [0].nBossIceLeftCount;
			nBossFireLeftCount = GameManager.Instance.cBossPanelListInfo [0].nBossFireLeftCount;
			nBossMusicLeftCount = GameManager.Instance.cBossPanelListInfo [0].nBossMusicLeftCount;
			
		}
		else
		{
			Debug.Log ("Load Init Info");
	
			bossElementList[0].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossIceLeftCount, nBossMaxLeftCount);
			bossElementList[0].bossLevel_Text.text = string.Format ("Lv {0}", minLevel);
			bossElementList[1].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossSasinLeftCount, nBossMaxLeftCount);
			bossElementList[1].bossLevel_Text.text = string.Format ("Lv {0}", minLevel);
			bossElementList[2].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossFireLeftCount, nBossMaxLeftCount);
			bossElementList[2].bossLevel_Text.text = string.Format ("Lv {0}", minLevel);
			bossElementList[3].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossMusicLeftCount, nBossMaxLeftCount);
			bossElementList[3].bossLevel_Text.text = string.Format ("Lv {0}", minLevel);

			bossConsumeItemInfo.nInviteMentCurCount = bossConsumeItemInfo.nInviteMentMaxCount;
			bossConsumeItemInfo.nPotionCount = 0;
			bossConsumeItemInfo.positionCount_Text.text = string.Format ("{0}", bossConsumeItemInfo.nPotionCount);
		}

	}


	public void BossCreateInit()
	{
		Debug.Log ("보스 CreateInit");
		uiDisable.isBossSummon = true;

		//ChangeSound
		SoundManager.instance.ChangeBGM(eSoundArray.BGM_Main, eSoundArray.BGM_BossBattle);

		//bossWeaponBlock_Obj.SetActive (true);
		SpawnManager.Instance.cameraShake.Shake (0.1f, 1.0f);

		SpawnManager.Instance.AllCharacterComplate ();

		//보스 선택 창
		bossPanel.SetActive (false);

		while (SpawnManager.Instance.bIsBossCreate == false)
		{
			Debug.Log ("손님들 들어가는중...");
		}
		//배경화면 전환
		bossUIDisable.SetActive (true);
		nBossIndex = bossPopUpWindow.nBossIndex;
		bossBackGround.StartChangeBackGroundToBossBackGround ();

		StartBossCreateInit ();
	

	}
	public void StartBossCreateInit()
	{
		StartCoroutine (BossCreate (nBossIndex));
	}


	public IEnumerator BossCreate(int _index) 
	{
		yield return null;

		if(bossConsumeItemInfo.nInviteMentCurCount != 0)
			bossConsumeItemInfo.nInviteMentCurCount--;

		SpawnManager.Instance.ApplyArbaitBossRepair ();

		if (_index == (int)E_BOSSNAME.E_BOSSNAME_ICE) 
		{
			//보스 리스트에서 해당 보스의 정보와 보스를 셋팅 한다.
			BossIce bossIce = bossList[0].GetComponent<BossIce> ();

			bossIce.nIndex = _index;
			bossIce.bossInfo = GameManager.Instance.bossInfo [_index];
			bossIce.bossEffect = bossEffect;
			bossIce.bossBackGround = bossBackGround;
			bossIce.bossPopUpWindow = bossPopUpWindow;
			bossIce.sBossWeaponSprite = "Weapons/Boss/BOSS1_Weapon";
			bossIce.bossTimer_Obj = bossTimer_Obj;
			bossIce.bossUIDisable = bossUIDisable;
			bossIce.bossTalkPanel = bossTalkPanel;
			bossIce.bossWeapon = bossWeapon_Obj;
			bossIce.uiDisable = uiDisable;
			bossIce.uiManager = uiManager;
			bossIce.bossPanel = bossPanel;
			bossIce.nCurLevel = bossElementList [0].curLevel;
			bossIce.GuestPanel = GuestPanel;

			bossIce.bossWord [(int)E_BOSSWORD.E_BOSSWORD_BEGIN] = "저... 무기좀... 고쳐주세요";
			bossIce.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE01] = "흐으음~~~";
			bossIce.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE02] = "눈보라 ~~~!";
			bossIce.bossWord [(int)E_BOSSWORD.E_BOSSWORD_END] = "그럼 이만!";

			bossList [0].SetActive (true);

			nBossIceLeftCount--;
			//사
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_SASIN) 
		{
			BossSasin bossSasin = bossList[1].GetComponent<BossSasin> ();
		
			bossSasin.nIndex = _index;
			bossSasin.bossInfo = GameManager.Instance.bossInfo [_index];
			bossSasin.bossEffect = bossEffect;
			bossSasin.bossBackGround = bossBackGround;
			bossSasin.bossPopUpWindow = bossPopUpWindow;
			bossSasin.sBossWeaponSprite = "Weapons/Boss/BOSS1_Weapon";
			bossSasin.bossTimer_Obj = bossTimer_Obj;
			bossSasin.bossTimer = bossTimer;
			bossSasin.bossUIDisable = bossUIDisable;
			bossSasin.bossTalkPanel = bossTalkPanel;
			bossSasin.bossWeapon = bossWeapon_Obj;
			bossSasin.uiDisable = uiDisable;
			bossSasin.uiManager = uiManager;
			bossSasin.bossPanel = bossPanel;
			bossSasin.nCurLevel = bossElementList [1].curLevel;
			bossSasin.GuestPanel = GuestPanel;

			bossSasin.bossWord [(int)E_BOSSWORD.E_BOSSWORD_BEGIN] = "내가 사신이지롱";
			bossSasin.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE01] = "나 화났어 ㅡ,ㅡ";
			bossSasin.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE02] = "뿌우우우우우!!!";
			bossSasin.bossWord [(int)E_BOSSWORD.E_BOSSWORD_END] = "꾸앙 ㅇㅁㅇ...";

			bossList [1].SetActive (true);

			nBossSasinLeftCount--;
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_FIRE) 
		{
			Debug.Log ("Fire Created!!!");	

			BossFire bossFire = bossList[2].GetComponent<BossFire> ();


			

			bossFire.nIndex = _index;
			bossFire.bossInfo = GameManager.Instance.bossInfo [_index];
			bossFire.bossEffect = bossEffect;
			bossFire.bossBackGround = bossBackGround;
			bossFire.bossPopUpWindow = bossPopUpWindow;
			bossFire.sBossWeaponSprite = "Weapons/Boss/deathnote";
			bossFire.bossTimer_Obj = bossTimer_Obj;
			bossFire.bossTimer = bossTimer;
			bossFire.bossUIDisable = bossUIDisable;
			bossFire.bossTalkPanel = bossTalkPanel;
			bossFire.bossWeapon = bossWeapon_Obj;
			bossFire.uiDisable = uiDisable;
			bossFire.uiManager = uiManager;
			bossFire.bossPanel = bossPanel;
			bossFire.nCurLevel = bossElementList [2].curLevel;
			bossFire.GuestPanel = GuestPanel;

			bossFire.bossWord [(int)E_BOSSWORD.E_BOSSWORD_BEGIN] = "Fire~~~";
			bossFire.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE01] = "흐으음~~~";
			bossFire.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE02] = "파이어 ~~~!";
			bossFire.bossWord [(int)E_BOSSWORD.E_BOSSWORD_END] = "Bye~~~!";

			bossList [2].SetActive (true);


			nBossFireLeftCount--;

		
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) 
		{
			BossMusic bossMusic = bossList[3].GetComponent<BossMusic> ();

	

			bossMusic.nIndex = _index;
			bossMusic.bossInfo = GameManager.Instance.bossInfo [_index];
			bossMusic.bossEffect = bossEffect;
			bossMusic.bossBackGround = bossBackGround;
			bossMusic.bossPopUpWindow = bossPopUpWindow;
			bossMusic.sBossWeaponSprite = "Weapons/Boss/deathnote";
			bossMusic.bossTimer_Obj = bossTimer_Obj;
			bossMusic.bossUIDisable = bossUIDisable;
			bossMusic.bossTalkPanel = bossTalkPanel;
			bossMusic.bossWeapon = bossWeapon_Obj;
			bossMusic.uiDisable = uiDisable;
			bossMusic.uiManager = uiManager;
			bossMusic.bossPanel = bossPanel;
			bossMusic.nCurLevel = bossElementList [3].curLevel;
			bossMusic.GuestPanel = GuestPanel;

			bossMusic.bossWord [(int)E_BOSSWORD.E_BOSSWORD_BEGIN] = "소리 질러~!";
			bossMusic.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE01] = "Whoh~";
			bossMusic.bossWord [(int)E_BOSSWORD.E_BOSSWORD_PHASE02] = "Drop the beat~!";
			bossMusic.bossWord [(int)E_BOSSWORD.E_BOSSWORD_END] = "SeeYa!";

			bossList [3].SetActive (true);
			nBossMusicLeftCount--;


		}
		BossPanelInfoSave ();
	}

	public void BossPanelInfoSave()
	{
		Debug.Log ("Save BossPanel Info!!");
		GameManager.Instance.cBossPanelListInfo [0].isSaved = true;

		GameManager.Instance.cBossPanelListInfo [0].nBossMusicLeftCount = nBossMusicLeftCount;
		GameManager.Instance.cBossPanelListInfo [0].nBossFireLeftCount = nBossFireLeftCount;
		GameManager.Instance.cBossPanelListInfo [0].nBossSasinLeftCount = nBossSasinLeftCount;
		GameManager.Instance.cBossPanelListInfo [0].nBossIceLeftCount = nBossIceLeftCount;
		GameManager.Instance.cBossPanelListInfo [0].nBossInviteMentCount = 	bossConsumeItemInfo.nInviteMentCurCount;

		GameManager.Instance.cBossPanelListInfo [0].nBossPotionCount = bossConsumeItemInfo.nPotionCount;

		GameManager.Instance.cBossPanelListInfo [0].nBossIceCurLevel = bossElementList [0].curLevel;
		GameManager.Instance.cBossPanelListInfo [0].nBossSasinCurLevel = bossElementList [1].curLevel;
		GameManager.Instance.cBossPanelListInfo [0].nBossFireCurLevel = bossElementList [2].curLevel;
		GameManager.Instance.cBossPanelListInfo [0].nBossMusicCurLevel = bossElementList [3].curLevel;

		bossConsumeItemInfo.BossInviteMentSaveTime ();
		bossRegenTimer.BossRegenTimeSave ();

		GameManager.Instance.SaveBossPanelInfoList ();
	}

	public void BossChanllengeCountToMax()
	{
		nBossMusicLeftCount = nBossMaxLeftCount;
		nBossIceLeftCount = nBossMaxLeftCount;
		nBossFireLeftCount = nBossMaxLeftCount;
		nBossSasinLeftCount = nBossMaxLeftCount;
	}

}
