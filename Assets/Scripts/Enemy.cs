using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    Rigidbody2D rig;
    Vector3 zAxis = new Vector3(0, 0, 1);

    public void SetAngleAndVelocity(float angle, float vel)
    {
        if(rig == null)
            rig = GetComponent<Rigidbody2D>();
        transform.Rotate(zAxis, angle * Mathf.Rad2Deg);
        rig.velocity = new Vector2(Mathf.Cos(angle) * vel, Mathf.Sin(angle) * vel);
    }

    void Update()
    {
        if (transform.position.x > Camera.main.orthographicSize)
            Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.gameObject.name.Equals("Player") || contact.otherCollider.gameObject.name.Equals("Player"))
            {
                //Debug.Log("Dead!");
                Destroy(gameObject);
                return;
            }
        }
    }
}
