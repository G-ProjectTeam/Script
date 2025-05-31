using UnityEngine;
using UnityEngine.SceneManagement;


public class startButton : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Level01Scene"); // 원하는 씬 이름 정확히 입력
    }
}
