using UnityEngine;
public class Ball : MonoBehaviour
{
    private float speed = 10;

    Rigidbody2D _rigidbody2D = null;
    public Rigidbody2D RigidBody
    {
        get { return _rigidbody2D ?? (_rigidbody2D = gameObject.GetComponent<Rigidbody2D>()); }
    }

    SpriteRenderer _renderer = null;
    public SpriteRenderer Renderer
    {
        get { return _renderer ?? (_renderer = gameObject.GetComponent<SpriteRenderer>()); }
    }

    float sprite_w;
    float sprite_h;
    Vector2 screen_max;
    Vector2 screen_min;

    void Start()
    {
        sprite_w = Renderer.bounds.size.x;
        sprite_h = Renderer.bounds.size.y;
        screen_max = Camera.main.ViewportToWorldPoint(Vector2.one);
        screen_min = Camera.main.ViewportToWorldPoint(Vector2.zero);

        /* float direction = Random.Range(0, 359); */
        float direction = 60;
        Vector2 v;
        v.x = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
        v.y = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;
        RigidBody.velocity = v;
    }

    void Update()
    {
        Vector2 position = transform.position;
        Vector2 v = RigidBody.velocity;
        if (position.x < screen_min.x + sprite_w / 2 && v.x < 0)
        {
            v.x *= -1;
        }
        else if (position.x > screen_max.x - sprite_w / 2 && v.x > 0)
        {
            v.x *= -1;
        }
        if (position.y < screen_min.y + sprite_h / 2 && v.y < 0)
        {
            v.y *= -1;
        }
        else if (position.y > screen_max.y - sprite_h / 2 && v.y > 0)
        {
            v.y *= -1;
        }
        RigidBody.velocity = v;
    }

    public void OnMouseDown()
    {
        Vector2 position = transform.position;
        for (int i = 0; i < 30; i++)
        {
            Particle.add(position.x, position.y);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Block")
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector3 relative_point = transform.InverseTransformPoint(contact.point);
            Vector2 v = RigidBody.velocity;
            if (relative_point.x > 0 && v.x > 0)
            {
                v.x *= -1;
            }
            else if (relative_point.x < 0 && v.x < 0)
            {
                v.x *= -1;
            }
            if (relative_point.y > 0 && v.y > 0)
            {
                v.y *= -1;
            }
            else if (relative_point.y < 0 && v.y < 0)
            {
                v.y *= -1;
            }
            RigidBody.velocity = v;
        }
    }
}
