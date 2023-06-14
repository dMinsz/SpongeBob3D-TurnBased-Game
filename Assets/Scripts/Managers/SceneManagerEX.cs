using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public BaseScene nextScene;

    string GetSceneName(BaseScene type)
    {
        string name = type.name; // C#¿« Reflection. Scene enum¿« 
        return name;
    }

    public void LoadScene(BaseScene type)
    {
        nextScene = type;
        SceneManager.LoadScene(GetSceneName(type));
    }

    public AsyncOperation LoadSceneAsync(BaseScene nextScene)
    {
        return SceneManager.LoadSceneAsync(GetSceneName(nextScene));
    }


    public void Clear()
    {
        CurrentScene.Clear();
    }
}
