using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cAccuracyRateInfo;

		nLevel = cPlayer.GetAccuracyRateLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		if (ScoreManager.ScoreInstance.GetGold() >= cGameSmith[nLevel].nGoldCost)
        {

			ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nGoldCost);

            nLevel++;

			cPlayer.SetAccuracyRate(cPlayer.GetAccuracyRate() + cGameSmith[nLevel].fPlusPercentValue);

            cPlayer.SetAccuracyRateLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
