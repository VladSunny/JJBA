/*
ParticleManipulator.cs
Used to manipulate particles that are with in a specific radius.
Color, Size, and Force can all be manipulated with this script.

*/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class ParticleManipulator : MonoBehaviour 
{
    
    [Header("Sphere Settings")]
    /// <summary>
    /// The radius in which the particles will be effected.
    /// </summary>
    public float radius = 10f;

    [Header("Color Settings")]
    /// <summary>
    /// The color settings for the particles (based on the distance from the center).
    /// </summary>
    public colorSettingsClass ColorSettings;

    [Header("Size Settings")]
    /// <summary>
    /// The size settings for the particles (based on distance from the center).
    /// </summary>
    public sizeSettingsClass SizeSettings;

    [Header("Movement Settings")]
    /// <summary>
    /// The gravity force settings.
    /// </summary>
    public gravityForceSettingsClass GravityForceSettings;
    /// <summary>
    /// The lerp to center settings.
    /// </summary>
    public lerpToCenterClass LerpToCenterSettings;

    [Space(10)]
    /// <summary>
    /// The destory particle at core.
    /// </summary>
    public bool destoryParticleAtCore;

    public float coreDistance = 0.1f;

    /// <summary>
    /// this will only manipulate particles that come from particle systems with this/these tag(s).
    /// </summary>
    public List<string> particleSystemTags = new List<string>();
    /// <summary>
    /// The particle systems.
    /// </summary>
    private ParticleSystem[] particleSystems;

    /// <summary>
    /// update our list of particle systems
    /// </summary>
    private void FixedUpdate()
    {
        particleSystems = null;
        int length = 0;

        ParticleSystem[] PSs = GameObject.FindObjectsOfType<ParticleSystem>();
        for(int i = 0; i < PSs.Length; i++)
        {
            if (particleSystemTags.Contains(PSs[i].tag))
            {
                length += 1;
                Array.Resize(ref particleSystems,length);
                particleSystems[length-1] = PSs[i];
            }
        }

    }

    /// <summary>
    /// Update all the particles
    /// </summary>
    void LateUpdate()
    {
        //loop through all particle systems

        if (particleSystems == null || particleSystems.Length == 0)
        {
            return;
        }

        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystems[i].particleCount + 1];
            int l = particleSystems[i].GetParticles(p);

            int j = 0;

            //for each particle
            while (j < l)
            {
                //get distance, distance ratio, and life time remaining
                float distance = Vector3.Distance(p[j].position, transform.position);
                float distanceRatio = (radius - distance) / radius;
                float lifeTimeRemaining = 1 - (p[j].remainingLifetime / p[j].startLifetime);

                //if particles are in the area
                if (inArea(p[j].position))
                {

                    //apply the colors settings
                    if (ColorSettings.enable)
                    {
                        if (ColorSettings.lifeTimeFilter.enable)
                        {
                            float minLTF = (ColorSettings.lifeTimeFilter.enable) ? Mathf.Clamp(ColorSettings.lifeTimeFilter.min, 0f, 1f) : 0f;
                            float maxLTF = (ColorSettings.lifeTimeFilter.enable) ? Mathf.Clamp(ColorSettings.lifeTimeFilter.max, 0f, 1f) : 1f;

                            if (minLTF <= lifeTimeRemaining && lifeTimeRemaining <= maxLTF )
                            {
                                p[j].startColor = ColorSettings.gradient.Evaluate(distanceRatio);
                            }
                        }
                        else
                        {
                            p[j].startColor = ColorSettings.gradient.Evaluate(distanceRatio);
                        }
                    }

                    //apply the size settings
                    if (SizeSettings.enable)
                    {
                        if (SizeSettings.lifeTimeFilter.enable)
                        {
                            float minLTF = (SizeSettings.lifeTimeFilter.enable) ? Mathf.Clamp(SizeSettings.lifeTimeFilter.min, 0f, 1f) : 0f;
                            float maxLTF = (SizeSettings.lifeTimeFilter.enable) ? Mathf.Clamp(SizeSettings.lifeTimeFilter.max, 0f, 1f) : 1f;

                            if (minLTF <= lifeTimeRemaining && lifeTimeRemaining <= maxLTF)
                            {
                                p[j].startSize = SizeSettings.distanceFromCoreSize.Evaluate(distanceRatio);
                            }
                        }
                        else
                        {
                            p[j].startSize = SizeSettings.distanceFromCoreSize.Evaluate(distanceRatio);
                        }
                    }


                    //destory the particles if at the core
                    if (destoryParticleAtCore)
                    {
                        //if (distanceRatio > 0.999f)
                        //{
                        //    p[j].remainingLifetime = 0;
                        //}

                        if ( distance <= coreDistance )
                        {
                            p[j].remainingLifetime = 0;
                        }
                        else
                        {
                            p[j].remainingLifetime = 1;
                        }

                    }


                    //calculate force direction
                    Vector3 forceDirection = (p[j].position - transform.position).normalized;

                    //apply gravity settings
                    if (GravityForceSettings.enable)
                    {
                        if (GravityForceSettings.lifeTimeFilter.enable)
                        {
                            float minLTF = (GravityForceSettings.lifeTimeFilter.enable) ? Mathf.Clamp(GravityForceSettings.lifeTimeFilter.min, 0f, 1f) : 0f;
                            float maxLTF = (GravityForceSettings.lifeTimeFilter.enable) ? Mathf.Clamp(GravityForceSettings.lifeTimeFilter.max, 0f, 1f) : 1f;

                            if (minLTF <= lifeTimeRemaining && lifeTimeRemaining <= maxLTF)
                            {
                                p[j].velocity += forceDirection * (GravityForceSettings.Force / 9.8f) * -1f;
                            }
                        }
                        else
                        {
                            p[j].velocity += forceDirection * (GravityForceSettings.Force / 9.8f) * -1f;
                        }


                    }

                    // apply Lerp To Center Settings --this overrides gravity
                    if (LerpToCenterSettings.enable)
                    {
                        if (LerpToCenterSettings.lifeTimeFilter.enable)
                        {
                            float minLTF = (LerpToCenterSettings.lifeTimeFilter.enable) ? Mathf.Clamp(LerpToCenterSettings.lifeTimeFilter.min, 0f, 1f) : 0f;
                            float maxLTF = (LerpToCenterSettings.lifeTimeFilter.enable) ? Mathf.Clamp(LerpToCenterSettings.lifeTimeFilter.max, 0f, 1f) : 1f;

                            if (minLTF <= lifeTimeRemaining && lifeTimeRemaining <= maxLTF)
                            {
                                p[j].velocity = Vector3.Lerp(p[j].velocity, forceDirection * LerpToCenterSettings.Force * -1f, distanceRatio);
                            }
                        }
                        else
                        {
                            p[j].velocity = Vector3.Lerp(p[j].velocity, forceDirection * LerpToCenterSettings.Force * -1f, distanceRatio);
                        }


                    }


                }

                j++;
            }

            particleSystems[i].SetParticles(p, l);
        }

    }

    //id particle in Area function
    private bool inArea(Vector3 position)
    {
        bool result = false;

        if (Vector3.Distance(position, transform.position) < radius)
        {
            result = true;
        }

        //if (thisCollider.bounds.Contains(position))
        //{
        //    result = true;
        //}

        //if (areaType == areaTypes.sphere)
        //{
        //    if (Vector3.Distance(position, transform.position) < radius)
        //    {
        //        result = true;
        //    }
        //}
        //else if (areaType == areaTypes.box)
        //{
        //}

        return result;
    }

    //stuff i might use for an update
    /*
    private Vector2 GetXYDirection(float angle, float magnitude)
    {
        angle *= -1f;
        angle -= 90f;
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * magnitude;
    }

    private float GetAngleDirection(Vector2 point1, Vector2 point2)
    {
        Vector2 v = point1 - point2;

        return (float)Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
    }
    */


    //void OnDrawGizmosSelected()
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, radius);

    }

}



/*
SETTING CLASSES AND STUFF!
*/


[System.Serializable]
public class colorSettingsClass
{
    public bool enable;
    public Gradient gradient;
    public LifeTimeFilter lifeTimeFilter;
}

[System.Serializable]
public class sizeSettingsClass
{
    public bool enable;
    public AnimationCurve distanceFromCoreSize;
    public LifeTimeFilter lifeTimeFilter;
}

[System.Serializable]
public class gravityForceSettingsClass
{
    public bool enable;
    public float Force;
    public LifeTimeFilter lifeTimeFilter;
}

[System.Serializable]
public class lerpToCenterClass
{
    public bool enable;
    public float Force;
    public LifeTimeFilter lifeTimeFilter;
}


[System.Serializable]
public class LifeTimeFilter
{
    public bool enable;
    [Range(0f, 1f)]
    public float min = 0f;
    [Range(0f, 1f)]
    public float max = 1f;
}