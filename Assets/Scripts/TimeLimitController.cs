using UnityEngine;
using System.Collections;

public class TimeLimitController : MonoBehaviour
{
    [SerializeField] float timeLimit = 60f; // Default time limit
    [SerializeField] GameObject timeCardPrefab;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] float timeCardChance = 0.2f;

    private float timeLeft;

    void Start()
    {
        timeLeft = timeLimit;
        StartCoroutine(UpdateTimeLimit());
    }

    IEnumerator UpdateTimeLimit()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
            if (timeLeft <= 0f)
            {
                // Game over logic
                break;
            }
            else if (timeLeft <= 15f)
            {
                ShowTimeCard();
            }
        }
    }

    public void ShowTimeCard()
    {
        if (Random.value < timeCardChance)
        {
            Instantiate(timeCardPrefab, cardSpawnPoint.position, Quaternion.identity);
        }
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }

    public void AddTime(float bonus)
    {
        timeLeft += bonus;
    }
}
