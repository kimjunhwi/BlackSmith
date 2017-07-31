using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBackGround : MonoBehaviour {

	public SpriteRenderer gameBackGround_Image;
	public SpriteRenderer gameBackGroundBoss_Image;

	public bool isBossBackGround = false;
	public bool isOriginBackGround = false;

	public BossCreator bossCreator;




	public void StartChangeBackGroundToBossBackGround()
	{
		StartCoroutine (ChangeBackGroundToBossBackGround ());
	}

	public	void StartReturnBossBackGroundToBackGround()
	{
		StartCoroutine (ReturnBossBackGroundToBackGround ());
	}

	public IEnumerator ChangeBackGroundToBossBackGround()
	{
		float fBackGroundBoss_AlphaValue = 0f;
		float fOriginBackGround_AlphaValue =0f;

		Color originColor = new Color (0f, 0f, 0f, 0f);
		Color originBossColor = new Color (0f, 0f, 0f, 0f);

		while (true)
		{
			fOriginBackGround_AlphaValue = gameBackGround_Image.color.a ;
			fBackGroundBoss_AlphaValue = gameBackGroundBoss_Image.color.a ;

			fOriginBackGround_AlphaValue -= Time.deltaTime * 0.5f;
			fBackGroundBoss_AlphaValue += Time.deltaTime * 0.5f;

			originColor = new Color (gameBackGround_Image.color.r, gameBackGround_Image.color.g, gameBackGround_Image.color.b, fOriginBackGround_AlphaValue);
			originBossColor = new Color (gameBackGroundBoss_Image.color.r, gameBackGroundBoss_Image.color.g, gameBackGroundBoss_Image.color.b, fBackGroundBoss_AlphaValue);

			gameBackGround_Image.color = originColor;
			gameBackGroundBoss_Image.color = originBossColor;

			if (fOriginBackGround_AlphaValue <= 0)
			{
				Debug.Log ("보스 배경 ChangeComplete!");
				isBossBackGround = true;
				isOriginBackGround = false;
				break;
			} 
			else
				yield return null;
		}


	}

	public IEnumerator ReturnBossBackGroundToBackGround()
	{
		float fBackGroundBoss_AlphaValue = 0f;
		float fOriginBackGround_AlphaValue =0f;
		Color originColor = new Color (0f, 0f, 0f, 0f);
		Color originBossColor = new Color (0f, 0f, 0f, 0f);

		while (true) 
		{
			fOriginBackGround_AlphaValue = gameBackGround_Image.color.a;
			fBackGroundBoss_AlphaValue = gameBackGroundBoss_Image.color.a;

			fOriginBackGround_AlphaValue += Time.deltaTime * 0.5f;
			fBackGroundBoss_AlphaValue -= Time.deltaTime * 0.5f;

			originColor = new Color (gameBackGround_Image.color.r, gameBackGround_Image.color.g, gameBackGround_Image.color.b, fOriginBackGround_AlphaValue);
			originBossColor = new Color (gameBackGroundBoss_Image.color.r, gameBackGroundBoss_Image.color.g, gameBackGroundBoss_Image.color.b, fBackGroundBoss_AlphaValue);

			gameBackGround_Image.color = originColor;
			gameBackGroundBoss_Image.color = originBossColor;

			if (fBackGroundBoss_AlphaValue <= 0)
			{
				Debug.Log ("원래 배경 ChangeComplete!");
				isOriginBackGround = true;	
				isBossBackGround = false;
				break;
			}
		
			else
				yield return null;
		}

	
	
	}
}
