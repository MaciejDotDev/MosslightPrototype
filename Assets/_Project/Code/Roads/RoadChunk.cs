using System.Collections.Generic;
using UnityEngine;

namespace Mosslight.Roads
{
    public sealed class RoadChunk : MonoBehaviour
    {
        [SerializeField] private Transform _entryPoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private List<RoadSocket> _sockets = new();

        public Transform EntryPoint => _entryPoint;
        public Transform ExitPoint => _exitPoint;
        public IReadOnlyList<RoadSocket> Sockets => _sockets;

        private void OnValidate()
        {
            if (_entryPoint == null)
            {
                Debug.LogWarning($"{nameof(RoadChunk)} on '{name}' needs an EntryPoint assigned.", this);
            }

            if (_exitPoint == null)
            {
                Debug.LogWarning($"{nameof(RoadChunk)} on '{name}' needs an ExitPoint assigned.", this);
            }
        }
    }
}
