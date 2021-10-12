using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 오브젝트 선택, 취소, 드래그를 감지하고 해당 오브젝트에게 알려주는 클래스
해당 오브젝트는 SelectableObject를 상속받아야한다.
 */
public class ObjectControlManager : MonoBehaviour
{
    SelectableObject selectedObject;
    
    void Update()
    {
        if (RayUtility.TryGetInputPosition(out Vector2 pos) )
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !RayUtility.isOnUI())//터치시작, UI위에 있지않음
            {
               
                if (RayUtility.Raycast(pos,out RaycastHit hit))
                {
                    SelectableObject s =  hit.collider.GetComponentInParent<SelectableObject>();//셀렉터블이면?
                    if (s != null)
                    {
                        if (selectedObject != null && selectedObject != s)
                        {//이미 선택된것이 있다면 취소
                            selectedObject.OnCancel();
                        }
                        
                        selectedObject = s;
                        //해당 오브젝트에게 알려준다.
                        s.OnSelect(hit);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)//터치 종료
            {
                if (selectedObject != null)
                {//있다면 해당 오브젝트에게 알려준다.
                    selectedObject.OnCancel();
                }
                selectedObject = null;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved) // 터치 종료
            {
                if (selectedObject != null)
                {//있다면 해당 오브젝트에게 알려준다.
                    if (RayUtility.Raycast(pos, out RaycastHit hit))
                    {//위치를 구해서 전해준다.
                        selectedObject.OnDrag(hit);
                    }
                }
            }
        }
    }
}
