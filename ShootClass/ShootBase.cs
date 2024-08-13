using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootBase : MonoBehaviour
{
    protected Unit unit;
    protected bool isEnd = false;
    protected ResourcesManager resourcesManager;

    protected void Start()
    {
        unit = GetComponent<Unit>();
        resourcesManager = GameManager.GetManagerClass<ResourcesManager>();
    }

    public abstract void ShootMethod(Unit targetUnit, int damage);


    public bool GetIsEnd() { return isEnd; }   
}
