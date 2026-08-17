using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform character;
    private Vector3 s = Vector3.zero;
    public Vector3 offset;
    public float cameraSpeed;

    private void Start()
    {
        character = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = character.position + offset;
        Vector3 CameraPosition = this.gameObject.transform.position;

        this.gameObject.transform.position = Vector3.SmoothDamp(CameraPosition, targetPosition, ref s, cameraSpeed * Time.fixedDeltaTime);
    }

}
