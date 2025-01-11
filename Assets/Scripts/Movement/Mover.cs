using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Fighter _fighter;
        private ActionScheduler _actionScheduler;
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            if (_navMeshAgent == null)
            {
                Debug.LogError("NavMesh Agent is Null!");
            }

            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator is null!");
            }

            _fighter = GetComponent<Fighter>();
            if(_fighter == null)
            {
                Debug.LogError("Fighter is Null!");
            }

            _actionScheduler = GetComponent<ActionScheduler>();
            if (_actionScheduler == null)
            {
                Debug.LogError("Action Scheduler is Null!");
            }
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = destination;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
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
}