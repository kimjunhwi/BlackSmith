using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadOnlys;

public class PlayerStatus : MonoBehaviour {

	float fDefaultAttack;

	List<Buff> buffs;

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
