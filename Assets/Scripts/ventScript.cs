using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 환풍구관련 스크립트
 */
public class ventScript : MonoBehaviour
{   //타겟 환풍구 참조
    public ventScript targetVent;

    //무한 순환이동방지용 변수
    public bool isTarget;
    private void OnEnable()
    {
        isTarget = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg"))//백돌 혹은 흑돌이라면
        {
            if (!isTarget)//내가 타겟이 아니라면
            {
                collision.transform.position = targetVent.transform.position;//타겟으로 이동
                targetVent.isTarget = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg"))
        {
            if (isTarget)//내가 타겟이었다면
            {//초기화
                isTarget = false;
            }
        }
    }
}
