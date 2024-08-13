using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManager,ILoadable
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
    private ResourcesManager resourcesManager;

    private void Start()
    {
        resourcesManager = GameManager.GetManagerClass<ResourcesManager>();
    }


    private void SpawnSelectableCharacter(int num)
    {
        string spawnName = string.Format("character_{0}", num);

        CharacterClass character = resourcesManager.GetCharacterClass(num);
        resourcesManager.SpawnObject(character.characterName, new Vector3(12.5f, 0 , -8 + 4*num),this);
    }

    public void GetGameInstace(GameObject game, string code)
    {
        
    }
}
