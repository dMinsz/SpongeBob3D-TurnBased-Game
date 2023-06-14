using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    //Loading
    public float progress { get; protected set; }
    protected abstract IEnumerator LoadingRoutine();

    public void LoadAsync()
    {
        StartCoroutine(LoadingRoutine());
    }

    //
    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
    }

    public abstract void Clear();
}
