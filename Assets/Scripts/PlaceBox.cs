using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 tire를 위치시키기 위해 존재하는 오브젝트 및 클래스
 */
public class PlaceBox : SelectableObject
{
    //타이어와 버튼 참조
    public Transform tire;
    public Button onBtn;
    public Button offBtn;
    private void Awake()
    {
        //초기화 및 전략모드일때만 동작하게 한다.
        if(IntroScript.gameMode != 1)
        {
            onBtn.gameObject.SetActive(false);
            offBtn.gameObject.SetActive(false);
        }
        else
        {
            onBtn.gameObject.SetActive(false);
            offBtn.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    public override void OnSelect(RaycastHit hit)
    {
        //바닥을 눌렀다면 타이어의 위치를 바꿔준다.
        Vector3 hitPose = hit.point;
        hitPose.y = transform.parent.position.y;
        tire.transform.position = hitPose;
    }

    public override void OnCancel()
    {

    }

    public override void OnDrag(RaycastHit hit)
    {
        //바닥을 드래그 했다면 타이어의 위치를 바꿔준다.
        Vector3 hitPose = hit.point;
        hitPose.y = transform.parent.position.y;
        tire.transform.position = hitPose;
    }
    public void OnPlaceBtnClicked(bool value)
    {
        //on 버튼이 눌리면 타이어 이동 활성화, off버튼이 눌리면 비활성화
        if (IntroScript.gameMode != 1) return;
        gameObject.SetActive(value);
        onBtn.gameObject.SetActive(!value);
        offBtn.gameObject.SetActive(value);
    }
}
