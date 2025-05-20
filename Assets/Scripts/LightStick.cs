using System;
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
    public int remainBullets;
    public int maxBullets;
    public List<SpriteRenderer> remainBulletSprites;
    public Sprite noBulletSprite;
    public Sprite yesBulletSprite;
    public AudioSource shoot;

    private void Update()
    {
        if (mainMovement.cleared || mainMovement.dead)
        {
            if (mainMovement.cleared)
            {
                mainSprite.flipX = false;
            }
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mouseWorld.y - transform.position.y, mouseWorld.x - transform.position.x);
        float angleDeg = angleRad * Mathf.Rad2Deg - 90f;

        mainSprite.flipX = (angleDeg > 0f || angleDeg < -180f);
        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        for (int i = remainBulletSprites.Count; i >= 1; i--)
        {
            remainBulletSprites[i - 1].sprite = (remainBullets < i ? noBulletSprite : yesBulletSprite);
        }

        if (maxBullets > 0 && remainBullets <= 0)
        {
            return;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            shoot.Play();
            UnityEngine.Object.Instantiate(bulletPrefab, bulletPoint.position, transform.rotation); // ✅ 수정된 부분
            remainBullets--;
        }
    }
}
