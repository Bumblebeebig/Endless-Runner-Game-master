using UnityEngine;

namespace Bumblebee_Asset.Scripts.Player
{
    public class CameraController : MonoBehaviour
    {
        private Transform _target;
        private Vector3 _offset;

        void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _offset = transform.position - _target.position;
        }

        void LateUpdate()
        {
            var transform1 = transform;
            var position = transform1.position;
            Vector3 newPosition = new Vector3(position.x, position.y, _offset.z + _target.position.z);
            transform.position = Vector3.Lerp(position, newPosition, 0.6f);
        }
    }
}
