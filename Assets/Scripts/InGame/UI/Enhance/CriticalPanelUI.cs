using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPanelUI : EnhanceUI {

	CGamePlayerEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cCriticalEnhance;

		nLevel = cPlayer.GetCriticalLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		if (ScoreManager.ScoreInstance.GetGold() >= cGameSmith[nLevel].nGoldCost)
        {

			ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nGoldCost);


            nLevel++;

			cPlayer.SetCriticalChance(cPlayer.GetCriticalChance() + cGameSmith[nLevel].fPlusPercentValue);

            cPlayer.SetCriticalLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
