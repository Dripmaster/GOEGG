using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
 Plane�߿� Stage�� �� Plane�� �������ִ� Ŭ����
 
 */

public class StagePlane : MonoBehaviour
{
    /* ����*/
    public GameObject Stage;
    public ARPlaneManager planeManager;
    public ObjectControlManager objCtrlManager;
    public Button doneBTN;
    int gameMode;

    //���������� Ȯ�����״��� Ȯ��
    bool gameStart;

    //���ð��� �������̴�.
    TrackableId selectedStage;
    Vector2 touchPosition;

    private void Awake()
    {//���� �ʱ�ȭ
        selectedStage = new TrackableId();
        Stage.SetActive(false);
        gameMode = IntroScript.gameMode;
    }
    public void DoneStage()
    {//���������� Ȯ���Ѵ�.
        gameStart = true;//flag true
        Stage.SetActive(true);//�������� Ȱ��ȭ
        planeManager.enabled = false;//plane�Ŵ��� ��Ȱ��ȭ
        Stage.GetComponent<StageScript>().setMode(gameMode);//���Ӹ�带 �����ش�.
        foreach (var p in planeManager.trackables)
        {//plane ����
            Destroy(p.gameObject);
        }
        doneBTN.gameObject.SetActive(false) ;//Ȯ����ư ��Ȱ��ȭ
    }

    void Update()
    {
        if (gameStart || !RayUtility.TryGetInputPosition(out touchPosition)) return;//���ӽ��۾��߰�, ��ġ�� �ִٸ�
        if (RayUtility.isOnUI()) return;//��ġ�� UI���� �ƴ϶��
        CancelPlane();//���� �ʱ�ȭ
        if(RayUtility.ARRaycast(touchPosition,out ARRaycastHit hit))
        {//���� �߻�
            if ((hit.hitType & TrackableType.Planes)!=0) // Plane�� Ray��.
            {
                if(!Stage.activeInHierarchy || selectedStage != hit.trackableId)
                {//�� plane�̰ų�, ���������� �����־��ٸ�
                    SelectPlane(hit);//����
                }
                else
                {//�ƴ϶�� �������� ��ġ�� �������ش�.
                    Stage.transform.position = hit.pose.position;
                }
            }
        }
        
    }
    void SelectPlane(ARRaycastHit hit)
    {
        if (planeManager.GetPlane(hit.trackableId).size.sqrMagnitude < 0.5f)
        {//�������� ũ�� ����(����)
        }
        else
        {//���õ� �÷��� ����
            selectedStage = hit.trackableId;

            var plane = planeManager.GetPlane(hit.trackableId);//�÷��� �Ŵ����κ��� �÷����� �޾ƿ´�.
            planeManager.enabled = false;//�÷��� �Ŵ��� ��Ȱ��ȭ(�÷��� Ȯ�� ����)
            Stage.transform.position = hit.pose.position;//��ġ���� �޾ƿ´�.
            float sizeAvg = (plane.size.x + plane.size.y) * 0.5f;//xyũ�� ����� ���Ѵ�.
            Vector3 StageSize = new Vector3(sizeAvg, 1, sizeAvg);
            Stage.transform.localScale = StageSize * 0.1f;//������״�.
            Stage.transform.rotation = Quaternion.identity;
            Stage.SetActive(true);//Ȱ��ȭ
        }
    }
    void CancelPlane()
    {//����ߴٸ� ���������� ��Ȱ��ȭ ���ְ� �÷��θŴ����� Ȱ��ȭ��Ų��.
        if (gameStart) return;
        Stage.SetActive(false);
        planeManager.enabled = true;
    }
}
