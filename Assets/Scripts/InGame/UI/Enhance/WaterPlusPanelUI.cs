using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlusPanelUI : EnhanceUI {

	CGameWaterPlusEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cWaterPlusEnhanceInfo;

		nLevel = cPlayer.GetWaterPlusLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
        if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith[nLevel].nCost)
        {

            ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nCost);

            nLevel++;

            cPlayer.SetWaterPlus(cPlayer.GetWaterPlus() + cGameSmith[nLevel].fResultValue);

            cPlayer.SetWaterPlusLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
