using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Definition.CameraMode mode = Definition.CameraMode.QuaterView;
    [SerializeField]
    public Vector3 offset = new Vector3(0, 6, -5);
    [SerializeField]
    GameObject player = null;

    void LateUpdate()
    {
        if (player == null)
            return;

        if (mode == Definition.CameraMode.QuaterView)
        {
            RaycastHit hitInfo;
            int layerMask = LayerMask.GetMask("Wall");

            if (Physics.Raycast(player.transform.position, offset, out hitInfo, offset.magnitude, layerMask))
            {
                float distance = (hitInfo.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + offset.normalized * distance;
                transform.LookAt(player.transform.position + Vector3.up);
            }
            else
            {
                transform.position = player.transform.position + offset;
                transform.LookAt(player.transform);
            }
        }
    }

    public void SetQuaterView()
    {
        mode = Definition.CameraMode.QuaterView;
        offset = new Vector3(0, 6, -5);
    }
}
