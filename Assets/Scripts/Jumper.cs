using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Player")]
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] List<AudioClip> clipList = new List<AudioClip>();
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            Jump();
            int index = collision.gameObject.GetComponent<Platform>().note.noteIndex;


            audioSource.PlayOneShot(clipList[index]);
        }
    }

    void Jump()
    {
        Vector2 Velocity = rigidBody.velocity;
        Velocity.y = jumpForce;
        rigidBody.velocity = Velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");

        Vector3 Direction = Vector3.zero;
        Direction.x = HorizontalAxis;

        Vector3 Translate = Direction * moveSpeed * Time.deltaTime;
        transform.Translate(Translate);
    }
}
