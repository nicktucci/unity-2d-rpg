using UnityEngine;
using System.Collections;
using System;

public class SmoothCamera2D : MonoBehaviour
{

    public Transform target;
    public BoxCollider2D bounds;
    [Space]
    [HideInInspector]
    public Vector2 zoomConstraints = new Vector2(5, 8);
    [HideInInspector]
    public float zoomSpeed = 10f;
    private float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private float desiredZoom;

    private Bounds cachedBounds;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();

        desiredZoom = cam.orthographicSize;


        if (target == null)
        {
            try
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch (System.Exception e)
            {
                e.ToString();
                Debug.Log("SmoothCamera2D could not locate a player and no target was set.");
                return;
            }
            transform.position = target.position;
        }
        if (bounds != null)
        {
            cachedBounds = bounds.bounds;
            bounds.isTrigger = true;
            bounds.enabled = false;
        }
        if (zoomConstraints.x < 2.5f) zoomConstraints.x = 2.5f;
    }

    private void Update()
    {
        //if (Game.State == GameState.World)
        //{
        //DoZoom();
        if (target != null)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = cam.aspect * halfHeight;



            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + (Vector3)Vector2.up + delta;
            destination.z = -10;

            destination.x = Mathf.Clamp(destination.x, cachedBounds.min.x + halfWidth, cachedBounds.max.x - halfWidth);
            destination.y = Mathf.Clamp(destination.y, cachedBounds.min.y + halfHeight, cachedBounds.max.y - halfHeight);

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

            destination.x = Mathf.Clamp(transform.position.x, cachedBounds.min.x + halfWidth, cachedBounds.max.x - halfWidth);
            destination.y = Mathf.Clamp(transform.position.y, cachedBounds.min.y + halfHeight, cachedBounds.max.y - halfHeight);
            transform.position = destination;
        }

        //}
        //else
        //{
        //    camera.orthographicSize = 4.1f;
        //}

    }
    private float zoomVelocity;
    private void DoZoom()
    {

        desiredZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        desiredZoom = Mathf.Clamp(desiredZoom, zoomConstraints.x, zoomConstraints.y);


        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, desiredZoom, ref zoomVelocity, dampTime);
    }
}