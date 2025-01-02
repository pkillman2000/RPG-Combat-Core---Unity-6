using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            Debug.LogError("NavMesh Agent is Null!");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is null!");
        }
    }
    
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            MoveToCursor();
        }

        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        // Ray is the ray between the camera and the mouse cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit contains information about the ray including
        // what the ray hit.
        RaycastHit hit;
        // Physics.Raycast() returns a bool that states if
        // the ray hit something or not.
        bool hasHit;

        hasHit = Physics.Raycast(ray, out hit);

        if(hasHit)
        {
            _navMeshAgent.destination = hit.point;
        }
    }

    private void UpdateAnimator()
    {
        // Get global velocity from NavMesh Agent
        Vector3 velocity = _navMeshAgent.velocity;
        // Convert global velocity to local velocity
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        // Get forward speed, (Z), from localVelocity
        float speed = localVelocity.z;
        // Pass speed to Animator
        _animator.SetFloat("forwardSpeed", speed);
    }
}
