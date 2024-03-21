using System.Collections.Generic;
using Bumblebee_Asset.Scripts.Game;
using UnityEngine;

namespace Bumblebee_Asset.Scripts.Tiles
{
    public class TileManager : MonoBehaviour
    {
        private List<GameObject> _activeTiles;
        public GameObject[] tilePrefabs;

        public float tileLength = 30;
        public int numberOfTiles = 3;
        public int totalNumOfTiles = 8;

        public float zSpawn;

        private Transform _playerTransform;

        private int _previousIndex;

        void Start()
        {
            _activeTiles = new List<GameObject>();
            for (int i = 0; i < numberOfTiles; i++)
            {
                if (i == 0)
                    SpawnTile();
                else
                    SpawnTile(Random.Range(0, totalNumOfTiles));
            }

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            if (_playerTransform.position.z - 30 >= zSpawn - (numberOfTiles * tileLength))
            {
                int index = Random.Range(0, totalNumOfTiles);
                while (index == _previousIndex)
                    index = Random.Range(0, totalNumOfTiles);

                DeleteTile();
                SpawnTile(index);
            }
        }

        public void SpawnTile(int index = 0)
        {
            GameObject tile = tilePrefabs[index];
            if (tile.activeInHierarchy)
                tile = tilePrefabs[index + 8];

            if (tile.activeInHierarchy)
                tile = tilePrefabs[index + 16];

            tile.transform.position = Vector3.forward * zSpawn;
            tile.transform.rotation = Quaternion.identity;
            tile.SetActive(true);

            _activeTiles.Add(tile);
            zSpawn += tileLength;
            _previousIndex = index;
        }

        private void DeleteTile()
        {
            _activeTiles[0].SetActive(false);
            _activeTiles.RemoveAt(0);
            GameManager.Score += 3;
        }
    }
}