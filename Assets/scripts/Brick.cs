using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Brick : MonoBehaviour
{
    public Sprite[] states = new Sprite[0];
    public int      points = 100;
    public bool     unbreakable;

    private SpriteRenderer _sr;
    private int            _health;

    private void Awake() => _sr = GetComponent<SpriteRenderer>();
    private void Start()  => ResetBrick();

    public void ResetBrick()
    {
        gameObject.SetActive(true);

        if (unbreakable) return;

        _health   = Mathf.Max(1, states.Length);
        if (states.Length > 0)
            _sr.sprite = states[_health - 1];
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ball") || unbreakable) return;

        _health--;

        if (_health <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (states.Length > 0)
        {
            _sr.sprite = states[Mathf.Clamp(_health - 1, 0, states.Length - 1)];
        }

        GameManager.Instance.UpdateScore(points);
        GameManager.Instance.OnBrickHit(this);
    }
}