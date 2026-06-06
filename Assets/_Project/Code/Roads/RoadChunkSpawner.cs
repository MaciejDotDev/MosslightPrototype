using System.Collections.Generic;
using UnityEngine;

namespace Mosslight.Roads
{
    public sealed class RoadChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _playerOrVehicle;
        [SerializeField] private List<RoadChunk> _roadChunkPrefabs = new();
        [SerializeField, Min(1)] private int _initialChunkCount = 5;
        [SerializeField, Min(1)] private int _chunksAhead = 3;
        [SerializeField, Min(1f)] private float _removeBehindDistance = 80f;
        [SerializeField] private bool _spawnPrefabsInOrder = true;

        private readonly List<RoadChunk> _spawnedChunks = new();
        private int _nextPrefabIndex;

        private void Start()
        {
            if (!CanSpawn())
            {
                return;
            }

            for (int i = 0; i < _initialChunkCount; i++)
            {
                SpawnNextChunk();
            }
        }

        private void Update()
        {
            if (_playerOrVehicle == null)
            {
                return;
            }

            if (_spawnedChunks.Count == 0 && CanSpawn())
            {
                SpawnNextChunk();
            }

            RemoveChunksBehindPlayer();
            SpawnChunksAhead();
        }

        private void SpawnChunksAhead()
        {
            int nearestChunkIndex = GetNearestChunkIndex();
            int chunksAhead = _spawnedChunks.Count - nearestChunkIndex - 1;

            while (chunksAhead < _chunksAhead && CanSpawn())
            {
                SpawnNextChunk();
                chunksAhead++;
            }
        }

        private void RemoveChunksBehindPlayer()
        {
            for (int i = _spawnedChunks.Count - 1; i >= 0; i--)
            {
                if (_spawnedChunks.Count <= _chunksAhead)
                {
                    return;
                }

                RoadChunk chunk = _spawnedChunks[i];

                if (chunk == null)
                {
                    _spawnedChunks.RemoveAt(i);
                    continue;
                }

                if (IsChunkBehindPlayer(chunk))
                {
                    _spawnedChunks.RemoveAt(i);
                    Destroy(chunk.gameObject);
                }
            }
        }

        private bool IsChunkBehindPlayer(RoadChunk chunk)
        {
            if (chunk.ExitPoint == null)
            {
                return false;
            }

            Vector3 playerFromExit = _playerOrVehicle.position - chunk.ExitPoint.position;
            float distancePastExit = Vector3.Dot(playerFromExit, chunk.ExitPoint.forward);

            return distancePastExit > _removeBehindDistance;
        }

        private int GetNearestChunkIndex()
        {
            int nearestIndex = 0;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < _spawnedChunks.Count; i++)
            {
                RoadChunk chunk = _spawnedChunks[i];

                if (chunk == null)
                {
                    continue;
                }

                float distance = Vector3.SqrMagnitude(GetChunkCenter(chunk) - _playerOrVehicle.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
        }

        private static Vector3 GetChunkCenter(RoadChunk chunk)
        {
            if (chunk.EntryPoint != null && chunk.ExitPoint != null)
            {
                return (chunk.EntryPoint.position + chunk.ExitPoint.position) * 0.5f;
            }

            return chunk.transform.position;
        }

        private void SpawnNextChunk()
        {
            RoadChunk prefab = GetNextPrefab();

            if (prefab == null)
            {
                return;
            }

            RoadChunk instance = Instantiate(prefab, transform.position, transform.rotation, transform);
            Transform connectionPoint = GetNextConnectionPoint();
            AlignEntryToConnection(instance, connectionPoint);
            _spawnedChunks.Add(instance);
        }

        private Transform GetNextConnectionPoint()
        {
            if (_spawnedChunks.Count == 0)
            {
                return transform;
            }

            RoadChunk lastChunk = _spawnedChunks[^1];
            return lastChunk != null && lastChunk.ExitPoint != null ? lastChunk.ExitPoint : transform;
        }

        private static void AlignEntryToConnection(RoadChunk chunk, Transform connectionPoint)
        {
            if (chunk.EntryPoint == null || connectionPoint == null)
            {
                return;
            }

            Quaternion rotationDelta = connectionPoint.rotation * Quaternion.Inverse(chunk.EntryPoint.rotation);
            chunk.transform.rotation = rotationDelta * chunk.transform.rotation;

            Vector3 positionDelta = connectionPoint.position - chunk.EntryPoint.position;
            chunk.transform.position += positionDelta;
        }

        private RoadChunk GetNextPrefab()
        {
            if (_roadChunkPrefabs.Count == 0)
            {
                return null;
            }

            if (!_spawnPrefabsInOrder)
            {
                return GetRandomPrefab();
            }

            for (int i = 0; i < _roadChunkPrefabs.Count; i++)
            {
                RoadChunk prefab = _roadChunkPrefabs[_nextPrefabIndex];
                _nextPrefabIndex = (_nextPrefabIndex + 1) % _roadChunkPrefabs.Count;

                if (prefab != null)
                {
                    return prefab;
                }
            }

            return null;
        }

        private RoadChunk GetRandomPrefab()
        {
            for (int i = 0; i < _roadChunkPrefabs.Count; i++)
            {
                RoadChunk prefab = _roadChunkPrefabs[Random.Range(0, _roadChunkPrefabs.Count)];

                if (prefab != null)
                {
                    return prefab;
                }
            }

            return null;
        }

        private bool CanSpawn()
        {
            if (_roadChunkPrefabs.Count == 0 || !_roadChunkPrefabs.Exists(prefab => prefab != null))
            {
                Debug.LogWarning($"{nameof(RoadChunkSpawner)} needs at least one road chunk prefab assigned.", this);
                return false;
            }

            return true;
        }
    }
}
