using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 ������Ʈ ����, ���, �巡�׸� �����ϰ� �ش� ������Ʈ���� �˷��ִ� Ŭ����
�ش� ������Ʈ�� SelectableObject�� ��ӹ޾ƾ��Ѵ�.
 */
public class ObjectControlManager : MonoBehaviour
{
    SelectableObject selectedObject;
    
    void Update()
    {
        if (RayUtility.TryGetInputPosition(out Vector2 pos) )
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !RayUtility.isOnUI())//��ġ����, UI���� ��������
            {
               
                if (RayUtility.Raycast(pos,out RaycastHit hit))
                {
                    SelectableObject s =  hit.collider.GetComponentInParent<SelectableObject>();//�����ͺ��̸�?
                    if (s != null)
                    {
                        if (selectedObject != null && selectedObject != s)
                        {//�̹� ���õȰ��� �ִٸ� ���
                            selectedObject.OnCancel();
                        }
                        
                        selectedObject = s;
                        //�ش� ������Ʈ���� �˷��ش�.
                        s.OnSelect(hit);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)//��ġ ����
            {
                if (selectedObject != null)
                {//�ִٸ� �ش� ������Ʈ���� �˷��ش�.
                    selectedObject.OnCancel();
                }
                selectedObject = null;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved) // ��ġ ����
            {
                if (selectedObject != null)
                {//�ִٸ� �ش� ������Ʈ���� �˷��ش�.
                    if (RayUtility.Raycast(pos, out RaycastHit hit))
                    {//��ġ�� ���ؼ� �����ش�.
                        selectedObject.OnDrag(hit);
                    }
                }
            }
        }
    }
}
