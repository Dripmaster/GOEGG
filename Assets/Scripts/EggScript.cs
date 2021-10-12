using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 �鵹�� ���� �� �̵�, ���� �� �ʱ�ȭ�� ����ϴ�Ŭ����
 �ʱ⿣ ������Ʈ ������ �ذ� �Ϸ� �ߴµ�, ���� state�� ���� ������ FSM���� �����ߴ�.

 */

public class EggScript : SelectableObject
{          /*����*/
    public GameObject Arrow;
    public GameObject EggChild;//�ڽ�(���� �����̴� ������Ʈ)
    public Rigidbody rigid;
    MeshRenderer arrowMesh;
      /*�÷��̾� ���� ����*/
    public float arrowScaleFactor = 0.1f;//ȭ��ǥ �þ�� ��� 
    public float speed = 5;//�ӵ����

    bool arrowSelected;//ȭ��ǥ�� ������� �˷��ִ� ����
    EggState state;//���� ����
    bool newState;//���ο���·� �����ߴ���?

    /*�̵� ����*/
    Vector3 firstTouchPoint;//�ʱ� ��ġ �� ��ġ
    public Vector3 moveDir; // ���� �̵�����
    public float length;    // ���� ���� ũ��(�ӵ� ��� ���� ��)
    public float templength;// �̵���, ���� ũ��

    /*�ʱ�ȭ��*/
    Vector3 tempEggPos;
    Vector3 tempEggSclae;
    Vector2 tempArrowScale;



    void Awake()
    {//�ʱ�ȭ�� ������ ���� ���� �ʱ�ȭ
        arrowMesh = Arrow.GetComponent<MeshRenderer>();
        tempArrowScale = Arrow.transform.localScale;
        length = 0;
        tempEggPos = EggChild.transform.localPosition;
        tempEggSclae = EggChild.transform.localScale;
    }
    private void OnEnable()
    {//Ȱ��ȭ �Ǹ鼭 ��ġ�� ���� ���� �ʱ�ȭ �Ѵ�.
        rigid.velocity = Vector3.zero;
        arrowSelected = true;
        EggChild.transform.localPosition = tempEggPos;
        newState = false;
        state = EggState.idle;

        templength = 0;
        transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);//�ʱ⿡ ������ �������� �����̼��� ��������.

        StartCoroutine(FSMmain());//���� �ڷ�ƾ�̴�.
    }
    IEnumerator FSMmain()
    {//FSM���� ��� �����Ǵ� �ڷ�ƾ�̴�. ���º��� �ڷ�ƾ�� �����Ű�� ���������� ��ٸ��ٰ� ������ �� ���� ������ �ڷ�ƾ�� �����Ų��.
        do
        {
            newState = false;
            yield return StartCoroutine(state.ToString());
        } while (true);
    }
    void Update()
    {
        //ȭ��ǥ�� �������� ���� �˷��ش�.
        arrowMesh.enabled = arrowSelected;
    }

    void InitShot()
    {//�鵹 �ִϸ��̼��� ������ Ű���������κ��� ȣ��Ǵ� �Լ��̴�.
        if (IntroScript.gameMode != 2)
        {//���Ѹ��, ��������� �߻�ȴ�.(�Ʊ� ������������ �ʱ�ȭ �����Ƿ� ������������ �߻�ȴ�.)
            setState(EggState.initShotMove);
        }
        else
        {//��������� �ٷ� �����Ѵ�.
            setState(EggState.idle);
        }
    }

    IEnumerator idle()
    {//�������̴�.
        //���� ���� �������� �ʱ�ȭ���ش�.
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
    {//���õǾ������� ���´�.
        //ȭ��ǥ�� ���̰� ���ش�.
        arrowSelected = true;
        do
        {
            rigid.velocity = Vector3.zero;
            yield return null;
        } while (!newState);
    }
    IEnumerator playerShotMove()
    {//playerShot�̶�°� ������ ������ �߻�Ǿ��ٴ°��̴�.(init�߻簡 �ƴ϶�� ��)
        //ȭ��ǥ�� ���ش�.
        arrowSelected = false;
        do
        {
            yield return null;
            //�� �����Ӹ��� ������ġ���� ���⺤�͸� �̿��� ���� ������ ��ġ�� ���Ѵ�.
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * templength);
            rigid.MovePosition(nextStep);//���ؼ� �����δ�.
            templength *= 0.9f;//���� �� ������ 90%�� �����Ѵ�.
            if (templength <= 0.01)
            {
                setState(EggState.idle);
            }
        } while (!newState);
        
    }
    IEnumerator initShotMove()
    {//�ʱ�ȭ �߻綧�� ���⺤�Ͱ� ���⶧���� �����̼����κ��� ���⺤�͸� ���ؾ��Ѵ�.
        float theta = transform.rotation.eulerAngles.y ;//������ �����ͼ�
        moveDir.x = Mathf.Cos(theta* Mathf.PI / 180);//����־���.
        moveDir.y = 0;
        moveDir.z = Mathf.Sin(theta* Mathf.PI / 180);//����־���.
        moveDir.Normalize();//����ȭ�� ����ó���Ѵ�. 
        arrowSelected = false;
        arrowMesh.enabled = arrowSelected;
        float eTime = 0;//playerShot�� �ٸ����ε�, 3�ʰ� ���� ���� ��ϰ� �ϱ����� ��߽ð����κ��� �󸶳� �������� ������ ������ �ʿ��ϴ�.
        length = moveDir.magnitude;//���� �����ش�. �翬�� 1�̴�.
        templength = length*5;//initSthot�� �ӵ��� �ӵ���� ������� 5��.
       
        do
        {
            yield return null;
            eTime += Time.deltaTime;//�������� ��ŸŸ���� �����༭
            if (eTime > 3f)//3�ʰ� �Ǹ�
            {//�����·ιٲ۴�.
                setState(EggState.idle);
            }
            //���ϴ� player���� �Ȱ��� �����. ������ ���ٴ����� �ٸ���.
            Vector3 nextStep = Vector3.Lerp(rigid.transform.position, rigid.transform.position + moveDir, Time.deltaTime * speed);
            rigid.MovePosition(nextStep);
        } while (!newState);

    }
    IEnumerator offStage()
    {//�����ߴٸ�
        arrowSelected = false;
        //Destroy Ʈ���Ÿ� �־��� ���� �ִϸ��̼��� �����Ų��.
        GetComponentInParent<Animator>().SetTrigger("Destroy");
        do
        {
            yield return null;
        } while (!newState);
    }
    public void Fall()
    {
        //EggPhyScript�κ��� ȣ��Ǵ� �Լ���. �����ߴٸ� offStage���·� �ٲ��ش�.
        setState(EggState.offStage);
    }
    public void DestroyAnimEnd()
    {//���� �ִϸ��̼� ���� �� ȣ��Ǵ� �Լ���. �浹�� �Ȱ���.
        gameObject.SetActive(false);
        EggChild.transform.localScale = tempEggSclae;
    }
    public void SetMoveDir(Vector3 dir)
    {//�ܺο��� ������ �ٲ� �� ȣ���ϴ� �Լ�.
        moveDir = dir;
    }
    public void setState(EggState s)
    {//���¸� �����ϴ� �Լ�.
        newState = true;//�̰� true�� �Ǹ� ���� �ִ� �ڷ�ƾ���� while(newState!=true)�� �ɷ��� �� ������. �׷��� ���� ���·� �� �� �ְԵȴ�.
        state = s;
    }

    public override void OnSelect(RaycastHit hit)
    {//���õǾ��ٸ�
        if (state != EggState.idle) return;//�����°� �ƴϸ� ����
        setState(EggState.selected);//���û��·� �ٲ��ְ�

        //���� ���� ������ �ʱ�ȭ ���ش�.
        moveDir = Vector3.zero;
        firstTouchPoint = EggChild.transform.position;
        firstTouchPoint.y = 0;
        templength = 0;
    }

    public override void OnCancel()
    {//������ ����Ǿ��ٸ�
        if (state == EggState.offStage) return;//Ȥ�ø� ���� ����� ���ϻ��¸� ����
        moveDir.y = 0;
        setState(EggState.playerShotMove);//�÷��̾� �� ����
    }

    public override void OnDrag(RaycastHit hit)
    {//�巡�� �Ǿ��ٸ�
        if (state != EggState.selected) return;
        moveDir = firstTouchPoint - hit.point;//���⺤�͸� ���ϰ�
        moveDir.y = 0;
        length = moveDir.magnitude * arrowScaleFactor;//���̸� ���ϰ�
        moveDir = moveDir.normalized;//������ ����ȭ
        Vector2 tempScale = tempArrowScale;//ȭ��ǥ ũ�� �ʱ�ȭ
        tempScale.x = tempScale.x * length;//ȭ��ǥ ũ�� ����
        Arrow.transform.localScale = tempScale;//����
        Arrow.transform.rotation = Quaternion.Euler(90, -GetAngle(-moveDir),0 );//ȭ��ǥ ȸ��
        templength = speed * length;//���� �����ش�.
    }
    public float GetAngle(Vector3 dir)
    {
        //������ �������ϱ� �Լ��̴�.
        return Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    public enum EggState
    {//�鵹�� ���¸� ������ enum
        idle =1,
        selected =2,
        playerShotMove=3,
        initShotMove =4,
        offStage = 5,
    }
}
