﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

[System.Serializable]
public class Player 
{	
	//변화될 값 
	public CGamePlayerData changeStats;

	//플레이어 제작 무기  
	private CreatorWeapon creatorWeapon;

    public List<CGameEquiment> List_items;

    CGameEquiment WeaponEquipment = null;
    CGameEquiment GearEquipmnet = null;
    CGameEquiment AccessoryEquipmnet = null;

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

    public int GetSasinMaterial() { return changeStats.nSasinMaterial; }
    public void SetSasinMaterial(int _nValue) { changeStats.nSasinMaterial = _nValue; }
    public int GetRusiuMaterial() { return changeStats.nRusiuMaterial; }
    public void SetRusiuMaterial(int _nValue) { changeStats.nRusiuMaterial = _nValue; }
    public int GetIceMaterial() { return changeStats.nIceMaterial; }
    public void SetIceMaterial(int _nValue) { changeStats.nIceMaterial = _nValue; }
    public int GetFireMaterial() { return changeStats.nFireMaterial; }
    public void SetFireMaterial(int _nValue) { changeStats.nFireMaterial = _nValue; }

    public int GetDay() { return changeStats.nDay; }
    public void SetDay(int _nValue) { changeStats.nDay = _nValue; }

    public int GetGuestCount() { return changeStats.nGuestCount; }

	//08.09
	//플레이어 제작 
	public void SetCreatorWeapon(CreatorWeapon _weapon)
	{
		creatorWeapon = _weapon; 
	}

	public float GetCreatorWeaponRepair(){
		return creatorWeapon.fRepair;
	}

	public float GetCreatorWeaponArbaitRepair(){
		return creatorWeapon.fArbaitRepair;
	}

	public float GetCreatorWeaponPlusHonorPercent(){
		return creatorWeapon.fPlusHonorPercent;
	}

	public float GetCreatorWeaponPlusGoldPercent(){
		return creatorWeapon.fPlusGoldPercent;
	}

	public float GetCreatorWeaponMaxWaterPlus(){
		return creatorWeapon.fMaxWaterPlus;
	}

	public float GetCreatorWeaponWaterPlus(){
		return creatorWeapon.fWaterPlus;
	}

	public float GetCreatorWeaponActiveWater(){
		return creatorWeapon.fActiveWater;
	}

	public float GetCreatorWeaponCriticalChance(){
		return creatorWeapon.fCriticalChance;
	}

	public float GetCreatorWeaponCriticalDamage(){
		return creatorWeapon.fCriticalDamage;
	}

	public float GetCreatorWeaponBigSuccessed(){
		return creatorWeapon.fBigSuccessed;
	}

	public float GetCreatorWeaponAccuracyRate(){
		return creatorWeapon.fAccuracyRate;
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

	/*
	public E_BOSS_WEAPON Check_Equipment()
	{
		if (WeaponEquipment != null)
			return WeaponEquipment.nIndex;

		return null;
	}*/

    //아이템을 장착할 경우
    public void EquipItem(CGameEquiment _item)
    {
        //아이템이 어디 부위인지 확인한다.
        switch (_item.nSlotIndex)
        {
            case (int)E_EQUIMNET_INDEX.E_WEAPON:

                //만약 무기가 있을 경우 그 무기가 현재 플레이어에 적용되는 값을 빼고 아이템을 넣어줌
                //그 후 다시 아이템 효과를 플레이어에게 적용한다.
                if (WeaponEquipment != null)
                {
                    WeaponEquipment.bIsEquip = false;

                    ApplyItemData(WeaponEquipment, false);
                }

                WeaponEquipment = _item;

                WeaponEquipment.bIsEquip = true;

                ApplyItemData(WeaponEquipment, true);
                break;
            case (int)E_EQUIMNET_INDEX.E_WEAR:

                if (GearEquipmnet != null)
                {
                    ApplyItemData(GearEquipmnet, false);

                    GearEquipmnet.bIsEquip = false;
                }

                GearEquipmnet = _item;

                GearEquipmnet.bIsEquip = true;

                ApplyItemData(GearEquipmnet, true);

                break;
            case (int)E_EQUIMNET_INDEX.E_ACCESSORY:


                if (AccessoryEquipmnet != null)
                {
                    ApplyItemData(AccessoryEquipmnet, false);

                    AccessoryEquipmnet.bIsEquip = false;
                }

                AccessoryEquipmnet = _item;

                AccessoryEquipmnet.bIsEquip = true;

                ApplyItemData(AccessoryEquipmnet, true);
                break;
        }

        //아이템을 장착했기에 다시 정렬
        inventory.inventorySlots[_item.nSlotIndex].RefreshDisplay();
    }

	//세이브를 위한 아이템 세팅
	public void SetAllItemData(bool bIsUp)
	{
		if (WeaponEquipment != null) 	ApplyItemData (WeaponEquipment, bIsUp);
		if (GearEquipmnet  != null)		ApplyItemData (GearEquipmnet, bIsUp);
		if (AccessoryEquipmnet != null)	ApplyItemData (AccessoryEquipmnet, bIsUp);
	}

    public void ApplyItemData(CGameEquiment _item, bool bIsPlus)
    {
        if(bIsPlus)
        {
            if (_item.fReapirPower != 0) changeStats.fRepairPower += _item.fReapirPower;
            if (_item.fArbaitRepair != 0) changeStats.fArbaitsPower += _item.fReapirPower;
            if (_item.fHonorPlus != 0) changeStats.fHornorPlusPercent += _item.fReapirPower;
            if (_item.fGoldPlus != 0) changeStats.fGoldPlusPercent += _item.fReapirPower;
            if (_item.fWaterMaxPlus != 0) changeStats.fMaxWaterPlus += _item.fReapirPower;
            if (_item.fWaterChargePlus != 0) changeStats.fWaterPlus += _item.fReapirPower;
            if (_item.fCritical != 0) changeStats.fCriticalChance += _item.fReapirPower;
            if (_item.fCriticalDamage != 0) changeStats.fCriticalDamage += _item.fReapirPower;
            if (_item.fBigCritical != 0) changeStats.fBigSuccessed += _item.fReapirPower;
            if (_item.fAccuracyRate != 0) changeStats.fAccuracyRate += _item.fReapirPower;
        }
        else 
        {
            if (_item.fReapirPower != 0) changeStats.fRepairPower -= _item.fReapirPower;
            if (_item.fArbaitRepair != 0) changeStats.fArbaitsPower -= _item.fReapirPower;
            if (_item.fHonorPlus != 0) changeStats.fHornorPlusPercent -= _item.fReapirPower;
            if (_item.fGoldPlus != 0) changeStats.fGoldPlusPercent -= _item.fReapirPower;
            if (_item.fWaterMaxPlus != 0) changeStats.fMaxWaterPlus -= _item.fReapirPower;
            if (_item.fWaterChargePlus != 0) changeStats.fWaterPlus -= _item.fReapirPower;
            if (_item.fCritical != 0) changeStats.fCriticalChance -= _item.fReapirPower;
            if (_item.fCriticalDamage != 0) changeStats.fCriticalDamage -= _item.fReapirPower;
            if (_item.fBigCritical != 0) changeStats.fBigSuccessed -= _item.fReapirPower;
            if (_item.fAccuracyRate != 0) changeStats.fAccuracyRate -= _item.fReapirPower;
        }
    }
}




