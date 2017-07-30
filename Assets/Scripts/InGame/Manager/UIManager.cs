using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{

    public GameObject obj;
	public BossConsumeItemInfo bossConsumeItemInfo;

	public GameObject []uiPanels;
	public GameObject []uiButtons;
	void Start()
	{
		AllDisable ();

		GameManager.Instance.Root_ui = gameObject;
	}

	public void ActiveMenu(int nIndex)
	{
		
		if (uiPanels [nIndex].activeSelf) 
		{
			//보스 패널 닫을시 시간 저장 
			if (nIndex == 3)
			{
				
				bossConsumeItemInfo.BossInviteMentSaveTime ();

			}
			uiPanels [nIndex].SetActive (false);

		}
		else
		{
			AllDisable ();
			//보스 패널 열시 시간 로드 
			if (nIndex == 3) 
			{
				bossConsumeItemInfo.BossInviteMentLoadTime ();
			}
			uiPanels [nIndex].SetActive (true);
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
