using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerPickup"))
        {
            gameManager.instance.inventory.AddCurrency(coinValue);
            Data.totalCoinCount += coinValue;
            Data.tempCoinCount += coinValue;
            Destroy(gameObject);
        }
    }

}
