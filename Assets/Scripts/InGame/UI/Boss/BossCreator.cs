using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum E_BOSSNAME 
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

	void Awake()
	{
		spawnManager = FindObjectOfType<SpawnManager> ();
		uiManager = FindObjectOfType<UIManager> ();
	}

	public void BossCreateInit(int _index)
	{
		//만약 보스가 이미 생성 됐다면 리턴
		if (bossRespawnPoint.transform.childCount > 0)
			return;
		//캐릭들을 전부 되돌림 
		spawnManager.AllCharacterComplate ();

		//1초 뒤에 생성
		//Invoke ("BossCreate", 1.0f);
		StartCoroutine(BossCreate(_index));
	}
	public IEnumerator BossCreate(int _index) 
	{
		yield return new WaitForSeconds(1.0f);

		if(bossConsumeItemInfo.nInviteMentCurCount != 0)
			bossConsumeItemInfo.nInviteMentCurCount--;

		if (_index == (int)E_BOSSNAME.E_BOSSNAME_ICE) 
		{
			
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_SASIN) 
		{
			GameObject bossInstance = (GameObject)Instantiate (Resources.Load ("Prefabs/BossSasin"));
			bossInstance.transform.SetParent (bossRespawnPoint.transform);
			bossInstance.transform.position = bossRespawnPoint.gameObject.transform.position;
			//bossInstance.AddComponent<BossSasin> ();
			BossSasin bossSasin = bossInstance.GetComponent<BossSasin> ();
			bossSasin.nIndex = _index;
			bossSasin.boss = GameManager.Instance.bossInfo [_index];
			SpawnManager.Instance.AllCharacterComplate ();

		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_FIRE) 
		{
			
		}

		else if (_index == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) 
		{
	
		}

		uiManager.AllDisable ();
	}
}
