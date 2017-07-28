using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossConsumeItemInfo : MonoBehaviour 
{
	public Text positionCount_Text;
	public Text inviteMentCount_Text;
	public Text inviteMentTimer_Text;

	private bool isTimeOn;

	private string strTime ="";

	System.DateTime StartedTime = new System.DateTime();				//시작일 
	System.DateTime EndData;											//게임을 끌 때의 데이터
	System.TimeSpan timeCal;

	private int nInviteMentMaxCount = 5;								//초대장 최대 개수
	public int nInviteMentCurCount = 5;									//초대장 현재 개수

	private int curMin;													//현재 분
	private int nInitTime_Min = 19;
	private int nInitTime_sec = 59;


	void Awake()
	{
		inviteMentCount_Text.text = "";
		curMin = 19;

	}

	public void BossInviteMentSaveTime()
	{
		EndData = System.DateTime.Now;
		PlayerPrefs.SetString ("BossInvitementSaveTime", EndData.ToString ());
		Debug.Log ("BossPanel Time Save : " + EndData.ToString ());
	}
	public void BossInviteMentLoadTime()
	{

		if (PlayerPrefs.HasKey ("BossInvitementSaveTime"))
		{
			strTime = PlayerPrefs.GetString ("BossInvitementSaveTime");
			EndData = System.Convert.ToDateTime (strTime);
		}
		else
			PlayerPrefs.SetString ("BossInvitementSaveTime", EndData.ToString ());


		Debug.Log ("BossPanel Time Load : " + EndData.ToString ());
		StartedTime = System.DateTime.Now;

		timeCal = StartedTime - EndData;

		int nStartTime = StartedTime.Hour * 3600 + StartedTime.Minute * 60 + StartedTime.Second;
		int nEndTime = EndData.Hour * 3600 + EndData.Minute * 60 + EndData.Second;

		int nCheck = Mathf.Abs(nEndTime - nStartTime);

		//하루가 지나거나 100분이 지나거나 현재 초대장이 가득 찼으면
		if (timeCal.Days != 0 || nCheck >= 6000 || nInviteMentCurCount == nInviteMentMaxCount) 
		{
			//초대장 갯수 풀로 할것
			nInviteMentCurCount = nInviteMentMaxCount;
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

			if (nPassedTime_Min < 20) 
			{
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
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


			inviteMentTimer_Text.enabled = true;
		}
	}



	public IEnumerator Timer(int _curMin, int _curSec)
	{
		int second = 0;

		float fCurSec = (float)_curSec;
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
				//isTimeOn = true;
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
