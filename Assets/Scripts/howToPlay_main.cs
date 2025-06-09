using UnityEngine;
using UnityEngine.SceneManagement;

public class howToPlay : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Main");  
    }
}
