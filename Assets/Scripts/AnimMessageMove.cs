using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ���ó : �ִϸ��̼� Ű������ �̺�Ʈ�� Ȱ���ϰ������,
          �ִϸ��̼��� �θ� ������Ʈ���� ����ǰ� �̺�Ʈ�� �ڽĿ��� �����ϱ� ������ ������ �޽��� ���� ���� Ŭ����
 */
public class AnimMessageMove : MonoBehaviour
{
    public void DestroyAnimEnd()
    {
        GetComponentInChildren<BlackScript>().DestroyAnimEnd();
    }
}
