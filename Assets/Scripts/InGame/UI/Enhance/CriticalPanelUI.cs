using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class CriticalPanelUI : EnhanceUI {

	SmithEnhance m_EnhanceData;

	protected override void Awake ()
	{
		base.Awake ();

		nLevel = cPlayer.GetWaterPlusLevel ();

		EnhanceText.text = strEnhanceName + nLevel;

		m_EnhanceData = enhanceDatas[(int)E_SMITH_INDEX.E_CRITICAL_CHANCE];
	}

	protected override void EnhanceButtonClick ()
	{

		if (nLevel != 0 && (nLevel % 10 == 0)) 
		{
			if (ScoreManager.ScoreInstance.GetHonor() >= m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue))
			{

				ScoreManager.ScoreInstance.HonorPlus (-(m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue)));

				nLevel++;

				cPlayer.SetBasicCriticalChance(cPlayer.GetBasicCriticalChance() + 1 * m_EnhanceData.fPlusPercent);

				cPlayer.SetCriticalLevel(nLevel);

				EnhanceText.text = strEnhanceName + nLevel;
			}

			return;
		}


		if (ScoreManager.ScoreInstance.GetGold() >= m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)) {

			ScoreManager.ScoreInstance.GoldPlus (-(m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)));

			nLevel++;

			cPlayer.SetBasicCriticalChance( cPlayer.GetBasicCriticalChance() + 1 * m_EnhanceData.fPlusPercent);

			cPlayer.SetCriticalLevel(nLevel);

			EnhanceText.text = strEnhanceName + nLevel;
		}
	}
}
