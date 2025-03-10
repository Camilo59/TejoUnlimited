using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Cannon Rotation
    private Vector2 CannonPosition;
    private Vector2 MousePosition;
    private Vector2 direction;

    // Bullet Pooling
    public GameObject CannonBallPrefab;
    public int PoolSize = 10;
    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    public float FireForce;
    public Transform FirePoint;

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        CannonRotate();
    }

    void CannonRotate()
    {
        CannonPosition = transform.position;
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = MousePosition - CannonPosition;
        transform.right = direction;
    }

    void Fire()
    {
        GameObject _CannonBall = GetPooledBullet();
        _CannonBall.transform.position = FirePoint.position;
        _CannonBall.transform.rotation = FirePoint.rotation;
        _CannonBall.SetActive(true);

        Rigidbody2D rb = _CannonBall.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero; // Reset velocity before firing
        rb.angularVelocity = 0f; // Reset rotation speed
        rb.AddForce(transform.right * FireForce, ForceMode2D.Impulse);

        StartCoroutine(DeactivateBullet(_CannonBall, 5f)); // Disable after 5 seconds
    }

    void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bullet = Instantiate(CannonBallPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    GameObject GetPooledBullet()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }
        else
        {
            // If pool is empty, create a new bullet (optional, depending on your needs)
            GameObject newBullet = Instantiate(CannonBallPrefab);
            return newBullet;
        }
    }

    IEnumerator DeactivateBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
