using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlys
{
	public struct Buff
	{
		public int nIndex;
		public float fValue;
		public bool bIsInfinity;

		public Buff(int _nIndex,float _fValue, bool _bIsInfinity)
		{
			nIndex = _nIndex;
			fValue = _fValue;
			bIsInfinity = _bIsInfinity;
		}
	}

    enum E_SortingSprite
    {
        E_BACK = 2,
        E_WALK,
    }


	public enum E_ArbaitState
    {
        //대기
        E_WAIT ,
        //수리중
        E_REPAIR,
    }

	public enum E_ARBAIT : int
	{
		E_BLUEHAIR = 0,
		E_REDHAIR,
		E_NURSE,
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
}
