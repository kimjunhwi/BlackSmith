using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//화면 해상도와 안드로이드 관리 부분이다
//현재 해상도는 10:16이다.
public class MobileSetting : GenericMonoSingleton<MobileSetting>
{
    private bool exit = false;
    private float time = 0.0f;


    void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
    }

    void Update()
    {
        ExitApplication();
    }

    void ExitApplication()
    {
        if (exit)
        {
            time = +Time.deltaTime;
            if (time >= 1f)
            {
                exit = false;
                time = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exit)
            {
                exit = true;
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
