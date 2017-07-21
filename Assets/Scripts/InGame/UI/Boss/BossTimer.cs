using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour {

	private Text bossTimer;
	private float fBossIndex = 0f;
	public float fTimer_min = 0f;
	public float fTime_sec = 0f;


	public BossSasin bossSasin;
	public BossMusic bossMusic;
	public BossIce bossIce;
	public BossFire bossFire;

	void Start () 
	{
		bossTimer = gameObject.GetComponentInChildren<Text> ();
		bossTimer.text = "";
		gameObject.SetActive (false);
	}
	public void StopTimer(float _Min, float _Sec, int _nBossIndex)
	{
		StopCoroutine (Timer (_Min, _Sec, _nBossIndex));
	}

	public void StartTimer(float _Min, float _Sec, int _nBossIndex)
	{
		StartCoroutine (Timer (_Min, _Sec, _nBossIndex));
	}

	public IEnumerator Timer(float _curMin, float _curSec, int _nBossIndex)
	{
		fBossIndex = _nBossIndex;
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
				if(fBossIndex == (int)E_BOSSNAME.E_BOSSNAME_SASIN)
					bossSasin.FailState ();
				if(fBossIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC)
					bossMusic.FailState ();
				if (fBossIndex == (int)E_BOSSNAME.E_BOSSNAME_ICE)
					bossIce.FailState ();
				if (fBossIndex == (int)E_BOSSNAME.E_BOSSNAME_FIRE)
					bossFire.FailState ();

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
