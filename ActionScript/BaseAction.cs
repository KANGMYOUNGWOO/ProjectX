using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;                     // �����ϴ� ��ü -> CameraManager �׼��� ���۵� �� ī�޶� ��ŷ
    public static event EventHandler OnAnyActionCompleted;                   // �����ϴ� ��ü -> CameraManager �׼��� ����� �� ī�޶� ��ŷ
                                                                          
                                                                             /*  ī�޶� ��ŷ�� �� �� unit�� cameraManager�� �����Ͽ� ī�޶� ��ŷ ����� �ؾ��Ѵ� 
                                                                                �̰��� ���� �ϱ� ���� ����� 
                                                                           
                                                                              1. cameraManager�� �̱���(static)���� ���� ��, ��� unit�� cameraManager�� field member�� ������ 
                                                                                 �ʿ��� �� cameraManager�� �����Ѵ�.

                                                                              2. cameraManager�� ��ü�� �ƴ� Ÿ��(class)�� ���� �̺�Ʈ �ڵ鷯�� �����ϰ�
                                                                                 unit�� BaseAction�� ���� cameraManager�� �����ϱ�.

                                                                                 �ش� ������Ʈ���� ��κ��� �۾��� 1������ ����������,
                                                                                 ��ǻ� eventHandler 2���� ó���ϴ� cameraManager�� ��� baseAction�� �ʵ� �����, ���� ������ ������
                                                                                 ������ ��ȿ�������� ���δ�. 
                                                                                 ���� ���Ǵ� 2���� �̺�Ʈ �ڵ鷯�� static���� ����� ���� ���ƺ��δ�.
                                                                              */ 
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();                                  // ���� ������ �ൿ�� �������� string���� ��ȯ 
    public abstract void TakeAction(GridPosition grid, Action onAction);     // ActionSystem�� �� �ൿ�� �����Ű�� ���� �߻� �Լ�
    public abstract List<GridPosition> GetValidActionGridPositionList();     // �ش� �ൿ�� �� �� �ִ� ��ġ(�̵� ������ ��ġ or ��� ������ ��ġ)�� �����ϴ� List

    public virtual bool IsValidAction(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositonList = GetValidActionGridPositionList();       
        return validGridPositonList.Contains(gridPosition);
    }

    public virtual int GetActionPointCost()
    {
        return 0;
    }

    #region ActionStart
    /*
       TakeAction()�� �Ű������� ���� �븮�� Action�� onActionComplete�� �Ҵ��Ѵ�.
       onActionComplete�� �翬�ϰԵ� �ൿ�� ���� �� �� ����� Action�̴�.
       ���� ������ Action�� ClearBusy() �ϳ� ���ε� -> �ൿ�� ������ �� ���� bool ���� Isbusy�� false�� �ٲ��� ���̴�. 
     */
    #endregion
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this,EventArgs.Empty);                   // ī�޶� ��ŷ action ����
    }
    #region ActionStart
    /*
       �ൿ�� ����� �� onActionComplete�� �Ҵ�� �븮�� Action�� �����Ѵ�
       �� �� ������ ���ϸ��̼��� Idle�� �����Ѵ�.
     */
    #endregion
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();       
        OnAnyActionCompleted?.Invoke(this,EventArgs.Empty);                // ī�޶� ��ŷ action ����
    }
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
