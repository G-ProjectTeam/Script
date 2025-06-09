using UnityEngine;
using UnityEngine.SceneManagement;

public class howPlayButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("howToPlay");  
    }
}
