using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.VFX
{
    public class ParticleManager : MonoBehaviour
    {
        public ParticleEffect[] particleEffect;
        [SerializeField] private bool debugKeys = false;
        [SerializeField] private string debugPlayName;

        private void Awake()
        {
            foreach (ParticleEffect pe in particleEffect)
            {
                GameObject instance = Instantiate(pe.particleEffect, pe.parent);
                instance.transform.position = pe.parent.position;
                pe.instance = instance;
            }
        }

        public void Play(string name, Vector3 forward = default, Vector3 positionOffset = default)
        {
            foreach (ParticleEffect pe in particleEffect)
            {
                if (pe.name == name)
                {
                    pe.instance.transform.forward = forward;
                    pe.instance.transform.position = pe.parent.position + positionOffset;

                    foreach (ParticleSystem ps in pe.instance.GetComponentsInChildren<ParticleSystem>())
                    {
                        ps.Play();
                    }

                    break;
                }
            }
        }

        public void Stop(string name)
        {
            foreach (ParticleEffect pe in particleEffect)
            {
                if (pe.name == name)
                {
                    foreach (ParticleSystem ps in pe.instance.GetComponentsInChildren<ParticleSystem>())
                        ps.Stop();

                    break;
                }
            }
        }

        private void Update()
        {
            if (debugKeys)
            {
                if (Input.GetKeyDown(KeyCode.P))
                    Play(debugPlayName);

                if (Input.GetKeyDown(KeyCode.O))
                    Stop(debugPlayName);
            }
        }
    }
}

