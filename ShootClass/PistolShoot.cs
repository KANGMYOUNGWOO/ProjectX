using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PistolShoot : ShootBase,ILoadable
{   
    private Unit targetUnit;

    public override void ShootMethod(Unit targetUnit, int Damage)
    {
        isEnd = false;
        this.targetUnit = targetUnit; 
        StartCoroutine(BulletShooter(targetUnit,Damage));
    }

    private IEnumerator BulletShooter(Unit targetUnit , int damage)
    {
        Vector3 vec = transform.position + transform.forward * 1.2f + transform.up * 1.2f;  
        yield return new WaitForSeconds(0.3f);
        resourcesManager.SpawnObject("Bang", vec, this);
        yield return new WaitForSeconds(0.12f);       
        targetUnit.Damage(damage);
        yield return new WaitForSeconds(0.9f);
        resourcesManager.SpawnObject("Bang", vec, this);
        yield return new WaitForSeconds(0.1f);
        targetUnit.Damage(damage);
        yield return new WaitForSeconds(0.7f);
        resourcesManager.SpawnObject("Bang", vec, this);
        targetUnit.Damage(damage);
        isEnd = true;
    }

    public void GetGameInstace(GameObject game, string code)
    {
        if (code == "Bullet")
        {
            Bullet bullet = game.GetComponent<Bullet>();
            Vector3 start = transform.position;
            Vector3 end = targetUnit.transform.position;
            start.y = 1.2f;
            end.y = 1.2f;
            bullet.SetPos(start,end);
        }

        if(code == "Bang")
        {

        }

       
    }
}
