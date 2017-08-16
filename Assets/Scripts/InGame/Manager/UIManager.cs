using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{

    public GameObject obj;
	public BossCreator bossCreator;

	public GameObject []uiPanels;
	public GameObject []uiButtons;
	public GameObject yesNoButton; //BossInfo
	void Start()
	{
		AllDisable ();

		GameManager.Instance.Root_ui = gameObject;
	}

	public void ActiveMenu(int nIndex)
	{
		SoundManager.instance.PlaySound (eSoundArray.ES_TouchSound_Menu);
		if (uiPanels [nIndex].activeSelf) 
		{
			//보스 패널 닫을시 시간 저장 
			if (nIndex == 3)
			{
				//bossCreator.bossConsumeItemInfo.BossInviteMentSaveTime ();
				//bossCreator.bossConsumeItemInfo.bossRegenTimer.BossRegenTimeSave ();
				bossCreator.BossPanelInfoSave ();
				yesNoButton.SetActive (false);
			}
			uiPanels [nIndex].SetActive (false);
		

		}
		else
		{
			AllDisable ();
			uiPanels [nIndex].SetActive (true);
			yesNoButton.SetActive (false);
			//보스 패널 열시 시간 로드 
			if (nIndex == 3) 
			{
				bossCreator.BossPanelSetUp ();
				bossCreator.bossConsumeItemInfo.BossInviteMentLoadTime ();
				bossCreator.bossConsumeItemInfo.bossRegenTimer.BossRegenTimeLoad ();

			}
		
		}
	}


	public void AllDisable()
	{
		
		foreach (GameObject obj in uiPanels) 
		{
			obj.SetActive (false);	
		}
	}
		

}
