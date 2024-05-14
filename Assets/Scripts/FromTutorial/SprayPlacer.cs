using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SprayPlacer : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject decal;
    [SerializeField] Vector3 decalSize = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] float sprayDistance = 10f;
    [SerializeField] KeyCode sprayKey = KeyCode.E;

    Camera cam;
    float hitDistance;
    Ray ray;
    RaycastHit raycastHit;
    
    private void Awake() {
        cam = Camera.main;
    }

    private void Update() {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out raycastHit, sprayDistance, layerMask, QueryTriggerInteraction.Ignore)) {
            hitDistance = raycastHit.distance;

            if (hitDistance <= sprayDistance && Input.GetKeyDown(sprayKey)) {
                MakeSpray();
                Debug.Log("Spray");
            }
        }
    }

    void MakeSpray() {
        GameObject spray = Instantiate(decal, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        //spray.GetComponent<DecalProjector>().size = decalSize;
        //spray.GetComponent<DecalProjector>().pivot = new Vector3(0f, 0f, size.z * .25f);
    }
}
