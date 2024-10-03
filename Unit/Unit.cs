using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 9;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    public static event EventHandler<Unit> OnAnyUnitAimed;
    public static event EventHandler OnAnyUnitAimedEnd;

    private int actionPoints;
    private Vector3 targetPosition;
    private GridPosition gridPosition;
    
    private BaseAction[] baseActionArray;
    private Animator animator;
    private HealthSystem healthSystem;

    LevelGrid levelGrid;
    
    [SerializeField] private bool IsEnemy;


    public enum animState 
    {
        Idle,
        Walk,
        Aiming,
        Shoot,
        Hit,
        Dead    
    }
    [SerializeField]private animState state;

    public CharacterClass characterClass { get; private set; }

    private void Awake()
    {
        targetPosition = transform.position;
        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        animator = GetComponentInChildren<Animator>();
        baseActionArray = GetComponents<BaseAction>();
       
    }

    private void Start()
    {
        GridPosition gridPosition = levelGrid.GetGridPosition(transform.position);
        levelGrid.AddUnitAtGridPosition(gridPosition, this);
        SetGridPosition(transform.position);
        actionPoints = ACTION_POINTS_MAX;
        OnAnyUnitSpawned?.Invoke(this,EventArgs.Empty);
        baseActionArray = GetComponents<BaseAction>();
      
    }


    public void OnSpawnInitialize(CharacterClass cc)
    {
        this.characterClass = cc;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetUnit(this);
        healthSystem.OnSpawnInitialize(characterClass);
    }


    public T GetAction<T>()  where T : BaseAction
    {
        foreach (BaseAction action in baseActionArray)
        {
            if(action is T) return (T)action;
        }
        return null;
    }

    

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
  
    public bool GetISEnemy()
    {
        return IsEnemy;
    }

    public void SetGridPosition(Vector3 worldPos)
    {
        gridPosition = levelGrid.GetGridPosition(worldPos);
        Debug.Log(string.Format("current-> x : {0} z:  {1}", gridPosition.x, gridPosition.z));
    }

    private void Update()
    {
       
    }

    public void GetBaseActionArray(out BaseAction[] baseActions)
    {
       baseActions = baseActionArray;
    }


    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
    }

    public void Damage(int damageAmount)
    {       
        healthSystem.Damage(damageAmount);
        animator.SetTrigger("Hit");
    }
   private void HealthSystem_OnDead(object sender,EventArgs e)
    {
        levelGrid.RemoveUnitAtGridPosition(gridPosition,this);
        OnAnyUnitDead?.Invoke(this,EventArgs.Empty);
    }
    
    public void AnimChange(animState state)
    {
        this.state = state;
       
        animator.SetTrigger(state.ToString());
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.identity;
    }

    public void OnAnyUnitAimedFunction(Unit unit)
    {
        OnAnyUnitAimed.Invoke(this, unit);
    }

    public void OnAnyUnitAimedEndFunction()
    {
        OnAnyUnitAimedEnd.Invoke(this,EventArgs.Empty);
    }

}



