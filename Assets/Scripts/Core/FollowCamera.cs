using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private void Start()
        {

        }

        /*
         * Using LateUpdate allows all of the animation
         * to occur before moving the camera
        */
        private void LateUpdate()
        {
            this.transform.position = _target.position;
        }
    }
}