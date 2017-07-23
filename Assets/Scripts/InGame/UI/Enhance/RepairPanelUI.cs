using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPanelUI : EnhanceUI {

	CGameRepairEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cRepairEnhanceInfo;

		nLevel = cPlayer.GetRepairLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
        if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith[nLevel].nCost)
        {

            ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nCost);

            nLevel++;

            cPlayer.SetRepairPower(cPlayer.GetRepairPower() + cGameSmith[nLevel].nResultValue);

            cPlayer.SetRepairLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
