using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 ������ ���۵ǰ� �����̴� ������Ʈ���� �����Ͽ� �ʱ�ȭ, ����, ��Ȱ��ȭ ���� ����Ѵ�.
 */
public class StageScript : SelectableObject
{
    //������Ʈ�� ����
    public GameObject[] Eggs;
    public GameObject[] Goals;

    public GameObject[] Walls;
    public GameObject Tire;
    public GameObject[] Vents;

    public CountText countText;

    //��ź ������
    public GameObject BombPrefab;


    // Start is called before the first frame update
    void Awake()
    {
    }
    private void OnEnable()
    {//������Ʈ ��Ȱ��ȭ
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
    {//�ʱ� ���� �������ִ� �Լ�
        foreach (var item in Walls)
        {
            item.SetActive(false);
        }
        //�������� �浹�� ���������ش�.
        Physics.IgnoreLayerCollision(7, 7,false);
        StartCoroutine(checkCount());//���� ī��Ʈ ����
        countText.enabled = true;//����ǥ�ڵ� Ȱ��ȭ
    }
    public void setMode(int gameMode)
    {//���Ӹ�忡 ���� �ʱ�ȭ���ִ� �Լ�

        countText.reText();//�ؽ�Ʈ �ʱ�ȭ
        StopAllCoroutines();//��� �ڷ�ƾ ����
        foreach (var item in Walls)
        {//������ �����ش�.
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
    {//���Ѹ��
        Physics.IgnoreLayerCollision(7,7,true);//��� �������� �浹�� ���ְ�
        for (int i = 0; i < 3; i++)
        {//������ ��ġ�� �浹�� ����ش�. �鵹�� ������ initShot�� �ϱ⶧���� ���� ��ġ�� �ʿ�� ����.

            Vector3 pos = getRandomPos();
            Eggs[i].transform.position = transform.position;
            Eggs[i].SetActive(true);
            Goals[i].transform.position = pos;
            Goals[i].SetActive(true);
        }

        Invoke("hideWalls", 5f);
    }
    void initSecondMode()
    {//�������
        Physics.IgnoreLayerCollision(7, 7, true);//��� �������� �浹�� ���ְ�
        //������ ��ġ�� �浹�� ����ش�. �鵹�� ������ initShot�� �ϱ⶧���� ���� ��ġ�� �ʿ�� ����.
        Vector3 pos = getRandomPos();
        Eggs[0].transform.position = transform.position;
            Eggs[0].SetActive(true);
            Goals[0].transform.position = pos;
            Goals[0].SetActive(true);

        //������ ��ġ�� Ÿ�̾�� ȯǳ���� ��ġ��Ų��.
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
    {//�������
        //�߾ӿ� �鵹 ����
            Eggs[0].transform.position = transform.position;
            Eggs[0].SetActive(true);
        hideWalls();
        StartCoroutine(spawnBomb());//��ź���� �ڷ�ƾ ����
    }
    IEnumerator spawnBomb()
    {//��ź ���� �ڷ�ƾ
        do
        {
            yield return new WaitForSeconds(1f);//1�ʸ���
            //������ ��ġ�� ��ź�� �����Ѵ�.
            Vector3 pos = getRandomPos();
            Instantiate(BombPrefab, pos,Quaternion.identity);
        } while (true);
    }
    IEnumerator checkCount()
    {//�������� �浹�� �鵹�� ������ üũ�� ���� ���ӻ�Ȳ�� �ݿ��Ѵ�
        do
        {
            int EggCount = 0;
            foreach (var e in Eggs)
            {
                if (e.activeInHierarchy) EggCount++;
            }
            if (EggCount <= 0)
            {//�鵹�� ��� �������ٸ� �ٽ� �����Ѵ�.
                setMode(IntroScript.gameMode);
            }
            int GoalCount = 0;
            foreach (var g in Goals)
            {
                if (g.activeInHierarchy) GoalCount++;
            }
            if (GoalCount <= 0) {//�浹�� ��� �������ٸ� ���Ѹ���϶� 3��, ��������ϴ� 1�� �����Ѵ�.
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
