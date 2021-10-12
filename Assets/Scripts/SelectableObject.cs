using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{//선택될수 있는 오브젝트라면 이 클래스를 상속받아야한다.
    public abstract void OnSelect(RaycastHit hit);
    public abstract void OnCancel();
    public abstract void OnDrag(RaycastHit hit);
}
