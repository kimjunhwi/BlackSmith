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


	EquipmentEnhance equipEnhanceData;

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


		if (equipEnhanceData.fBasicGold + ItemData.nStrenthCount * equipEnhanceData.fPlusGoldValue <= ScoreManager.ScoreInstance.GetGold ()) {
				
			ScoreManager.ScoreInstance.GoldPlus (-(equipEnhanceData.fBasicGold + ItemData.nStrenthCount * equipEnhanceData.fPlusGoldValue));

			Debug.Log ("강화 성공!!");

			if (ItemData.fReapirPower 		!= 0) ItemData.fReapirPower 	+= equipEnhanceData.fPlusPercent;
			if (ItemData.fArbaitRepair      != 0) ItemData.fArbaitRepair 	+= equipEnhanceData.fPlusPercent;
			if (ItemData.fHonorPlus         != 0) ItemData.fHonorPlus 		+= equipEnhanceData.fPlusPercent;
			if (ItemData.fGoldPlus          != 0) ItemData.fGoldPlus 		+= equipEnhanceData.fPlusPercent;
			if (ItemData.fWaterChargePlus   != 0) ItemData.fWaterChargePlus += equipEnhanceData.fPlusPercent;
			if (ItemData.fCritical          != 0) ItemData.fCritical 		+= equipEnhanceData.fPlusPercent;
			if (ItemData.fCriticalDamage    != 0) ItemData.fCriticalDamage 	+= equipEnhanceData.fPlusPercent;
			if (ItemData.fBigCritical       != 0) ItemData.fBigCritical 	+= equipEnhanceData.fPlusPercent;
			if (ItemData.fAccuracyRate      != 0) ItemData.fAccuracyRate 	+= equipEnhanceData.fPlusPercent;

			ItemData.nStrenthCount++;

			EnhanceCostText.text = (equipEnhanceData.fBasicGold + ItemData.nStrenthCount * equipEnhanceData.fPlusGoldValue).ToString();

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

	public void SetUp(CGameEquiment _equiment,string strExplain = null)
	{
		ItemData = _equiment;

		NameText.text = string.Format("{0} +{1}", ItemData.strName , ItemData.nStrenthCount);

		GradeText.text = ItemData.sGrade;

		equipEnhanceData = GameManager.Instance.GetEnhanceArbaitData (ItemData.sGrade);

		WeaponImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(ItemData.strResource);
	
		ResetItemText ();

		EnhanceCostText.text = (equipEnhanceData.fBasicGold + ItemData.nStrenthCount * equipEnhanceData.fPlusGoldValue).ToString();

		if (strExplain != null)
			CreateText (strExplain, 0f);


		gameObject.SetActive (true);
	}

	public void CreateText(string strName, float nValue)
	{
		GameObject textObject = simpleTextPool.GetObject();
		textObject.transform.SetParent(contentsPanel,false);
		textObject.transform.localScale = Vector3.one;

		Text newText = textObject.GetComponent<Text>();

		if(nValue == 0.0f)
			newText.text = string.Format("{0}",  strName);

		else
			newText.text = string.Format("{0} {1}",  strName , nValue.ToString());
	}


	private void ResetItemText()
	{
		RemoveText ();

		if (ItemData.fReapirPower       != 0) CreateText("수리력 : ", ItemData.fReapirPower);
		if (ItemData.fArbaitRepair      != 0) CreateText("알바수리력증가 : ", ItemData.fArbaitRepair);
		if (ItemData.fHonorPlus         != 0) CreateText("명예증가량 : ", ItemData.fHonorPlus);
		if (ItemData.fGoldPlus          != 0) CreateText("골드증가량 : ", ItemData.fGoldPlus);
		if (ItemData.fWaterChargePlus   != 0) CreateText("물 충전 증가량 : ", ItemData.fWaterChargePlus);
		if (ItemData.fCritical          != 0) CreateText("크리티컬확률 : ", ItemData.fCritical);
		if (ItemData.fCriticalDamage    != 0) CreateText("크리티컬데미지 : ", ItemData.fCriticalDamage);
		if (ItemData.fBigCritical       != 0) CreateText("대성공 : ", ItemData.fBigCritical);
		if (ItemData.fAccuracyRate      != 0) CreateText("명중률 : ", ItemData.fAccuracyRate);
	}
}
