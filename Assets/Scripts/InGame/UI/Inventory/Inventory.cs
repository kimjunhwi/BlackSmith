using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    Player player;

    private List<CGameEquiment> List_Equiments;

    public InventoryScrollList[] inventory;

    public void Awake()
    {
		if (GameManager.Instance.player == null)
			return;

        GameManager.Instance.player.SetInventroy(this);
    }

    public void SetInventory(Player _player, List<CGameEquiment> _list)
    {
        player = _player;

		if (_list != null)
			List_Equiments = _list;
		
		else
			List_Equiments = new List<CGameEquiment> ();

        for(int nIndex = 0; nIndex < inventory.Length; nIndex++)
        {
            if (GetItemList(nIndex) != 0)
                inventory[nIndex].SetInitList(GetEquimnetList(nIndex));

            else
                inventory[nIndex].SetInitList();
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
        List_Equiments.Add(_getEquimnet);

        inventory[_getEquimnet.nSlotIndex].AddItem(_getEquimnet);
    }

    public List<CGameEquiment> GetItemList()
    {
		if (List_Equiments == null)
			return null;

        return List_Equiments;
    }
}
