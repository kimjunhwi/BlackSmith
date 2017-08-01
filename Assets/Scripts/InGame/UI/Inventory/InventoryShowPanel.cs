﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryShowPanel : MonoBehaviour {

	public Text NameText;
	public Text GradeText;
	public Image WeaponImage;
	public Text EnhanceCostText;

	public Button SellButton;
	public Button EquipButton;
	public Button EnhanceButton;

	public Button CloseButton;

	public Image EquiImage;

	public Sprite ClearSprite;
	public Sprite EquipSprite;

	public Transform contentsPanel;

	public SimpleObjectPool simpleTextPool;

	CGameEquiment ItemData;

	CGameEnhanceData[] enhanceData;

    Player player;

	void Awake()
	{
		SellButton.onClick.AddListener (SellItem);
        EquipButton.onClick.AddListener(EquipItem);
		EnhanceButton.onClick.AddListener (EnhanceItem);
		CloseButton.onClick.AddListener (ClosePanel);

        player = GameManager.Instance.player;

		gameObject.SetActive (false);
	}

	private void EnhanceItem()
	{
		Debug.Log ("강화 시작!!");
		bool bIsSuccessed = false;

		if (enhanceData [ItemData.nStrenthCount].nGoldCost != 0) {
			
			if (enhanceData [ItemData.nStrenthCount].nGoldCost <= ScoreManager.ScoreInstance.GetGold()) 
			{
				ScoreManager.ScoreInstance.GoldPlus (-enhanceData [ItemData.nStrenthCount].nGoldCost);

				bIsSuccessed = true;
			}
		} else {
			if (enhanceData [ItemData.nStrenthCount].nHonorCost <= ScoreManager.ScoreInstance.GetHonor()) 
			{
				ScoreManager.ScoreInstance.GoldPlus (-enhanceData [ItemData.nStrenthCount].nHonorCost);

				bIsSuccessed = true;
			}
		}

		if (bIsSuccessed) {

			Debug.Log ("강화 성공!!");

			if (ItemData.fReapirPower != 0) ItemData.fReapirPower += ItemData.fReapirPower * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fTemperaPlus       != 0)ItemData.fTemperaPlus += ItemData.fTemperaPlus * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fTemperaDown       != 0) ItemData.fTemperaDown += ItemData.fTemperaDown * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fArbaitRepair      != 0) ItemData.fArbaitRepair += ItemData.fArbaitRepair * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fHonorPlus         != 0) ItemData.fHonorPlus += ItemData.fHonorPlus * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fGoldPlus          != 0) ItemData.fGoldPlus += ItemData.fGoldPlus * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fWaterMaxPlus      != 0)ItemData.fWaterMaxPlus += ItemData.fWaterMaxPlus * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fWaterChargePlus   != 0) ItemData.fWaterChargePlus += ItemData.fWaterChargePlus * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fCritical          != 0) ItemData.fCritical += ItemData.fCritical * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fCriticalDamage    != 0) ItemData.fCriticalDamage += ItemData.fCriticalDamage * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fBigCritical       != 0) ItemData.fBigCritical += ItemData.fBigCritical * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;
			if (ItemData.fAccuracyRate      != 0) ItemData.fAccuracyRate += ItemData.fAccuracyRate * enhanceData [ItemData.nStrenthCount].nPercent * 0.01f;

			ItemData.nStrenthCount++;

			if (enhanceData [ItemData.nStrenthCount].nGoldCost != 0)
				EnhanceCostText.text = enhanceData [ItemData.nStrenthCount].nGoldCost.ToString ();
			else
				EnhanceCostText.text = enhanceData [ItemData.nStrenthCount].nHonorCost.ToString ();

			ResetItemText ();

			NameText.text = string.Format("{0} +{1}", ItemData.strName , ItemData.nStrenthCount);
		}
	}

	private void SellItem()
	{

	}

	private void EquipItem()
	{
        player.EquipItem(ItemData);
	}

	private void ClosePanel()
	{
		gameObject.SetActive (false);
	}

	private void OnDisable()
	{
		RemoveText ();
	}

	private void RemoveText()
	{
		while (contentsPanel.childCount > 0)
		{
			GameObject toRemove = contentsPanel.GetChild(0).gameObject;
			simpleTextPool.ReturnObject(toRemove);
		}
	}

	public void SetUp(CGameEquiment _equiment)
	{
		ItemData = _equiment;

		NameText.text = string.Format("{0} +{1}", ItemData.strName , ItemData.nStrenthCount);


		GradeText.text = ItemData.nGrade.ToString ();

		enhanceData = GameManager.Instance.GetEnhanceArbaitData (ItemData.nGrade);

		WeaponImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(ItemData.strResource);
	
		ResetItemText ();

		if(enhanceData[ItemData.nStrenthCount].nGoldCost != 0)
			EnhanceCostText.text = enhanceData [ItemData.nStrenthCount].nGoldCost.ToString();

		else
			EnhanceCostText.text = enhanceData [ItemData.nStrenthCount].nHonorCost.ToString();
		

		gameObject.SetActive (true);
	}

	public void CreateText(string strName, float nValue)
	{
		GameObject textObject = simpleTextPool.GetObject();
		textObject.transform.SetParent(contentsPanel,false);
		textObject.transform.localScale = Vector3.one;

		Text newText = textObject.GetComponent<Text>();
		newText.text = string.Format("{0} {1}",  strName , nValue.ToString());
	}


	private void ResetItemText()
	{
		RemoveText ();

		if (ItemData.fReapirPower       != 0) CreateText("수리력 : ", ItemData.fReapirPower);
		if (ItemData.fTemperaPlus       != 0) CreateText("온도증가량 : ", ItemData.fTemperaPlus);
		if (ItemData.fTemperaDown       != 0) CreateText("온도감소량 : ", ItemData.fTemperaDown);
		if (ItemData.fArbaitRepair      != 0) CreateText("알바수리력증가 : ", ItemData.fArbaitRepair);
		if (ItemData.fHonorPlus         != 0) CreateText("명예증가량 : ", ItemData.fHonorPlus);
		if (ItemData.fGoldPlus          != 0) CreateText("골드증가량 : ", ItemData.fGoldPlus);
		if (ItemData.fWaterMaxPlus      != 0) CreateText("물최대치증가 : ", ItemData.fWaterMaxPlus);
		if (ItemData.fWaterChargePlus   != 0) CreateText("물확률 : ", ItemData.fWaterChargePlus);
		if (ItemData.fCritical          != 0) CreateText("크리티컬확률 : ", ItemData.fCritical);
		if (ItemData.fCriticalDamage    != 0) CreateText("크리티컬데미지 : ", ItemData.fCriticalDamage);
		if (ItemData.fBigCritical       != 0) CreateText("대성공 : ", ItemData.fBigCritical);
		if (ItemData.fAccuracyRate      != 0) CreateText("명중률 : ", ItemData.fAccuracyRate);
	}
}
