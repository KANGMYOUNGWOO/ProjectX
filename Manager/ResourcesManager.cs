using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;




public class ResourcesManager : MonoBehaviour, IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    private List<CharacterClass> characterDatas = new List<CharacterClass>();

    public event EventHandler OnLoadedEnd;

    private void Awake()
    {
        LoadCharacterData("CharacterClass");
    }

    public void SpawnObject(string code, ILoadable sender)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(code);
        StartCoroutine(WaitForSpawnComplete(handle,sender, code));
    }

    public void SpawnObject(string code, Vector3 position, ILoadable sender)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(code,position,Quaternion.identity);
        StartCoroutine(WaitForSpawnComplete(handle, sender, code));
    }

    IEnumerator WaitForSpawnComplete(AsyncOperationHandle<GameObject> handle, ILoadable sender, string code)
    {
        while (handle.IsDone == false)
        {
            yield return handle;
        }

        OnSpawnComplete(handle,sender, code);
    }

    private void LoadCharacterData(string code)
    {
        Addressables.LoadAssetsAsync<CharacterClass>(code, OnCharacterClassLoaded).Completed += (handle) =>
        {
            if (handle.IsDone) LoadEnd();
        };
    }

    private void OnCharacterClassLoaded(CharacterClass cc)
    {
        characterDatas.Add(cc);
    }

    private void OnSpawnComplete(AsyncOperationHandle<GameObject> handle, ILoadable sender, string code)
    {
        sender.GetGameInstace(handle.Result, code);       
    }
   
    private void LoadEnd()
    {
        OnLoadedEnd.Invoke(this,EventArgs.Empty);
    }


    public CharacterClass GetCharacterClass(int num)
    {
       return characterDatas[num];
    }

    public void LoadScene(string scene)
    {
        Addressables.LoadSceneAsync(scene).Completed +=(handle)=>
        { 
           switch(scene)
            {
                case "GameScene":
                    gameManager.OnGameScene();
                    break;

                case "SelectScene":

                    break;

                case "TutoScene":
                    gameManager.OnGameScene();
                    break;
            }
        };
    }
}
