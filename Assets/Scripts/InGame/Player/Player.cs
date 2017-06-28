using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	//변화될 값 
	public CGamePlayerData changeStats = new CGamePlayerData();

	//기본 값 
	public CGamePlayerData defaultStats = new CGamePlayerData();


    public List<CGameEquiment> List_items;

	CGameEquiment BaseEquimnet = null;
    CGameEquiment WeaponEquimnet;
    CGameEquiment GearEquimnet;
    CGameEquiment AccessoryEquimnet;

    public Inventory inventory;

	public float GetWaterPlus() { return changeStats.fWaterPlus; }

	public float GetRepairPower(){ return changeStats.fRepairPower; }

	public float GetMaxWaterPlus() { return changeStats.fMaxWaterPlus; }

	public float GetAccuracyRate() { return changeStats.fAccuracyRate; }

	public float GetTemperatureMinus() { return changeStats.fTemperatureMinus; }

	public void SetWaterPlus(float _fValue) { changeStats.fWaterPlus = _fValue; }

	public void SetRepairPower(float _fValue) { changeStats.fRepairPower = _fValue; }

	public void SetMaxWaterPlus(float _fValue) { changeStats.fMaxWaterPlus = _fValue; }

	public void SetAccuracyRate(float _fValue) { changeStats.fAccuracyRate = _fValue; }

	public void SetTemperatureMinus(float _fValue) { changeStats.fTemperatureMinus = _fValue; }

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
		changeStats = _defaultStats;


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




