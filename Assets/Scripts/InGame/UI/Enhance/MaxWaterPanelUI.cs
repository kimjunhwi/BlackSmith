using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxWaterPanelUI : EnhanceUI {

	CGameMaxWaterEnhance[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cMaxWaterEnhanceInfo;

		nLevel = cPlayer.GetMaxWaterLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
        ScoreManager.ScoreInstance.GoldPlus(-cGameSmith[nLevel].nCost);

        nLevel++;

        cPlayer.SetMaxWaterPlus(cPlayer.GetMaxWaterPlus() + cGameSmith[nLevel].nResultValue);

        cPlayer.SetMaxWaterLevel(nLevel);

        EnhanceText.text = strEnhanceName + nLevel;
	}
}
