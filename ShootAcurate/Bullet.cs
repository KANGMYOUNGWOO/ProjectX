using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     private Vector3 targetPosition;
     private Vector3 startPosition;
     private bool bisEnd = false;

    public void SetPos(Vector3 start, Vector3 end)
    {
        startPosition  = start;
        targetPosition = end;
        bisEnd = false;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        Debug.Log(start);
    }

      // Update is called once per frame
    void Update()
    {
       
        if(Vector3.Distance(targetPosition,startPosition) < Vector3.Distance(transform.position,startPosition))
        {
            bisEnd = true;
        }
        if(bisEnd)
        {
            Destroy(gameObject,0.5f);
        }
        else
        {
            transform.Translate((targetPosition - startPosition).normalized * 30f * Time.deltaTime);            
        }
    }
}
