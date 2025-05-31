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

    private Vector3 defaultLocalPosition;

    private void Awake()
    {
        defaultLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // mainMovement 연결 안 되어 있으면 오류 출력
        if (mainMovement == null)
        {
            Debug.LogError("mainMovement가 연결되지 않았습니다.");
            return;
        }

        // PlayerMovement의 필드가 접근 가능해야 함
        if (mainMovement.cleared || mainMovement.dead)
            return;

        // 방향 반전
        SpriteRenderer sr = mainMovement.GetComponent<SpriteRenderer>();
        bool isFlipped = sr != null && sr.flipX;

        Vector3 pos = defaultLocalPosition;
        pos.x *= isFlipped ? -1f : 1f;
        transform.localPosition = pos;

        // 마우스를 따라 회전
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
                Instantiate(bulletPrefab, bulletPoint.position, transform.rotation);
                remainBullets--;
            }
            else
            {
                Debug.LogWarning("bulletPrefab 또는 bulletPoint가 연결되지 않았습니다!");
            }
        }
    }
}
