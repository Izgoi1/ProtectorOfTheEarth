using UnityEngine;


public class ShipController : MonoBehaviour
{
    public Bullet bulletPrefab;
    public GameObject gameOverScreen;

    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioScript audioScript;

    private bool bulletActive;

    private void Start()
    {
        audioScript.startRaundSound.Play();
        audioScript.invaderSoundAttack.Play();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D)) 
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (transform.position.x < -10)
        {
            transform.position = new Vector3(transform.position.x + 20f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 10) 
        {
            transform.position = new Vector3(transform.position.x - 20f, transform.position.y, transform.position.z);
        }

    }


    private void Shoot()
    {
        if (!bulletActive)
        {
            Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, Quaternion.identity);
            bullet.destroyed += bulletDestroyed;
            bulletActive = true;
            audioScript.shootSound.Play();
        }
    }

    private void bulletDestroyed()
    {
        bulletActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            GameOver();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        ScoreManager.score = 0;
        audioScript.invaderSoundAttack.Stop();
        audioScript.invaderDeathSound.Play();

    }
}
