using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{    
    [SerializeField] private GameObject actionCameraGameObject;
   

    private  Vector3 SHOOT_CAMERA_VECTOR = new Vector3(1.33f, 1.20f, 2.04f);
    private  Vector3 SHOOT_CAMERA_ROTATION = new Vector3(21, -4, 0);

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }


    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender,EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:

                Unit shooterUnit = shootAction.GetUnit();
                Unit TargetUnit = shootAction.GetTargetUnit();

               Vector3 cameraCharacterHeight = Vector3.up * 1.24f ;
                
                Vector3 ShootDir = (TargetUnit.transform.position - shooterUnit.transform.position).normalized;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * ShootDir;
                
                Vector3 actionCameraPosition =
                 shooterUnit.transform.position + 
                    cameraCharacterHeight +                    
                     shoulderOffset + 
                       (ShootDir * -1.5f);

              
               
                actionCameraGameObject.transform.position = actionCameraPosition;
                Vector3 targetPos = TargetUnit.transform.position + TargetUnit.transform.right * 0.5f;
                actionCameraGameObject.transform.LookAt(targetPos + cameraCharacterHeight);


                //Debug.Log(actionCameraPosition - SHOOT_CAMERA_VECTOR);

                ShowActionCamera();
                break;

            case MoveAction moveAction:
                HideActionCamera();
                break;

            case TestAction testAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        HideActionCamera();
    }


}
