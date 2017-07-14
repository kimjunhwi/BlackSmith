using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{

    public GameObject obj;


	public GameObject []uiPanels;
	public GameObject []uiButtons;
	void Start()
	{
		AllDisable ();

		GameManager.Instance.Root_ui = gameObject;
	}

	public void ActiveMenu(int nIndex)
	{
		
		if (uiPanels [nIndex].activeSelf) 
		{
			uiPanels [nIndex].SetActive (false);

		}
		else
		{
			AllDisable ();
			uiPanels [nIndex].SetActive (true);
		}
	}


	public void AllDisable()
	{
		foreach (GameObject obj in uiPanels) 
		{
			obj.SetActive (false);
		}
	}
		

}
