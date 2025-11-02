using UnityEngine;

namespace CameraControl
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
                _cameraTransform.LookAt(target);
            }
        }
    }
}