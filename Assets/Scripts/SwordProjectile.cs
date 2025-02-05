using UnityEngine;
using System.Collections;

public class SwordProjectile : Projectile
{
    public Sprite[] sprites;
    public GameObject spritePrefab;
    public float radius = 0.5f;

    private bool _triggered = false;
    private float _lifeTime = 0f;

    protected override void TriggerImpactAction(float lifeTime)
    {
        if (!_triggered)
        {
            _triggered = true;
            _lifeTime = lifeTime;
            SpawnSwordExplosionSprites();
        }
    }

    private void SpawnSwordExplosionSprites()
    {
        if (sprites == null || sprites.Length < 4)
        {
            Debug.LogWarning("Sword Projectiles is not set up properly");
            return;
        }

        Vector3[] directions = new Vector3[] 
        {
            (Vector3.up + Vector3.left).normalized,   // NW
            (Vector3.up + Vector3.right).normalized,  // NE
            (Vector3.down + Vector3.left).normalized, // SW
            (Vector3.down + Vector3.right).normalized // SE
        };

        for (int i = 0; i < directions.Length; i++)
        {
            GameObject spriteObject = Instantiate(spritePrefab, transform.position + directions[i] * 0.5f, Quaternion.Euler(0, 0, 45));
            SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[i];
            StartCoroutine(MoveSpriteOutwards(spriteObject, directions[i]));
        }
    }

    private IEnumerator MoveSpriteOutwards(GameObject spriteObject, Vector3 direction)
    {
        float elapsedTime = 0f;
        float speed = radius / _lifeTime;

        while (elapsedTime < _lifeTime)
        {
            Vector3 previousPosition = spriteObject.transform.position;
            spriteObject.transform.position += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(spriteObject);
    }
}