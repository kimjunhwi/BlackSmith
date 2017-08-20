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

		nameText.text = string.Format("{0} +{1}", ItemData.strName , ItemData.nStrenthCount);

		NoneActiveImage.SetAsFirstSibling ();

        itemImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache(ItemData.strResource);

        RemoveText();

        //골드 얼마 사용할지

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
    public void CreateText(string strName, float fValue)
    {
        GameObject textObject = simpleTextPool.GetObject();
		textObject.transform.SetParent(parentObject,false);
		textObject.transform.localScale = Vector3.one;

        Text newText = textObject.GetComponent<Text>();
			newText.text = string.Format("{0} {1}", strName , fValue);
	}
}
