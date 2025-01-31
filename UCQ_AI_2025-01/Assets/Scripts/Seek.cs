using UnityEngine;

public class Seek : MonoBehaviour
{
    [Header("Seek Settings")]
    public float maxSpeed = 5f; // Maximum speed of the object
    public float maxForce = 10f; // Maximum steering force

    private Rigidbody rb;
    private VisionCone visionCone;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        visionCone = GetComponent<VisionCone>(); // Get the VisionCone component
    }

    void FixedUpdate()
    {
        SeekTarget(); // Call the seek behavior in FixedUpdate for better physics handling
    }

    void SeekTarget()
    {
        GameObject target = visionCone.GetTarget(); // Get the detected target
        if (target != null)
        {
            Vector3 desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
            Vector3 steering = desiredVelocity - rb.linearVelocity;
            steering = Vector3.ClampMagnitude(steering, maxForce);

            rb.AddForce(steering, ForceMode.Acceleration); // Apply force to seek the target smoothly
        }
    }
}

// Steering Behaviors (Seek) - Craig Reynolds
// https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
// https://www.youtube.com/watch?v=TLq_wSJVYys