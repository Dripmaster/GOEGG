using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 백돌의 물리 관련한 코드를 따로 구현했다.
 초기에 이 코드를 이용해 흑돌에도 적용하려 했는데, 흑돌은 다른 물체에 의해서만 밀리기 때문에 재사용하는 부분이 거의 없어 따로 구현했다.
 */
public class EggPhyScript : MonoBehaviour
{
    public Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {//벽을 만났다면?
            if (collision.contacts[0].normal != Vector3.up)
            {//벽의 노말벡터를 가져와 반사벡터를 구한다. 그리고 넣어준다.
                Vector3 reflect = Vector3.Reflect(transform.parent.GetComponent<EggScript>().moveDir, collision.contacts[0].normal);
                transform.parent.GetComponent<EggScript>().SetMoveDir(reflect.normalized);
            }
        }
        else if (collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg")|| collision.gameObject.name == ("tire"))
        {//돌 혹은 타이어를 만났다면 1차적으로 반사벡터를 구해 튕기려고 한다.
            Vector3 reflect = Vector3.Reflect(transform.parent.GetComponent<EggScript>().moveDir, collision.contacts[0].normal);
            transform.parent.GetComponent<EggScript>().SetMoveDir(reflect.normalized);
            if (collision.gameObject.transform.parent.GetComponent<EggScript>() != null)
            {//하지만 백돌끼리 만난것이라면
                transform.parent.GetComponent<EggScript>().templength = //나의 힘과 충돌한 백돌의 힘 중 큰것을 가져온다.
                    collision.gameObject.transform.parent.GetComponent<EggScript>().templength > transform.parent.GetComponent<EggScript>().templength ?
                    collision.gameObject.transform.parent.GetComponent<EggScript>().templength : transform.parent.GetComponent<EggScript>().templength;
                transform.parent.GetComponent<EggScript>().SetMoveDir((transform.position - collision.transform.position).normalized);//방향벡터 계산 및 정규화
                transform.parent.GetComponent<EggScript>().setState(EggScript.EggState.playerShotMove);
            }

            if (collision.gameObject.name == ("tire"))
            {//타이어라면 힘 두배
                transform.parent.GetComponent<EggScript>().templength*=2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == ("FallBox"))
        {
            //낙하 충돌박스를 나갔다면낙하
            transform.parent.GetComponent<EggScript>().Fall();
        }
    }

}
