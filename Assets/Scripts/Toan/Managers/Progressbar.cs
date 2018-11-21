using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public class Progressbar : MonoBehaviour
    {
        private AsyncOperation asyncOperation;
        private float progress;

        public static Progressbar Instance { get; private set; }
        public Image ProgressImg;
        public GameObject Panel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            Panel.SetActive(false);
        }

        private void QuickLoad(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadScene(int index)
        {
            progress = 0.0f;
            ProgressImg.fillAmount = 0.0f;
            Panel.SetActive(true);
            StartCoroutine(StartLoadScene(index));
        }

        private IEnumerator StartLoadScene(int index)
        {
            asyncOperation = SceneManager.LoadSceneAsync(index);
            asyncOperation.allowSceneActivation = false;

            while(!asyncOperation.isDone && progress < 0.9f)
            {
                progress = Mathf.MoveTowards(progress, asyncOperation.progress, 0.1f);
                ProgressImg.fillAmount = progress;
                yield return null;
            }

            while(progress < 1)
            {
                progress = Mathf.MoveTowards(progress, 1.0f, 0.1f);
                ProgressImg.fillAmount = progress;
                yield return null;
            }

            LoadDone();

            yield return new WaitForSeconds(0.5f);

            Panel.SetActive(false);
            yield break;
        }

        private void LoadDone()
        {
            asyncOperation.allowSceneActivation = true;
        }

        public void LoadChooseClass()
        {
            QuickLoad(1);
        }
    }
}
