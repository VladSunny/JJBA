using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.VFX
{
    public class DecalManager : MonoBehaviour
    {
        [SerializeField] private DecalEffect[] decalEffect;
        [SerializeField] private float hitDistance = 10f;

        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        public void SprayDecal(string name, Vector3 direction, Vector3 origin)
        {
            foreach (DecalEffect de in decalEffect)
            {
                if (de.name == name)
                {
                    RaycastHit raycastHit;
                    Ray ray = new Ray(origin, direction);

                    if (Physics.Raycast(ray, out raycastHit, hitDistance, de.layerMask, QueryTriggerInteraction.Ignore))
                    {
                        MakeSpray(de.decal, raycastHit);
                        Debug.Log("Spray");
                    }

                    // Debug ray
                    Debug.DrawRay(ray.origin, ray.direction * hitDistance, Color.red, 10f);

                    break;
                }
            }
        }

        void MakeSpray(GameObject decal, RaycastHit raycastHit)
        {
            GameObject spray = Instantiate(decal, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        }
    }
}
