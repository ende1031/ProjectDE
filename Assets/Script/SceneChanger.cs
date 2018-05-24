using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance = null;
    public CanvasGroup fadeCanvasGroup; //페이드인아웃 캔바스
    public float fadeDuration = 1f;
    private bool isFading;

    public string startingSceneName = "Title"; //첫 씬 이름.
    public int playerGrid = 5; //씬 로드 후 플레이어의 위치

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    //첫 씬 로드
    private IEnumerator Start()
    {
        fadeCanvasGroup.alpha = 1f;
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));
        StartCoroutine(Fade(0f));
    }

    //신 체인지 할때 이 함수를 불러올 것
    public void FadeAndLoadScene(string sceneName, int moveAfterGrid = 0)
    {
        playerGrid = moveAfterGrid;
        if (isFading == false)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        if (sceneName != "Persistent")
        {
            yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        }

        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        fadeCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(fadeCanvasGroup.alpha - finalAlpha) / fadeDuration;
        while (Mathf.Approximately(fadeCanvasGroup.alpha, finalAlpha) == false)
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;

        isFading = false;
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
