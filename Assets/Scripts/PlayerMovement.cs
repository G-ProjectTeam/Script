using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Transform _door;
    public static PlayerMovement Instance;

    public Rigidbody2D rigid;
    public Animator animator;
    public bool cleared;
    public bool dead;
    public Collider2D floorCollider;
    public List<GameObject> disableWhenDead;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float raycastCheck = 1f;
    public float jumpPower = 1f;
    public LayerMask floorLayer;

    [Header("Audio")]
    public AudioSource footstep;
    public float footstepInterval = 0.2f;
    private float _footstepCountdown;
    public AudioSource footstepClear;
    public AudioSource hurt;

    private void Awake()
    {
        Instance = this;

        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (footstep == null)
            footstep = GetComponent<AudioSource>();

        if (footstepClear == null)
            footstepClear = GetComponent<AudioSource>();

        if (hurt == null)
            hurt = GetComponent<AudioSource>();
    }

    public void Clear(Transform door)
    {
        if (cleared)
            return;

        _door = door;
        if (footstepClear != null)
            footstepClear.Play();

        cleared = true;
        animator.SetTrigger("Clear");
        transform.position = door.position - new Vector3(0f, 0.998f, 0f);

        foreach (GameObject obj in disableWhenDead)
            obj.SetActive(false);
    }

    public IEnumerator Kill()
    {
        if (dead) yield break;

        dead = true;
        animator.SetTrigger("Dead");

        if (hurt != null)
            hurt.Play();

        yield return new WaitForSeconds(1f);

        TransitionManager.Transition(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        if (cleared || dead)
        {
            animator.SetBool("IsWalking", false);
            return;
        }

        float axisRaw = Input.GetAxisRaw("Horizontal");

        // ▶ 부드러운 속도 이동 방식
        rigid.linearVelocity = new Vector2(axisRaw * moveSpeed, rigid.linearVelocity.y);

        animator.SetBool("IsWalking", Mathf.Abs(axisRaw) > 0f);

        // 👉 캐릭터 방향 설정 (오른쪽 = true)
        if (axisRaw != 0f)
        {
            GetComponent<SpriteRenderer>().flipX = axisRaw > 0f;
        }
    }

    private void Jump()
    {
        rigid.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TransitionManager.Transition(SceneManager.GetActiveScene().name);
        }

        if (cleared)
        {
            transform.position = _door.position - new Vector3(0f, 0.998f, 0f);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastCheck, floorLayer);

        if (!cleared && !dead)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f && hit)
            {
                _footstepCountdown -= Time.deltaTime;
                if (_footstepCountdown <= 0f)
                {
                    _footstepCountdown = footstepInterval;
                    if (footstep != null)
                        footstep.Play();
                }
            }
            else
            {
                _footstepCountdown = 0f;
            }
        }

        if (!cleared && !dead && Input.GetButtonDown("Jump") && hit)
        {
            Jump();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, raycastCheck, 0f));
    }
}
