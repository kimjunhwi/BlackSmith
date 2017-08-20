using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//제네릭 싱글톤을 사용하지 않은 이유는
//이 씬에서만 유지되게 하기 위함
public class ScoreManager : MonoBehaviour 
{

    private static ScoreManager scireInstance;

    public static ScoreManager ScoreInstance
    {
        get
        {
            if (scireInstance == null)
            {
                scireInstance = GameObject.FindObjectOfType<ScoreManager>();

                //없을경우 만들어서 넣어줌
                if (null == scireInstance)
                {
                    var go = new GameObject("ScoreManager");

                    scireInstance = go.AddComponent<ScoreManager>();
                }
            }

            return scireInstance;
        }
    }

	public PlayerDaysInfo playerDaysInfo;

    public Text goldText;
	public Text honorText;
	public Text SuccessedGuestCount;
	public Text FaieldGuestCount;

    private float m_fGetGold = 0;
	private float m_fGetHonor = 0;

    private void Awake()
    {
        scireInstance = this;

		if (PlayerPrefs.HasKey ("Gold"))
			m_fGetGold = PlayerPrefs.GetFloat ("Gold");
		else
			m_fGetGold = 0.0f;

		if (PlayerPrefs.HasKey ("Honor"))
			m_fGetHonor = PlayerPrefs.GetFloat ("Honor");
		else
			m_fGetGold = 0.0f;

		goldText.text = m_fGetGold.ToString("F1");
		honorText.text = m_fGetHonor.ToString("F1");

		SetCurrentDays (GameManager.Instance.player.GetDay ());
		SetMaxDays (GameManager.Instance.player.GetMaxDay ());
    }

    public float GetGold() { return m_fGetGold; }
	public float GetHonor() { return m_fGetHonor; }

	public void SetSuccessedGuestCount(int _nValue)
	{
		SuccessedGuestCount.text = string.Format ("Successed {0} / 10", _nValue);
	}

	public void SetFaieldGuestCount(int _nValue)
	{
		FaieldGuestCount.text = string.Format ("Faield {0} / 1", _nValue);
	}


    private string GetCurrentcyIntoString(float _fValueToConvert)
    {
        string converted;

        if (_fValueToConvert > 1000)
            converted = string.Format("{0}{1}", (_fValueToConvert * 0.001).ToString("f1") , "K");
        else
            converted = string.Format("{0}",(int)_fValueToConvert);

        return converted;
    }

    public void GoldPlus(float _fValue)
    {
        m_fGetGold += _fValue;

        goldText.text = GetCurrentcyIntoString(m_fGetGold);
    }

	public void HonorPlus(float _fValue)
	{
		m_fGetHonor += _fValue;

		honorText.text = GetCurrentcyIntoString (m_fGetHonor);
	}

	public void SetCurrentDays(int _Days)
	{
		playerDaysInfo.CurrentDaysText.text = string.Format ("{0}", _Days);
		GameManager.Instance.player.SetDay (_Days);
	}

	public void SetMaxDays(int _Days)
	{
		playerDaysInfo.MaxDaysText.text = string.Format ("{0}", _Days);
		GameManager.Instance.player.SetMaxDay (_Days);
	}

}
