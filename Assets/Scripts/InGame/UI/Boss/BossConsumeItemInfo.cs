using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_BOSSCONSUMEINFO
{
	E_BOSSCONSUMEINFO_BOSSINVITEMENTCOUNT =0,
	E_BOSSCONSUMEINFO_BOSSSASINLEFTCOUNT,
	E_BOSSCONSUMEINFO_BOSSMUSICLEFTCOUNT,
	E_BOSSCONSUMEINFO_BOSSICELEFTCOUNT,
	E_BOSSCONSUMEINFO_BOSSFIRELEFTCOUNT,
	E_BOSSCONSUMEINFO_BOSSPOTIONCOUNT,
	E_BOSSCONSUMEINFO_BOSSINVITEMENTCURMIN,
	E_BOSSCONSUMEINFO_BOSSINVITEMNETCURSEC,
	E_BOSSCONSUMEINFO_BOSSREGENCURMIN,
	E_BOSSCONSUMEINFO_BOSSREGENCURSEC,
}


public class BossConsumeItemInfo : MonoBehaviour 
{
	public Text positionCount_Text;			//포션 개수 텍스트
	public Text inviteMentCount_Text;		//초대장 개수	텍스트
	public Text inviteMentTimer_Text;		//초대장 타이머 개수 텍스트

	public bool isTimeOn_BossInviteMentTimer;

	private string strTime ="";

	System.DateTime StartedTime = new System.DateTime();				//시작일 
	System.DateTime EndData;											//게임을 끌 때의 데이터
	System.TimeSpan timeCal;

	private int curMin;													//현재 분
	private float fCurSec;
	private int nInitTime_Min = 19;
	private int nInitTime_sec = 59;

	public int nInviteMentMaxCount = 1;									//초대장 최대 개수
	public int nInviteMentCurCount = 1;									//초대장 현재 개수
	public int nPotionCount =0;											//포션 개수
	public BossRegenTimer bossRegenTimer;
	public BossCreator bossCreator;

	public void BossInviteMentSaveTime()
	{
		EndData = System.DateTime.Now;
		PlayerPrefs.SetString ("BossInvitementSaveTime", EndData.ToString ());
		GameManager.Instance.cBossPanelListInfo [0].nBossInviteMentCurMin = curMin;
		GameManager.Instance.cBossPanelListInfo [0].fBossInviteMentCurSec = fCurSec;

		Debug.Log ("BossInvitement Time Save : " + EndData.ToString ());
	}
	public void BossInviteMentLoadTime()
	{
		//저장된 초대장 시간이 있다면
		if (PlayerPrefs.HasKey ("BossInvitementSaveTime"))
		{
			strTime = PlayerPrefs.GetString ("BossInvitementSaveTime");
			EndData = System.Convert.ToDateTime (strTime);

			Debug.Log ("BossInvitement Time Load : " + EndData.ToString ());
		} 
		//없으면 (맨 처음 최초 실행때만 호출)
		else 
		{
			PlayerPrefs.SetString ("BossInvitementSaveTime", EndData.ToString ());
			Debug.Log ("BossInvitement init Time : " + EndData.ToString ());
			inviteMentCount_Text.text = nInviteMentCurCount.ToString () + " / " + nInviteMentMaxCount.ToString ();
			inviteMentTimer_Text.enabled = false;

			curMin = 19;
			fCurSec = 59f;

			return;
		}

		StartedTime = System.DateTime.Now;
		timeCal = StartedTime - EndData;

		int nStartTime = StartedTime.Hour * 3600 + StartedTime.Minute * 60 + StartedTime.Second;
		int nEndTime = EndData.Hour * 3600 + EndData.Minute * 60 + EndData.Second;

		int nCheck = Mathf.Abs(nEndTime - nStartTime);

		//하루가 지나거나 100분이 지나거나 현재 초대장이 가득 찼으면
		if (timeCal.Days != 0 || nCheck >= 4500 || nInviteMentCurCount == nInviteMentMaxCount) 
		{
			//초대장 갯수 풀로 할것
			nInviteMentCurCount = nInviteMentMaxCount;
			isTimeOn_BossInviteMentTimer = false;
			inviteMentTimer_Text.enabled = false;
			inviteMentCount_Text.text = nInviteMentCurCount.ToString () + " / " + nInviteMentMaxCount.ToString ();
		}
		//
		else 
		{
			//StartTimer;
			//6000s
			//100m
			//1h + 40m
			int nPassedTime_Min = (int)timeCal.TotalMinutes;
			int nPassedTime_Sec = (int)timeCal.Seconds % 60;

			//15분이 지나지 않았다면 저장된 분에서 지나간 분 만큼 뺀 시간을 시작한다
			if (nPassedTime_Min < 15) 
			{

				int ResultTime_Min = GameManager.Instance.cBossPanelListInfo [0].nBossInviteMentCurMin - nPassedTime_Min;

				int ResultTime_Sec = (int)GameManager.Instance.cBossPanelListInfo [0].fBossInviteMentCurSec - nPassedTime_Sec;
				if (ResultTime_Sec < 0)
					ResultTime_Sec = -ResultTime_Sec;

				StartCoroutine (Timer (ResultTime_Min, ResultTime_Sec));
			}
			/*
			else if (nPassedTime_Min < 40)
			{
				nInviteMentCurCount += 1;
				nPassedTime_Min = (int)nPassedTime_Min - 20;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 1200) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			} 
			else if (nPassedTime_Min < 60)
			{
				nInviteMentCurCount += 2;
				nPassedTime_Min = (int)nPassedTime_Min - 40;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 2400) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 80)
			{
				nInviteMentCurCount += 3;
				nPassedTime_Min = (int)nPassedTime_Min - 60;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 3600) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec -nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 100)
			{
				nInviteMentCurCount += 4;
				nPassedTime_Min = (int)nPassedTime_Min - 80;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 4800) % 60);
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			*/

			inviteMentTimer_Text.enabled = true;
		}
	}

	public void StartBossInviteMentTimer()
	{
		gameObject.SetActive (true);
		inviteMentTimer_Text.enabled = true;
		StartCoroutine (Timer (19, 59));
	}

	public void InitBossInviteMentTimer()
	{
		inviteMentTimer_Text.enabled = true;
		gameObject.SetActive (false);
		isTimeOn_BossInviteMentTimer = false;
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
				inviteMentTimer_Text.text = curMin.ToString () + ":" +"0"+second.ToString ();
			else
				inviteMentTimer_Text.text = curMin.ToString () + ":" + second.ToString ();

			inviteMentCount_Text.text = nInviteMentCurCount.ToString () + " / " + nInviteMentMaxCount.ToString ();


			nInitTime_Min = curMin;
			nInitTime_sec = second;

			if (curMin == 0 && second == 0f)
			{
				isTimeOn_BossInviteMentTimer = true;
				//break;
				nInviteMentCurCount++;
				nInitTime_Min = 19;
				nInitTime_sec = 59;

				if (nInviteMentCurCount == nInviteMentMaxCount) 
				{
					nInitTime_Min = 19;
					nInitTime_sec = 59;
					inviteMentCount_Text.text = nInviteMentCurCount.ToString () + " / " + nInviteMentMaxCount.ToString ();
					inviteMentTimer_Text.enabled = false;
					yield break;

				}
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


