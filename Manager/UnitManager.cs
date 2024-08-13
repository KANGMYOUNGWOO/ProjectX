using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour,IManager,ILoadable
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
   
    private ResourcesManager resourcesManager;
    private LevelGrid levelGrid;

    private List<Unit> UnitList;
    private List<Unit> FriendlyUnitList;
    private List<Unit> EnemyUnitList;
    private CharacterClass[] EntryArray = new CharacterClass[3];

    private int UnitCount = 0;

    public event EventHandler<Unit> OnUnitAimed;
    public event EventHandler OnUnitAimedEnd;

    private void Awake()
    {
        UnitList = new List<Unit>();
        FriendlyUnitList = new List<Unit>();
        EnemyUnitList = new List<Unit>();        
    }

    private void Start()
    {
        resourcesManager = GameManager.GetManagerClass<ResourcesManager>();
        

        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;

        gameManager.OnGameSceneStart += GAMEMANAGER__OnGameSceneStart;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        UnitList.Add(unit);
        switch (unit.GetISEnemy())
        {
            case true:
                EnemyUnitList.Add(unit);
                break;
            case false:
                FriendlyUnitList.Add(unit);
                break;
        }        
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        
        Unit unit = sender as Unit;
        UnitList.Remove(unit);
        switch (unit.GetISEnemy())
        {
            case true:
                EnemyUnitList.Remove(unit);
                break;
            case false:
                FriendlyUnitList.Remove(unit);
                break;
        }
        
    }

    private void GAMEMANAGER__OnGameSceneStart(object sender , EventArgs e)
    {
        resourcesManager.SpawnObject(string.Format("Unit_{0}", EntryArray[0].characterName),new Vector3(1f,0, 1f),this);  
        resourcesManager.SpawnObject(string.Format("Unit_{0}", EntryArray[1].characterName), new Vector3(3f,0,3),this);  
        resourcesManager.SpawnObject(string.Format("Unit_{0}", EntryArray[2].characterName),new Vector3(6.05f, 0,6),this);

        resourcesManager.SpawnObject("Enemy", new Vector3(4f, 0, 14f), this);
    }

    public void OnUnitAimedFunction(object sender, Unit unit)
    {
        OnUnitAimed.Invoke(this,unit);
    }

    public void OnUnitAimedEndFunction()
    {
        OnUnitAimedEnd.Invoke(this,EventArgs.Empty);
    }

    public void GetGameInstace(GameObject game, string code)
    {
        Unit unit = game.GetComponent<Unit>();

        if (code.Contains("Unit")) { unit.OnSpawnInitialize(EntryArray[UnitCount++]); FriendlyUnitList.Add(unit); }
        else if (code.Contains("Enemy")) { unit.OnSpawnInitialize(resourcesManager.GetCharacterClass(1)); EnemyUnitList.Add(unit); }

        UnitList.Add(unit);
    }

    public List<Unit> GetUnitList()
    {
        return UnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return EnemyUnitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return FriendlyUnitList;
    }

    public void SetEntryArray(ref CharacterClass[] cc)
    {
        for(int i=0;i<EntryArray.Length;i++)
        {
            EntryArray[i] = cc[i];
        }
    }

    
}
