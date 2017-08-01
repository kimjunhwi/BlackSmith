using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cRepairEnhanceInfo;

		nLevel = cPlayer.GetRepairLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		if (ScoreManager.ScoreInstance.GetGold() >= cGameSmith[nLevel].nGoldCost)
        {

			ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nGoldCost);

            nLevel++;

			cPlayer.SetRepairPower(cPlayer.GetRepairPower() + cGameSmith[nLevel].fPlusPercentValue);

            cPlayer.SetRepairLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
