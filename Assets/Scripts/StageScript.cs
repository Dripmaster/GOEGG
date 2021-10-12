using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 게임이 시작되고 움직이는 오브젝트들을 관리하여 초기화, 생성, 비활성화 등을 담당한다.
 */
public class StageScript : SelectableObject
{
    //오브젝트들 참조
    public GameObject[] Eggs;
    public GameObject[] Goals;

    public GameObject[] Walls;
    public GameObject Tire;
    public GameObject[] Vents;

    public CountText countText;

    //폭탄 프리팹
    public GameObject BombPrefab;


    // Start is called before the first frame update
    void Awake()
    {
    }
    private void OnEnable()
    {//오브젝트 비활성화
        foreach (var e in Eggs)
        {
            e.SetActive(false);
        }
        foreach (var g in Goals)
        {
            g.SetActive(false);
        }

        foreach (var v in Vents)
        {
            v.SetActive(false);
        }
        foreach (var item in Walls)
        {
            item.SetActive(true);
        }
        Tire.SetActive(false);
        countText.enabled = false;
    }

    public void hideWalls()
    {//초기 벽을 제거해주는 함수
        foreach (var item in Walls)
        {
            item.SetActive(false);
        }
        //돌끼리의 충돌도 복구시켜준다.
        Physics.IgnoreLayerCollision(7, 7,false);
        StartCoroutine(checkCount());//점수 카운트 시작
        countText.enabled = true;//점수표코드 활성화
    }
    public void setMode(int gameMode)
    {//게임모드에 따라 초기화해주는 함수

        countText.reText();//텍스트 초기화
        StopAllCoroutines();//모든 코루틴 정지
        foreach (var item in Walls)
        {//벽들을 세워준다.
            item.SetActive(true);
        }
        switch (gameMode)
        {
            case 0:
                initFirstMode();break;
            case 1:
                initSecondMode();break;
            case 2:
                initThirdMode(); break;
            default:
                break;
        }

    }
    void initFirstMode()
    {//무한모드
        Physics.IgnoreLayerCollision(7,7,true);//잠시 돌끼리의 충돌을 꺼주고
        for (int i = 0; i < 3; i++)
        {//랜덤한 위치에 흑돌을 깔아준다. 백돌은 어차피 initShot을 하기때문에 랜덤 위치일 필요는 없다.

            Vector3 pos = getRandomPos();
            Eggs[i].transform.position = transform.position;
            Eggs[i].SetActive(true);
            Goals[i].transform.position = pos;
            Goals[i].SetActive(true);
        }

        Invoke("hideWalls", 5f);
    }
    void initSecondMode()
    {//전략모드
        Physics.IgnoreLayerCollision(7, 7, true);//잠시 돌끼리의 충돌을 꺼주고
        //랜덤한 위치에 흑돌을 깔아준다. 백돌은 어차피 initShot을 하기때문에 랜덤 위치일 필요는 없다.
        Vector3 pos = getRandomPos();
        Eggs[0].transform.position = transform.position;
            Eggs[0].SetActive(true);
            Goals[0].transform.position = pos;
            Goals[0].SetActive(true);

        //랜덤한 위치에 타이어와 환풍구를 위치시킨다.
        pos = getRandomPos();
        Tire.transform.position = pos;
        Tire.SetActive(true);

        foreach (var v in Vents)
        {
            pos = getRandomPos();
            v.transform.position = pos;

            v.SetActive(true);
        }
        Invoke("hideWalls", 5f);
    }
    void initThirdMode()
    {//생존모드
        //중앙에 백돌 생성
            Eggs[0].transform.position = transform.position;
            Eggs[0].SetActive(true);
        hideWalls();
        StartCoroutine(spawnBomb());//폭탄생성 코루틴 시작
    }
    IEnumerator spawnBomb()
    {//폭탄 생성 코루틴
        do
        {
            yield return new WaitForSeconds(1f);//1초마다
            //랜덤한 위치에 폭탄을 생성한다.
            Vector3 pos = getRandomPos();
            Instantiate(BombPrefab, pos,Quaternion.identity);
        } while (true);
    }
    IEnumerator checkCount()
    {//매프레임 흑돌과 백돌의 갯수를 체크해 현재 게임상황에 반영한다
        do
        {
            int EggCount = 0;
            foreach (var e in Eggs)
            {
                if (e.activeInHierarchy) EggCount++;
            }
            if (EggCount <= 0)
            {//백돌이 모두 없어졌다면 다시 시작한다.
                setMode(IntroScript.gameMode);
            }
            int GoalCount = 0;
            foreach (var g in Goals)
            {
                if (g.activeInHierarchy) GoalCount++;
            }
            if (GoalCount <= 0) {//흑돌이 모두 없어졌다면 무한모드일땐 3개, 전략모드일댄 1개 생성한다.
                if (IntroScript.gameMode == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {

                        Vector3 pos = getRandomPos();
                        Goals[i].transform.position = pos;
                        Goals[i].SetActive(true);
                    }
                }
                if (IntroScript.gameMode == 1)
                {
                    Vector3 pos = getRandomPos();
                        Goals[0].transform.position = pos;
                        Goals[0].SetActive(true);
                }
            }
            yield return null;
        } while (true);
    }
    // Update is called once per frame
    void Update()
    {
    }
    Vector3 getRandomPos()
    {
        return new Vector3(Random.Range(transform.position.x -transform.localScale.x * 0.35f * 10f, transform.position.x + transform.localScale.x * 0.35f * 10f),
                                                                 transform.position.y,
                                                                 Random.Range(transform.position.z - transform.localScale.z * 0.35f * 10f, transform.position.z + transform.localScale.z * 0.35f * 10f));
    }
    public override void OnSelect(RaycastHit hit)
    {
    }

    public override void OnCancel()
    {

    }

    public override void OnDrag(RaycastHit hit)
    {
    }
}
