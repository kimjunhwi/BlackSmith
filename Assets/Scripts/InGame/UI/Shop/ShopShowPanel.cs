using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopShowPanel : MonoBehaviour {

    public Text nameText;
    
	public Image itemImage;
    
	public Text goldText;

	public Button buyButton;

	public Transform NoneActiveImage;

    public Transform parentObject;

    public CGameEquiment ItemData;

    public SimpleObjectPool simpleTextPool;

	void Awake()
	{
		buyButton.onClick.AddListener (BuyClick);
	}

    void OnEnable()
    {
		NoneActiveImage.SetAsLastSibling ();

        RemoveText();
    }

	private void BuyClick()
	{
		if (ItemData != null)
			GameManager.Instance.player.inventory.GetEquimnet (ItemData);
	}

    private void RemoveText()
    {
        while (parentObject.childCount > 0)
        {
            GameObject toRemove = parentObject.GetChild(0).gameObject;
            simpleTextPool.ReturnObject(toRemove);
        }
    }

    public void Setting(CGameEquiment _ItemData)
    {
        ItemData = _ItemData;

        nameText.text = ItemData.strName;

		NoneActiveImage.SetAsFirstSibling ();

        itemImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(ItemData.strResource);

        RemoveText();

        //골드 얼마 사용할지
        //goldText.text = ItemData.nGold;

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
    public void CreateText(string strName, int nValue)
    {
        GameObject textObject = simpleTextPool.GetObject();
		textObject.transform.SetParent(parentObject,false);
		textObject.transform.localScale = Vector3.one;

        Text newText = textObject.GetComponent<Text>();
        newText.text = strName + nValue.ToString();
    }
}
