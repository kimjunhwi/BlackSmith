using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbaitPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		if (cPlayer == null)
			return;

		cGameSmith = GameManager.Instance.cPlayerArbaitEnhace;

		nLevel = cPlayer.GetArbaitEnhanceLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{

		if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith [nLevel].nGoldCost) {

			ScoreManager.ScoreInstance.GoldPlus (-cGameSmith [nLevel].nGoldCost);

			nLevel++;

			cPlayer.SetArbaitRepairPower (cPlayer.GetArbaitRepairPower () + cGameSmith [nLevel].fPlusPercentValue);

			cPlayer.SetMaxWaterLevel (nLevel);

			EnhanceText.text = strEnhanceName + nLevel;

		}
	}
}
