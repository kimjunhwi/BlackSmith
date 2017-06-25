using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlys;

public class ArbaitCharacter : MonoBehaviour {

    public int nIndex;
    private int nGetBatchIndex;

    protected bool m_bIsBatch = false;

	private Button m_BuyButtonEvent;
	private Button m_EnhanceButtonEvent;

    protected Sprite m_NoneImage;
    protected Sprite m_CharacterImage;
    
    protected Image m_CharacterImageObject;
    protected ArbaitData m_CharacterData;


    private Toggle m_SettingToggle;

    private GameObject m_BuyButton;
    private GameObject m_SettingPanel;
    private SpawnManager spawnManager;



    public virtual void Upgrade(){}

	// Use this for initialization
	protected void Awake () {
		
        m_BuyButton = transform.FindChild("BuyButton").gameObject;

		m_BuyButtonEvent = m_BuyButton.GetComponent<Button> ();

		m_BuyButtonEvent.onClick.AddListener (BuyCharacter);
        
		m_SettingPanel = transform.FindChild("SettingPanel").gameObject;

		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
		m_CharacterImageObject = transform.FindChild("CharacterImage").GetComponent<Image>();
        
		m_SettingToggle = m_SettingPanel.transform.FindChild("SettingToggle").GetComponent<Toggle>();

		m_EnhanceButtonEvent = m_SettingPanel.transform.FindChild ("EnhanceButton").GetComponent<Button> ();

		m_SettingToggle.onValueChanged.AddListener (OnBatchToggle);

		m_EnhanceButtonEvent.onClick.AddListener (EnhanceEvent);
	}

	public void SetUp(int _nIndex,Sprite _sprite)
	{
		nIndex = _nIndex;

		m_CharacterImageObject.sprite = _sprite;

		m_CharacterData = GameManager.Instance.GetArbaitData(nIndex);
		
		CheckBuyCharacter();
	}

    public void BuyCharacter()
    {
        //스코어 매니저를 만들었을 경우 개선

        if(m_CharacterData.nScoutGold > 5)
        {
            //추후 추가
            m_CharacterData.level = 1;
            
        }
        CheckBuyCharacter();
    }

    private void CheckBuyCharacter()
    {
        //구매하지 않았을 경우
        if (m_CharacterData.level == 0)
        {
            m_BuyButton.SetActive(true);
            m_SettingPanel.SetActive(false);
            //m_CharacterImageObject.sprite = m_CharacterNoneSprite;
        }

        //구매했을경우
        else
        {
            m_BuyButton.SetActive(false);
            m_SettingPanel.SetActive(true);
            
            if (m_CharacterData.batch != -1)
                m_SettingToggle.isOn = true;
            else
                m_SettingToggle.isOn = false;
        }
    }

	public void OnBatchToggle(bool _bIsToggle)
    {
		bool bIsToggle = _bIsToggle;
        
		Debug.Log(m_SettingToggle.isOn);

        if(bIsToggle && m_bIsBatch == false)
        {
            nGetBatchIndex = spawnManager.AddArbaitCheck();

            if (nGetBatchIndex != (int)E_CHECK.E_FAIL)
            {
                spawnManager.AddArbait(nGetBatchIndex, gameObject, m_CharacterData);
                m_bIsBatch = true;
            }
        }
        else if((bIsToggle == false) && m_bIsBatch)
        {
            spawnManager.DeleteArbait(gameObject);

            m_bIsBatch = false;
        }

        Debug.Log(m_SettingToggle.isOn);
    }

	public void EnhanceEvent()
	{

	}
}
