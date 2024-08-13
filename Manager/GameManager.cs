using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager _GameManagerInstance = null;
    public List<IManager> _ManagerClass = null;

    public event EventHandler OnGameSceneStart;

    public static GameManager gameManager {
        get {
            if (!_GameManagerInstance)
            {
                _GameManagerInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
                _GameManagerInstance.InitializeGameManager();
            }

            return _GameManagerInstance;
        }
    }

    public void OnGameScene()
    {
        OnGameSceneStart.Invoke(this,EventArgs.Empty);
    }


    private void InitializeGameManager()
    {
        _ManagerClass = new List<IManager>();

        //  > 하위 매니저 클래스를 등록합니다.
        RegisterManagerClass<UnitManager>();
        RegisterManagerClass<UIManager>();
        RegisterManagerClass<ResourcesManager>();
        RegisterManagerClass<LevelGrid>();
        RegisterManagerClass<ActionSystem>();
        RegisterManagerClass<Pathfinding>();
    }

    private void RegisterManagerClass<T>() where T : IManager
    {
        _ManagerClass.Add(transform.GetComponentInChildren<T>());
    }

    public static T GetManagerClass<T>() where T : class, IManager
    {
        
        return gameManager._ManagerClass.Find(
            (IManager managerClass) => managerClass.GetType() == typeof(T)) as T;
    }


    private void Awake()
    {
        if (_GameManagerInstance && _GameManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
