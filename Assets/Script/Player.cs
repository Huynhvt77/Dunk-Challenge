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
    bool touchUp = false;
    bool touchDown = false;

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
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
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
        rb.velocity = Vector3.zero;
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.checkUpPos && touchDown == false)
        {
            touchUp = true;
        }
        if (touchUp && collision.gameObject == GameManager.Instance.checkDownPos)
        {
            touchDown = true;
            GameManager.Instance.win = true;
            HandleWin();
        }
    }

    void HandleWin()
    {
        touchUp = false;
        touchDown = false;
        GameManager.Instance.HandleWin();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.checkUpPos && touchDown == false)
        {
            touchUp = false;
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
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        rb.constraints = RigidbodyConstraints2D.None;
        transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
