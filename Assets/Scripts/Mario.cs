using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mario : MonoBehaviour
{
    Animator m_Anim;
	Rigidbody2D m_Rigidibody2D;
	AudioSource m_AudioSource;
    
    public Text ScoreText;

   


    private float startTime;
    private float fJumpPower = 10f;

     private int nGetGold = 0;
	private int nJumpCount;
	private const int MAX_JUMP_COUNT = 2;

	

	void Start()
	{
        startTime = Time.time;

		m_Anim 			= GetComponent<Animator>(); 		//마리오의 애니메이터 컴포넌트
		m_Rigidibody2D	= GetComponent<Rigidbody2D>();		//마리오 리지디 바디 
		m_AudioSource	= GetComponent<AudioSource>();		//마리오 오디오 관련

		Physics2D.IgnoreLayerCollision(8, 11); 			    // 적캐릭터의 부모와 물리적 충돌을 금지한다. isTrigger = true인 자식은 제외
		m_Anim.SetBool("run", true);						// 달리기 시작

		nJumpCount = MAX_JUMP_COUNT;
	}

	void Update()
	{
        ScoreText.text = (Time.time - startTime + nGetGold).ToString("0.0");

		if (Input.GetKeyDown(KeyCode.Space) && nJumpCount > 0) // 점프키를 눌렀을때
        {
			m_Anim.SetBool ("run", false);

			nJumpCount--;

			m_AudioSource.Play(); // 점프 사운드 연주

			//점프 부분 
			Vector2 tmpVelocity = m_Rigidibody2D.velocity;

			tmpVelocity.y = fJumpPower;

			m_Rigidibody2D.velocity = tmpVelocity;

			m_Anim.SetTrigger("jump"); // 점프 애니메이션 실행

        }
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		// 몬스터나 지면에 닿았을 경우 점프 부분을 초기화 해줌
		if (other.collider.CompareTag ("Grid")) {
			ResetJumpCount ();
		}

        if (other.collider.CompareTag("Monster"))
        {
            Debug.Log("몬스터 충돌");
        }
        else if (other.collider.CompareTag("Coin"))
            other.gameObject.SetActive(false);
	}

	public void ResetJumpCount()
	{
		nJumpCount = MAX_JUMP_COUNT;
		m_Anim.SetBool ("run", true);
	}

    public void GetGold()
    {
        nGetGold++;
    }
}
