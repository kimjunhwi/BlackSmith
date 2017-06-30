using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class SmithPanelUI : EnhanceUI {

	CGameSmithEnhace[] cGameSmith;

	protected override void Awake ()
	{
		base.Awake ();

		cGameSmith = GameManager.Instance.cSmithEnhaceInfo;

		nLevel = cPlayer.GetSmithLevel ();

		EnhanceText.text = strEnhanceName + nLevel;
	}

	protected override void EnhanceButtonClick ()
	{
		string strExplain = cGameSmith [nLevel].nGoldCost.ToString () +  "의 강화비용이 듭니다";;

		GameManager.Instance.Window_yesno ("강 화 창", strExplain, rt => { 

			//Yes
			if (rt == "0") {
				if (ScoreManager.ScoreInstance.m_fGetGold >= cGameSmith [nLevel].nGoldCost) {
					
					ScoreManager.ScoreInstance.GoldPlus (-cGameSmith [nLevel].nGoldCost);

					GameManager.Instance.Window_notice ("강화의 성공했습니다.", srt => { if (srt == "0") print("notice");  });

					nLevel++;

					cPlayer.SetSmithLevel(nLevel);

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
