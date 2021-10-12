using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 인트로 씬의 버튼을 통해 인게임으로 들어가게 해주는 클래스
 gameMode가 static이기 때문에 게임씬으로 넘어가도 사용이 가능하다.
 */
public class IntroScript : MonoBehaviour
{
    public static int gameMode;
    public void startGame(int mode)
    {
        SceneManager.LoadScene(1);
        gameMode = mode;
    }
    
}
