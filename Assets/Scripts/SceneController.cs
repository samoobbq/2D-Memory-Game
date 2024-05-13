using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private const float timeLimit = 30.0f; // Time limit in seconds
    private const float specialCardSpawnInterval = 25.0f;
    private const float additionalTime = 5.0f;

    private bool isPaused = false;

    private int score = 0;
    private float timer = timeLimit;
    private Coroutine countdownCoroutine;
    private Coroutine specialCardCoroutine;
    private bool specialCardSpawned = false;

    private AudioManager audioManager;

    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text timerLabel;
    [SerializeField] GameObject specialCardPrefab;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;

    public int gridRows;
    public int gridCols;

    // Add offsetX and offsetY variables here
    public float offsetX = 2.0f;
    public float offsetY = 2.5f;

    public float moveSpeed = 1.25f; // Speed the time bonus card moves at

    public bool canReveal {
        get { return !isPaused && secondRevealed == null; }
    }

    void Start()
    {
        // Start the timer
        countdownCoroutine = StartCoroutine(Countdown());

        specialCardCoroutine = StartCoroutine(SpawnSpecialCards());

        audioManager = AudioManager.instance;

        // Initialize UI
        UpdateScoreUI();
        UpdateTimerUI();

        // Place cards
        PlaceCards();
    }

    IEnumerator SpawnSpecialCards()
    {
        while (timer > 0)
        {
            if (timer <= specialCardSpawnInterval && !specialCardSpawned)
            {
                specialCardSpawned = true;
                SpawnSpecialCard();
            }
            yield return new WaitForSeconds(Random.Range(5f, 10f)); // Wait for a random time between 5 to 10 seconds
        }
    }

    private void SpawnSpecialCard()
    {
        // Spawn special card at a random position within the visible area
        Vector3 spawnPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-1.5f, 1.5f), -0.22f);
        GameObject specialCard = Instantiate(specialCardPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(BounceSpecialCard(specialCard.transform));
    }

    IEnumerator BounceSpecialCard(Transform specialCardTransform)
    {
        float bounceDuration = 5f;
        float elapsedTime = 0f;
        Vector3 direction = Vector3.up;

        // Ensure the special card transform is not null
        if (specialCardTransform == null)
        {
            yield break; // Exit the coroutine if the transform is null
        }

        while (elapsedTime < bounceDuration)
        {
            float bounceSpeed = 3f; // Adjust the bounce speed as needed

            // Check if the special card transform is still valid
            if (specialCardTransform != null)
            {
                // Translate the special card
                specialCardTransform.Translate(direction * bounceSpeed * Time.deltaTime);

                // Reverse direction when hitting top or bottom
                if (specialCardTransform.position.y >= 1.5f || specialCardTransform.position.y <= -1.5f)
                {
                    direction = -direction;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for a short time before disappearing
        yield return new WaitForSeconds(0.5f);

        // If the special card hasn't been destroyed before disappearing, destroy it
        if (specialCardTransform != null && specialCardTransform.gameObject.activeSelf)
        {
            Destroy(specialCardTransform.gameObject);
            specialCardSpawned = false; // Reset flag for spawning new special card
        }
    }


    // Add this method to your script to handle the click event on the special card
    public void OnSpecialCardClicked()
    {
        // Add time to the timer variable when the special card is clicked
        timer += additionalTime;
        // Destroy the clicked special card immediately
       // Destroy(GameObject.FindWithTag("SpecialCard"));
        specialCardSpawned = false; // Reset flag for spawning new special card
    }

    void PlaceCards()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = GenerateRandomNumbers(gridRows * gridCols / 2);
        numbers = ShuffleArray(numbers);
        
        for (int i = 0; i < gridCols; i++) {
            for (int j = 0; j < gridRows; j++) {
                MemoryCard card;

                if (i == 0 && j == 0) {
                    card = originalCard;
                } else {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    public void AddTime(float additionalTime)
    {
        timer += additionalTime;
        UpdateTimerUI();
    }

    private int[] GenerateRandomNumbers(int count) {
        List<int> numbers = new List<int>();
        for (int i = 0; i < count; i++) {
            numbers.Add(i);
            numbers.Add(i);
        }
        return numbers.ToArray();
    }

    private int[] ShuffleArray(int[] numbers) {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++) {
            int temp = newArray[i];
            int rand = Random.Range(i, newArray.Length);
            newArray[i] = newArray[rand];
            newArray[rand] = temp;
        }
        return newArray;
    }

    public void CardRevealed(MemoryCard card) {
        if (firstRevealed == null) {
            firstRevealed = card;
        } else {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch() {
        if (firstRevealed.Id == secondRevealed.Id) {
            score++;
            audioManager.PlayMatchSound();
            UpdateScoreUI();
            CheckWinCondition();
        } else {
            audioManager.PlayMismatchSound();
            yield return new WaitForSeconds(0.4f);
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    private IEnumerator Countdown()
    {
        while (timer > 0)
        {
            if (!isPaused) {
                yield return new WaitForSeconds(1.0f);
                timer -= 1.0f;
                UpdateTimerUI();

                if (timer <= 0)
                {
                    LoadMenuScene();
                    audioManager.PlayLoseSound();
                    break;
                }

                if (timer <= specialCardSpawnInterval && !specialCardSpawned)
                {
                    SpawnSpecialCard();
                    specialCardSpawned = true;
                }
            } else {
                yield return null; // Wait for next frame when paused
            }
        }
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    private void CheckWinCondition()
    {
        // Check if all pairs of cards are matched
        if (score == gridCols * gridRows / 2)
        {
            // All cards matched, load Menu scene
            audioManager.PlayWinSound();

            LoadMenuScene();
        }
    }

    void UpdateScoreUI()
    {
        scoreLabel.text = "Score: " + score;
    }

    void UpdateTimerUI()
    {
        timerLabel.text = "Time: " + Mathf.Ceil(timer).ToString();
    }

    public void Pause()
    {
        if (!isPaused) {
            isPaused = true;
            if (countdownCoroutine != null)
                Time.timeScale = 0;
                StopCoroutine(countdownCoroutine);
        }
    }

    public void Resume()
    {
        if (isPaused) {
            isPaused = false;
            Time.timeScale = 1;
            countdownCoroutine = StartCoroutine(Countdown());
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
