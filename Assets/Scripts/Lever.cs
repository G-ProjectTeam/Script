using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool leverDown;
    public SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public AudioSource touch;
    public AudioSource clear;

    public IEnumerator TouchBullet(float bulletTime)
    {
        if (leverDown)
            yield break;

        leverDown = true;

        if (spriteRenderer != null && downSprite != null)
            spriteRenderer.sprite = downSprite;

        if (touch != null)
            touch.Play();

        // 대기 시간 처리 (기존 bulletTime 활용)
        yield return new WaitForSeconds(bulletTime);

        if (clear != null)
            clear.Play();
    }
}
