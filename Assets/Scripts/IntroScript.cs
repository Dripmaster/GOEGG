using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 ��Ʈ�� ���� ��ư�� ���� �ΰ������� ���� ���ִ� Ŭ����
 gameMode�� static�̱� ������ ���Ӿ����� �Ѿ�� ����� �����ϴ�.
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
