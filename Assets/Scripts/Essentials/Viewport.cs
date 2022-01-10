using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX;

    float minY;

    float maxX;

    float maxY;

    float middleX;
    float rightMiddleX;
    float leftMiddleY;

    public float MaxX => maxX;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
        rightMiddleX = mainCamera.ViewportToWorldPoint(new Vector3(0.85f, 0f, 0f)).x;

        leftMiddleY = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0f)).y;

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    public Vector3 RandomEnemySpawnPositonFromLeft(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = minX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    public Vector3 FixedEnemySpawnPosition()
    {
        Vector3 position = Vector3.zero;

        return position;
    }

    public Vector3 RandomRighHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;

    }

    public Vector3 RightHalfPosition(float paddingX)
    {
        Vector3 position = Vector3.zero;
        
        position.x = Random.Range(minX + paddingX, maxX - paddingX) ;
        position.y = transform.position.y;

        return position;

    }

    public Vector3 StayInRightToMiddlePos()
    {
        Vector3 position = Vector3.zero;

        position.x = rightMiddleX;
        position.y = leftMiddleY;

        return position;
    }
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;

    }
}
