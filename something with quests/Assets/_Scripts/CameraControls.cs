using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cam;
    private void Update()
    {               

        if (Input.GetMouseButton(1))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cam.m_XAxis.m_MaxSpeed = 130;
            cam.m_YAxis.m_MaxSpeed = 0.8f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cam.m_XAxis.m_MaxSpeed = 0;
            cam.m_YAxis.m_MaxSpeed = 0;
        }
    }
}
