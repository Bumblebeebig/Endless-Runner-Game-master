using Bumblebee_Asset.Scripts.Game;
using UnityEngine;

namespace Bumblebee_Asset.Scripts.Tiles
{
    public class Gem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                GameObject effect = ObjectPool.Instance.GetPooledObject();

                if(effect!= null)
                {
                    effect.transform.position = transform.position;
                    effect.transform.rotation = effect.transform.rotation;
                    effect.SetActive(true);
                }

                PlayerPrefs.SetInt("TotalGems", PlayerPrefs.GetInt("TotalGems", 0) + 1);
                FindObjectOfType<AudioManager>().PlaySound("PickUp");
                GameManager.Score += 2;
                gameObject.SetActive(false);
            }
        }
    }
}
