using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class PlayerStatus{

	float fDefaultAttack;

	List<Buff> buffs;

    public void AddBuff(Buff _buff)
    {
        buffs.Add(_buff);
    }

    public void DeleteBuff(Buff _buff)
    {
        buffs.Remove(_buff);
    }

	public float attack
	{
		private set { attack = value; }

		get
		{
			float accumulatedAttackBuff = 0.0f;

			for(int i = 0; i < buffs.Count; ++i)
			{
				// accumulate
			}

			return fDefaultAttack + accumulatedAttackBuff;
		}
	}
}
