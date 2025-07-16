using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Paddle : MonoBehaviour
{
    public float speed          = 30f;
    public float maxBounceAngle = 150f;

    private Rigidbody2D _rb;
    private AudioSource _audio;
    private Vector2     _dir;

    private void Awake()
    {
        _rb    = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    private void Start() => ResetPaddle();

    public void ResetPaddle()
    {
        _rb.velocity = Vector2.zero;
        transform.position = new Vector2(0f, transform.position.y);
    }

    private void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");         // cmd: A/D or arrows
        _dir = Mathf.Abs(axis) > 0.1f ? new Vector2(Mathf.Sign(axis), 0f) : Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (_dir != Vector2.zero)
            _rb.AddForce(_dir * speed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ball")) return;
        _audio.Play();

        Rigidbody2D ballRig   = col.rigidbody;
        Collider2D  paddleCol = col.otherCollider;

        Vector2 inDir  = ballRig.velocity.normalized;
        Vector2 offset = paddleCol.bounds.center - ballRig.transform.position;
        float   angle  = (offset.x / paddleCol.bounds.size.x) * maxBounceAngle;

        Vector2 outDir = Quaternion.AngleAxis(angle, Vector3.forward) * inDir;
        ballRig.velocity = outDir * ballRig.velocity.magnitude;
    }
}