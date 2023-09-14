using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public Bullet missilePrefab;
    public GameObject levelCompletedScreen;

    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public float missileAttackRate = 2f;
    public int amountKilled {  get; private set; }
    public int amountAlive => this.totlaInvaders - this.amountKilled;
    public int totlaInvaders => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled / (float)this.totlaInvaders;

    private Vector3 direction = Vector2.right;

    [SerializeField] private AudioScript audioScript;

    private void Awake()
    {
        for (int row = 0; row < this.rows; ++row)
        {
            float width = 0.75f * (this.columns - 1);
            float height = 0.2f * (this.rows - 1);
            Vector3 centering = new Vector2(-width / 1f, -height / 1f);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 1.5f), 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;

                Vector3 position = rowPosition;
                position.x += col * 1.5f;
                invader.transform.position = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
            }
        }


    }

    private void AdvanceRow()
    {
        direction.x *= -1f;

        Vector3 position = this.transform.position;
        position.y -= 1f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0 / (float)this.amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        this.amountKilled++;

        if (this.amountKilled >= this.totlaInvaders)
        {
            levelCompletedScreen.SetActive(true);
            audioScript.invaderSoundAttack.Stop();
            audioScript.protectorWinSound.Play();
            Time.timeScale = 0;
        }

    }

}
