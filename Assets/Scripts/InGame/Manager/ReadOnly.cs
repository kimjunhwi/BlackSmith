using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlys
{
    enum E_SortingSprite
    {
        E_BACK = 2,
        E_WALK,
    }


    enum E_ArbaitState
    {
        //대기
        E_WAIT ,
        //수리중
        E_REPAIR,
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
