using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : BaseAction
{
    public class BaseParametes { }
    public class TestBaseParametes { }

    public override string GetActionName()
    {
        return "Test";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return null;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return null;
    }
}
