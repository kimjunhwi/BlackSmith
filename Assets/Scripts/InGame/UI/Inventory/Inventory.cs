using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    Player player;

    private List<CGameEquiment> List_Equiments;

    public InventoryScrollList[] inventorySlots;

    public void Awake()
    {
		if (GameManager.Instance.player == null)
			return;

        GameManager.Instance.player.SetInventroy(this);
    }


    //인벤토리 세팅
    public void SetInventory(Player _player, List<CGameEquiment> _list)
    {
        player = _player;

		if (_list != null)
			List_Equiments = _list;
		
		else
			List_Equiments = new List<CGameEquiment> ();

        
        for(int nIndex = 0; nIndex < inventorySlots.Length; nIndex++)
        {
            //만약 아이템이 있을경우 아이템을 넣어줌
            if (GetItemList(nIndex) != 0)
                inventorySlots[nIndex].SetInitList(GetEquimnetList(nIndex));

            else
                inventorySlots[nIndex].SetInitList();
        }

		gameObject.SetActive (false);
    }

    public int GetItemList(int _nIndex)
    {
		if (List_Equiments == null)
			return 0;

        int nAmount = 0;

        for (int nIndex = 0; nIndex < List_Equiments.Count; nIndex++)
            if (List_Equiments[nIndex].nSlotIndex == _nIndex)
                nAmount++;

        return nAmount;
    }

    public List<CGameEquiment> GetEquimnetList(int _nIndex)
    {
        List<CGameEquiment> list = new List<CGameEquiment>();

        for (int nIndex = 0; nIndex < List_Equiments.Count; nIndex++)
            if (List_Equiments[nIndex].nSlotIndex == _nIndex)
                list.Add(List_Equiments[nIndex]);

        return list;
    }

    public void GetEquimnet(CGameEquiment _getEquimnet)
    {
		CGameEquiment newItem = new CGameEquiment (_getEquimnet);

		List_Equiments.Add(newItem);

		inventorySlots[_getEquimnet.nSlotIndex].AddItem(newItem);
    }

    

    public List<CGameEquiment> GetItemList()
    {
		if (List_Equiments == null)
			return null;

        return List_Equiments;
    }
}
