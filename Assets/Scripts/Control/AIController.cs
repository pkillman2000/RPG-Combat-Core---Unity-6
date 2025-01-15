using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float _chaseDistance = 5f;
        [SerializeField]
        private float _suspicionTime = 3f;
        [SerializeField]
        private PatrolPath _patrolPath;
        [SerializeField]
        private float _waypointTolerance = .5f;

        private GameObject _player;
        private Mover _mover;
        private Fighter _fighter;
        private Health _health;
        private ActionScheduler _actionScheduler;

        private Vector3 _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        public int _currentWaypointIndex = 0;
        private float _waypointDwellTime = 2f;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
            {
                Debug.LogError("Player is Null!");
            }

            _mover = GetComponent<Mover>();
            if (_mover == null)
            {
                Debug.LogError("Mover is Null!");
            }

            _fighter = GetComponent<Fighter>();
            if (_fighter == null)
            {
                Debug.LogError("Fighter is Null!");
            }

            _health = GetComponent<Health>();
            if (_health == null)
            {
                Debug.LogError("Health is Null!");
            }

            _actionScheduler = GetComponent<ActionScheduler>(); 
            if(_actionScheduler == null)
            {
                Debug.LogError("Action Scheduler is Null!");
            }

            _guardPosition = this.transform.position;
        }

        private void Update()
        {
            if (_health.IsDead())
            {
                return;
            }

            if (InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                AttackBehavior();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehavior()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }

        private void SuspicionBehavior()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition;

            if(_patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    _timeSinceArrivedAtWaypoint = 0f;
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(_timeSinceArrivedAtWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(this.transform.position, _player.transform.position) <= _chaseDistance;
        }
    }
}