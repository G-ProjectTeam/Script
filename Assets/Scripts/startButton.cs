using UnityEngine;
using UnityEngine.SceneManagement;


public class startButton : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Level01Scene"); // ���ϴ� �� �̸� ��Ȯ�� �Է�
    }
}
