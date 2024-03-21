using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Bumblebee_Asset.Scripts.Game
{
    public class Randomness : MonoBehaviour
    {
        private Volume _volume;
        private ColorAdjustments _colorAdjustments;
        public float[] randomHueShitf;

        void Start()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _colorAdjustments);
            _colorAdjustments.hueShift.value = randomHueShitf[Random.Range(0, randomHueShitf.Length)];
        }
    }
}
