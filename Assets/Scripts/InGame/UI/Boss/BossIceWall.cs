using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceWall : MonoBehaviour 
{
	private int nCountBreakWall = 15;
	public BossIce bossIce;



	public void TapWall(int _hitDamage)
	{
		nCountBreakWall -= _hitDamage;
		if (nCountBreakWall == 0) {
			nCountBreakWall = 15;
			bossIce.ActiveIceWall ();
		}
	}

}
