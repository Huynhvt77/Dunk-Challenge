using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform particle;
    Vector3 originalPos;

    bool leftRightForce = true;
    public float forceMagnitude = 10f;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.isPause)
        {
            HandleForce();
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    void HandleForce()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ApplyForce();
            }
        }
    }

    void ApplyForce()
    {
        float goc = leftRightForce ? 15f : -15f;
        float angleInRadians = goc * Mathf.Deg2Rad;
        Vector2 forceDirection = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

        leftRightForce = !leftRightForce;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            StartCoroutine(HandleEnemyCollision());
        }
    }

    IEnumerator HandleEnemyCollision()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        particle.gameObject.SetActive(true);
        ParticleSystem particleSystem = particle.gameObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            yield return new WaitForSeconds(0.5f);
            particle.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Particle System not found!");
        }

        GameManager.Instance.SetActiveLosePanel();
    }

    public void Reset()
    {
        transform.position = originalPos;
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
