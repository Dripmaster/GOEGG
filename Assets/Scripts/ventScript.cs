using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ȯǳ������ ��ũ��Ʈ
 */
public class ventScript : MonoBehaviour
{   //Ÿ�� ȯǳ�� ����
    public ventScript targetVent;

    //���� ��ȯ�̵������� ����
    public bool isTarget;
    private void OnEnable()
    {
        isTarget = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg"))//�鵹 Ȥ�� �浹�̶��
        {
            if (!isTarget)//���� Ÿ���� �ƴ϶��
            {
                collision.transform.position = targetVent.transform.position;//Ÿ������ �̵�
                targetVent.isTarget = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("WhiteEgg") || collision.collider.CompareTag("BlackEgg"))
        {
            if (isTarget)//���� Ÿ���̾��ٸ�
            {//�ʱ�ȭ
                isTarget = false;
            }
        }
    }
}
