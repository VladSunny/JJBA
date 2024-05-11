using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JJBA.VFX
{
    public class ParticleManager : MonoBehaviour
    {
        public ParticleEffect[] particleEffect;

        private void Awake()
        {
            foreach (ParticleEffect pe in particleEffect)
            {
                GameObject instance = Instantiate(pe.particleEffect, pe.parent);
                pe.instance = instance;
            }
        }

        public void Play(string name)
        {
            foreach (ParticleEffect pe in particleEffect)
            {
                if (pe.name == name)
                {
                    foreach (ParticleSystem ps in pe.instance.GetComponentsInChildren<ParticleSystem>())
                        ps.Play();

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
            if (Input.GetKeyDown(KeyCode.P))
                Play("Burst");

            if (Input.GetKeyDown(KeyCode.O))
                Stop("Burst");
        }
    }
}

