﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubUIManager : MonoBehaviour, IUIManager
{
    private UILayer managerLayer;
    private StateMachine<EUiId> stateMachine;
    private Stack<EUiId> uiStack;
    private Dictionary<EUiId, Transform> objectPool; 

    public void Init(UILayer uiLayer, StateMachine<EUiId> machine)
    {
        managerLayer = uiLayer;
       
        uiStack = new Stack<EUiId>();

        objectPool = new Dictionary<EUiId, Transform>();

        stateMachine = machine;
    }

    public bool ShowUI(EUiId id, IPara para)
    {
        Transform uiTrans = SpawnUI(id);
        if (uiTrans != null)
        {
            IUIState ui = uiTrans.GetComponent<IUIState>();
            if (ui != null)
            {
                AUIEffect uIEffect = uiTrans.GetComponent<AUIEffect>();
                stateMachine.AddUI(id, ui, uIEffect);
                objectPool[id] = uiTrans;
                uiStack.Push(id);
                stateMachine.ChangeUI(id);
                return true;
            }
            else
            {
                Debug.LogError("the prefab cannot find IUIState");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool HideUI(UILayer layer)
    {
        if (layer > managerLayer)
            return false;
        if (uiStack.Count > 0)
        {
            stateMachine.Hide(uiStack.Pop());
        }
        return true;
    }

    private Transform SpawnUI(EUiId id)
    {
        string path = UIPathManager.GetPath(id);
        if (!string.IsNullOrEmpty(path))
        {
            if (!objectPool.ContainsKey(id) || objectPool[id] == null)
            {
                objectPool[id] = Instantiate(Resources.Load<Transform>(path), transform);
            }
            return objectPool[id];
        }
        else
        {
            return null;
        }
    }

    public bool Back()
    {
        if (uiStack.Count > 1)
        {
            stateMachine.Hide(uiStack.Pop());
            stateMachine.Show(uiStack.Peek());
            return true;
        }
        else
        {
            return false;
        }
    }
}
