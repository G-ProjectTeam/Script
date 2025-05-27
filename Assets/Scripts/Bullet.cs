using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    [NonSerialized]
    public bool IsSpawnedByMirror;

    [Header("Bullet")]
    public float mirrorCheckDistance = 0.36f;
    public float bulletSpeed = 1f;
    public Light2D bulletLight;

    [Header("Touch")]
    public Light2D touchLight;
    public Vector3 touchScale;
    public LayerMask wallMask;

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
        // 닿은 후에는 멈추고 빛만 유지됨 (아무 처리 없음)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_touched) return;

        int layer = other.gameObject.layer;

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

        if (IsSpawnedByMirror)
        {
            return;
        }

        if ((wallMask.value & (1 << layer)) != 0)
        {
            _touched = true;
            bulletLight.gameObject.SetActive(false);
            touchLight.gameObject.SetActive(true);
            transform.localScale = touchScale;
        }

        if (layer == 12)
        {
            other.GetComponent<Mirror>()?.Reflect(this);
        }

        if (layer == 13)
        {
            Handle handle = other.GetComponent<Handle>();
            if (handle != null)
            {
                StartCoroutine(handle.Rotate(1));
            }
        }

        if (layer == 14)
        {
            Destroy(gameObject);
        }

        if (layer == 15)
        {
            other.GetComponent<Prism>()?.Do(this);
        }
    }

    private void Start()
    {
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
