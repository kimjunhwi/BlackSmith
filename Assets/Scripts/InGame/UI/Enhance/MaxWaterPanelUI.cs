using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxWaterPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		if (cPlayer == null)
			return;

		cGameSmith = GameManager.Instance.cMaxWaterEnhanceInfo;

		nLevel = cPlayer.GetMaxWaterLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{

		if (ScoreManager.ScoreInstance.GetGold() >= cGameSmith [nLevel].nGoldCost) {
			
			ScoreManager.ScoreInstance.GoldPlus (-cGameSmith [nLevel].nGoldCost);

			nLevel++;

			cPlayer.SetMaxWaterPlus (cPlayer.GetMaxWaterPlus () + cGameSmith [nLevel].fPlusPercentValue);

			cPlayer.SetMaxWaterLevel (nLevel);

			EnhanceText.text = strEnhanceName + nLevel;

		}
	}
}
