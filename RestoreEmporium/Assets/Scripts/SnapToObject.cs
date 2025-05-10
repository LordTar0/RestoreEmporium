using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapToObject : MonoBehaviour
{
    public float snapDistance = 1f;
    public float snapSpeed = 10f;
    public List<Transform> nodes = new List<Transform>();

    public UnityEvent onSnap;

    private Vector3? snapTarget = null;
    private bool isSnapping = false;
    private Transform lastSnappedNode = null;

    void Update()
    {
        // smoothly moves toward snap target if one exists
        if (snapTarget.HasValue)
        {
            transform.position = Vector3.Lerp(transform.position, snapTarget.Value, Time.deltaTime * snapSpeed);

            // when close enough, finalize the snap
            if (Vector3.Distance(transform.position, snapTarget.Value) < 0.01f)
            {
                transform.position = snapTarget.Value;
                snapTarget = null;
                isSnapping = false;

                if (lastSnappedNode != null)
                {
                    onSnap?.Invoke();
                }
            }
        }

        // check for mouse release then try snapping
        if (Input.GetMouseButtonUp(0))
        {
            TrySnap();
        }
    }

    void TrySnap()
    {
        Transform closestNode = null;
        float smallestDistance = snapDistance;

        foreach (Transform node in nodes)
        {
            if (node == null) continue;

            float distance = Vector3.Distance(transform.position, node.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestNode = node;
            }
        }

        // If a nearby node was found, snap to it
        if (closestNode != null && closestNode != lastSnappedNode)
        {
            snapTarget = closestNode.position;
            isSnapping = true;
            lastSnappedNode = closestNode;
        }
    }
}
