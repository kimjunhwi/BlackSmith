using UnityEngine;
using System.Collections;
using System;

public enum WINDOW_TYPE
{
    None = -1,
    Main,
    Modal
}

public class CWindow : MonoBehaviour {

    WINDOW_TYPE m_Type = WINDOW_TYPE.None;

    GameObject  m_Root;             // 부모객체.

    public Action<string> callback_func = null;

    bool m_Active = false;   // 활성여부.
        
    //void Start()
    //{
    //}    
    //void Update()
    //{
    //}

    public virtual void Show(GameObject _root, Action<string> _callback)
    {
        m_Root = _root;
        callback_func = _callback;

        //CGameSnd.instance.PlaySound(eSound.popupon);
        m_Active = true;
    }
    
    public virtual void Close()
    {
        //CGameSnd.instance.PlaySound(eSound.popupoff);
        m_Active = false;
    }
    
    public bool IsActive()
    {
        return m_Active;
    }

}

/*
    // 상속함수. 윈도우 메인 컴포넌트.
    public class ClassName : CWindow
    {
        public override void Show(GameObject _root, Action<string> _callback, string _param = "")
        {
            base.Show(_root, _callback, _param);
        }
    }

    //사용 법.
    System.Action<string> callback = delegate (string rt)
    {
        //Debug.Log(rt);
    };
    CGame.Instance.Show_Window("Prefab/Terms", callback);

    //사용 법.
    CGame.Instance.Show_Notice(CGame.Instance.GetText(00000), rt => 
    {
        if (rt == "0") PlayEnd("socket_close");
    });


    // 범용함수.
    public void Show_Window(string _prefab, GameObject _root, Action<string> _callback, string _param = "")
    {
        //GameObject Root_ui = GameObject.Find("UI Root (3D)"); //ngui
        GameObject go = GameObject.Instantiate(Resources.Load(_prefab), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        go.transform.parent = Root_ui.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = new Vector3(1, 1, 1);

        CWindow w = go.GetComponent<CWindow>();
        w.Show(_root, _callback, _param);
    }

*/
