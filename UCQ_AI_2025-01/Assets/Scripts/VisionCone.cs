using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [Header("Vision Cone Settings")]
    public float visionRange = 10f; // Maximum distance the vision cone can reach
    public float visionAngle = 45f; // Angle of the vision cone in degrees
    public LayerMask targetMask; // Layer mask to detect target objects
    public LayerMask obstacleMask; // Layer mask to detect obstacles blocking vision

    [Header("Colors")]
    public Color idleColor = Color.green; // Color of the vision cone when no target is detected
    public Color alertColor = Color.red; // Color of the vision cone when a target is detected

    private GameObject target; // Reference to the detected target

    void Update()
    {
        DetectTarget(); // Check if any targets are within the vision cone
    }

    void DetectTarget()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, visionRange, targetMask);
        
        foreach (Collider targetCollider in targetsInViewRadius)
        {
            Transform targetTransform = targetCollider.transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < visionAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, targetTransform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    target = targetTransform.gameObject; // Set the detected target
                    return;
                }
            }
        }
        
        target = null; // No target detected
    }

    void OnDrawGizmos()
    {
        // Set the Gizmos color based on whether a target is detected
        Gizmos.color = target ? alertColor : idleColor;
        
        // Calculate the left and right boundaries of the vision cone
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionRange;
        
        // Draw lines representing the vision cone boundaries
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        
        // Draw a wire sphere to visualize the vision range
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    public GameObject GetTarget()
    {
        return target; // Return the detected target (if any)
    }
}

// https://docs.unity3d.com/ScriptReference/Gizmos.html
// https://learn.unity.com/tutorial/implementing-field-of-view
// https://www.youtube.com/watch?v=lV47ED8h61k
