using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 �浹 ���� ��ũ��Ʈ
 ��ġ �ʱ�ȭ, �浹ó��, ����ó��
 */
public class BlackScript : MonoBehaviour
{
    Rigidbody rigid;

    Vector3 moveDir; //�̵�����
    float power;     //�̵����۽� ��
    float tempPower; //�̵��� ��

    Vector3 tempEggPos;  //�ʱ�ȭ�� ��ġ ����
    Vector3 tempEggSclae;//�ʱ�ȭ�� ũ�� ����

    CountText countText;//���� �� �̺�Ʈ ����

    private void Awake()//�ʱ�ȭ�� ������ �ʱⵥ���� ����, ���۷��� ���� �ʱ�ȭ
    {
        tempEggPos = transform.localPosition;
        tempEggSclae = transform.localScale;
        rigid = GetComponent<Rigidbody>();
        countText = GameObject.Find("Count").GetComponent<CountText>();
    }
    private void OnEnable()
    {//��ġ �ʱ�ȭ
        transform.localPosition = tempEggPos;
    }
    private void OnCollisionEnter(Collision collision)//�鵹, �浹, Ÿ�̾� �浹 ó��
    {
        if (collision.gameObject.CompareTag("WhiteEgg"))
        {//�鵹
            moveDir = transform.position- collision.transform.position;         //��������
            moveDir = moveDir.normalized;                                       //����ȭ
            power = collision.transform.parent.GetComponent<EggScript>().templength;//�鵹�κ��� ���� �����´�.
            tempPower = power;//�̵��� �� �ʱⰪ ����
        }else
        if (collision.gameObject.name == ("BlackEgg"))
        {//�浹

            power = //�浹�� �ڽ��� ���� �浹�� �浹�� ���� ���� �� ū ���� ������.
                tempPower > collision.gameObject.GetComponentInParent<BlackScript>().tempPower ?
                tempPower : collision.gameObject.GetComponentInParent<BlackScript>().tempPower;
            moveDir = (transform.position - collision.gameObject.transform.position).normalized; //�������� �� ����ȭ
            tempPower = power; // �̵��� �� �ʱⰪ ����
        }else if (collision.gameObject.name == ("tire"))
        {//Ÿ�̾ ���� �� �ݻ�
            moveDir = Vector3.Reflect(moveDir, collision.contacts[0].normal);//Ÿ�̾�� �����Ǿ��ֱ⶧����, this�� ���⺤���� �ݻ纤�͸� ���� �����Ѵ�.
            tempPower *= 2;//Ÿ�̾ �ݻ�Ǹ� ���� �ι�� Ű���.
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == ("FallBox"))
        {//���� �浹 ���� ������ �������� ��
            GetComponentInParent<Animator>().SetTrigger("Destroy");//���� �ִϸ��̼��� �����Ų��.
        }
    }
    public void DestroyAnimEnd()
    {//���� �ִϸ��̼��� �Ĺݺ� Ű���������� ȣ��Ǵ� �Լ�.
        transform.parent.gameObject.SetActive(false); // ���ӿ�����Ʈ�� ��Ȱ��ȭ�Ѵ�.

        transform.localScale = tempEggSclae;//������Ʈ ũ�⸦ �ʱ�ȭ�Ѵ�.
        countText.onBlackDestroyed(); // �����ǿ��� �ڽ��� ���ϸ� �˸���.
    }
    private void Update()
    {//�浹 ������Ʈ������ �� ������ ���� �����ִٸ� ������ �̵��������� ����ŭ �̵��Ѵ�.
        if (tempPower > 0)
        {
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * tempPower);
            rigid.MovePosition(nextStep);
            tempPower *= 0.9f;//�׸��� �� ������ ���� 90%�� ���ҽ�Ų��.
            if (tempPower <= 0.01)//���� 0.01���ϰ� �Ǹ� ����
            {
                tempPower = 0;
            }
        }
        else
        {
            rigid.velocity = Vector3.zero;//���� ���ٸ� ���� ��Ų��.
        }
    }
}
