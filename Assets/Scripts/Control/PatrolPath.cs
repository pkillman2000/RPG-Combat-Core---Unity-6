using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField]
        private Color _gizmoColor  = Color.white;
        [SerializeField]
        private float _gizmoRadius = .25f;

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = _gizmoColor;
                Gizmos.DrawSphere(GetWaypoint(i), _gizmoRadius);

                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public Vector3 GetWaypoint(int i)
        {           
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if(i + 1 >= transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }
    }
}