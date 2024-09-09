using System.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MoveCell : MonoBehaviour
{

    public static MoveCell Instance;

    public UIParticleSystem particleSystem;

    public float speedMove = 10;
    public float timeScale = 1;

    private void Awake()
    {
        Instance = this;
    }

    //public static void StartMoving(Transform item, Vector3[] positions)
    //{
    //    Instance.StartCoroutine(MoveToPositions(item, positions));
    //}

    public static IEnumerator MoveToPositions(Transform item, Transform secondItem, Vector3[] positions)
    {
        item.SetParent(item.root);
        secondItem.SetParent(secondItem.root);

        foreach (var target in positions)
        {
            while (Vector3.Distance(item.position, target) > 0.01f)
            {

                item.position = Vector3.MoveTowards(item.position, target, Instance.speedMove * Time.deltaTime);
                yield return null;
            }

            item.position = target;
        }

        Instance.particleSystem.transform.position = positions[^1];
        Instance.particleSystem.StartParticleEmission();
        Vector3 originalScale = item.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < Instance.timeScale)
        {
            item.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime);
            secondItem.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.localScale = targetScale;
        secondItem.localScale = targetScale;
    }
}
