using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    [NonSerialized]
    public bool IsSpawnedByMirror;

    [Header("Bullet")]
    [Tooltip("빛이 나아가는 속도")]
    public float bulletSpeed = 5f;
    public float mirrorCheckDistance = 0.36f;
    public Light2D bulletLight;

    [Header("Touch")]
    public Light2D touchLight;
    public Vector3 touchScale;
    public LayerMask wallMask;

    [Header("Wall Hit Effect")]
    public GameObject lightOnWallPrefab;

    private bool _touched;

    private void Awake()
    {
        bulletLight.gameObject.SetActive(true);
        touchLight.gameObject.SetActive(false);
    }

    public Vector3 GetForwardVector(float angle)
    {
        float f = (angle + 90f) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
    }

    private void Update()
    {
        if (!_touched)
        {
            transform.position += GetForwardVector(transform.rotation.eulerAngles.z) * bulletSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_touched) return;

        int layer = other.gameObject.layer;

        // Lever와 충돌
        if (layer == 10)
        {
            _touched = true;
            bulletLight.gameObject.SetActive(false);
            touchLight.gameObject.SetActive(true);
            transform.localScale = touchScale;

            Lever lever = other.GetComponent<Lever>();
            if (lever != null)
            {
                StartCoroutine(lever.TouchBullet(1f));
            }
        }

        if (IsSpawnedByMirror) return;

        // 벽과 충돌 → 빛 고정 Prefab 생성
        if ((wallMask.value & (1 << layer)) != 0)
        {
            _touched = true;

            // 빛 Prefab 생성
            if (lightOnWallPrefab != null)
            {
                Instantiate(lightOnWallPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("lightOnWallPrefab이 할당되지 않음!");
            }

            // 기존 Bullet 제거
            Destroy(gameObject);
        }

        // Mirror와 충돌
        if (layer == 12)
        {
            other.GetComponent<Mirror>()?.Reflect(this);
        }

        // Handle와 충돌
        if (layer == 13)
        {
            Handle handle = other.GetComponent<Handle>();
            if (handle != null)
            {
                StartCoroutine(handle.Rotate(1));
            }
        }

        // Spike와 충돌 → Destroy
        if (layer == 14)
        {
            Destroy(gameObject);
        }

        // Prism과 충돌
        if (layer == 15)
        {
            other.GetComponent<Prism>()?.Do(this);
        }
    }

    private void Start()
    {
        Debug.Log("총알 속도: " + bulletSpeed);

        if (IsSpawnedByMirror)
        {
            Invoke("NoMirror", 0.05f);
        }
    }

    private void NoMirror()
    {
        IsSpawnedByMirror = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetForwardVector(transform.rotation.eulerAngles.z));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * -mirrorCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * mirrorCheckDistance);
    }
}
