using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    public GameObject brickDestroyEffect;

    private int currentBrickCount;
    private int totalBrickCount;

    [SerializeField] private int score;
    [SerializeField] private ScoreCounterUI scoreCounter;


    private void OnEnable()
    {
        if (brickDestroyEffect == null)
        {
            brickDestroyEffect = Resources.Load<GameObject>("BrickBreakParticles");

            if (brickDestroyEffect == null)
            {
                Debug.LogError("BrickBreakParticles prefab not found! Make sure it's inside a 'Resources' folder.");
            }
            else
            {
                Debug.Log("BrickDestroyEffect successfully loaded from Resources!");
            }
        }
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        if (brickDestroyEffect != null)
        {
            Debug.Log("brick effect");
            GameObject effect = Instantiate(brickDestroyEffect, position, Quaternion.identity);
            Destroy(effect, 1f); // Cleanup to prevent memory leaks
        }
        // fire audio here
        // implement particle effect here
        // add camera shake here
        CameraShake.Shake(0.5f, 1f);
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if (currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();

        // Add counter for score
        score++;
        scoreCounter.UpdateScore(score);
    }

    public void KillBall()
    {
        maxLives--;
        // update lives on HUD here
        // game over UI if maxLives < 0, then exit to main menu after delay
        ball.ResetBall();
    }
}
