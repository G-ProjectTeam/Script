using System;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public GameObject bulletPrefab;
    public LayerMask mirrorMask;
    public Transform centerPos;
    public float checkStep = 0.05f;
    public AudioSource reflectSfx;

    public void Reflect(Bullet bullet)
    {
        UnityEngine.Object.Destroy(bullet.gameObject);
        reflectSfx.Play();

        float num = bullet.mirrorCheckDistance;
        float num2 = 0f;
        float z = bullet.transform.rotation.eulerAngles.z;

        for (;;)
        {
            RaycastHit2D leftHit = Physics2D.Raycast(bullet.transform.position - bullet.GetForwardVector(z) * num2, bullet.transform.right * -1f, num, mirrorMask);
            RaycastHit2D rightHit = Physics2D.Raycast(bullet.transform.position - bullet.GetForwardVector(z) * num2, bullet.transform.right, num, mirrorMask);

            Debug.Log($"{leftHit.collider != null} / {rightHit.collider != null}");

            if (leftHit.collider != null && rightHit.collider == null)
                break;

            if (leftHit.collider == null && rightHit.collider != null)
                goto ReflectRight;

            if (leftHit.collider == null && rightHit.collider == null)
                goto ReflectBack;

            Debug.LogWarning("Unexpected raycast state");
            num -= checkStep;
            num2 += checkStep;

            if (num <= 0f)
                goto ReflectFail;
        }

        ReflectSpwn(bullet, GetFrontDeg() * 2f - bullet.transform.rotation.eulerAngles.z);
        return;

    ReflectRight:
        ReflectSpwn(bullet, GetFrontDeg() * -2f - bullet.transform.rotation.eulerAngles.z);
        return;

    ReflectBack:
        ReflectSpwn(bullet, -bullet.transform.rotation.eulerAngles.z);
        return;

    ReflectFail:
        ReflectSpwn(bullet, -bullet.transform.rotation.eulerAngles.z);
    }

    public float GetFrontDeg()
    {
        return transform.rotation.eulerAngles.z - 90f;
    }

    public Vector3 GetFrontVector()
    {
        float rad = GetFrontDeg() * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        if (centerPos != null)
        {
            position = centerPos.position;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(position, position + GetFrontVector());
    }

    private void ReflectSpwn(Bullet bullet, float angle)
    {
        Debug.Log($"{bullet.transform.rotation.eulerAngles.z} / {angle}");

        GameObject spawned = UnityEngine.Object.Instantiate(bulletPrefab, bullet.transform.position, Quaternion.Euler(0f, 0f, angle));
        Bullet newBullet = spawned.GetComponent<Bullet>();
        newBullet.IsSpawnedByMirror = true;
        newBullet.GetComponent<SpriteRenderer>().color = bullet.GetComponent<SpriteRenderer>().color;
    }
}
