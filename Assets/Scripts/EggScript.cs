using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 백돌의 선택 및 이동, 생성 및 초기화를 담당하는클래스
 초기엔 업데이트 내에서 해결 하려 했는데, 점점 state가 생겨 간단한 FSM으로 구현했다.

 */

public class EggScript : SelectableObject
{          /*참조*/
    public GameObject Arrow;
    public GameObject EggChild;//자식(실제 움직이는 오브젝트)
    public Rigidbody rigid;
    MeshRenderer arrowMesh;
      /*플레이어 설정 변수*/
    public float arrowScaleFactor = 0.1f;//화살표 늘어나는 계수 
    public float speed = 5;//속도계수

    bool arrowSelected;//화살표를 출력할지 알려주는 변수
    EggState state;//현재 상태
    bool newState;//새로운상태로 변경했는지?

    /*이동 계산용*/
    Vector3 firstTouchPoint;//초기 터치 시 위치
    public Vector3 moveDir; // 계산된 이동방향
    public float length;    // 계산된 힘의 크기(속도 계수 적용 전)
    public float templength;// 이동중, 힘의 크기

    /*초기화용*/
    Vector3 tempEggPos;
    Vector3 tempEggSclae;
    Vector2 tempArrowScale;



    void Awake()
    {//초기화용 변수와 참조 변수 초기화
        arrowMesh = Arrow.GetComponent<MeshRenderer>();
        tempArrowScale = Arrow.transform.localScale;
        length = 0;
        tempEggPos = EggChild.transform.localPosition;
        tempEggSclae = EggChild.transform.localScale;
    }
    private void OnEnable()
    {//활성화 되면서 위치와 상태 등을 초기화 한다.
        rigid.velocity = Vector3.zero;
        arrowSelected = true;
        EggChild.transform.localPosition = tempEggPos;
        newState = false;
        state = EggState.idle;

        templength = 0;
        transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);//초기에 랜덤한 방향으로 로테이션이 정해진다.

        StartCoroutine(FSMmain());//메인 코루틴이다.
    }
    IEnumerator FSMmain()
    {//FSM에서 계속 유지되는 코루틴이다. 상태별로 코루틴을 실행시키고 끝날때까지 기다리다가 끝나면 또 다음 상태의 코루틴을 실행시킨다.
        do
        {
            newState = false;
            yield return StartCoroutine(state.ToString());
        } while (true);
    }
    void Update()
    {
        //화살표를 보여줄지 말지 알려준다.
        arrowMesh.enabled = arrowSelected;
    }

    void InitShot()
    {//백돌 애니메이션의 마지막 키프레임으로부터 호출되는 함수이다.
        if (IntroScript.gameMode != 2)
        {//무한모드, 전략모드라면 발사된다.(아까 랜덤방향으로 초기화 했으므로 랜덤방향으로 발사된다.)
            setState(EggState.initShotMove);
        }
        else
        {//생존모드라면 바로 시작한다.
            setState(EggState.idle);
        }
    }

    IEnumerator idle()
    {//대기상태이다.
        //각종 계산용 변수들을 초기화해준다.
        templength = 0;
        length = 0;
        moveDir = Vector3.zero;
        arrowSelected = false;
        do
        {
            rigid.velocity = Vector3.zero;
            yield return null;
        } while (!newState);
    }
    IEnumerator selected()
    {//선택되었을때의 상태다.
        //화살표가 보이게 해준다.
        arrowSelected = true;
        do
        {
            rigid.velocity = Vector3.zero;
            yield return null;
        } while (!newState);
    }
    IEnumerator playerShotMove()
    {//playerShot이라는건 모종의 이유로 발사되었다는것이다.(init발사가 아니라는 뜻)
        //화살표를 꺼준다.
        arrowSelected = false;
        do
        {
            yield return null;
            //매 프레임마다 현재위치에서 방향벡터를 이용해 다음 프레임 위치를 구한다.
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * templength);
            rigid.MovePosition(nextStep);//구해서 움직인다.
            templength *= 0.9f;//힘은 매 프레임 90%로 감소한다.
            if (templength <= 0.01)
            {
                setState(EggState.idle);
            }
        } while (!newState);
        
    }
    IEnumerator initShotMove()
    {//초기화 발사때는 방향벡터가 없기때문에 로테이션으로부터 방향벡터를 구해야한다.
        float theta = transform.rotation.eulerAngles.y ;//각도를 가져와서
        moveDir.x = Mathf.Cos(theta* Mathf.PI / 180);//집어넣었다.
        moveDir.y = 0;
        moveDir.z = Mathf.Sin(theta* Mathf.PI / 180);//집어넣었다.
        moveDir.Normalize();//정규화는 습관처럼한다. 
        arrowSelected = false;
        arrowMesh.enabled = arrowSelected;
        float eTime = 0;//playerShot과 다른점인데, 3초간 감속 없이 운동하게 하기위해 출발시간으로부터 얼마나 지났는지 저장할 변수가 필요하다.
        length = moveDir.magnitude;//힘을 구해준다. 당연히 1이다.
        templength = length*5;//initSthot의 속도는 속도계수 영향없이 5다.
       
        do
        {
            yield return null;
            eTime += Time.deltaTime;//매프레임 델타타임을 더해줘서
            if (eTime > 3f)//3초가 되면
            {//대기상태로바꾼다.
                setState(EggState.idle);
            }
            //이하는 player샷과 똑같은 무브다. 감속이 없다는점이 다르다.
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * speed);
            rigid.MovePosition(nextStep);
        } while (!newState);

    }
    IEnumerator offStage()
    {//낙하했다면
        arrowSelected = false;
        //Destroy 트리거를 넣어줘 낙하 애니메이션을 재생시킨다.
        GetComponentInParent<Animator>().SetTrigger("Destroy");
        do
        {
            yield return null;
        } while (!newState);
    }
    public void Fall()
    {
        //EggPhyScript로부터 호출되는 함수다. 낙하했다면 offStage상태로 바꿔준다.
        setState(EggState.offStage);
    }
    public void DestroyAnimEnd()
    {//낙하 애니메이션 종료 시 호출되는 함수다. 흑돌과 똑같다.
        gameObject.SetActive(false);
        EggChild.transform.localScale = tempEggSclae;
    }
    public void SetMoveDir(Vector3 dir)
    {//외부에서 방향을 바꿀 때 호출하는 함수.
        moveDir = dir;
    }
    public void setState(EggState s)
    {//상태를 변경하는 함수.
        newState = true;//이게 true가 되면 위에 있는 코루틴들의 while(newState!=true)가 걸려서 다 꺼진다. 그러면 다음 상태로 갈 수 있게된다.
        state = s;
    }

    public override void OnSelect(RaycastHit hit)
    {//선택되었다면
        if (state != EggState.idle) return;//대기상태가 아니면 리턴
        setState(EggState.selected);//선택상태로 바꿔주고

        //각종 계산용 변수를 초기화 해준다.
        moveDir = Vector3.zero;
        firstTouchPoint = EggChild.transform.position;
        firstTouchPoint.y = 0;
        templength = 0;
    }

    public override void OnCancel()
    {//선택이 종료되었다면
        if (state == EggState.offStage) return;//혹시모를 일을 대비해 낙하상태면 리턴
        moveDir.y = 0;
        setState(EggState.playerShotMove);//플레이어 샷 상태
    }

    public override void OnDrag(RaycastHit hit)
    {//드래그 되었다면
        if (state != EggState.selected) return;
        moveDir = firstTouchPoint - hit.point;//방향벡터를 구하고
        moveDir.y = 0;
        length = moveDir.magnitude * arrowScaleFactor;//길이를 구하고
        moveDir = moveDir.normalized;//방향은 정규화
        Vector2 tempScale = tempArrowScale;//화살표 크기 초기화
        tempScale.x = tempScale.x * length;//화살표 크기 증가
        Arrow.transform.localScale = tempScale;//적용
        Arrow.transform.rotation = Quaternion.Euler(90, -GetAngle(-moveDir),0 );//화살표 회전
        templength = speed * length;//힘을 구해준다.
    }
    public float GetAngle(Vector3 dir)
    {
        //간단한 각도구하기 함수이다.
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    public enum EggState
    {//백돌의 상태를 정의한 enum
        idle =1,
        selected =2,
        playerShotMove=3,
        initShotMove =4,
        offStage = 5,
    }
}
