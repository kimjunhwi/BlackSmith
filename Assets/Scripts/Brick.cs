using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져온다.
    }

	void Update()
	{
		if (Random.Range(0, 300) == 0) // 0.003%의 확률로 메카님의 twinkle 애니메이션을 수행한다.
		{
            anim.SetTrigger("twinkle"); // 트리거는 SetBool과 달리 값이 유지되지 않고 1회만 호출된다.
		}
	}
}
