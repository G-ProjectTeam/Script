using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private bool _transRunning;
    private Vector2 _preSize;
    public float transTime;
    public RectTransform transitObj;
    public CutoffMaskUI cutoffMask;
    public RectTransform blackObj;

    private void Awake()
    {
        transform.SetParent(null);
        if (Object.FindObjectsByType<TransitionManager>(FindObjectsSortMode.None).Length == 1)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Transition(string sceneName)
    {
        if (Instance == null)
        {
            Debug.Log("Transit to " + sceneName + " without Transition");
            SceneManager.LoadScene(sceneName);
            return;
        }

        Debug.Log("Transit to " + sceneName + " with Transition");
        Instance.StartCoroutine(Instance.TransitionInternal(sceneName)); // 👈 코루틴 호출
    }

    private IEnumerator TransitionInternal(string sceneName)
    {
        if (_transRunning)
            yield break;

        _transRunning = true;

        // 1. 화면을 점점 어둡게 (닫히는 애니메이션)
        float time = 0f;
        _preSize = transitObj.sizeDelta;

        while (time < transTime)
        {
            time += Time.deltaTime;
            float t = time / transTime;
            if (cutoffMask != null)
                cutoffMask.SetRadius(1f - t); // 예시: 밖에서 안으로 어두워짐
            yield return null;
        }

        if (cutoffMask != null)
            cutoffMask.SetRadius(0f);

        // 2. 씬 전환
        SceneManager.LoadScene(sceneName);
        yield return null;

        // 3. 밝아지는 애니메이션
        time = 0f;
        while (time < transTime)
        {
            time += Time.deltaTime;
            float t = time / transTime;
            if (cutoffMask != null)
                cutoffMask.SetRadius(t); // 예시: 다시 밝아짐
            yield return null;
        }

        if (cutoffMask != null)
            cutoffMask.SetRadius(1f);

        _transRunning = false;
    }
}
