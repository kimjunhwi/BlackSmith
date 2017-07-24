using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPanelUI : EnhanceUI {

	CGameCriticalEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cCriticalEnhance;

		nLevel = cPlayer.GetCriticalLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
        if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith[nLevel].nCost)
        {

            ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nCost);


            nLevel++;

            cPlayer.SetCriticalChance(cPlayer.GetCriticalChance() + cGameSmith[nLevel].fResultValue);

            cPlayer.SetCriticalLevel(nLevel);

            EnhanceText.text = strEnhanceName + nLevel;
        }
	}
}
