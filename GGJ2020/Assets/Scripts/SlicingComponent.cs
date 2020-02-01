using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class SlicingComponent : MonoBehaviour
{
    [SerializeField] private shipManager _manager;
    [SerializeField] private bool _debug;

    private List<RaycastHit> hits = new List<RaycastHit>();
    
    private void Start()
    {
        _manager.OnStartMovement.AddListener(FindObstacles);
        _manager.OnFinishMovement.AddListener(SliceObstacles);
    }
    
    private void FindObstacles()
    {
        Vector3 dir = _manager.GetMovementDirection();
        float length = _manager.GetActionDistance();

        var ray = new Ray(_manager.transform.position, dir * length);
        var oppositeRay = new Ray(_manager.transform.position + dir * length, - dir * length);
        
        int ignoreLayerMask = 1 << LayerMask.GetMask(Layers.SHIP_LAYER);
        
        hits = new List<RaycastHit>(Physics.RaycastAll(ray, length, ignoreLayerMask));
        // do we need the exit points?
        // hits.AddRange(Physics.RaycastAll(oppositeRay, length, ignoreLayerMask));

        hits.OrderBy(
            hit => Vector3.Distance(_manager.transform.position, hit.point)
        );

        if (_debug) {
            Debug.DrawLine(oppositeRay.origin, oppositeRay.direction, Color.yellow);
            Debug.DrawLine(ray.origin, ray.direction, Color.red);
        }
    }

    private void SliceObstacles()
    {
        foreach (var hit in hits) {
            ISliceable sliceable = hit.collider.gameObject.GetComponent<ISliceable>();
            if (sliceable != null) {
                sliceable.OnSliced(_manager.transform.position, hit.point);
            }

            if (_debug) {
                Debug.Log("hit " + hit.point + " name " + hit.collider.name);
            }
        }
    }
}