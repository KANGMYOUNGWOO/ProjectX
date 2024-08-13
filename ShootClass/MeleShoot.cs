using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleShoot : ShootBase , ILoadable
{
    ResourcesManager resourcesManager;

    private Unit targetUnit;
    // Start is called before the first frame update
    public override void ShootMethod(Unit targetUnit, int Damage)
    {
        isEnd = false;
        this.targetUnit = targetUnit;
        StartCoroutine(BulletShooter(targetUnit, Damage));
    }

    private IEnumerator BulletShooter(Unit targetUnit, int damage)
    {
        yield return new WaitForSeconds(0.3f);
        resourcesManager.SpawnObject("Bang", transform.position, this);
        yield return new WaitForSeconds(0.12f);
        targetUnit.Damage(damage);
        yield return new WaitForSeconds(0.9f);
        resourcesManager.SpawnObject("Bang", transform.position, this);
        yield return new WaitForSeconds(0.1f);
        targetUnit.Damage(damage);
        yield return new WaitForSeconds(0.7f);
        resourcesManager.SpawnObject("Bang", transform.position, this);
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
            bullet.SetPos(start, end);
        }

        if (code == "Bang")
        {

        }


    }
}
