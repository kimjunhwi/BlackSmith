
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_BOSSNAME 
{
	E_BOSSNAME_ICE = 0,
	E_BOSSNAME_SASIN = 1,
	E_BOSSNAME_FIRE = 2,
	E_BOSSNAME_MUSIC = 3,
}

public class BossCreator : MonoBehaviour
{
	private UIManager uiManager;
	public GameObject bossRespawnPoint;
	public BossConsumeItemInfo bossConsumeItemInfo;
	public BossBackGround bossBackGround;
	public BossPopUpWindow bossPopUpWindow;
	public GameObject bossUIDisable;
	public GameObject bossTimer_Obj;
	public GameObject bossPanel;
	public BossTalkPanel bossTalkPanel;
	public GameObject bossWeapon_Obj;

	private BossTimer bossTimer;

	public GameObject[] bossList;
	public BossElement[] bossElementList;
	public BossEffect bossEffect;

	public int nBossSasinLeftCount = 3;
	public int nBossIceLeftCount = 3;
	public int nBossFireLeftCount = 3;
	public int nBossMusicLeftCount = 3;

	public int nBossMaxLeftCount = 3;

	private int nBossIndex =0; 

	void Awake()
	{
		uiManager = FindObjectOfType<UIManager> ();
		bossTimer = bossTimer_Obj.GetComponent<BossTimer> ();
		bossUIDisable.SetActive (false);

	}

	private void OnEnable()
	{
		bossElementList[0].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossIceLeftCount, nBossMaxLeftCount);
		bossElementList[1].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossSasinLeftCount, nBossMaxLeftCount);
		bossElementList[2].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossFireLeftCount, nBossMaxLeftCount);
		bossElementList[3].BossLeftCount_Text.text = string.Format("{0} / {1}", nBossMusicLeftCount, nBossMaxLeftCount);
	}

	public void BossCreateInit(int _index)
	{
		//만약 보스가 이미 생성 됐다면 리턴
		//if (bossRespawnPoint.transform.childCount > 0)
		//	return;
		//캐릭들을 전부 되돌림 

		SpawnManager.Instance.cameraShake.Shake (0.1f, 1.0f);

		SpawnManager.Instance.AllCharacterComplate ();
		//배경화면 전환
		bossUIDisable.SetActive (true);
		nBossIndex = _index;
		bossBackGround.StartChangeBackGroundToBossBackGround ();
	
		//보스 선택 창
		bossPanel.SetActive (false);
	
		StartCoroutine (StartBossCreate ());
	}
	public IEnumerator StartBossCreate()
	{

		while (true) 
		{
			if (bossBackGround.isBossBackGround == true)
			{
				StartCoroutine (BossCreate (nBossIndex));
				yield break;
			}

			yield return null;

		}
		yield return null;
	}

	public IEnumerator BossCreate(int _index) 
	{
		


		if(bossConsumeItemInfo.nInviteMentCurCount != 0)
			bossConsumeItemInfo.nInviteMentCurCount--;

		SpawnManager.Instance.ApplyArbaitBossRepair ();

		if (_index == (int)E_BOSSNAME.E_BOSSNAME_ICE) 
		{
				
			nBossIceLeftCount--;	//해당보스의 횟수 차감

			//보스 리스트에서 해당 보스의 정보와 보스를 셋팅 한다.
				
			BossIce bossIce = bossList[0].GetComponent<BossIce> ();

			bossIce.nIndex = _index;
			bossIce.bossInfo = GameManager.Instance.bossInfo [_index];
			bossIce.bossEffect = bossEffect;
			bossIce.bossBackGround = bossBackGround;
			bossIce.bossPopUpWindow = bossPopUpWindow;
			bossIce.sBossWeaponSprite = "Weapons/Boss/deathnote";
			bossIce.bossTimer_Obj = bossTimer_Obj;
			bossIce.bossUIDisable = bossUIDisable;
			bossIce.bossTalkPanel = bossTalkPanel;
			bossIce.bossWeapon = bossWeapon_Obj;
			bossList [0].SetActive (true);
			nBossSasinLeftCount--;
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_SASIN) 
		{
	
			BossSasin bossSasin = bossList[1].GetComponent<BossSasin> ();

			bossSasin.nIndex = _index;
			bossSasin.bossInfo = GameManager.Instance.bossInfo [_index];
			bossSasin.bossEffect = bossEffect;
			bossSasin.bossBackGround = bossBackGround;
			bossSasin.bossPopUpWindow = bossPopUpWindow;
			bossSasin.sBossWeaponSprite = "Weapons/Boss/deathnote";
			bossSasin.bossTimer_Obj = bossTimer_Obj;
			bossSasin.bossTimer = bossTimer;
			bossSasin.bossUIDisable = bossUIDisable;
			bossSasin.bossTalkPanel = bossTalkPanel;
			bossSasin.bossWeapon = bossWeapon_Obj;

			bossList [1].SetActive (true);


			nBossSasinLeftCount--;
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_FIRE) 
		{
			Debug.Log ("Fire Created!!!");	
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

			bossList [3].SetActive (true);
			nBossMusicLeftCount--;
		}
		bossPanel.SetActive (true);
		uiManager.AllDisable ();

		yield break;
	}
}
