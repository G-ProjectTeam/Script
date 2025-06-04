using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LightStick : MonoBehaviour
{
    public SpriteRenderer mainSprite;
    public PlayerMovement mainMovement;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletPoint;
    public int remainBullets = 99;
    public int maxBullets = 99;
    public AudioSource shoot;
    public float bulletSpeed = 5f; // Inspector에서 설정 가능

    private Vector3 defaultLocalPosition;

    private void Awake()
    {
        defaultLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (mainMovement == null)
        {
            Debug.LogError("mainMovement가 연결되지 않았습니다.");
            return;
        }

        if (mainMovement.cleared || mainMovement.dead)
            return;

        SpriteRenderer sr = mainMovement.GetComponent<SpriteRenderer>();
        bool isFlipped = sr != null && sr.flipX;

        Vector3 pos = defaultLocalPosition;
        pos.x *= isFlipped ? -1f : 1f;
        transform.localPosition = pos;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mouseWorld - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (maxBullets > 0 && remainBullets <= 0)
            return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (shoot != null) shoot.Play();

            if (bulletPrefab != null && bulletPoint != null)
            {
                Vector3 spawnPos = bulletPoint.position + transform.up * 0.6f + Vector3.up * 0.15f;
                GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, transform.rotation);

                // 💡 여기서 bulletSpeed를 명시적으로 전달
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                if (bullet != null)
                {
                    bullet.bulletSpeed = bulletSpeed;
                    Debug.Log("총알 속도: " + bullet.bulletSpeed);
                }

                Collider2D playerCollider = mainMovement.GetComponent<Collider2D>();
                Collider2D bulletCollider = bulletObj.GetComponent<Collider2D>();
                if (playerCollider != null && bulletCollider != null)
                {
                    Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                }

                remainBullets--;
            }
            else
            {
                Debug.LogWarning("bulletPrefab 또는 bulletPoint가 연결되지 않았습니다!");
            }
        }
    }
}
