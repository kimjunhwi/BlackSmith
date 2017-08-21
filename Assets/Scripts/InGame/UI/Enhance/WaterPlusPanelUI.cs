using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class WaterPlusPanelUI : EnhanceUI {

	SmithEnhance m_EnhanceData;


	protected override void Awake ()
	{
		base.Awake ();

		nLevel = cPlayer.GetWaterPlusLevel ();

		EnhanceText.text = strEnhanceName + nLevel;

		m_EnhanceData = enhanceDatas[(int)E_SMITH_INDEX.E_WATER_CHARGING];
	}

	protected override void EnhanceButtonClick ()
	{

		if (nLevel != 0 && (nLevel % 10 == 0)) 
		{
			if (ScoreManager.ScoreInstance.GetHonor() >= m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue))
			{

				ScoreManager.ScoreInstance.HonorPlus (-(m_EnhanceData.fBasicHonor + (nLevel * m_EnhanceData.fPlusHonorValue)));

				nLevel++;

				cPlayer.SetBasicWaterPlus(cPlayer.GetBasicWaterPlus() + 1 * m_EnhanceData.fPlusPercent);

				cPlayer.SetWaterPlusLevel(nLevel);

				EnhanceText.text = strEnhanceName + nLevel;

				//물 최대 량증가
				//repairObject.fMaxWater =  GameManager.Instance.player.GetBasicMaxWaterPlus() + ( (cPlayer.GetMaxWaterLevel () -1 ) * 1000f);
				//repairObject.WaterSlider.maxValue = repairObject.fMaxWater;/.
				//if(cPlayer.GetWaterPlusLevel() >= 
			}

			return;
		}


		if (ScoreManager.ScoreInstance.GetGold() >= m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)) {

			ScoreManager.ScoreInstance.GoldPlus (-(m_EnhanceData.fBasicGold + (nLevel * m_EnhanceData.fPlusGoldValue)));

			nLevel++;

			cPlayer.SetBasicWaterPlus(cPlayer.GetBasicWaterPlus() + 1 * m_EnhanceData.fPlusPercent);

			cPlayer.SetWaterPlusLevel(nLevel);

			EnhanceText.text = strEnhanceName + nLevel;

			//물 최대 량증가
			//repairObject.fMaxWater =  GameManager.Instance.player.GetBasicMaxWaterPlus() +( (cPlayer.GetWaterPlusLevel () -1 ) * 1000f);
			//repairObject.WaterSlider.maxValue = repairObject.fMaxWater;
			//Debug.Log ("MaxWater : " + repairObject.fMaxWater);

		}
	}
}
