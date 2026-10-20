
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 9f;

    private bool finished = false;

    void Update()
    {
        if (finished)
            return;

        // 障礙物持續往飛機方向移動
        transform.position +=
            Vector3.back * speed * Time.deltaTime;

        // 障礙物飛過飛機之後消失
        if (transform.position.z < -2f)
        {
            finished = true;

            if (GameManager.Instance != null)
                GameManager.Instance.RegisterSuccess();

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (finished)
            return;

        if (other.CompareTag("Player"))
        {
            finished = true;

            if (GameManager.Instance != null)
                GameManager.Instance.RegisterHit();

            Destroy(gameObject);
        }
    }
}
