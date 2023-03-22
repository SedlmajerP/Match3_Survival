using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrAnimations : MonoBehaviour
{
    [SerializeField] private PlayerBoardManager pBoardManager;

    public void PlayDestroyAnim()
    {
        for (int x = 0; x < pBoardManager.width; x++)
        {
            for (int y = 0; y < pBoardManager.height; y++)
            {
                if (pBoardManager.allElementsArray[x, y].GetComponent<PlayerControls>().isMatched)
                {
                    Transform elementTranform = pBoardManager.allElementsArray[x,y].GetComponent<Transform>();
                    if (elementTranform != null)
                    {
                        elementTranform.DOScale(1, 0.2f).OnComplete(() =>
                        {
                            elementTranform.DOScale(0, 0.2f);
                        });
                    }
                }
            }

        }
    }

    public void PlayRefillAnim(Transform newElementTransform)
    {
		newElementTransform.DOScale(1, 0.3f).OnComplete(() =>
        {
			newElementTransform.DOScale(0.8f, 0.2f);
        });
    }

    
}
