using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class SlicingComponent : MonoBehaviour
{
    [SerializeField] private ShipManager _manager;
    [SerializeField] private bool _debug;

    private List<RaycastHit> hits = new List<RaycastHit>();
    private Vector3 startPosition;
    
    private void Start()
    {
        _manager.OnShipStartMovement.AddListener(FindObstacles);
        _manager.OnShipFinishMovement.AddListener(SliceObstacles);
    }
    
    private void FindObstacles()
    {
        startPosition = _manager.transform.position;
        Vector3 dir = _manager.GetMovementDirection();
        float length = _manager.GetActionDistance();

        var ray = new Ray(startPosition, dir);
        var oppositeRay = new Ray(startPosition + dir, - dir);
        
        int ignoreLayerMask = 1 << LayerMask.GetMask(Layers.NON_OBSTACLE_LAYER);
        
        hits = new List<RaycastHit>(Physics.RaycastAll(ray, length, ignoreLayerMask));
        //hits.AddRange(Physics.RaycastAll(oppositeRay, length, ignoreLayerMask));

        hits.OrderBy(
            hit => Vector3.Distance(_manager.transform.position, hit.point)
        );

        if (_debug) {
            Debug.DrawRay(ray.origin, ray.direction * length, Color.red, 1f);
           // Debug.DrawRay(oppositeRay.origin + oppositeRay.direction * length, -oppositeRay.direction, Color.yellow, 1f);
        }
    }

    private void SliceObstacles()
    {
        foreach (var hit in hits) {
            ISliceable sliceable = hit.collider.gameObject.GetComponent<ISliceable>();
            if (sliceable != null) {
                sliceable.OnSliced(startPosition, hit.point);
            }

            if (_debug) {
                Debug.Log("hit " + hit.point + " name " + hit.collider.name);
            }
        }
    }
}