using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 흑돌 관련 스크립트
 위치 초기화, 충돌처리, 낙하처리
 */
public class BlackScript : MonoBehaviour
{
    Rigidbody rigid;

    Vector3 moveDir; //이동방향
    float power;     //이동시작시 힘
    float tempPower; //이동중 힘

    Vector3 tempEggPos;  //초기화용 위치 변수
    Vector3 tempEggSclae;//초기화용 크기 변수

    CountText countText;//낙하 시 이벤트 전달

    private void Awake()//초기화용 변수에 초기데이터 저장, 레퍼런스 변수 초기화
    {
        tempEggPos = transform.localPosition;
        tempEggSclae = transform.localScale;
        rigid = GetComponent<Rigidbody>();
        countText = GameObject.Find("Count").GetComponent<CountText>();
    }
    private void OnEnable()
    {//위치 초기화
        transform.localPosition = tempEggPos;
    }
    private void OnCollisionEnter(Collision collision)//백돌, 흑돌, 타이어 충돌 처리
    {
        if (collision.gameObject.CompareTag("WhiteEgg"))
        {//백돌
            moveDir = transform.position- collision.transform.position;         //방향지정
            moveDir = moveDir.normalized;                                       //정규화
            power = collision.transform.parent.GetComponent<EggScript>().templength;//백돌로부터 힘을 가져온다.
            tempPower = power;//이동중 힘 초기값 지정
        }else
        if (collision.gameObject.name == ("BlackEgg"))
        {//흑돌

            power = //흑돌은 자신의 힘과 충돌한 흑돌의 힘을 비교해 더 큰 쪽을 가진다.
                tempPower > collision.gameObject.GetComponentInParent<BlackScript>().tempPower ?
                tempPower : collision.gameObject.GetComponentInParent<BlackScript>().tempPower;
            moveDir = (transform.position - collision.gameObject.transform.position).normalized; //방향지정 및 정규화
            tempPower = power; // 이동중 힘 초기값 지정
        }else if (collision.gameObject.name == ("tire"))
        {//타이어에 닿을 시 반사
            moveDir = Vector3.Reflect(moveDir, collision.contacts[0].normal);//타이어는 고정되어있기때문에, this의 방향벡터의 반사벡터를 구해 적용한다.
            tempPower *= 2;//타이어에 반사되면 힘을 두배로 키운다.
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == ("FallBox"))
        {//낙하 충돌 범위 밖으로 떨어졌을 시
            GetComponentInParent<Animator>().SetTrigger("Destroy");//낙하 애니메이션을 재생시킨다.
        }
    }
    public void DestroyAnimEnd()
    {//낙하 애니메이션의 후반부 키프레임으로 호출되는 함수.
        transform.parent.gameObject.SetActive(false); // 게임오브젝트를 비활성화한다.

        transform.localScale = tempEggSclae;//오브젝트 크기를 초기화한다.
        countText.onBlackDestroyed(); // 점수판에게 자신의 낙하를 알린다.
    }
    private void Update()
    {//흑돌 업데이트에서는 매 프레임 힘이 남아있다면 지정된 이동방향으로 힘만큼 이동한다.
        if (tempPower > 0)
        {
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * tempPower);
            rigid.MovePosition(nextStep);
            tempPower *= 0.9f;//그리고 매 프레임 힘을 90%로 감소시킨다.
            if (tempPower <= 0.01)//힘이 0.01이하가 되면 정지
            {
                tempPower = 0;
            }
        }
        else
        {
            rigid.velocity = Vector3.zero;//힘이 없다면 정지 시킨다.
        }
    }
}
