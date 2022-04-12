using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the hand attack from the enemies.
/// </summary>
public class EnemyHitScript : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerStatsScript>().TakeDamage();
		}
	}
}
