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
	private SpawnManager spawnManager;
	public GameObject bossRespawnPoint;
	public BossConsumeItemInfo bossConsumeItemInfo;
	public BossBackGround bossBackGround;

	private int nBossIndex =0; 

	void Awake()
	{
		spawnManager = FindObjectOfType<SpawnManager> ();
		uiManager = FindObjectOfType<UIManager> ();
	}

	void Update()
	{
		if (bossBackGround.isBossBackGround == true)
		{
			StartBossCreate ();
			bossBackGround.isBossBackGround = false;
		}
	}

	public void BossCreateInit(int _index)
	{
		//만약 보스가 이미 생성 됐다면 리턴
		if (bossRespawnPoint.transform.childCount > 0)
			return;
		//캐릭들을 전부 되돌림 
		SpawnManager.Instance.AllCharacterComplate ();
		//배경화면 전환
		bossBackGround.StartChangeBackGroundToBossBackGround ();
		bossBackGround.isBossBackGround = true;
		nBossIndex = _index;
	}
	public void StartBossCreate()
	{
		StartCoroutine(BossCreate(nBossIndex));
	}

	public IEnumerator BossCreate(int _index) 
	{
		yield return new WaitForSeconds(0.2f);

		if(bossConsumeItemInfo.nInviteMentCurCount != 0)
			bossConsumeItemInfo.nInviteMentCurCount--;

		if (_index == (int)E_BOSSNAME.E_BOSSNAME_ICE) 
		{
			Debug.Log ("Ice Created!!!");	
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_SASIN) 
		{
			
			GameObject bossInstance = (GameObject)Instantiate (Resources.Load ("Prefabs/BossCharacterPrefab/BossSasin"));
			bossInstance.transform.SetParent (bossRespawnPoint.transform);
			bossInstance.transform.position = bossRespawnPoint.gameObject.transform.position;
			bossInstance.name = "Sasin";
			//bossInstance.AddComponent<BossSasin> ();
			BossSasin bossSasin = bossInstance.GetComponent<BossSasin> ();
			bossSasin.nIndex = _index;
			bossSasin.boss = GameManager.Instance.bossInfo [_index];


		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_FIRE) 
		{
			Debug.Log ("Fire Created!!!");	
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) 
		{
			GameObject bossInstance = (GameObject)Instantiate (Resources.Load ("Prefabs/BossCharacterPrefab/BossMusic"));
			bossInstance.transform.SetParent (bossRespawnPoint.transform);
			bossInstance.transform.position = bossRespawnPoint.gameObject.transform.position;
			//bossInstance.AddComponent<BossSasin> ();
			BossMusic bossMusic = bossInstance.GetComponent<BossMusic> ();
			bossMusic.nIndex = _index;
			bossMusic.boss = GameManager.Instance.bossInfo [_index];
			SpawnManager.Instance.AllCharacterComplate ();
		}

		uiManager.AllDisable ();
	}
}
