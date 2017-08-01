using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlusPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cWaterPlusEnhanceInfo;

		nLevel = cPlayer.GetWaterPlusLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		if (ScoreManager.ScoreInstance.GetGold() >= cGameSmith[nLevel].nGoldCost)
        {

			ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nGoldCost);

            nLevel++;

			cPlayer.SetWaterPlus(cPlayer.GetWaterPlus() + cGameSmith[nLevel].fPlusPercentValue);

            cPlayer.SetWaterPlusLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
