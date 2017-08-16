using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSoulFire : BossSoul {

	protected override void InsertButton()
	{
		if (bIsCheck) 
		{
			bIsCheck = false;

			BossSoulPanel.sprite = unActiveBossSoulPanel;
			SoulCheckSlot.sprite = unActiveBossSoulCheckSlot;
		} 
		else 
		{
			if (MakingUI.CheckMake () == false)
				return;

			//개수가 있는지 확인 , 현재 활성화 된 버튼이 몇 개인지 확인
			if (playerData.GetSasinMaterial () > 0) {

				bIsCheck = true;

				BossSoulPanel.sprite = ActiveBossSoulPanel;
				SoulCheckSlot.sprite = ActiveBossSoulCheckSlot;
			}
		}
	}
}
