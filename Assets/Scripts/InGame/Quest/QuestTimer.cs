using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTimer : MonoBehaviour 
{
	public Text QuestTimer_Text;

	private string strTime ="";

	System.DateTime StartedTime = new System.DateTime();				//시작일 
	System.DateTime EndData;											//게임을 끌 때의 데이터
	System.TimeSpan timeCal;

	public float fCurSec;
	public int curMin;													//현재 분
	private int nInitTime_Min = 179;
	private int nInitTime_sec = 59;

	public bool isTimeOn = false;				//시간이 켜져 있는지 아닌지
	public bool isTimeEnd = false;				//시간이 끝났는지 아닌지

	public QusetManager questManager;

	public void SaveTime()
	{
		System.DateTime EndData = System.DateTime.Now;
		PlayerPrefs.SetString ("EndSaveTime", EndData.ToString ());
		PlayerPrefs.Save ();
		Debug.Log ("EndTime :" + EndData.ToString ());
	}


	//초기화 시간이 지났는지
	public bool checkIsTimeGone()
	{
		if (PlayerPrefs.HasKey ("EndSaveTime"))
		{
			strTime = PlayerPrefs.GetString ("EndSaveTime");
			EndData = System.Convert.ToDateTime (strTime);
		}
		StartedTime = System.DateTime.Now;
		Debug.Log ("StartTime :"+ StartedTime + " / EndTime :" + EndData);
		timeCal = StartedTime - EndData;

		int nStartTime = StartedTime.Hour * 3600 + StartedTime.Minute * 60 + StartedTime.Second;
		int nEndTime = EndData.Hour * 3600 + EndData.Minute * 60 + EndData.Second;

		int nCheck = Mathf.Abs(nEndTime - nStartTime);

		//하루가 지나거나 100분이 지나면 그냥 현재 퀘스트 개수만 띄워준다.
		if (timeCal.Days != 0 || nCheck >= 6000) {
			//QuestTimer_Text.text = questManager.nQuestCount.ToString () + " / " + questManager.nQuestMaxCount.ToString ();
			return true;
		} else
			return false;
	}

	public void LoadTime()
	{
		if (PlayerPrefs.HasKey ("EndSaveTime"))
		{
			strTime = PlayerPrefs.GetString ("EndSaveTime");
			EndData = System.Convert.ToDateTime (strTime);
		}
		StartedTime = System.DateTime.Now;
		Debug.Log ("StartTime :"+ StartedTime + " / EndTime :" + EndData);
		timeCal = StartedTime - EndData;

		int nStartTime = StartedTime.Hour * 3600 + StartedTime.Minute * 60 + StartedTime.Second;
		int nEndTime = EndData.Hour * 3600 + EndData.Minute * 60 + EndData.Second;

		int nCheck = Mathf.Abs(nEndTime - nStartTime);

		//하루가 지나거나 100분이 지나면 그냥 현재 퀘스트 개수만 띄워준다.
		if (timeCal.Days != 0 || nCheck >= 6000 ) 
		{
			//QuestTimer_Text.text = questManager.nQuestCount.ToString () + " / " + questManager.nQuestMaxCount.ToString ();
			QuestTimer_Text.enabled = false;
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
			isTimeOn = true;
			if (nPassedTime_Min < 20) 
			{
				Debug.Log ("Start Timer");
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 40)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 20;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 1200) % 60);
				Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			} 
			else if (nPassedTime_Min < 60)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 40;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 2400) % 60);
				Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 80)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 60;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 3600) % 60);
				Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec -nPassedTime_Sec));
			}
			else if (nPassedTime_Min < 100)
			{
				nPassedTime_Min = (int)nPassedTime_Min - 80;
				nPassedTime_Sec = (int)((nPassedTime_Sec - 4800) % 60);
				Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
				StartCoroutine (Timer (nInitTime_Min - nPassedTime_Min, nInitTime_sec - nPassedTime_Sec));
			}
		
			QuestTimer_Text.enabled = true;
		}
	}

	public void StartQuestTimer()
	{
		gameObject.SetActive (true);
		QuestTimer_Text.enabled = true;
		isTimeOn = true;
		nInitTime_Min = 179;
		nInitTime_sec = 59;
		Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
		StartCoroutine (Timer (nInitTime_Min, nInitTime_sec));
	}
	public void InitQuestTimer()
	{
		nInitTime_Min = 179;
		nInitTime_sec = 59;
		Debug.Log ("QuestTimer is On : " + isTimeOn + " Start Timer !");
		QuestTimer_Text.enabled = false;
		isTimeOn = false;
		gameObject.SetActive (false);
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
				QuestTimer_Text.text = curMin.ToString () + ":" +"0"+second.ToString ();
			else
				QuestTimer_Text.text = curMin.ToString () + ":" + second.ToString ();

			//inviteMentCount_Text.text = nInviteMentCurCount.ToString () + " / " + nInviteMentMaxCount.ToString ();


			nInitTime_Min = curMin;
			nInitTime_sec = second;

			if (curMin == 0 && second == 0f)
			{
				isTimeEnd = true;
				//break;
				curMin = 0;
				fCurSec = 10;
				//시간이 다되면 자동으로 갱신
				questManager.QuestInit ();
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
