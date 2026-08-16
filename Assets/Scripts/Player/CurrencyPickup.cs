using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private int currencyAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();

        if (pickup != null)
        {
            pickup.AddCurrency(currencyAmount);
            Destroy(gameObject);
        }
    }
}