using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDisable : MonoBehaviour, IPointerDownHandler
{
	GameObject getInfoGameObject;

	public void OnPointerDown (PointerEventData eventData)
	{
		getInfoGameObject = eventData.pointerEnter;

		if (getInfoGameObject.gameObject == null)
			return;

		if(getInfoGameObject.gameObject.name == "BackGroundPanel")
			gameObject.SetActive (false);
	}
}
