using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScrollList : MonoBehaviour {

	private int nMaxItemList = 50;

    public GameObject parentObject;

    public List<CGameEquiment> itemList;

    public Transform contentPanel;

	public InventoryShowPanel inventoryPanel;

    public SimpleObjectPool buttonObjectPool;

    void RefreshDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    public void SetInitList(List<CGameEquiment> list = null)
    {
		if (list != null)
			itemList = list;
		
		else
			itemList = new List<CGameEquiment> ();

        RefreshDisplay();
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
		for (int i = 0; i < nMaxItemList; i++)
        {
			if (i < itemList.Count) {
				CGameEquiment item = itemList [i];

				GameObject newButton = buttonObjectPool.GetObject ();
				newButton.transform.SetParent (contentPanel,false);
				newButton.transform.localScale = Vector3.one;


				InventoryButton sampleButton = newButton.GetComponent<InventoryButton> ();
				sampleButton.Setup (item,inventoryPanel);
			} else {
				GameObject newButton = buttonObjectPool.GetObject ();
				newButton.transform.SetParent (contentPanel,false);
				newButton.transform.localScale = Vector3.one;
			}
        }
    }

    //public void TryTransferItemToOtherShop(Item item)
    //{
    //    if (otherShop.gold >= item.price) 
    //    {
    //        gold += item.price;
    //        otherShop.gold -= item.price;

    //        AddItem(item, otherShop);
    //        RemoveItem(item, this);

    //        RefreshDisplay();
    //        otherShop.RefreshDisplay();
    //        Debug.Log ("enough gold");

    //    }
    //    Debug.Log ("attempted");
    //}

    public void AddItem(CGameEquiment itemToAdd)
    {
        itemList.Add(itemToAdd);

        RefreshDisplay();
    }

    public void DisableActive()
    {
        gameObject.SetActive(false);
    }
}
