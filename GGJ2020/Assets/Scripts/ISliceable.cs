using UnityEngine;

namespace DefaultNamespace
{
    public interface ISliceable
    {
        void OnSliced(Vector3 entry, Vector3 exit);
    }
}