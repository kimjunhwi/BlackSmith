﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTouchPool : SimpleObjectPool {

    // 싱글톤(Singleton) 형식으로 타격 이펙트1을 생성하는 오브젝트 풀을 생성합니다.
    private static NormalTouchPool _instance;
    public static NormalTouchPool Instance
    {
        get
        {
            // 이미 이 클래스가 객체로 존재하는 지 Double Check!
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(NormalTouchPool)) as NormalTouchPool;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    _instance = container.AddComponent(typeof(NormalTouchPool)) as NormalTouchPool;
                }
            }

            return _instance;
        }
    }

    public void Init()
    {
        // 실제 경로는  /Assets/Resources/ 하위입니다.
        strPrefabName = "Prefabs/FxPrefab/NormalTouch";

        //게임오브젝트를 미리 할당해줌
        prefab = Resources.Load(strPrefabName) as GameObject;

        // 초기에 만들 오브젝트 수를 정합니다.
        nPoolSize = 20;
        // 오브젝트 풀의 게임 오브젝트 이름을 설정합니다.
        gameObject.name = "NormalTouch";
        gameObject.layer = 2;
        // 오브젝트를 미리 생성해둡니다.
        PreloadPool();
    }
}