using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineTransposer cinemachineTransposer;
    const float MAX_SCROLL = 12f;
    const float MIN_SCROLL = 2f;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
       
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        Vector3 rotationVector = new Vector3(0, 0, 0);
        Vector3 m_follwerOffset = cinemachineTransposer.m_FollowOffset;
        //float zoomAmount = 1f;

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        if(Input.mouseScrollDelta.y >0)
        {
            m_follwerOffset.y += 1;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            m_follwerOffset.y -= 1;
        }

        m_follwerOffset.y = Mathf.Clamp(m_follwerOffset.y, MIN_SCROLL, MAX_SCROLL);
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        //cinemachineTransposer.m_FollowOffset  = m_follwerOffset;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, m_follwerOffset, Time.deltaTime * 10f);
        transform.position += moveVector * 10 * Time.deltaTime;
        transform.eulerAngles += rotationVector * 100f * Time.deltaTime;

    }
}
