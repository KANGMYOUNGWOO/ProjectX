using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;                     // 구독하는 객체 -> CameraManager 액션이 시작될 떄 카메라 워킹
    public static event EventHandler OnAnyActionCompleted;                   // 구독하는 객체 -> CameraManager 액션이 종료될 떄 카메라 워킹
                                                                          
                                                                             /*  카메라 워킹을 할 때 unit이 cameraManager에 접근하여 카메라 워킹 명령을 해야한다 
                                                                                이것을 구현 하기 위한 방법은 
                                                                           
                                                                              1. cameraManager를 싱글톤(static)으로 구현 후, 모든 unit이 cameraManager를 field member로 가지며 
                                                                                 필요할 때 cameraManager에 접근한다.

                                                                              2. cameraManager가 객체가 아닌 타입(class)의 정적 이벤트 핸들러를 구독하고
                                                                                 unit이 BaseAction를 통해 cameraManager를 제어하기.

                                                                                 해당 프로젝트에서 대부분의 작업을 1번으로 구현했지만,
                                                                                 사실상 eventHandler 2개만 처리하는 cameraManager가 모든 baseAction의 필드 멤버로, 전역 변수로 남으면
                                                                                 굉장히 비효율적으로 보인다. 
                                                                                 차라리 사용되는 2개의 이벤트 핸들러를 static으로 만드는 것이 좋아보인다.
                                                                              */ 
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();                                  // 지금 선택한 행동이 무엇인지 string으로 반환 
    public abstract void TakeAction(GridPosition grid, Action onAction);     // ActionSystem이 각 행동을 실행시키기 위한 추상 함수
    public abstract List<GridPosition> GetValidActionGridPositionList();     // 해당 행동을 할 수 있는 위치(이동 가능한 위치 or 사격 가능한 위치)를 저장하는 List

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
       TakeAction()의 매개변수로 받은 대리자 Action을 onActionComplete에 할당한다.
       onActionComplete는 당연하게도 행동이 종료 될 때 실행될 Action이다.
       현재 구현된 Action은 ClearBusy() 하나 뿐인데 -> 행동이 끝났을 때 제어 bool 변수 Isbusy를 false로 바꿔줄 뿐이다. 
     */
    #endregion
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this,EventArgs.Empty);                   // 카메라 워킹 action 실행
    }
    #region ActionStart
    /*
       행동이 종료될 때 onActionComplete에 할당된 대리자 Action을 실행한다
       그 후 유닛의 에니메이션을 Idle로 변경한다.
     */
    #endregion
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();       
        OnAnyActionCompleted?.Invoke(this,EventArgs.Empty);                // 카메라 워킹 action 종료
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
