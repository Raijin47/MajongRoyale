using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MoveCell : MonoBehaviour
{
    public static MoveCell Instance;

    [SerializeField] private GameObject _illusion;
    [SerializeField] private Transform _content;

    private static readonly List<GameObject> _freeIllusion = new();
    private static readonly List<GameObject> _usedIllusion = new();

    public static Transform ParentTarget { get; set; }
    public UIParticleSystem particleSystem;

    public float speedMove = 10;
    public float timeScale = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            _freeIllusion.Add(Instantiate(_illusion, _content));
            _freeIllusion[i].SetActive(false);
        }
    }

    public static IEnumerator MoveToPositions(Transform item, Transform secondItem, Vector3[] positions, Sprite image)
    {
        GameObject first = GetIllusion(item, image);
        GameObject second = GetIllusion(secondItem, image);

        foreach (var target in positions)
        {
            while (Vector3.Distance(first.transform.position, target) > 0.01f)
            {
                first.transform.position = Vector3.MoveTowards(first.transform.position, target, Instance.speedMove * Time.deltaTime);
                yield return null;
            }

            first.transform.position = target;
        }

        Instance.particleSystem.transform.position = positions[^1];
        Instance.particleSystem.StartParticleEmission();
        Vector2 originalScale = item.localScale;
        Vector2 targetScale = Vector2.zero;
        float elapsedTime = 0f;

        AudioController.Instance.Play(2);

        while (elapsedTime < Instance.timeScale)
        {
            first.transform.localScale = Vector2.Lerp(originalScale, targetScale, elapsedTime);
            second.transform.localScale = Vector2.Lerp(originalScale, targetScale, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        first.transform.localScale = targetScale;
        second.transform.localScale = targetScale;

        Release(first);
        Release(second);
    }

    private static GameObject GetIllusion(Transform pos, Sprite image)
    {
        GameObject obj = _freeIllusion[^1];
        _freeIllusion.Remove(obj);
        _usedIllusion.Add(obj);
        obj.GetComponent<UnityEngine.UI.Image>().sprite = image;
        obj.transform.position = pos.position;
        obj.SetActive(true);

        return obj;
    }

    private static void Release(GameObject obj)
    {
        _usedIllusion.Remove(obj);
        _freeIllusion.Add(obj);
        obj.SetActive(false);

        obj.transform.localScale = Vector2.one;
    }
}