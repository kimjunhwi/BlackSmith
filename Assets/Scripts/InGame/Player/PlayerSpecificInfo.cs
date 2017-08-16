using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecificInfo : MonoBehaviour {

    Player player;

    public Text RepairPowerText;
    public Text ArbaitRepairIncreaseText;
    public Text CriticalDamageText;
    public Text CriticalChanceText;
    public Text BigSuccessesChanceText;
    public Text AccuracyRateText;
    public Text WaterMaxText;
    public Text WaterRechargeAmountText;


    void Awake()
    {
        player = GameManager.Instance.player;

    }

	public void OnEnable()
    {
        if(player == null)
            player = GameManager.Instance.player;

		if (player == null)
			return;

        RepairPowerText.text = string.Format("수리력 : {0}",player.GetRepairPower());
		ArbaitRepairIncreaseText.text = string.Format("아르바이트 수리력 : {0}", player.GetArbaitRepairPower());
		CriticalDamageText.text = string.Format("크리데미지 : {0}", player.GetCriticalDamage());
        CriticalChanceText.text = string.Format("크리 확률 : {0}", player.GetCriticalChance());
		BigSuccessesChanceText.text = string.Format("대성공 확률 : {0}", player.GetBigSuccessed());
        AccuracyRateText.text = string.Format("명중률 : {0}", player.GetAccuracyRate());
        WaterMaxText.text = string.Format("최대 물 충전량 : {0}", player.GetBasicMaxWaterPlus());
        WaterRechargeAmountText.text = string.Format("물 충전량 : {0}", player.GetWaterPlus()); 
    }
}
