using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public string nextScene;

    private LoadingUI loadingUI;

    private void Awake()
    {
        LoadingUI loadingUI = GameManager.Resource.Load<LoadingUI>("PreFabs/UI/LoadingUI");
        this.loadingUI = Instantiate(loadingUI);
        this.loadingUI.transform.SetParent(transform);
    }



    string GetSceneName(BaseScene scene)
    {
        string name = scene.name; // C#�� Reflection. Scene enum�� 
        return name;
    }

    public void LoadScene(BaseScene scene)
    {
        nextScene = scene.name;
        //SceneManager.LoadScene(GetSceneName(scene));
        StartCoroutine(LoadingRoutine(nextScene));
    }

    public void LoadScene(string name)
    {
        nextScene = name;
        //SceneManager.LoadScene(name);
        StartCoroutine(LoadingRoutine(nextScene));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        AsyncOperation oper = LoadSceneAsync(sceneName);

        oper.allowSceneActivation = false; // Scene Load �� ������ �ٷ� ������ �Ѿ���ʰ�
        Time.timeScale = 0f;
        loadingUI.SetProgress(0f);
        loadingUI.FadeOut();

        yield return new WaitForSecondsRealtime(0.5f); // Wait fade out

        while (oper.progress < 0.9f)
        {
            loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress)); // Scene Loading for 50%
            yield return null;
        }

        BaseScene curScene = CurrentScene;

        //�߰��ε��Ұ͵� �ε�
        if (curScene != null)
        {
            curScene.LoadAsync();
            while (curScene.progress < 1f)
            {
                loadingUI.SetProgress(Mathf.Lerp(0.5f, 1f, curScene.progress));
                yield return null;
            }
        }

        oper.allowSceneActivation = true;
        Time.timeScale = 1f;
        loadingUI.SetProgress(1f);
        loadingUI.FadeIn();
        yield return new WaitForSecondsRealtime(0.5f); // wait Fade In
    }


    public AsyncOperation LoadSceneAsync(BaseScene nextScene)
    {
        return SceneManager.LoadSceneAsync(GetSceneName(nextScene));
    }

    public AsyncOperation LoadSceneAsync(string nextScene)
    {
        return SceneManager.LoadSceneAsync(nextScene);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
