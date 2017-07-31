using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakingUI : MonoBehaviour {

	public Image WeaponImage;
	public Image BossSlotOne;
	public Image BossSlotTwo;

	public Text NowRepairPower;

	public Text RandomRepairPower;
	public InputField CostDayText;

	public Button MinusDayButton;
	public Button PlusDayButton;
	public Button MakingButton;

	public BossSoul[] BossSoulSlots;

	Player playerData;


	//base Data
	int nGoldCost = 1200;
	int nHonorCost = 300;
	int nMinRepair = 6;
	int nMaxRepair = 10;
	int nAddMinOption = 3;
	int nAddMaxOption = 5;
	int nBossOptionMin = 3;
	int nBossOptionMax = 5;


	//Calc Data
	int nCalcGoldCost = 0;
	int nCalcHonorCost = 0;
	float fCalcMinRepair = 0;
	float fCalcMaxRepair = 0;
	int nCalcAddMinOption = 0;
	int nCalcAddMaxOption = 0;
	int nCalcBossOptionMin = 0;
	int nCalcBossOptionMax = 0;

	void Awake()
	{
		playerData = GameManager.Instance.GetPlayer ();
		MinusDayButton.onClick.AddListener (MinusDay);
		PlusDayButton.onClick.AddListener (PlusDay);
		MakingButton.onClick.AddListener (MakeWeapon);

		CostDayText.onValueChanged.AddListener (delegate {InputText();});
	}

	void Start()
	{
		for (int nIndex = 0; nIndex < BossSoulSlots.Length; nIndex++) 
		{
			BossSoulSlots [nIndex].SetUp (this, playerData);
		}
	}

	void MinusDay()
	{
		
	}

	void PlusDay()
	{

	}

	public void InputText()
	{
		string result = CostDayText.text;

		Debug.Log (result);

		int numChk = 0;
		bool isNum = int.TryParse(result , out numChk);
		if (!isNum)
			//숫자가 아님
			numChk = 0;
		
		else{
			//숫자
			if(playerData.GetDay() < numChk) 
				numChk = playerData.GetDay();
		}

		CostDayText.text = numChk.ToString();

		fCalcMinRepair = nMinRepair + (float)(nMinRepair * (10 * numChk * 0.01f));
		fCalcMaxRepair = nMaxRepair + nMaxRepair * (10 * numChk * 0.01f);

		RandomRepairPower.text = string.Format("제작시 수리력 {0:F1} ~ {1:F1}",fCalcMinRepair,fCalcMaxRepair);
	}

	void MakeWeapon()
	{



	}

	public bool CheckMake()
	{
		int nAmount = 0;

		for (int nIndex = 0; nIndex < BossSoulSlots.Length; nIndex++) {
			if (BossSoulSlots [nIndex].bIsCheck)
				nAmount++;
		}

		if (nAmount < 2)
			return true;

		return false;
	}
}
