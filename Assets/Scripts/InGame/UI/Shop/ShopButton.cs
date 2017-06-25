using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopButton : MonoBehaviour {

    public int nIndex;
    public bool bIsBuy { get; set; }

    CGameEquiment equitMent;

    ShopShowPanel showPanel;

    public GameObject BuyButton;
	public GameObject ItemButton;

    public SpriteRenderer CanBuyImage;
    public SpriteRenderer CantBuyImage;

    public Button buttonCheck;
    public Text EquitName;
    public Text EquitGold;
    public Image EquitImage;

    public Inventory inventory;


    
    private void Awake()
    {
        BuyButton = transform.FindChild("BuyButton").gameObject;
		ItemButton = transform.FindChild ("ShopButton").gameObject;
        buttonCheck = transform.FindChild("ShopButton").GetComponent<Button>();
        buttonCheck.onClick.AddListener(ClickButton);

		BuyButton.SetActive (true);
		ItemButton.SetActive (false);
    }


    public void GetEquiment(Inventory _inventory,ShopShowPanel _showPanel, CGameEquiment _equimnet)
    {
        bIsBuy = true;

        equitMent = _equimnet;

        inventory = _inventory;

        showPanel = _showPanel;

        BuyButton.SetActive(false);
		ItemButton.SetActive (true);

        EquitName.text = equitMent.strName;

        EquitImage.sprite = ObjectCashing.Instance.LoadSpriteFromCache("Characters/" + equitMent.strResource);
    }

    public void ClickButton()
    {
        //옆에 설명창에 보내줌
        Debug.Log("click");

		if (showPanel == null)
			return;

        showPanel.Setting(equitMent);
        
    }
}
