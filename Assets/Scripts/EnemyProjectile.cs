using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public void moveProjectile()
	{
		transform.DOMove(new Vector2(0, 0), 0.5f);
	}
}
