using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    //public SceneDefine.Scene SceneType { get; protected set; } = SceneDefine.Scene.Unknown; // ����Ʈ�� Unknow �̶�� �ʱ�ȭ

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
    }

    public abstract void Clear();
}
