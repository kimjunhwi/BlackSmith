using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour {

	private Text bossTimer;
	public float fTimer_min = 0f;
	public float fTime_sec = 0f;
	public BossSasin bossSasin;
	public BossMusic bossMusic;
	public BossIce bossIce;

	void Start () 
	{
		bossTimer = gameObject.GetComponentInChildren<Text> ();
		bossTimer.text = "";
		gameObject.SetActive (false);
	}

	public void StartTimer(float _Min, float _Sec, int _nBossIndex)
	{
		StartCoroutine (Timer (_Min, _Sec, _nBossIndex));
	}

	public IEnumerator Timer(float _curMin, float _curSec, int _nBossIndex)
	{
		float curMin = _curMin;
		float curSecond = _curSec;
		int second = 0;
		//isTimeOn = false;
		while (curMin >= 0f) 
		{
			curSecond -= Time.deltaTime;
			second = (int)curSecond;
			if(second >= 10)
				bossTimer.text = curMin.ToString () + ":" + second.ToString ();
			else
				bossTimer.text = curMin.ToString () + " : " + "0" +second.ToString ();

			if (curMin == 0 && second == 0f)
			{
				bossTimer.text = "";
				if(_nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_SASIN)
					bossSasin.FailState ();
				if(_nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC)
					bossMusic.FailState ();
				if (_nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_ICE)
					bossIce.FailState ();

				break;
			}

			if (curMin != 0 && second == 0f) 
			{
				curSecond = 60f;
				curMin--;
			}



			yield return null;
		}
		yield  break;
	}
}
