using UnityEngine;

public class Gun : MonoBehaviour {

	public int damagePerShot = 10;
	public float timeBetweenBullets = 0.15f;
	public float range = 200f;
	public ParticleSystem muzzleFlash; 
	public GameObject impactEffect;
	public float recoil = 0.0f;
	public float maxRecoil_x = -20f;
	public float maxRecoil_y = 20f;
	public float recoilSpeed = 2f;

	public float impactForce = 300f;

	public Camera fpsCam;

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	AudioSource gunAudio;                           // Reference to the audio source.
	Light gunLight;                                 // Reference to the light component.
	float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.


	void Awake ()
	{
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Shootable");

		// Set up the references.
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		// If the Fire1 button is being press and it's time to fire...
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets)
		{
			Shoot ();
		}

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			DisableEffects ();
		}
		recoiling ();
		
	}

	public void DisableEffects ()
	{
		// Disable the light.
		gunLight.enabled = false;
	}

	void Shoot ()
	{
		// Reset the timer.
		timer = 0f;

		// Play the gun shot audioclip.
		gunAudio.Play ();

		// Enable the light.
		gunLight.enabled = true;

		// Stop the particles from playing if they were, then start the particles.
		muzzleFlash.Stop ();
		muzzleFlash.Play ();

		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			// Try and find an EnemyHealth script on the gameobject hit.
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}

			if (shootHit.rigidbody != null) {
				// TODO: Fix this
				shootHit.rigidbody.AddForce (-shootHit.normal * impactForce);
			}

			GameObject impactGameObject = Instantiate (impactEffect, shootHit.point, Quaternion.LookRotation(shootHit.normal));
			Destroy (impactGameObject, 2f);
		}

		// TESTING RECOIL]
		recoil += 30;
	}

	void recoiling ()
	{
	}
}
