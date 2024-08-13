using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnitManager unitManager;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Start()
    {
        unitManager = GameManager.GetManagerClass<UnitManager>();
        state = State.WaitingForEnemyTurn;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn:

                break;

            case State.TakingTurn:
                TryTakeEnemyAIAction(SetStateTakingTurn);
                break;

            case State.Busy:

                break;
        }
    }
    
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {

        for (int i = 0; i < unitManager.GetEnemyUnitList().Count; i++)
        {
            if(TryTakeEnemyAIAction(unitManager.GetEnemyUnitList()[i], onEnemyAIActionComplete))  
                return true;
        }
        return false;
    }


    private bool TryTakeEnemyAIAction(Unit EnemyUnit, Action onEnemyAIActionComplete)
    {
        BaseAction[] baseAction;
        EnemyUnit.GetBaseActionArray(out baseAction);

        EnemyAIAction preferEnemyAction = null;
        BaseAction preferBaseAction = null;

        for(int i=0;i<baseAction.Length;i++ )
        {
            if (!EnemyUnit.CanSpendActionPointsToTakeAction(baseAction[i])) continue;
            if(preferEnemyAction == null)
            {
                preferEnemyAction = baseAction[i].GetPreferEnemyAction();
                preferBaseAction = baseAction[i];
            }
            else
            {
                EnemyAIAction testEnemyAction = baseAction[i].GetPreferEnemyAction();
                if(testEnemyAction != null && testEnemyAction.actionValue > preferEnemyAction.actionValue)
                {
                    preferEnemyAction = testEnemyAction;
                    preferBaseAction = baseAction[i];
                }
            }            
        }

        if(preferEnemyAction != null && EnemyUnit.TrySpendActionPointsToTakeAction(preferBaseAction))
        {
            preferBaseAction.TakeAction(preferEnemyAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }               
    }
}
