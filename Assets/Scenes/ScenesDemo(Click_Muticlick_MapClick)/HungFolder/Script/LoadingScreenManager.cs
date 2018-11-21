using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{

    [Header("Loading Visuals")]
    public Image loadingIcon;
    public Image loadingDoneIcon;
    public Text loadingText;
    public Image progressBar;
    public Image fadeOverlay;

    [Header("Timing Settings")]
    public float waitOnLoadEnd = 2f;
    public float fadeDuration = 2f;

    [Header("Loading Settings")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Other")]
    // If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
    public AudioListener audioListener;

    AsyncOperation operation;
    Scene currentScene;

    public static int sceneToLoad = -1;
    // IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
    static int loadingSceneIndex = 2;

    public static void LoadScene(int levelNum)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelNum;
        SceneManager.LoadScene(loadingSceneIndex);

      
    }

    void Start()
    {
        if (sceneToLoad < 0)
            return;

        fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(sceneToLoad));
     
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        ShowLoadingVisuals();

        yield return null;

        FadeIn();
        StartOperation(levelNum);
        progressBar.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (!operation.isDone && progressBar.fillAmount < 0.9f)
        {  
                progressBar.fillAmount =Mathf.MoveTowards(progressBar.fillAmount, operation.progress,0.01f);
            yield return null;
        }

        if (loadSceneMode == LoadSceneMode.Additive)
            audioListener.enabled = false;

        ShowCompletionVisuals();
        yield return new WaitForSeconds(0.5f);
      

        FadeOut();

        yield return new WaitForSeconds(fadeDuration);

        if (loadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadSceneAsync(currentScene.name);
        else
            operation.allowSceneActivation = true;
        if(sceneToLoad!=0)
        SoundManager.instanece.ChangeMusic(sceneToLoad);
        operation.allowSceneActivation = true;
     
       
        yield return new WaitForSeconds(waitOnLoadEnd);
       



    }

    private void StartOperation(int levelNum)
    {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;

        operation.allowSceneActivation = false;
    }

    

    void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
    }

    void FadeOut()
    {
        fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
    }

    void ShowLoadingVisuals()
    {
        loadingIcon.gameObject.SetActive(true);
        loadingDoneIcon.gameObject.SetActive(false);

        progressBar.fillAmount = 0f;
        loadingText.text = "LOADING...";
    }

    void ShowCompletionVisuals()
    {
        loadingIcon.gameObject.SetActive(false);
        loadingDoneIcon.gameObject.SetActive(true);

        progressBar.fillAmount = 1f;
        loadingText.text = "LOADING DONE";
    }


}