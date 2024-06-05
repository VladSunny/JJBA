/*
ShootOnClick.cs
This script is will shoot a projectile on click.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootOnClick : MonoBehaviour 
{

    /// <summary>
    /// The projectile.
    /// </summary>
	//public GameObject projectile;
 
    /// <summary>
    /// the force of the projectile
    /// </summary>
	//public float force = 500f;


    public LayerMask layerMask = -1;

    public int EmitCnt = 25;

	// Update is called once per frame
	void Update () 
	{
		//On Click
		if (Input.GetMouseButtonDown(0))
		{

		    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit, 1000f,layerMask))
            {
                hit.collider.gameObject.GetComponent<ParticleSystem>().Emit(EmitCnt);
            }
                
            
		}
	
	}
}
