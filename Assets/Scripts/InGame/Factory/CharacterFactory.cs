using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterFactory : IFactory {

	public GameObject Get(CharacterType type)
    {
        GameObject character = Create(type);

        return character;
    }

    protected abstract GameObject Create(CharacterType type);
}
