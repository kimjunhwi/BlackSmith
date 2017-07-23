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

    public Text goldText;

    public float m_fGetGold = 0;

    private void Awake()
    {
        scireInstance = this;
        goldText.text = "0";
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

}
