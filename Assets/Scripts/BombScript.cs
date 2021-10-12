using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
폭탄의 이동, 초기화, 바닥 충돌 시 이벤트를 구현한 클래스  
 */
public class BombScript : MonoBehaviour
{
    public float downSpeed; // 낙하속도
    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0); // 로컬 높이를 0.5정도로 초기화 해준다.
        StartCoroutine(down());//매프레임 낙하하도록 코루틴을 생성 및 실행한다.
    }
    IEnumerator down()
    {
        do
        {
            transform.Translate(Vector3.down*Time.deltaTime* downSpeed);//별다른 물리 요소가 없기때문에 간단하게 구현했다.
            yield return null;
        } while (true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == ("plane"))
        {//plane은 스테이지의 보드를 말한다. 즉, 이하 코드는 보드에 닿았을 때 실행된다.
            foreach (var egg in GameObject.FindGameObjectsWithTag("WhiteEgg"))
            {//활성화된 백돌들을 확인한다.
                if (Vector3.Distance(egg.transform.position, transform.parent.position)<=0.2f)//거리를 비교해 0.2 이하라면
                {
                    egg.GetComponentInParent<EggScript>().SetMoveDir((egg.transform.position - transform.parent.position).normalized);//폭탄->백돌 방향을 지정해 전송한다.
                    egg.GetComponentInParent<EggScript>().templength = 2; // 폭탄에 맞으면 2의 힘으로 밀려나게 했다.
                    egg.GetComponentInParent<EggScript>().setState(EggScript.EggState.playerShotMove); // 위에서 이동에 필요한 장전을 했다면 여기서 격발을 한것이다.
                }
            }



            Destroy(transform.parent.gameObject);//폭탄은 바닥에 닿았으니 없애준다.
        }
    }

}
