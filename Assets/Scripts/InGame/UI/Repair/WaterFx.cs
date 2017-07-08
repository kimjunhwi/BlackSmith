using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFx : MonoBehaviour 
{

	public Animator bossWaterCat_animator;						//보스무기일때의 고양이
	public Animator weaponWaterCat_animator;					//그냥무기일때의 고양이
	public Animator CatWater_animator;							//물 이펙트

	public RepairObject repairObj;

	void Start()
	{
		CatWater_animator = GetComponent<Animator> ();
	}

	public IEnumerator StartWaterFx()
	{
		while (true)
		{

			if (repairObj.bossCharacter != null) 
			{
				//BossWepaonWater
				if (repairObj.isTouchWater == true)
				{
					gameObject.transform.SetAsLastSibling ();
					Debug.Log("BossWeaponWaterFx !!");
					CatWater_animator.SetBool ("isTouchWater", true);

					if (CatWater_animator.GetCurrentAnimatorStateInfo (0).IsName ("Water_Fx_spread"))
					{

						yield return new WaitForSeconds (0.5f);
						Debug.Log ("BossWeaponWaterFinish !!");
						gameObject.transform.SetAsFirstSibling ();
						//bossWaterCat_animator.SetBool ("isTouchWater", false);
						CatWater_animator.SetBool ("isTouchWater", false);
						repairObj.isTouchWater = false;
						//bossWaterCat_animator.Play ("WaterCat_Idle");
						CatWater_animator.Play ("Water_Fx_Idle");

					}


					else
						yield return null;

				}
			} 
			else 
			{
				//normalWeaponWater
				if (repairObj.isTouchWater == true)
				{
					gameObject.transform.SetAsLastSibling ();
					Debug.Log("NormalWeaponWater !!");
					//weaponWaterCat_animator.Play ("WaterCat_spread");
					weaponWaterCat_animator.SetBool ("isTouchWater", true);
					//yield return new WaitForSeconds(0.2f);
					CatWater_animator.SetBool ("isTouchWater", true);

					if (CatWater_animator.GetCurrentAnimatorStateInfo (0).IsName ("Water_Fx_spread")) {
						yield return new WaitForSeconds (0.5f);
						Debug.Log ("WeaponWaterFinish !!");
						gameObject.transform.SetAsFirstSibling ();
						weaponWaterCat_animator.SetBool ("isTouchWater", false);
						CatWater_animator.SetBool ("isTouchWater", false);
						repairObj.isTouchWater = false;
						weaponWaterCat_animator.Play ("WaterCat_Idle");
						CatWater_animator.Play ("Water_Fx_Idle");
					} 

				}
			}
			yield return null;

		}

	}
}
