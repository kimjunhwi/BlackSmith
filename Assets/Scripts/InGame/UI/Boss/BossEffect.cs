using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BOSSEFFECT
{
	BOSSEFFECT_SASINANGRY = 0,
	BOSSEFFECT_RUCIOVOLUMEUP = 1,
	BOSSEFFECT_ICEBLLIZARD = 2,
	BOSSEFFECT_FIREANGRY = 3,
}

public class BossEffect : MonoBehaviour
{
	public GameObject[] bossEffectArray;
	public int bossEffectCount;

	void Start()
	{
		for (int nIndex = 0; nIndex < bossEffectCount; nIndex++)
			bossEffectArray [nIndex].SetActive (false);
	}

	public void ActiveEffect(BOSSEFFECT _index)
	{
		if (!bossEffectArray [(int)_index].activeSelf)
			bossEffectArray [(int)_index].SetActive (true);
		else
			bossEffectArray [(int)_index].SetActive (false);
	}
}
