using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScrollList : MonoBehaviour
{

	private int nMaxItemList = 50;

    public GameObject parentObject;

    public List<CGameEquiment> itemList;

    public Transform contentPanel;

	public InventoryShowPanel inventoryPanel;

    public SimpleObjectPool buttonObjectPool;

    public void RefreshDisplay()
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

		for (int nIndex = 0; nIndex < itemList.Count; nIndex++)
			if (itemList [nIndex].bIsEquip)
				GameManager.Instance.GetPlayer ().EquipItem (itemList [nIndex]);

		RefreshDisplay ();
	}

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    //인벤토리 추가 정렬 방식이다
    //1 순위는 장착된 장비이며
    //2 순위는 등급이 높은것
    //3 순위는 강화가 높은순으로 정렬할 예정이다.
    //추후 수정 예정
    private void AddButtons()
    {
        //장착된것을 정렬
        for (int nIndex = 0; nIndex < itemList.Count; nIndex++)
        {
            if (itemList[nIndex].bIsEquip == true)
            {
                if (nIndex != 0)
                {
                    CGameEquiment temp = itemList[nIndex];

                    itemList[nIndex] = itemList[0];

                    itemList[0] = temp;
                }

                break;
            }
        }

		/*
        //등급이 높은 것을 정렬
        itemList.Sort(delegate(CGameEquiment A, CGameEquiment B)
        {
			if (A.sGrade < B.sGrade) return 1;
				else if (A.sGrade > B.sGrade) return -1;
            else return 0;
        });

        //강화가 높은것을 정렬
        itemList.Sort(delegate(CGameEquiment A, CGameEquiment B)
        {
				if (A.sGrade == B.sGrade && A.nStrenthCount < B.nStrenthCount) return 1;
				else if (A.sGrade != B.sGrade || A.nStrenthCount > B.nStrenthCount) return -1;
            else return 0;
        });
		*/

        for (int i = 0; i < nMaxItemList; i++)
        {
            if (i < itemList.Count)
            {
                CGameEquiment item = itemList[i];

                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel, false);
                newButton.transform.localScale = Vector3.one;

                InventoryButton sampleButton = newButton.GetComponent<InventoryButton>();
                sampleButton.Setup(item, inventoryPanel);
            }
            else
            {
                GameObject newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel, false);
                newButton.transform.localScale = Vector3.one;

				InventoryButton sampleButton = newButton.GetComponent<InventoryButton>();
				sampleButton.SetInit ();
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

    public void RemoveItem(CGameEquiment _item)
    {
        itemList.Remove(_item);

        RefreshDisplay();
    }

    public void DisableActive()
    {
        gameObject.SetActive(false);
    }
}
