using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDisable : MonoBehaviour, IPointerDownHandler
{
	GameObject getInfoGameObject;
	public bool isBossSummon = false;				//보스 소환중
	public BossConsumeItemInfo bossConumeItemInfo;
	public BossCreator bossCreator;
	public GameObject yesNoButton;

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;

		if (getInfoGameObject.gameObject.name == "BackGroundPanel")
			getInfoGameObject.transform.parent.gameObject.SetActive (false);
		
		if (getInfoGameObject.gameObject.name == "BossBackGround" && isBossSummon == false)
		{
			//보스 창이 닫히면 시간 저장
			bossCreator.BossPanelInfoSave ();
			yesNoButton.SetActive (false);
			getInfoGameObject.SetActive (false);
		}
	}
}
