using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicators : MonoBehaviour
{
    private ShipManager _shipManager;
    public GameObject directionIndicator;
    public Camera _camera;
  
    public enum PlacingMode
    {
        OnActionEndPoint,
        AtMousePosition,
        OnPlayerPosition,
    }

    public PlacingMode placingMode;

    public bool onlyShowWhenNotMoving;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _shipManager = GameObject.FindWithTag(Tags.SHIP).GetComponent<ShipManager>();

        _shipManager.OnShipMoving.AddListener(HideIndicator);
        _shipManager.OnShipFinishMovement.AddListener(ShowIndicator);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos =Input.mousePosition;
        mousePos.z = _camera.transform.position.y;
        Vector3 mouse2World = _camera.ScreenToWorldPoint(mousePos);
        
        Vector3 dir = _shipManager.transform.position -  mouse2World ;
        Vector3 rotDir = Vector3.zero;
        switch (placingMode)
        {
            case PlacingMode.AtMousePosition:
                directionIndicator.transform.position = mouse2World;
                rotDir = _shipManager.transform.position -  mouse2World ;
                break;
            case PlacingMode.OnPlayerPosition:
                directionIndicator.transform.position = _shipManager.transform.position;
                rotDir = mouse2World - _shipManager.transform.position;
                break;
            case PlacingMode.OnActionEndPoint:
                directionIndicator.transform.position = _shipManager.transform.position - dir.normalized * _shipManager.GetActionDistance();
                rotDir = _shipManager.transform.position -  directionIndicator.transform.position ;
               
                break;
        }
        
        directionIndicator.transform.rotation = Quaternion.LookRotation(rotDir, Vector3.up);
    
    }

    public void HideIndicator()
         {
             directionIndicator.SetActive(false);
         }
    public void ShowIndicator()
    {
        directionIndicator.SetActive(true);
    }
}
