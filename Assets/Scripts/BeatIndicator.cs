using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform rightSpawner, leftSpawner;
    [SerializeField] private GameObject objToSpawn;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float destroyDistance = 10f;

    private Camera uiCamera;
    private Transform player;
    private Canvas canvas;
    private RythmManager rythmManager;
    private bool hasSpawnedThisBeat = false;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.Player != null && RythmManager.Instance != null);

        rythmManager = RythmManager.Instance;
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        player = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        if (rythmManager.IsOnBeat())
        {
            if (!hasSpawnedThisBeat)
            {
                Vector3 targetPos = WorldToUIPosition(player.position);
                SpawnUIObject(leftSpawner, targetPos);
                SpawnUIObject(rightSpawner, targetPos);
                hasSpawnedThisBeat = true;
            }
        }
        else
        {
            hasSpawnedThisBeat = false;
        }
    }

    private void SpawnUIObject(RectTransform spawnPoint, Vector3 targetPosition)
    {
        GameObject obj = Instantiate(objToSpawn, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        StartCoroutine(MoveUIAndDestroy(obj.GetComponent<RectTransform>(), targetPosition));
    }

    private IEnumerator MoveUIAndDestroy(RectTransform rectTransform, Vector3 target)
    {
        while (Vector3.Distance(rectTransform.position, target) > destroyDistance)
        {
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(rectTransform.gameObject);
    }

    private Vector3 WorldToUIPosition(Vector3 worldPos)
    {
        // Convert world position to screen point, then to UI (canvas) local position
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 uiPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPoint,
            uiCamera,
            out uiPosition
        );
        return uiPosition;
    }
}
