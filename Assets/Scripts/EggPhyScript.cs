using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 �鵹�� ���� ������ �ڵ带 ���� �����ߴ�.
 �ʱ⿡ �� �ڵ带 �̿��� �浹���� �����Ϸ� �ߴµ�, �浹�� �ٸ� ��ü�� ���ؼ��� �и��� ������ �����ϴ� �κ��� ���� ���� ���� �����ߴ�.
 */
public class EggPhyScript : MonoBehaviour
{
    public Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {//���� �����ٸ�?
            if (collision.contacts[0].normal != Vector3.up)
            {//���� �븻���͸� ������ �ݻ纤�͸� ���Ѵ�. �׸��� �־��ش�.
                Vector3 reflect = Vector3.Reflect(transform.parent.GetComponent<EggScript>().moveDir, collision.contacts[0].normal);
                transform.parent.GetComponent<EggScript>().SetMoveDir(reflect.normalized);
            }
        }
        else if (collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg")|| collision.gameObject.name == ("tire"))
        {//�� Ȥ�� Ÿ�̾ �����ٸ� 1�������� �ݻ纤�͸� ���� ƨ����� �Ѵ�.
            Vector3 reflect = Vector3.Reflect(transform.parent.GetComponent<EggScript>().moveDir, collision.contacts[0].normal);
            transform.parent.GetComponent<EggScript>().SetMoveDir(reflect.normalized);
            if (collision.gameObject.transform.parent.GetComponent<EggScript>() != null)
            {//������ �鵹���� �������̶��
                transform.parent.GetComponent<EggScript>().templength = //���� ���� �浹�� �鵹�� �� �� ū���� �����´�.
                    collision.gameObject.transform.parent.GetComponent<EggScript>().templength > transform.parent.GetComponent<EggScript>().templength ?
                    collision.gameObject.transform.parent.GetComponent<EggScript>().templength : transform.parent.GetComponent<EggScript>().templength;
                transform.parent.GetComponent<EggScript>().SetMoveDir((transform.position - collision.transform.position).normalized);//���⺤�� ��� �� ����ȭ
                transform.parent.GetComponent<EggScript>().setState(EggScript.EggState.playerShotMove);
            }

            if (collision.gameObject.name == ("tire"))
            {//Ÿ�̾��� �� �ι�
                transform.parent.GetComponent<EggScript>().templength*=2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == ("FallBox"))
        {
            //���� �浹�ڽ��� �����ٸ鳫��
            transform.parent.GetComponent<EggScript>().Fall();
        }
    }

}
