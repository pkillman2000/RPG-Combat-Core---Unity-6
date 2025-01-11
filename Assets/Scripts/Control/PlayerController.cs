using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;

        private void Start()
        {
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
        }

        private void Update()
        {
            if(InteractWithCombat())
            {
                return;
            }

            if (InteractWithMovement())
            {
                return;
            }
        }

        private bool InteractWithCombat()
        {
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                    if(!_fighter.CanAttack(target))
                    {
                        continue;
                    }
                    /*
                     * NOTE: Checking for mouse input is here because
                     * later in the course, the mouse cursor will be
                     * changing based on what it is hovering over.
                     * If no code is ran if the mouse button isn't clicked,
                     * it can't raycast and see what it's hovering over.
                    */
                    if (Input.GetMouseButtonDown(0))
                    {
                        _fighter.Attack(target);
                    }
                    
                    return true;
                }
                return false;
        }

        private bool InteractWithMovement()
        {
            // RaycastHit contains information about the ray including
            // what the ray hit.
            RaycastHit hit;
            // Physics.Raycast() returns a bool that states if
            // the ray hit something or not.
            bool hasHit;

            hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            // Ray is the ray between the camera and the mouse cursor
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}