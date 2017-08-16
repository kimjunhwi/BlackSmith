﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class SmithPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	SmithEnhance m_EnhanceData;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cSmithEnhaceInfo;

		nLevel = cPlayer.GetSmithLevel ();

		EnhanceText.text = strEnhanceName + nLevel;

		m_EnhanceData = enhanceDatas[(int)E_SMITH_INDEX.E_SMITH];
	}

	protected override void EnhanceButtonClick ()
	{
		if (nLevel != 0 && (nLevel % 10 == 0)) 
		{
			if (ScoreManager.ScoreInstance.GetHonor() >= m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue))
			{

				ScoreManager.ScoreInstance.HonorPlus (-(m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue)));

				nLevel++;

				cPlayer.SetBasicBigSuccessedPercent(cPlayer.GetBasicBigSuccessedPercent() + nLevel * m_EnhanceData.fPlusPercent);

				cPlayer.SetSmithLevel(nLevel);

				EnhanceText.text = strEnhanceName + nLevel;
			}

			return;
		}


		if (ScoreManager.ScoreInstance.GetGold() >= m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)) {

			ScoreManager.ScoreInstance.GoldPlus (-(m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)));

			nLevel++;

			cPlayer.SetSmithLevel(nLevel);

			EnhanceText.text = strEnhanceName + nLevel;
		}
	}
}
