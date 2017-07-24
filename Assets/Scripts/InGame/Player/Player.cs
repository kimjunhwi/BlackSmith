using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	//변화될 값 
	public CGamePlayerData changeStats;


    public List<CGameEquiment> List_items;

	CGameEquiment BaseEquimnet = null;
    //CGameEquiment WeaponEquimnet;
    //CGameEquiment GearEquimnet;
    //CGameEquiment AccessoryEquimnet;

    public Inventory inventory;

	public int GetSmithLevel() { return changeStats.nBlackSmithLevel; }
	public void SetSmithLevel(int _nValue) { changeStats.nBlackSmithLevel = _nValue; }

	public int GetRepairLevel() {return changeStats.nEnhanceRepaireLevel; }
	public void SetRepairLevel(int _nValue){changeStats.nEnhanceRepaireLevel = _nValue;}

	public int GetMaxWaterLevel(){return changeStats.nEnhanceMaxWaterLevel;}
	public void SetMaxWaterLevel(int _nValue) {changeStats.nEnhanceMaxWaterLevel = _nValue; }

	public int GetWaterPlusLevel(){ return changeStats.nEnhanceWaterPlusLevel;}
	public void SetWaterPlusLevel(int _nValue){changeStats.nEnhanceWaterPlusLevel = _nValue;}

	public int GetAccuracyRateLevel(){return changeStats.nEnhanceAccuracyRateLevel;}
	public void SetAccuracyRateLevel(int _nValue){changeStats.nEnhanceAccuracyRateLevel = _nValue;}

	public int GetCriticalLevel() {return changeStats.nEnhanceCriticalLevel; }
	public void SetCriticalLevel(int _nValue){changeStats.nEnhanceCriticalLevel = _nValue;}

	public int GetArbaitEnhanceLevel() {return changeStats.nEnhanceArbaitLevel; }
	public void SetArbaitEnhancLevel(int _nValue){changeStats.nEnhanceArbaitLevel = _nValue;}

	public float GetWaterPlus() { return changeStats.fWaterPlus; }
	public void SetWaterPlus(float _fValue) { changeStats.fWaterPlus = _fValue; }

	public float GetRepairPower(){ return changeStats.fRepairPower; }
	public void SetRepairPower(float _fValue) { changeStats.fRepairPower = _fValue; }

	public float GetArbaitRepairPower() {return changeStats.fArbaitsPower;}
	public void SetArbaitRepairPower(float _fValue){ changeStats.fArbaitsPower = _fValue;}

	public float GetMaxWaterPlus() { return changeStats.fMaxWaterPlus; }
	public void SetMaxWaterPlus(float _fValue) { changeStats.fMaxWaterPlus = _fValue; }

	public float GetAccuracyRate() { return changeStats.fAccuracyRate; }
	public void SetAccuracyRate(float _fValue) { changeStats.fAccuracyRate = _fValue; }

    public float GetBigSuccessedPercent() { return changeStats.fBigSuccessed; }
    public void SetBigSuccessedPercent(float _fValue) { changeStats.fBigSuccessed = _fValue; }

    public float GetCriticalChance() { return changeStats.fCriticalChance; }
	public void SetCriticalChance(float _fValue) { changeStats.fCriticalChance = _fValue; }

	public float GetTemperatureMinus() { return changeStats.fTemperatureMinus; }
    public void SetTemperatureMinus(float _fValue) { changeStats.fTemperatureMinus = _fValue; }


    public void Awake()
    {
        BaseEquimnet = new CGameEquiment();

        //WeaponEquimnet = BaseEquimnet;
        //GearEquimnet = BaseEquimnet;
        //AccessoryEquimnet = BaseEquimnet;
    }

    public void Init(List<CGameEquiment> _itemList, CGamePlayerData _defaultStats)
    {
        List_items = _itemList;
		changeStats = new CGamePlayerData(_defaultStats);
    }

    public void SetInventroy(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.SetInventory(this, List_items);
    }

	public int GetItemListCount()
	{
		if (inventory == null)
			return 0;

		List_items = inventory.GetItemList ();

		if (List_items == null)
			return 0;

		return List_items.Count;
	}
}




