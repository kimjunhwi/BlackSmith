using System.Collections;
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

	void Awake()
	{
		SellButton.onClick.AddListener (EnhanceItem);
		EquipButton.onClick.AddListener (SellItem);
		EnhanceButton.onClick.AddListener (EquiItem);
		CloseButton.onClick.AddListener (ClosePanel);

		gameObject.SetActive (false);
	}

	private void EnhanceItem()
	{

	}

	private void SellItem()
	{

	}

	private void EquiItem()
	{

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

		NameText.text = ItemData.strName;

		GradeText.text = ItemData.nGrade.ToString ();

		enhanceData = GameManager.Instance.GetEnhanceArbaitData (ItemData.nGrade);

		WeaponImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(ItemData.strResource);
	
		ResetItemText ();

		gameObject.SetActive (true);
	}

	public void CreateText(string strName, int nValue)
	{
		GameObject textObject = simpleTextPool.GetObject();
		textObject.transform.SetParent(contentsPanel,false);
		textObject.transform.localScale = Vector3.one;

		Text newText = textObject.GetComponent<Text>();
		newText.text = strName + nValue.ToString();
	}


	private void ResetItemText()
	{
		RemoveText ();

		if (ItemData.nReapirPower       != 0) CreateText("수리력 : ", ItemData.nReapirPower);
		if (ItemData.nTemperaPlus       != 0) CreateText("온도증가량 : ", ItemData.nTemperaPlus);
		if (ItemData.nTemperaDown       != 0) CreateText("온도감소량 : ", ItemData.nTemperaDown);
		if (ItemData.nArbaitRepair      != 0) CreateText("알바수리력증가 : ", ItemData.nArbaitRepair);
		if (ItemData.nHonorPlus         != 0) CreateText("명예증가량 : ", ItemData.nHonorPlus);
		if (ItemData.nGoldPlus          != 0) CreateText("골드증가량 : ", ItemData.nGoldPlus);
		if (ItemData.nWaterMaxPlus      != 0) CreateText("물최대치증가 : ", ItemData.nWaterMaxPlus);
		if (ItemData.nWaterChargePlus   != 0) CreateText("물확률 : ", ItemData.nWaterChargePlus);
		if (ItemData.nCritical          != 0) CreateText("크리티컬확률 : ", ItemData.nCritical);
		if (ItemData.nCriticalDamage    != 0) CreateText("크리티컬데미지 : ", ItemData.nCriticalDamage);
		if (ItemData.nBigCritical       != 0) CreateText("대성공 : ", ItemData.nBigCritical);
		if (ItemData.nAccuracyRate      != 0) CreateText("명중률 : ", ItemData.nAccuracyRate);
	}
}
