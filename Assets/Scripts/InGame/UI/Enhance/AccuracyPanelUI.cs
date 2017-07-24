using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyPanelUI : EnhanceUI {

	CGameAccuracyRate[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cAccuracyRateInfo;

		nLevel = cPlayer.GetAccuracyRateLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
        if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith[nLevel].nCost)
        {

            ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nCost);

            nLevel++;

            cPlayer.SetAccuracyRate(cPlayer.GetAccuracyRate() + cGameSmith[nLevel].fResultValue);

            cPlayer.SetAccuracyRateLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
