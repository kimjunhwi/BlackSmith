using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : CharacterFactory {
    protected override GameObject Create(CharacterType type)
    {
        GameObject monster = null;

        if(CharacterType.LFMonster == type)
            monster = Resources.Load("Prefabs/LFMonster") as GameObject;
        
        else if (CharacterType.LerpMonster == type)
            monster = Resources.Load("Prefabs/LerpMonster") as GameObject;

        else if (CharacterType.StreightMonster == type)
            monster = Resources.Load("Prefabs/SterightMonster") as GameObject;


        return monster;
    }
}
