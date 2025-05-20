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
    public float scaleSpeed = 1f;
    public float bulletLightIntensitySpeed;
    public Light2D bulletLight;

    [Header("Touch")]
    public Light2D touchLight;
    public float touchScaleSpeed;
    public float touchLightIntensitySpeed;
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
            bulletLight.intensity -= bulletLightIntensitySpeed * Time.deltaTime;
            transform.position += GetForwardVector(transform.rotation.eulerAngles.z) * bulletSpeed * Time.deltaTime;
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
        }
        else
        {
            touchLight.intensity -= touchLightIntensitySpeed * Time.deltaTime;
            transform.localScale -= new Vector3(touchScaleSpeed, touchScaleSpeed, touchScaleSpeed) * Time.deltaTime;
        }

        if (transform.localScale.x <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            _touched = true;
            bulletLight.gameObject.SetActive(false);
            touchLight.gameObject.SetActive(true);
            transform.localScale = touchScale;

            Lever lever = other.gameObject.GetComponent<Lever>();
            if (lever != null)
            {
                StartCoroutine(lever.TouchBullet(transform.localScale.x / touchScaleSpeed));
            }
        }

        if (IsSpawnedByMirror)
        {
            return;
        }

        Debug.Log("Something triggered: " + other.gameObject.name);

        if ((wallMask.value & (1 << other.gameObject.layer)) != 0)
        {
            _touched = true;
            bulletLight.gameObject.SetActive(false);
            touchLight.gameObject.SetActive(true);
            transform.localScale = touchScale;
        }

        if (other.gameObject.layer == 12)
        {
            Debug.Log("Mirror Reflect");
            other.gameObject.GetComponent<Mirror>()?.Reflect(this);
        }

        if (other.gameObject.layer == 13)
        {
            Debug.Log("Handle Interact");
            Handle handle = other.gameObject.GetComponent<Handle>();
            if (handle != null)
            {
                StartCoroutine(handle.Rotate(1));
            }
        }

        if (other.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == 15)
        {
            Debug.Log("Prism Interact");
            other.gameObject.GetComponent<Prism>()?.Do(this);
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
