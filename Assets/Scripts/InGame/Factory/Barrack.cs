using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : GenericMonoSingleton<Barrack> {

    private IFactory characterFactory = new MonsterFactory();

    public GameObject MonsterGet(CharacterType type)
    {
        return characterFactory.Get(type);
    }

    public GameObject Get<T>(CharacterType type) where T : CharacterFactory, new()
    {
        var factory = new T();
        return factory.Get(type);
    }

}
