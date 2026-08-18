using UnityEngine;

public class EnemyCurrencyDrop : MonoBehaviour
{
    [SerializeField] private ItemData currencyItem;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 1f;

    public void DropCurrency()
    {
        if (currencyItem == null ||
            currencyItem.worldPrefab == null)
        {
            Debug.LogWarning(
                "Enemy currency drop is missing its item or world prefab.",
                this
            );

            return;
        }

        if (Random.value > dropChance)
            return;

        Instantiate(
            currencyItem.worldPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}