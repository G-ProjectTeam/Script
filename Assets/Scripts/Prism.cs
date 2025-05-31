using System;
using UnityEngine;

// 유틸리티 클래스 정의
public static class ColorConverting
{
    public struct HSV
    {
        public float h;
        public float s;
        public float v;
    }

    public static Color ColorFromHSL(HSV hsv)
    {
        return Color.HSVToRGB(hsv.h / 360f, hsv.s, hsv.v);
    }
}

public class Prism : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        float num = 0f;
        for (int i = 0; i < this.counts; i++)
        {
            float f = (-this.maxAngle / 2f + num + this.previewAngleOffset + this.distort) * Mathf.Deg2Rad;
            Vector3 b = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
            Gizmos.color = this.GetColorAtAngle(num);
            Gizmos.DrawLine(transform.position, transform.position + b);
            num += this.maxAngle / (float)(this.counts - 1);
        }

        float f2 = this.previewAngleOffset * Mathf.Deg2Rad;
        Vector3 b2 = new Vector3(Mathf.Cos(f2), Mathf.Sin(f2), 0f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position - b2);

        f2 = (this.previewAngleOffset + this.distort) * Mathf.Deg2Rad;
        b2 = new Vector3(Mathf.Cos(f2), Mathf.Sin(f2), 0f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position - b2);
    }

    public Color GetColorAtAngle(float angle)
    {
        Color c = ColorConverting.ColorFromHSL(new ColorConverting.HSV
        {
            h = angle / this.maxAngle * 230f,
            s = 1f,
            v = 0.5f
        });

        return new Color(c.r, c.g, c.b); // UnityEngine.Color uses .r .g .b
    }

    public void Do(Bullet bullet)
    {
        UnityEngine.Object.Destroy(bullet.gameObject);
        sfx.Play();

        float baseAngle = bullet.transform.rotation.eulerAngles.z + 90f;
        Debug.Log("[AG] " + baseAngle);

        float num = 0f;
        for (int i = 0; i < counts; i++)
        {
            float angle = (-maxAngle / 2f + num + baseAngle + distort) * Mathf.Deg2Rad;
            Spawn(angle * Mathf.Rad2Deg, GetColorAtAngle(num));
            num += maxAngle / (float)(counts - 1);
        }
    }

    private void Spawn(float angle, Color color)
    {
        float f = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
        Bullet bullet = UnityEngine.Object.Instantiate(bulletPrefab, transform.position + direction * 0.05f, Quaternion.Euler(0f, 0f, angle - 90f));
        bullet.IsSpawnedByMirror = true;
        bullet.GetComponent<SpriteRenderer>().color = color;
    }

    public Bullet bulletPrefab;
    public float maxAngle;
    public int counts;
    public AudioSource sfx;
    public float distort;

    [Header("PREVIEW")]
    public float previewAngleOffset;
}
