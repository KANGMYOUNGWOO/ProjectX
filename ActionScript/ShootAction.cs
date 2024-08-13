using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{

    private event EventHandler OnShoot;
    private ShootBase shootProcess;
   
   
    private enum State 
    { 
       Aiming,
       Shooting,
       Cooloff    
    }
    [SerializeField] private int maxShootDistance = 7;
    private float stateTimer;
    private Unit TargetUnit;
    private bool canShootBullet;

    private LevelGrid levelGrid;
    private Pathfinding pathFinding;
    private UIManager uiManager;
  
    private State state;

    private void Start()
    {
        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        uiManager = GameManager.GetManagerClass<UIManager>();       
        shootProcess = gameObject.GetComponent<ShootBase>();        
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {        
        GridPosition unitGridPosition = unit.GetGridPosition();
      
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!levelGrid.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance) continue;
                if (unitGridPosition == testGridPosition) continue;
                if (!levelGrid.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                Unit targetUnit = levelGrid.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.GetISEnemy() == unit.GetISEnemy()) continue;
                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition grid, Action onAction)
    {
        unit.AnimChange(Unit.animState.Aiming);       
        TargetUnit = levelGrid.GetUnitAtGridPosition(grid);
        canShootBullet = true;
        NextState(State.Aiming, 0.1f);       
        uiManager.SetAccuracyUI(TargetUnit.transform.position, 50, this);
        unit.OnAnyUnitAimedFunction(TargetUnit);
        ActionStart(onAction);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
      return GetValidActionGridPositionList(gridPosition).Count;
    }

    private void NextState(State st, float time)
    {
        state = st;
        stateTimer = time;
    }

    private void StateMachine()
    {
        switch (state)
        {
            case State.Aiming:
                Vector3 AimDirection = (TargetUnit.transform.position - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, AimDirection, Time.deltaTime * 10f);              
                    //NextState(State.Shooting, 0.4f);
               
                break;

            case State.Shooting:
                if (canShootBullet)
                {
                    canShootBullet = false;
                    //Shoot();
                    OnShoot?.Invoke(this, EventArgs.Empty);
                    unit.AnimChange(Unit.animState.Shoot);
                    shootProcess.ShootMethod(TargetUnit,unit.characterClass.attackPoint);
                    uiManager.DisableAccuracyUI();
                }
                if (shootProcess.GetIsEnd()) NextState(State.Cooloff, 1f);
                break;

            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    unit.OnAnyUnitAimedEndFunction();
                    ActionComplete();                   
                }
                break;
        }
    }


    private void Update()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        StateMachine();
    }

    public Unit GetTargetUnit()
    {
        return TargetUnit;
    }

    public int GetShootRange()
    {
        return maxShootDistance;
    }

    public void Fire()
    {      
        NextState(State.Shooting, 0.4f);
       
    }

    public void EndAction()
    {
        unit.OnAnyUnitAimedEndFunction();
        ActionComplete();        
    }
    
}
