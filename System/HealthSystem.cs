using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthSystem : MonoBehaviour,ILoadable
{   
    private UIManager uiManager;
    private ResourcesManager resourceManager;

    private CharacterClass characterClass;

    private GameObject ShieldObject;
    private HPUI hpUI;
    private Unit unit;

    private int Health;
    private int Shield;

    public struct EventStruct
    {
        public int health;
        public int shield;
        public int healthIndex;
        public int shieldIndex;

        public EventStruct(int health, int healthIndex, int shield, int shieldIndex)
        {
            this.health = health;
            this.shield = shield;
            this.healthIndex = healthIndex;
            this.shieldIndex = shieldIndex;
        }
    }

    public enum shieldType
    {
        Normal,
        Count,
        Explosion,
        Recover
    }

    public shieldType s_type;

    public event EventHandler<EventStruct> OnDamaged;

    private void Start()
    {
        resourceManager = GameManager.GetManagerClass<ResourcesManager>();
        uiManager = GameManager.GetManagerClass<UIManager>();
        
        Unit.OnAnyUnitAimed += UNIT_OnAimTarget;
        Unit.OnAnyUnitAimedEnd += UNIT_OnAimTargetEnd;
    }

    public void OnSpawnInitialize(CharacterClass cc)
    {
        this.characterClass = cc;
        Health = cc.healthPoint;
        Shield = cc.ShieldPoint;
        
        resourceManager.SpawnObject("CharacterSlot", this);
    }

    public void Damage(int damageAmount)
    {
        int prevHealth = Health;
        int prevShield = Shield;
        int leftOverDamage = 0;

        if (Shield > 0)
        {
            leftOverDamage = damageAmount - Shield;
            Shield -= damageAmount;           
            Health = leftOverDamage > 0 ? Health - leftOverDamage : Health;
            leftOverDamage = leftOverDamage > 0 ? leftOverDamage : damageAmount;
            SetDamagedEffect(Vector3.up * 1f, "ShieldStrike");
            Debug.Log(string.Format("leftoverDamage -> {0}\nshield -> {1}\nHealth -> {2}",leftOverDamage,Shield,Health));
            if (Shield <= 0) ShieldBreak();
        }

        else
        {
            SetDamagedEffect(Vector3.up * 1f,"Damage");
            Health -= damageAmount;
            leftOverDamage = damageAmount;
        }

        EventStruct es = new EventStruct(prevHealth,leftOverDamage,prevShield,damageAmount-leftOverDamage);

        OnDamaged.Invoke(this,es);

        void SetDamagedEffect(Vector3 pos, string code)
        {
            Vector3 position = transform.position;
            position += pos;
            resourceManager.SpawnObject(code, position , this);
        }

       

        
        if (Health < 0)
        {
            Health = 0;
        }
    }

    private void UNIT_OnAimTarget(object sender, Unit e)
    {
        hpUI.gameObject.SetActive(e == unit);
        hpUI.SetIsAimed(e == unit);

    }

    private void UNIT_OnAimTargetEnd(object sender, EventArgs e)
    {
       hpUI.gameObject.SetActive(true);
       hpUI.SetIsAimed(false);
    }



    private void ShieldBreak()
    {       
       Shield = 0;
       //OnSHiledBreak?.Invoke(this, EventArgs.Empty);
        
    }

    public shieldType GetShieldType(){ return s_type; }
    public int GetShieldAmount()     { return Shield; }
    public Unit GetUnit() { return unit; }
    public void SetUnit(Unit unit)   { this.unit = unit; /*UNIT_OnAnyUnitSpawned(this, EventArgs.Empty);*/ }

    private void UNIT_OnAnyUnitSpawned(object sender, EventArgs e)
    {       
        
    }


    


    public void GetGameInstace(GameObject game, string code)
    {
        switch(code)
        {
            case "Orange_Shield":
                ShieldObject = game;
                game.transform.parent = this.transform;
                break;

            case "Blue_Shield":
                ShieldObject = game;
                game.transform.parent = this.transform;
                break;

            case "CharacterSlot":
                hpUI = game.GetComponent<HPUI>();                
                hpUI.transform.parent = uiManager.GetCanvas().transform;
                hpUI.OnSpawnInitialize(this, characterClass.healthPoint, characterClass.ShieldPoint,characterClass.characterImage,characterClass.characterName);               
                //hpUI.SetCanvas(uiManager.GetCanvas(),this);
                //hpUI.SetHealth(this.Health,this.shieldAmount);
                break;
        }        
    }   
}
