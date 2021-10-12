using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 사용처 : 애니메이션 키프레임 이벤트를 활용하고싶은데,
          애니메이션은 부모 오브젝트에서 실행되고 이벤트는 자식에서 구현하기 때문에 구현한 메시지 전달 역할 클래스
 */
public class AnimMessageMove : MonoBehaviour
{
    public void DestroyAnimEnd()
    {
        GetComponentInChildren<BlackScript>().DestroyAnimEnd();
    }
}
