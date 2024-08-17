using System;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint: MonoBehaviour
{
    #region Singleton
    
    public static EntryPoint Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    private List<InitializeRequest> _initializeQueue = new List<InitializeRequest>();
    
    private void Update()
    {
        while (_initializeQueue.Count > 0)
        {
            _initializeQueue[0].Action?.Invoke();
            _initializeQueue.RemoveAt(0);
        }
    }
    
    public void AddToInitializeQueue(Action action, int priority = 0)
    {
        _initializeQueue.Add(new InitializeRequest(action, priority));
        _initializeQueue.Sort((x, y) => x.Priority.CompareTo(y.Priority));
    }
    
    private class InitializeRequest
    {
        public Action Action;
        public int Priority;
        
        public InitializeRequest(Action action, int priority)
        {
            Action = action;
            Priority = priority;
        }
    }
}
