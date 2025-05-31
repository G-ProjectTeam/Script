using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance;

    private SpriteRenderer _spriteRenderer;

    public string nextStage = "Lv_01";
    public Sprite doorOpenSprite;

    [Header("LOCK")]
    public bool locked;
    public GameObject lockObject;
    public List<Lever> levers;

    private bool _unlocked;
    private bool _going;

    private void Awake()
    {
        Instance = this;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        lockObject.SetActive(locked);
        if (!locked)
            _unlocked = true;
    }

    private void Update()
    {
        if (locked)
        {
            _unlocked = true;
            foreach (Lever lever in levers)
            {
                if (!lever.leverDown)
                {
                    _unlocked = false;
                }
            }
            lockObject.SetActive(!_unlocked);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_going)
            return;

        if (locked && !_unlocked)
            return;

        if (other.gameObject.layer == 3)
        {
            _going = true;
            StartCoroutine(Clear(other)); // 변경된 코루틴 호출
        }
    }

    public void NextStage()
    {
        TransitionManager.Transition(nextStage);
    }

    private IEnumerator Clear(Collider2D other)
    {
        // 플레이어가 Door에 닿았을 때 Clear 처리
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Clear(transform);
        }

        if (_spriteRenderer != null && doorOpenSprite != null)
        {
            _spriteRenderer.sprite = doorOpenSprite;
        }

        yield return new WaitForSeconds(1.2f); // 원래 UniTask.Delay로 대기했던 시간

        NextStage();
    }
}
