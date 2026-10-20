
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 障礙物
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.5f;
    public float spawnZ = 30f;

    // 計數文字
    public TextMeshProUGUI scoreText;

    // 三種音效
    public AudioClip flyingSound;
    public AudioClip collisionSound;
    public AudioClip successSound;

    private AudioSource audioSource;

    private int hitCount = 0;
    private int successCount = 0;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        UpdateScore();

        // 持續播放飛行音效
        if (flyingSound != null)
        {
            audioSource.clip = flyingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        // 持續產生障礙物
        InvokeRepeating(
            nameof(SpawnObstacle),
            1f,
            spawnInterval
        );
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
            return;

        float x = Random.Range(-3.2f, 3.2f);
        float y = Random.Range(-2f, 2f);

        Vector3 position = new Vector3(x, y, spawnZ);

        Instantiate(
            obstaclePrefab,
            position,
            Quaternion.identity
        );
    }

    // 撞到障礙物
    public void RegisterHit()
    {
        hitCount++;

        if (collisionSound != null)
            audioSource.PlayOneShot(collisionSound);

        UpdateScore();
    }

    // 成功通過障礙物
    public void RegisterSuccess()
    {
        successCount++;

        if (successSound != null)
            audioSource.PlayOneShot(successSound);

        UpdateScore();
    }

    // 更新顯示數字
    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text =
                "Hits: " + hitCount +
                "\nPassed: " + successCount;
        }
    }
}
