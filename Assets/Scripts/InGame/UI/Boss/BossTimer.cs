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

	public void StartTimer(float _Min, float _Sec)
	{
		StartCoroutine (Timer (_Min, _Sec));
	}

	public IEnumerator Timer(float _curMin, float _curSec)
	{
		float curMin = _curMin;
		float curSecond = _curSec;
		int second = 0;
		//isTimeOn = false;
		while (curMin >= 0f) 
		{
			curSecond -= Time.deltaTime;
			second = (int)curSecond;
			bossTimer.text = curMin.ToString () + ":" + second.ToString ();


			if (curMin != 0 && second == 0f) 
			{
				curSecond = 60f;
				curMin--;
			}

			if (curMin == 0 && second == 0f)
			{
				bossTimer.text = "";
				bossSasin.FailState ();
				break;
			}

			yield return null;
		}
		yield  break;
	}
}
