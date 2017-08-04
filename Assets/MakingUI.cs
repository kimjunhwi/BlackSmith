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

	//기본 값
	const int m_nBasicGold = 1200;
	const int m_nBasicHonor = 300;
	const int m_nBasicMinRepair = 6;
	const int m_nBasicMaxRepair = 10;
	const int m_nBasicMinOption = 3;
	const int m_nBasicMaxOption = 5;

	//일차 별로 증가하는 값, 단 추가옵션과 보스옵션 같은 경우 10 레벨 마다 증가 한다.
	const int m_nPlusGoldPercent = 20;
	const int m_nPlusHonorPercent = 10;
	const int m_nPlusRepairMinPercent = 10;
	const int m_nPlusRepairMaxPercent = 10;
	const int m_nPlusOptionMinPercent = 10;
	const int m_nPlusOptionMaxPercent = 10;

	//Calc Data
	int nCalcGoldCost = 0;
	int nCalcHonorCost = 0;
	float fCalcMinRepair = 0;
	float fCalcMaxRepair = 0;
	int nCalcAddMinOption = 0;
	int nCalcAddMaxOption = 0;


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

		fCalcMinRepair = m_nBasicMinRepair + (float)(m_nBasicMinRepair * (m_nPlusRepairMinPercent * numChk * 0.01f));
		fCalcMaxRepair = m_nBasicMaxRepair + m_nBasicMaxRepair * (m_nPlusRepairMaxPercent * numChk * 0.01f);

		RandomRepairPower.text = string.Format("제작시 수리력 {0:F1} ~ {1:F1}",fCalcMinRepair,fCalcMaxRepair);
	}

	void MakeWeapon()
	{
		if (CostDayText.text == "0")
			return;

		int nDight = 0;
		int nDightCost = int.Parse (CostDayText.text);
		float nCostDay = (float)nDightCost;

		while (nCostDay >= 10) 
		{
			nCostDay *= 0.1f;
			nDight++;
		}

		nCalcGoldCost = (int)(m_nBasicGold + m_nBasicGold * (m_nPlusGoldPercent * nCostDay * 0.01f));
		nCalcHonorCost = (int)(m_nBasicHonor + m_nBasicHonor * (m_nPlusHonorPercent * nDight * 0.01f));


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
