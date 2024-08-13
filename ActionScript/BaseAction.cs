using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition grid, Action onAction);
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual bool IsValidAction(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositonList = GetValidActionGridPositionList();       
        return validGridPositonList.Contains(gridPosition);
    }

    public virtual int GetActionPointCost()
    {
        return 0;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this,EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();       
        OnAnyActionCompleted?.Invoke(this,EventArgs.Empty);
        unit.AnimChange(Unit.animState.Idle);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetPreferEnemyAction()
    {
        List<EnemyAIAction> enemyActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        for(int i=0; i<validActionGridPositionList.Count;i++ )
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(validActionGridPositionList[i]);
            enemyActionList.Add(enemyAIAction);
        }
               
        if(enemyActionList.Count > 0)
        {
            enemyActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyActionList[0];
        }

        else return null;
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    

    
   
}
