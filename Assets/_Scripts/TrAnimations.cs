using DG.Tweening;
using UnityEngine;

public class TrAnimations : MonoBehaviour
{

	public void PlaySpawnAnim(Transform newElementTransform)
	{
		newElementTransform.DOScale(1.1f, 0.3f).OnComplete(() =>
		{
			newElementTransform.DOScale(0.9f, 0.2f);
		});
	}
}


