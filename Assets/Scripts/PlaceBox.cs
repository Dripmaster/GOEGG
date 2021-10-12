using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 tire�� ��ġ��Ű�� ���� �����ϴ� ������Ʈ �� Ŭ����
 */
public class PlaceBox : SelectableObject
{
    //Ÿ�̾�� ��ư ����
    public Transform tire;
    public Button onBtn;
    public Button offBtn;
    private void Awake()
    {
        //�ʱ�ȭ �� ��������϶��� �����ϰ� �Ѵ�.
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
        //�ٴ��� �����ٸ� Ÿ�̾��� ��ġ�� �ٲ��ش�.
        Vector3 hitPose = hit.point;
        hitPose.y = transform.parent.position.y;
        tire.transform.position = hitPose;
    }

    public override void OnCancel()
    {

    }

    public override void OnDrag(RaycastHit hit)
    {
        //�ٴ��� �巡�� �ߴٸ� Ÿ�̾��� ��ġ�� �ٲ��ش�.
        Vector3 hitPose = hit.point;
        hitPose.y = transform.parent.position.y;
        tire.transform.position = hitPose;
    }
    public void OnPlaceBtnClicked(bool value)
    {
        //on ��ư�� ������ Ÿ�̾� �̵� Ȱ��ȭ, off��ư�� ������ ��Ȱ��ȭ
        if (IntroScript.gameMode != 1) return;
        gameObject.SetActive(value);
        onBtn.gameObject.SetActive(!value);
        offBtn.gameObject.SetActive(value);
    }
}
