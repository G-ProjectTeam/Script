using System.Collections;
using UnityEngine;

public class Handle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _hovered;
    private bool _rotating;

    public Sprite nonHover;
    public Sprite hover;
    public GameObject linked;
    public AudioSource sfx;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Rotate(int axis)
    {
        if (_rotating)
            yield break;

        _rotating = true;

        if (linked != null)
        {
            Vector3 rot = linked.transform.eulerAngles;
            rot.z += 90 * axis;
            linked.transform.eulerAngles = rot;
        }

        if (sfx != null)
            sfx.Play();

        yield return new WaitForSeconds(0.2f); // 기존 UniTask.Delay 유사 시간

        _rotating = false;
    }

    private void Update()
    {
        if (_hovered && Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0f)
        {
            int direction = Input.GetAxis("Mouse ScrollWheel") < 0f ? -1 : 1;
            StartCoroutine(this.Rotate(direction)); // 코루틴 호출로 변경
        }
    }

    private void OnMouseEnter()
    {
        _hovered = true;
        if (_spriteRenderer != null && hover != null)
            _spriteRenderer.sprite = hover;
    }

    private void OnMouseExit()
    {
        _hovered = false;
        if (_spriteRenderer != null && nonHover != null)
            _spriteRenderer.sprite = nonHover;
    }
}
