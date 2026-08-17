using UnityEngine;
using UnityEngine.Rendering;


public class EnemyStats : MonoBehaviour
{
    public EnemyBehavior.EnemyType type;
    public gameManager.ColorType Color;
    // Update is called once per frame

    public float maxHp = 100f;
    public float currentHp;
    public float speed;
    public float damage;
    public float firerate;
    public float stopDistance;
    public float contactDamage;

    public GameObject simpleModel;
    public GameObject chargerModel;
    public GameObject shooterModel;


    void Awake()
    {
        currentHp = maxHp;
    }

    public void LoadModel()
    {
        GameObject model = null;

        switch (type)
        {
            case EnemyBehavior.EnemyType.Simple:
                model = simpleModel;
                break;
            case EnemyBehavior.EnemyType.Charger:
                model = chargerModel;
                break;
            case EnemyBehavior.EnemyType.Shooter:
                model = shooterModel;
                break;

        }

        if (model != null)
        {
            GameObject spawndedModel = Instantiate(model, transform.position, transform.rotation);
            spawndedModel.transform.SetParent(this.transform);

            SetModelColor(spawndedModel);
        }
    }
    private void SetModelColor(GameObject model)
    {
        string ColorName = Color.ToString(); ;


        foreach (Transform child in model.GetComponentsInChildren<Transform>(true))
        {
            if (child == model.transform) continue;
            if (child.name.Equals(ColorName, System.StringComparison.OrdinalIgnoreCase))
            {
                child.gameObject.SetActive(true);

            }
            else
            {
                if (IsAnEnemyColorName(child.name))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    private bool IsAnEnemyColorName(string name)
    {
        string upperName = name.ToUpper();
        return upperName == "RED" || upperName == "ORANGE" || upperName == "YELLOW" || upperName == "GREEN" || upperName == "BLUE" || upperName == "PURPLE";
            }
}
