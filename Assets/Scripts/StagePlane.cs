using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
 Plane중에 Stage가 될 Plane을 선택해주는 클래스
 
 */

public class StagePlane : MonoBehaviour
{
    /* 참조*/
    public GameObject Stage;
    public ARPlaneManager planeManager;
    public ObjectControlManager objCtrlManager;
    public Button doneBTN;
    int gameMode;

    //스테이지를 확정시켰는지 확인
    bool gameStart;

    //선택관련 변수들이다.
    TrackableId selectedStage;
    Vector2 touchPosition;

    private void Awake()
    {//변수 초기화
        selectedStage = new TrackableId();
        Stage.SetActive(false);
        gameMode = IntroScript.gameMode;
    }
    public void DoneStage()
    {//스테이지를 확정한다.
        gameStart = true;//flag true
        Stage.SetActive(true);//스테이지 활성화
        planeManager.enabled = false;//plane매니저 비활성화
        Stage.GetComponent<StageScript>().setMode(gameMode);//게임모드를 보내준다.
        foreach (var p in planeManager.trackables)
        {//plane 제거
            Destroy(p.gameObject);
        }
        doneBTN.gameObject.SetActive(false) ;//확정버튼 비활성화
    }

    void Update()
    {
        if (gameStart || !RayUtility.TryGetInputPosition(out touchPosition)) return;//게임시작안했고, 터치가 있다면
        if (RayUtility.isOnUI()) return;//터치가 UI위가 아니라면
        CancelPlane();//선택 초기화
        if(RayUtility.ARRaycast(touchPosition,out ARRaycastHit hit))
        {//레이 발사
            if ((hit.hitType & TrackableType.Planes)!=0) // Plane에 Ray됨.
            {
                if(!Stage.activeInHierarchy || selectedStage != hit.trackableId)
                {//새 plane이거나, 스테이지가 꺼져있었다면
                    SelectPlane(hit);//선택
                }
                else
                {//아니라면 스테이지 위치를 갱신해준다.
                    Stage.transform.position = hit.pose.position;
                }
            }
        }
        
    }
    void SelectPlane(ARRaycastHit hit)
    {
        if (planeManager.GetPlane(hit.trackableId).size.sqrMagnitude < 0.5f)
        {//스테이지 크기 제한(최저)
        }
        else
        {//선택된 플레인 저장
            selectedStage = hit.trackableId;

            var plane = planeManager.GetPlane(hit.trackableId);//플레인 매니저로부터 플레인을 받아온다.
            planeManager.enabled = false;//플레인 매니져 비활성화(플레인 확장 정지)
            Stage.transform.position = hit.pose.position;//위치값을 받아온다.
            float sizeAvg = (plane.size.x + plane.size.y) * 0.5f;//xy크기 평균을 구한다.
            Vector3 StageSize = new Vector3(sizeAvg, 1, sizeAvg);
            Stage.transform.localScale = StageSize * 0.1f;//적용시켰다.
            Stage.transform.rotation = Quaternion.identity;
            Stage.SetActive(true);//활성화
        }
    }
    void CancelPlane()
    {//취소했다면 스테이지를 비활성화 해주고 플레인매니져를 활성화시킨다.
        if (gameStart) return;
        Stage.SetActive(false);
        planeManager.enabled = true;
    }
}
