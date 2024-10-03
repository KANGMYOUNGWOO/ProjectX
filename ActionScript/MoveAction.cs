using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

   
    //private Vector3 targetPosition;
    private Vector3 StartPosition;

    private List<Vector3> positionList;

    private int currentPositionIndex;

    private LevelGrid levelGrid;
    private Pathfinding pathFinding;

  
    // Start is called before the first frame update
   
    void Start()
    {
        pathFinding = GameManager.GetManagerClass<Pathfinding>();
       
        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        unit = gameObject.GetComponent<Unit>();
                    
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };

    }


    public override void TakeAction(GridPosition grid, Action onAction)
    {
        List<GridPosition> pathGridPostionList = pathFinding.FindPath(levelGrid.GetGridPosition(transform.position),grid,out int pathLength);        
        currentPositionIndex = 0;
        positionList = new List<Vector3>();
        StartPosition = transform.position;

        unit.AnimChange(Unit.animState.Walk);
        foreach(GridPosition pathGridPosition in pathGridPostionList)
        {
            positionList.Add(levelGrid.GetWorldPosition(pathGridPosition));            
        }
                    
        ActionStart(onAction);
    }

    
    void Update()
    {
        if (!isActive) return;
        float stopdistance = 0.1f;
        Vector3 targetPosition = positionList[currentPositionIndex];
      
        if (Vector3.Distance(transform.position,targetPosition) > stopdistance)
        {          
            float moveSpeed = 4f;
            float rotateSpeed = 25f;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward,moveDirection,Time.deltaTime * rotateSpeed);
                     
        }
        else
        {
            if (onActionComplete != null) 
            {
              
                currentPositionIndex++;
                if (currentPositionIndex >= positionList.Count)
                {
                    GridPosition prevPosition = levelGrid.GetGridPosition(StartPosition);
                    GridPosition Current = levelGrid.GetGridPosition(targetPosition);
                    levelGrid.UnitMoveAtGridPosition(prevPosition, Current, unit);
                    unit.SetGridPosition(transform.position);
                    ActionComplete();
                }               
            }
        }
        
    }

    public bool ISValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override int GetActionPointCost()
    {
        return 1;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
 
        int maxMoveDistance = 6;

        GridPosition unitGridPosition = levelGrid.GetGridPosition(transform.position);

      

        for (int x = -maxMoveDistance; x<= maxMoveDistance; x++)
        {
          
            for(int z = -maxMoveDistance; z<=maxMoveDistance; z++)
            {
                GridPosition offsetGridPositin = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPositin;

                //if(levelGrid.IsValidGridPosition(testGridPosition)) Debug.Log(string.Format("x : {0} z:  {1}", testGridPosition.x , testGridPosition.z));
                if (!levelGrid.IsValidGridPosition(testGridPosition))  continue; 
                if (unitGridPosition == testGridPosition)  continue; 
                if (levelGrid.HasAnyUnitOnGridPosition(testGridPosition))  continue;
                if(!pathFinding.IsWalkableGridPosition(testGridPosition)) continue;
                if (!pathFinding.HasPath(unitGridPosition, testGridPosition)) continue;
                if (pathFinding.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * 10) continue;
               //Debug.Log(string.Format("x : {0} z:  {1}", testGridPosition.x, testGridPosition.z));
                validGridPositionList.Add(testGridPosition);                     
            }
        }       
        return validGridPositionList;
    }

   
}
