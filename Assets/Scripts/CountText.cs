using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 �������� �����ϴ� Ŭ����
 */
public class CountText : MonoBehaviour
{
    public Text myText; // UIText�� ����
    int count; // �����̴�. ������忡���� �ð��̴�.
    void OnEnable()
    {
        StopAllCoroutines();//�ڷ�ƾ�� ��ġ�� ��Ȳ�� �����ϱ� ���� �ʱ�ȭ �� ��� �ڷ�ƾ�� �����Ѵ�.(Enable���°� �ƴϸ� �翬�� �ڷ�ƾ�� ��� ����Ǿ� ������, ���⿡ �̷��� ������������� �Ҿ��ϴ�.)
       
        //������ �ؽ�Ʈ�� �ʱ�ȭ�Ѵ�.
        count = 0;
        setText(count);

        if(IntroScript.gameMode==2)//��������� �ð�üũ�� �����Ѵ�.
        StartCoroutine(checkTime());
    }
    IEnumerator checkTime()
    {//1�ʸ��� count���� �� �ؽ�Ʈ �ݿ�.
        do
        {
            setText(count++);
            
            yield return new WaitForSeconds(1);
        } while (true);
    }
    void setText(int v)
    {//��忡 �°� �ؽ�Ʈ�� �ݿ����ش�.

        switch (IntroScript.gameMode)
        {
            case 0:
                myText.text = "���� : "+v;
                break;
            case 1:
                myText.text = "���� : "+v;
                break;
            case 2:
                myText.text = "�����ð� : "+v+"��";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reText()
    {//stage �ʱ�ȭ �� count, text�� �ʱ�ȭ ���ش�.
        count = 0;
        setText(count);
    }
    public void onBlackDestroyed()
    {//�浹�κ��� �޾ƿ��� �浹 ���� �̺�Ʈ ó��, ������ 1�ø��� �ؽ�Ʈ �ݿ�
        count++;
        setText(count);
    }
}
