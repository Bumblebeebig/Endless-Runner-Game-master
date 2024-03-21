using UnityEngine;

namespace Bumblebee_Asset.Scripts.Tiles
{
    public class DisableAfter : MonoBehaviour
    {
        private float _timer = 0;
        public float delay = 0.7f;

        private void OnEnable()
        {
            _timer = 0;
        }
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= delay)
                gameObject.SetActive(false);
        }
    }
}
