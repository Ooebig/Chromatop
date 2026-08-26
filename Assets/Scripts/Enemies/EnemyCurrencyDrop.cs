using UnityEngine;

public class EnemyCurrencyDrop : MonoBehaviour
{
    [Header("Currency Drop")]
    [SerializeField] private GameObject currencyPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 1f;

    public void DropCurrency()
    {
        if (currencyPrefab == null)
        {
            Debug.LogWarning(
                "EnemyCurrencyDrop is missing its currency prefab.",
                this
            );

            return;
        }

        if (Random.value > dropChance)
            return;

        Instantiate(
            currencyPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}