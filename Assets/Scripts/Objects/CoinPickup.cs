using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PickupRangeController pickupRange =
            other.GetComponent<PickupRangeController>();

        if (pickupRange == null)
            return;

        Inventory inventory =
            other.GetComponentInParent<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning(
                "Coin entered PickupRange, but Inventory was not found.",
                other
            );

            return;
        }

        collected = true;

        inventory.AddCurrency(coinValue);

        Data.totalCoinCount += coinValue;
        Data.tempCoinCount += coinValue;

        // Activate effects that happen whenever currency is collected.
        inventory.ActivatePocket(
            Inventory.PocketType.OnPickup
        );

        Debug.Log(
            $"Collected {coinValue} coin. Total: {inventory.currentCurrency}",
            inventory
        );

        Destroy(gameObject);
    }
}