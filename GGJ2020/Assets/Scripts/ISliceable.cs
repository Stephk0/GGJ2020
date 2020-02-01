using UnityEngine;

namespace DefaultNamespace
{
    public interface ISliceable
    {
        void OnSliced(Vector3 startPosition, Vector3 hitPosition);
    }
}