using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    // Speed low to match neurorehab criteria
    //TODO: implement adaptive speed from patient progress
    public float speed          = 50f; // Normal Speed: 500f
    public float maxBounceAngle = 150f;
    public bool  inPlay;
    public Transform paddle;
    public Transform origin;
    public GameManager gm;

    private Rigidbody2D _rb;
    private AudioSource _audio;
    private static readonly Vector2 START_POS = new(0f, -2f);

    private void Awake()
    {
        _rb    = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    private void Start() => ResetBall();

    public void go() => transform.position = START_POS;

    public void ResetBall()
    {
        inPlay          = false;
        _rb.velocity    = Vector2.zero;
        transform.position = START_POS;

        CancelInvoke(nameof(LaunchRandom));
        Invoke(nameof(LaunchRandom), 1f);
    }

    private void LaunchRandom()
    {
        Vector2 dir = new(Random.Range(-1f, 1f), -1f);
        _rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
        inPlay = true;
    }

    private void Update()
    {
        if (!inPlay && Input.GetButtonDown("Jump"))
            LaunchRandom();
    }

    private void FixedUpdate()
    {
        if (inPlay && _rb.velocity.sqrMagnitude > 0.01f)
            _rb.velocity = _rb.velocity.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Brick"))
        {
            GameManager.Instance.UpdateScore(100);
            _audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("bottom")) return;

        Debug.Log("Ball hit the bottom – life lost");
        GameManager.Instance.UpdateLives(-1);
        ResetBall();
    }
}