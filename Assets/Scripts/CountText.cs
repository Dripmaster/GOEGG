using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 점수판을 관리하는 클래스
 */
public class CountText : MonoBehaviour
{
    public Text myText; // UIText를 저장
    int count; // 점수이다. 생존모드에서는 시간이다.
    void OnEnable()
    {
        StopAllCoroutines();//코루틴이 겹치는 상황을 방지하기 위해 초기화 시 모든 코루틴을 종료한다.(Enable상태가 아니면 당연히 코루틴은 모두 종료되어 있지만, 여기에 이렇게 명시하지않으면 불안하다.)
       
        //점수와 텍스트를 초기화한다.
        count = 0;
        setText(count);

        if(IntroScript.gameMode==2)//생존모드라면 시간체크를 시작한다.
        StartCoroutine(checkTime());
    }
    IEnumerator checkTime()
    {//1초마다 count증가 및 텍스트 반영.
        do
        {
            setText(count++);
            
            yield return new WaitForSeconds(1);
        } while (true);
    }
    void setText(int v)
    {//모드에 맞게 텍스트를 반영해준다.

        switch (IntroScript.gameMode)
        {
            case 0:
                myText.text = "점수 : "+v;
                break;
            case 1:
                myText.text = "점수 : "+v;
                break;
            case 2:
                myText.text = "생존시간 : "+v+"초";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reText()
    {//stage 초기화 시 count, text를 초기화 해준다.
        count = 0;
        setText(count);
    }
    public void onBlackDestroyed()
    {//흑돌로부터 받아오는 흑돌 낙하 이벤트 처리, 점수를 1올리고 텍스트 반영
        count++;
        setText(count);
    }
}
