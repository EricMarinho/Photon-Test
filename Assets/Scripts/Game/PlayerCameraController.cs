using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -4);
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private bool allowRotation = true;

    private float yaw = 0f;
    private float pitch = 10f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    private void Start()
    {
        StartCoroutine(WaitForLocalPlayer());
    }

    private IEnumerator WaitForLocalPlayer()
    {
        while (target == null)
        {
            var players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.InstanceID);
            foreach (var player in players)
            {
                if (player.photonView != null && player.photonView.IsMine)
                {
                    target = player.transform;
                    break;
                }
            }

            yield return null;
        }

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (allowRotation)
        {
            yaw += Input.GetAxis("Mouse X") * rotateSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}