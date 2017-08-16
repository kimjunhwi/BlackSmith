using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRegenTimer : MonoBehaviour 
{
	public Text bossRegenTimerText;

	private bool isTimeOn;
	private string strTime ="";

	System.DateTime StartedTime = new System.DateTime();				//시작일 
	System.DateTime EndData;											//게임을 끌 때의 데이터
	System.TimeSpan timeCal;



	private int curMin;													//현재 분
	private float fCurSec;
	private int nInitTime_Min = 119;
	private int nInitTime_sec = 59;

	public BossCreator bossCreator;

	public void BossRegenTimeSave()
	{
		EndData = System.DateTime.Now;
		PlayerPrefs.SetString ("BossRegenTime", EndData.ToString ());
		GameManager.Instance.cBossPanelListInfo [0].nBossRegenCurMin = curMin;
		GameManager.Instance.cBossPanelListInfo [0].fBossRegenCurSec = fCurSec;
		Debug.Log ("BossRegen Time Save : " + EndData.ToString ());
	}


	public void BossRegenTimeLoad()
	{
		//BossInvitementSaveTime
		//저장된 초대장 시간이 있다면
		if (PlayerPrefs.HasKey ("BossRegenTime"))
		{
			strTime = PlayerPrefs.GetString ("BossRegenTime");
			EndData = System.Convert.ToDateTime (strTime);

			Debug.Log ("BossRegen Time Load : " + EndData.ToString ());
		} 
		//없으면
		else 
		{
			EndData = System.DateTime.Now;
			PlayerPrefs.SetString ("BossRegenTime", EndData.ToString ());
			Debug.Log ("BossRegen init Time : " + EndData.ToString ());
			StartCoroutine (Timer (119, 59)); 
			return;
		}

		StartedTime = System.DateTime.Now;
		timeCal = StartedTime - EndData;

		int nStartTime = StartedTime.Hour * 3600 + StartedTime.Minute * 60 + StartedTime.Second;
		int nEndTime = EndData.Hour * 3600 + EndData.Minute * 60 + EndData.Second;
		int nCheck = Mathf.Abs(nEndTime - nStartTime);

		//하루가 지나거나 2시간이 지나거나 현재 초대장이 가득 찼으면
		if (timeCal.Days != 0 || nCheck >= 7200) 
		{
			Debug.Log ("BossChanllege ReFill");
			//남은시간을 계산해서 계속 해서 흐른다
			bossRegenTimerText.enabled = false;
			//지난 분에서 전체 분을 나머지 계산해서 구한다
			int nPassedTime_Min = (int)timeCal.TotalMinutes % 20;	//전체 분
			int nPassedTime_Sec = (int)timeCal.Seconds % 60; 		//전채 초에서 나머지

			//bossCreator.BossChanllengeCountToMax ();

			StartCoroutine (Timer (nPassedTime_Min, nPassedTime_Sec));

		}
		//
		else 
		{
			//StartTimer;
			//6000s
			//100m
			//1h + 40m
			int nPassedTime_Min = (int)timeCal.TotalMinutes;	//전체 분s
			int nPassedTime_Sec = (int)timeCal.Seconds % 60; 	//전채 초에서 나머지

			//20분이 지나지 않았다면 저장된 분에서 지나간 분 만큼 뺀 시간을 시작한다
			if (nPassedTime_Min < 119) 
			{
				int ResultTime_Min = GameManager.Instance.cBossPanelListInfo [0].nBossRegenCurMin - nPassedTime_Min;

				int ResultTime_Sec = (int)GameManager.Instance.cBossPanelListInfo [0].fBossRegenCurSec - nPassedTime_Sec;
				if (ResultTime_Sec < 0)
					ResultTime_Sec = -ResultTime_Sec;


				StartCoroutine (Timer (ResultTime_Min, ResultTime_Sec));
			} 
			/*
			else
			{
				int ResultTime_Min = (nPassedTime_Min - GameManager.Instance.cBossPanelListInfo [0].nBossRegenCurMin) % 20;
				int ResultTime_Sec = (nPassedTime_Sec - GameManager.Instance.cBossPanelListInfo [0].nBossRegenCurMin);
				if (ResultTime_Sec < 0)
					ResultTime_Sec = -(ResultTime_Sec);

				StartCoroutine (Timer (ResultTime_Min, ResultTime_Sec));
			}
			*/

			/*
			else if (nPassedTime_Min < 40)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 20;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 1200) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			} 
			else if (nPassedTime_Min < 60)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 40;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 2400) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 80)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 60;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 3600) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec -nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 100)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 80;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 4800) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			*/

			bossRegenTimerText.enabled = true;
		}
	}



	public IEnumerator Timer(int _curMin, int _curSec)
	{
		int second = 0;

		fCurSec = (float)_curSec;
		curMin = _curMin;


		while (curMin >= 0f) 
		{
			fCurSec -= Time.deltaTime;
			second = (int)fCurSec;

			if(second < 10)
				bossRegenTimerText.text = curMin.ToString () + ":" +"0"+second.ToString ();
			else
				bossRegenTimerText.text = curMin.ToString () + ":" + second.ToString ();
			


			nInitTime_Min = curMin;
			nInitTime_sec = second;

			if (curMin == 0 && second == 0f)
			{
				//isTimeOn = true;
				//break;

				nInitTime_Min = 19;
				nInitTime_sec = 59;
				break;
			}

			if (curMin != 0 && second == 0f) 
			{
				fCurSec = 59f;
				curMin--;
			}
				
			yield return null;
		}
		yield  break;
	}


}
