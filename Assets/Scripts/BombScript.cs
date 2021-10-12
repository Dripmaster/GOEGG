using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
��ź�� �̵�, �ʱ�ȭ, �ٴ� �浹 �� �̺�Ʈ�� ������ Ŭ����  
 */
public class BombScript : MonoBehaviour
{
    public float downSpeed; // ���ϼӵ�
    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0); // ���� ���̸� 0.5������ �ʱ�ȭ ���ش�.
        StartCoroutine(down());//�������� �����ϵ��� �ڷ�ƾ�� ���� �� �����Ѵ�.
    }
    IEnumerator down()
    {
        do
        {
            transform.Translate(Vector3.down*Time.deltaTime* downSpeed);//���ٸ� ���� ��Ұ� ���⶧���� �����ϰ� �����ߴ�.
            yield return null;
        } while (true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == ("plane"))
        {//plane�� ���������� ���带 ���Ѵ�. ��, ���� �ڵ�� ���忡 ����� �� ����ȴ�.
            foreach (var egg in GameObject.FindGameObjectsWithTag("WhiteEgg"))
            {//Ȱ��ȭ�� �鵹���� Ȯ���Ѵ�.
                if (Vector3.Distance(egg.transform.position, transform.parent.position)<=0.2f)//�Ÿ��� ���� 0.2 ���϶��
                {
                    egg.GetComponentInParent<EggScript>().SetMoveDir((egg.transform.position - transform.parent.position).normalized);//��ź->�鵹 ������ ������ �����Ѵ�.
                    egg.GetComponentInParent<EggScript>().templength = 2; // ��ź�� ������ 2�� ������ �з����� �ߴ�.
                    egg.GetComponentInParent<EggScript>().setState(EggScript.EggState.playerShotMove); // ������ �̵��� �ʿ��� ������ �ߴٸ� ���⼭ �ݹ��� �Ѱ��̴�.
                }
            }



            Destroy(transform.parent.gameObject);//��ź�� �ٴڿ� ������� �����ش�.
        }
    }

}
