using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class SmithPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cSmithEnhaceInfo;

		nLevel = cPlayer.GetSmithLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith [nLevel].nGoldCost) {

			ScoreManager.ScoreInstance.GoldPlus (-cGameSmith [nLevel].nGoldCost);

			nLevel++;

			cPlayer.SetSmithLevel(nLevel);

			EnhanceText.text = strEnhanceName + nLevel;
		}
	}
}
