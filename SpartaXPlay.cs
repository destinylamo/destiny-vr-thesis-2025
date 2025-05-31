using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpartaXPlay : MonoBehaviour
{
    public Transform player9;
    public Transform player10; // XR Origin Rig (Player 10)
    public Transform player12;
    public Transform player14;
    public Transform defender10;
    public Transform defender12;
    public Transform defender14;
    public Transform ball;

    public float passSpeed = 7f;
    public float playerMoveSpeed = 3f;
    public float defenderSpeed = 1.5f;

    private Collider ballCollider;
    private bool isInteractable = true;
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();

    void Start()
    {
        ballCollider = ball.GetComponent<Collider>();

        // Store initial positions
        initialPositions[player9] = player9.position;
        initialPositions[player10] = player10.position;
        initialPositions[player12] = player12.position;
        initialPositions[player14] = player14.position;
        initialPositions[defender10] = defender10.position;
        initialPositions[defender12] = defender12.position;
        initialPositions[defender14] = defender14.position;
        initialPositions[ball] = ball.position;
    }

    public void StartPlay()
    {
        StartCoroutine(PassBall());
    }

    public void ResetPlay()
    {

        foreach (var entry in initialPositions)
        {
            entry.Key.position = entry.Value;
        }
    }

    IEnumerator PassBall()
    {
        yield return new WaitForSeconds(1f);

        // First pass: Player 9 to Player 10
        Vector3 player10Position = player10.position;
        yield return StartCoroutine(MoveBallTowards(player9.position, player10Position));

        StartCoroutine(MoveDefendersForward());

        DisableInteractability();
        yield return new WaitForSeconds(0.5f);

        // Pass to player 12
        Vector3 player12Position = player12.position + Vector3.up * 0.3f;
        yield return StartCoroutine(MoveBallTowards(player10Position, player12Position));

        EnableInteractability();

        // Move player 12 with the ball
        Vector3 player12MovePosition = player12Position + Vector3.forward * 2f;
        StartCoroutine(MovePlayerWithBall(player12, player12MovePosition));
        yield return StartCoroutine(MoveBallTowards(player12Position, player12MovePosition));

        // Both player 12 and player 14 start moving at the same time towards the crossing point
        StartCoroutine(CrossBetweenPlayers(player12, player14));

    }

    IEnumerator CrossBetweenPlayers(Transform player12, Transform player14)
    {
        // Use player12's current Y level to prevent sinking
        float groundY = player12.position.y;
        Vector3 crossingPosition = new Vector3(15.49f, groundY, 1.5f);

        // Move both players to the crossing point
        StartCoroutine(MovePlayerWithBall(player12, crossingPosition));
        StartCoroutine(MovePlayerWithBall(player14, crossingPosition));

        // Wait for a moment before making the pass
        yield return new WaitForSeconds(1f);

        // Pass the ball from player 12 to player 14
        Vector3 player14Position = player14.position + Vector3.up * 0.3f;
        yield return StartCoroutine(MoveBallTowards(player12.position, player14Position));

        // Final movement for player 14
        Vector3 player14FinalPosition = new Vector3(10.17f, groundY, 8.6f);
        StartCoroutine(MovePlayerWithBall(player14, player14FinalPosition));
        yield return StartCoroutine(MoveBallTowards(player14Position, player14FinalPosition));
    }

    IEnumerator MoveBallTowards(Vector3 startPosition, Vector3 targetPosition)
    {
        ballCollider.enabled = false;

        while (Vector3.Distance(ball.position, targetPosition) > 0.1f)
        {
            ball.position = Vector3.MoveTowards(ball.position, targetPosition, passSpeed * Time.deltaTime);
            yield return null;
        }

        ball.position = targetPosition;
        ballCollider.enabled = true;
    }

    IEnumerator MovePlayerWithBall(Transform player, Vector3 targetPosition)
    {
        while (Vector3.Distance(player.position, targetPosition) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosition, playerMoveSpeed * Time.deltaTime);
            ball.position = player.position + Vector3.up * 0.3f;
            yield return null;
        }

        player.position = targetPosition;
        ball.position = player.position + Vector3.up * 0.3f;
    }

    IEnumerator MoveDefendersForward()
    {
        Vector3 moveDirection = -Vector3.forward;
        float duration = 4f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            defender10.position += moveDirection * defenderSpeed * Time.deltaTime;
            defender12.position += moveDirection * defenderSpeed * Time.deltaTime;
            defender14.position += moveDirection * defenderSpeed * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void DisableInteractability()
    {
        isInteractable = false;
        ballCollider.enabled = false;
    }

    private void EnableInteractability()
    {
        isInteractable = true;
        ballCollider.enabled = true;
    }
}

