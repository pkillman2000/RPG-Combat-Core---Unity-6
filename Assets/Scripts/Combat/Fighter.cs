using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float _weaponRange = 1.2f;
        [SerializeField]
        private int _weaponDamage = 10;
        [SerializeField]
        private float _timeBetweenAttacks = 1.0f;

        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            if(_mover == null)
            {
                Debug.LogError("Mover is Null!");
            }

            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("Action Scheduler is Null!");
            }

            _animator = GetComponent<Animator>();
            if(_animator == null)
            {
                Debug.LogError("Animator is Null!");
            }
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if(_target == null)
            {
                return;
            }

            if(_target.IsDead())
            {
                return;
            }
            
            if(!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(this.transform.position, _target.transform.position) < _weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehavior()
        {
            this.transform.LookAt(_target.transform.position);

            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if(combatTarget == null)
            {
                return false;
            }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        // This is an animation event
        public void Hit()
        {
            if(_target == null)
            {
                return;
            }

            _target.TakeDamage(_weaponDamage);
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
        }

        private void StopAttack()
        {
            _animator.SetTrigger("stopAttack");
            _animator.ResetTrigger("attack");
        }
    }
}