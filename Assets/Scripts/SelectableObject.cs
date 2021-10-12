using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{//���õɼ� �ִ� ������Ʈ��� �� Ŭ������ ��ӹ޾ƾ��Ѵ�.
    public abstract void OnSelect(RaycastHit hit);
    public abstract void OnCancel();
    public abstract void OnDrag(RaycastHit hit);
}
