using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlys
{
	public struct Buff
	{
		public int nIndex;
		public float fValue;
        public float fCurrentFloat;
        public string strBuffExplain;

		public Buff(int _nIndex,float _fValue, float _fCurrentFloat ,string _strBuffExplain)
		{
			nIndex = _nIndex;
			fValue = _fValue;
            fCurrentFloat = _fCurrentFloat;
            strBuffExplain = _strBuffExplain;
		}
	}

	enum E_Enhace
	{
		E_SMITH = 0,
		E_REPAIR,
		E_MAXWATER,
		E_WATERPLUS,
		E_ACCURACY,
		E_CRITICAL,
	}

    enum E_SortingSprite
    {
        E_BACK = 2,
        E_WALK,
    }

	public enum E_ArbaitGrade
	{
		E_Cgrade = 0,
		E_Bgrade,
		E_Agrade,
		E_Sgrade,

	}

	public enum E_ArbaitState
    {
        //대기
        E_WAIT ,
        //수리중
        E_REPAIR,
		E_FREEZE,
		E_BOSSREPAIR,
    }

	public enum E_ARBAIT : int
	{
		E_ROY = 0,
		E_MIA,
		E_NURSE,
		E_CLEA,
		E_ROSA,
		E_LUNA,
		E_GLAUS,
		E_ELLIE,
		E_MICHEAL,
		E_BELL,
	}

	public enum E_SPEECH : int
	{
		E_ARBAITONE = 0,
		E_ARBAITTWO,
		E_ARBAITTHREE,
		E_PLAYER,
		E_NONE,
	}

    enum E_CHECK
    {
        E_FAIL = -1,
        E_SUCCESS,
    }


    enum E_EQUIMNET_INDEX
    {
        E_WEAPON = 0,
        E_WEAR,
        E_ACCESSORY,
    }


	//Smith Enhance
	enum E_SMITH_INDEX
	{
		E_REPAIR = 0,
		E_SMITH ,
		E_MAX_WATER,
		E_WATER_CHARGING,
		E_ACCURACY_RATE,
		E_CRITICAL_CHANCE,
	}
}
