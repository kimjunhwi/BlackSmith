﻿using System.Collections;
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
		string strExplain = cGameSmith [nLevel].nCost.ToString () +  "의 강화비용이 듭니다";;

		GameManager.Instance.Window_yesno ("강 화 창", strExplain, rt => { 

			//Yes
			if (rt == "0") {
				if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith [nLevel].nCost) {

					ScoreManager.ScoreInstance.GoldPlus (-cGameSmith [nLevel].nCost);

					GameManager.Instance.Window_notice ("강화의 성공했습니다.", srt => { if (srt == "0") print("notice");  });

					nLevel++;

					cPlayer.SetDefaultWaterPlus(cGameSmith [nLevel].fResultValue);

					cPlayer.SetWaterPlus(cPlayer.GetWaterPlus() + cGameSmith [nLevel].fResultValue);

					cPlayer.SetWaterPlusLevel(nLevel);

					EnhanceText.text = strEnhanceName + nLevel;
				}
				else
				{
					GameManager.Instance.Window_notice ("강화의 실패했습니다.", srt => { if (srt == "0") print("notice");  });
				}
			}

		});
	}
}