using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAccurate : MonoBehaviour
{   
    private bool CalculateAccurate(int acc)
    {
        int ex = UnityEngine.Random.Range(0, 100);
        
        return ex < acc;
    }

}
