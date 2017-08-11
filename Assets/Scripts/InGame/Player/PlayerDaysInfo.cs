using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerDaysInfo : MonoBehaviour , IPointerDownHandler
{
	private Player player;

	public GameObject CurrentDaysPanel;
	public GameObject MaxDaysPanel;

	public Text CurrentDaysText;
	public Text MaxDaysText;


	void Awake()
	{
		if (player == null)
			player = GameManager.Instance.player;


		if (CurrentDaysText.text == "")
			CurrentDaysText.text = player.GetDay ().ToString();
	
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		GameObject getInfoGameObject = eventData.pointerEnter;
		//Debug.Log ("Touched : " + getInfoGameObject.name);
		if (getInfoGameObject.gameObject == null)
			return;

		//MaxDay
		if (getInfoGameObject.gameObject.name == "CurrentDaysText" || 
			getInfoGameObject.gameObject.name == "DayText") 
		{
			Debug.Log ("Touched : " + getInfoGameObject.name);
		

			CurrentDaysText.text = "";
			MaxDaysText.text = player.GetMaxDay ().ToString ();


			CurrentDaysPanel.SetActive (false);
			MaxDaysPanel.SetActive (true);


		}

		//CurDay
		if (getInfoGameObject.gameObject.name == "MaxDaysText"|| 
			getInfoGameObject.gameObject.name == "DayText") 
		{
			Debug.Log ("Touched : " + getInfoGameObject.name);

			CurrentDaysText.text = player.GetDay ().ToString ();
			MaxDaysText.text = "";

			CurrentDaysPanel.SetActive (true);
			MaxDaysPanel.SetActive (false);

		}
	}
}
