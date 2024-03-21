using System.Collections.Generic;
using UnityEngine;

namespace Bumblebee_Asset.Scripts.Tiles
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;

        public GameObject objectToPool;
        public List<GameObject> pooledObjects;
        public int amountToPool = 15;

        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            for(int i = 0; i < amountToPool; i++)
            {
                GameObject go = Instantiate(objectToPool);
                go.transform.rotation = objectToPool.transform.rotation;
                go.SetActive(false);
                pooledObjects.Add(go);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                    return pooledObjects[i];
            }

            return null;
        }
    }
}
