using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Runtime.ExceptionServices;
using System.Linq;

public class FieldOfView : MonoBehaviour {

	public int hp;
	public int ammo;
	public List<GameObject> lives = new List<GameObject>();
	public List<GameObject> ammos = new List<GameObject>();
	public List<GameObject> safeZones = new List<GameObject>();

	public GameObject randomWalkTarget;

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;
	public Animator animator;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public bool isShooting;
	public bool isSafe;

	public float currentDistance;


	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public int getHp()
    {
		return hp;
    }
	public int getAmmo()
	{
		return ammo;
	}
	public float getCurrentDistance()
	{
		return currentDistance;
	}
	public void setHp(int hp)
	{
		this.hp = hp;
	}
	public void setAmmo(int ammo)
	{
		this.ammo = ammo;
	}

	void Start() {
		StartCoroutine ("FindTargetsWithDelay", .2f);
		setHp(100);
		setAmmo(50);
		isSafe = true;
	}


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
			
				if (hp >= 2 && hp <= 10&&isSafe)
				{
					takeLive();
				}
				else if (isDie())
				{
					die();

				}
				//In Battle:
				else if (visibleTargets.Count != 0)
				{
					foreach (Transform visibleTarget in visibleTargets)
					{
						if (isDie())
						{
							isShooting = false;
							break;
						}
						else if (visibleTarget.GetComponent<FieldOfView>().isDie())
						{
							visibleTarget.GetComponent<Unit>().speed = 0;
							isShooting = false;
							isSafe = true;

						}
						else if (visibleTargets.Count==0)
						{
							isShooting = false;
							isSafe = true;

						}
						else if(getAmmo() <= 10 && isSafe)
						{
							isShooting = false;
							takeAmmo();
						}
						else
						{
							isShooting = true;

							currentDistance = Vector3.Distance(visibleTarget.transform.position, transform.position);
							GetComponent<Unit>().speed = 0;
							transform.rotation = Quaternion.LookRotation(visibleTarget.position - transform.position);
							GetComponent<PlayerWeapon>().Fire();
							setAmmo(GetComponent<FieldOfView>().getAmmo() - 1);
							takeSafeZone(visibleTarget);
							if (getCurrentDistance() <= 5 && getCurrentDistance() >= 0)
							{
								visibleTarget.GetComponent<FieldOfView>().setHp(visibleTarget.GetComponent<FieldOfView>().getHp() - 15);

							}
							else if (getCurrentDistance() <= 10 && getCurrentDistance() >= 6)
							{
								visibleTarget.GetComponent<FieldOfView>().setHp(visibleTarget.GetComponent<FieldOfView>().getHp() - 10);
	
							}
							else if (getCurrentDistance() <= 15 && getCurrentDistance() >= 11)
							{
							visibleTarget.GetComponent<FieldOfView>().setHp(visibleTarget.GetComponent<FieldOfView>().getHp() - 5);

							}
							else if (getCurrentDistance() <= 21 && getCurrentDistance() >= 16)
							{
								visibleTarget.GetComponent<FieldOfView>().setHp(visibleTarget.GetComponent<FieldOfView>().getHp() - 1);

							}
							else
							{
								visibleTarget.GetComponent<FieldOfView>().setHp(visibleTarget.GetComponent<FieldOfView>().getHp() - 5);

							}
						}
					}

				}
				else
				{
					if (visibleTargets.Count == 0&&isSafe)
					{
						isShooting = false;
						if (getAmmo() > 10)
						{
							animator.Play("WalkFront_Shoot_AR");
							GetComponent<Unit>().speed = 2;
							GetComponent<Unit>().target = randomWalkTarget.transform;

						}
						else 
						{
							takeAmmo();

						}

					}
					else if(visibleTargets.Count == 0 && !isSafe)
					{
					takeSafeZone(transform);
					}
				}
			
		}
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) 
		{
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) 
			{
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add (target);
					
				}
				
           
			}
		}

	}


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
	private void die()
	{
		GetComponent<Unit>().speed = 0;
		animator.Play("Die");
		Destroy(this.gameObject,1.5f);
		Destroy(this.randomWalkTarget);
	}
	private bool isDie()
	{
        if (hp <= 0)
        {
			return true;
        }
        else
        {
			return false;
        }
	}
	public void takeAmmo()
    {
		isShooting = false;
		animator.Play("Run_guard_AR");
		GetComponent<Unit>().speed = 5;

		GetComponent<FieldOfView>().lives.Sort(SortByDis);
		GetComponent<Unit>().target = lives[0].transform;
	}
	public void takeLive()
    {
		isShooting = false;
		animator.Play("Run_guard_AR");
		GetComponent<Unit>().speed = 5;

		GetComponent<FieldOfView>().ammos.Sort(SortByDis);
		GetComponent<Unit>().target = ammos[0].transform;

	}
	public void takeSafeZone(Transform visibleTarget)
	{
		visibleTarget.GetComponent<Animator>().Play("Run_guard_AR");

		visibleTarget.GetComponent<Unit>().speed = 5;
		visibleTarget.GetComponent<FieldOfView>().safeZones.Sort(SortByDis);

		visibleTarget.GetComponent<Unit>().target = visibleTarget.GetComponent<FieldOfView>().safeZones[0].transform;




	}

	public int SortByDis(GameObject safeZone1, GameObject safeZone2)
	{

		float num1 = Vector3.Distance(transform.position, safeZone1.transform.position);

		float num2 = Vector3.Distance(transform.position, safeZone2.transform.position);

		return num1.CompareTo(num2);
	}


}
