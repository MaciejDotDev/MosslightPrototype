using UnityEngine;

namespace Mosslight.Roads
{
    public enum RoadSocketType
    {
        Generic,
        Sign,
        Event,
        Tree,
        Layby,
        SideRoad
    }

    public sealed class RoadSocket : MonoBehaviour
    {
        [SerializeField] private RoadSocketType _socketType = RoadSocketType.Generic;

        public RoadSocketType SocketType => _socketType;
        public Transform Transform => transform;
    }
}
