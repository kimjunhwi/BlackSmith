using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum PANELINDEX
{
	PANEL_BOSS = 10000,
	PANEL_ARBAIT,
	PANEL_INVENTORY,
	PANEL_SHOP,
	PANEL_WEAR,
	PANEL_QUEST,
	PANEL_INHANCE,
}


public class PopUpWindow : MonoBehaviour
{
	public GameObject PopUpWindow_Yes;
	public GameObject PopUpWindow_YesNo;

	private Text PopUpWindow_Yes_Text;
	private Text PopUpWindow_YesNo_Text;

	private Button PopUpWindow_Yes_Button;
	private Button PopUpWindow_YesNo_YesButton;
	private Button PopUpWindow_YesNo_NoButton;

	public BossCreator bossCreator;
	public int nBossIndex;

	private bool isActivePopUp =false;


	void Start()
	{
		PopUpWindow_Yes_Text = PopUpWindow_Yes.GetComponentInChildren<Text> ();
		PopUpWindow_Yes_Button = PopUpWindow_Yes.GetComponentInChildren<Button> ();

		PopUpWindow_YesNo_Text = PopUpWindow_YesNo.GetComponentInChildren<Text> ();
		PopUpWindow_YesNo_YesButton = PopUpWindow_YesNo.transform.GetChild (1).GetComponent<Button> ();
		PopUpWindow_YesNo_NoButton = PopUpWindow_YesNo.transform.GetChild(2).GetComponent<Button>();

		PopUpWindow_YesNo_NoButton.onClick.AddListener (PopUpWindowYesNo_Switch);
		PopUpWindow_YesNo_YesButton.onClick.AddListener (PopUpWindowYesNo_Switch);
		PopUpWindow_YesNo_YesButton.onClick.AddListener (() => bossCreator.BossCreateInit (nBossIndex));


		PopUpWindow_Yes.SetActive (false);
		PopUpWindow_YesNo.SetActive (false);
	}


	public void PopUpWindowYes_Switch()
	{
		if (PopUpWindow_Yes.activeSelf != true)
			PopUpWindow_Yes.SetActive (true);
		else
			PopUpWindow_Yes.SetActive (false);	
	}

	public void PopUpWindowYesNo_Switch()
	{
		if (PopUpWindow_YesNo.activeSelf != true)
		{
			
				isActivePopUp = true;
			
			if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_SASIN) {
				
				PopUpWindow_YesNo_Text.text = "보스(사신)을 소환 하시겠습니까?";
		
				
			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_ICE) {
				//PopUpWindow_YesNo_YesButton.onClick.AddListener (() => bossCreator.BossCreateInit (nBossIndex));
				PopUpWindow_YesNo_Text.text = "보스(얼음)을 소환 하시겠습니까?";
			

			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_FIRE) {
				//PopUpWindow_YesNo_YesButton.onClick.AddListener (() => bossCreator.BossCreateInit (nBossIndex));
				PopUpWindow_YesNo_Text.text = "보스(불)을 소환 하시겠습니까?";

			} else if (nBossIndex == (int)E_BOSSNAME.E_BOSSNAME_MUSIC) {
				//PopUpWindow_YesNo_YesButton.onClick.AddListener (() => bossCreator.BossCreateInit (nBossIndex));
				PopUpWindow_YesNo_Text.text = "보스(음악)을 소환 하시겠습니까?";
		
			} else
				PopUpWindow_YesNo_Text.text = "보스 소환 실패";
			
			PopUpWindow_YesNo.SetActive (true);
		}

		else
			PopUpWindow_YesNo.SetActive (false);
	}

	public void GetBossIndex(int _index)
	{
		nBossIndex = _index;
	}



}
