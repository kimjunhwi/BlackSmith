using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	

    public List<CGameEquiment> List_items;

	CGameEquiment BaseEquimnet = null;
    CGameEquiment WeaponEquimnet;
    CGameEquiment GearEquimnet;
    CGameEquiment AccessoryEquimnet;

    public Inventory inventory;
    public CGamePlayerData defaultStats;
    public PlayerStatus playerStats;

	public float GetWaterPlus() { return defaultStats.fWaterPlus; }

	public float GetRepairPower(){ return defaultStats.fRepairPower; }

    public float GetMaxWaterPlus() { return defaultStats.fMaxWaterPlus; }

    public float GetAccuracyRate() { return defaultStats.fAccuracyRate; }

    public float GetTemperatureMinus() { return defaultStats.fTemperatureMinus; }

    public void Awake()
    {
        BaseEquimnet = new CGameEquiment();

        WeaponEquimnet = BaseEquimnet;
        GearEquimnet = BaseEquimnet;
        AccessoryEquimnet = BaseEquimnet;
    }

    public void Init(List<CGameEquiment> _itemList, CGamePlayerData _defaultStats)
    {
        List_items = _itemList;
        defaultStats = _defaultStats;
    }

    public void SetInventroy(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.SetInventory(this, List_items);
    }

	public void SetGold(float _gold)
	{
        defaultStats.fGold = _gold;
		Debug.Log ("Player Gold :  " + _gold);
	}

	public int GetItemListCount()
	{
		List_items = inventory.GetItemList ();

		if (List_items == null)
			return 0;

		return List_items.Count;
	}
}




