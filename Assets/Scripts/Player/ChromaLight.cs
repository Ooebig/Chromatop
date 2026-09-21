using UnityEngine;

public class ChromaLight : MonoBehaviour
{
    [SerializeField] Light targetLight;
    [SerializeField] float intensity = 4f;

    void Awake()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
    }

    void LateUpdate() // LateUpdate allows the light to match the color wheel.
    {
        if (targetLight == null || gameManager.instance == null)
        {
            return;
        }

        Material mat = gameManager.instance.activeMaterial;
        if (mat == null)
        {
            return;
        }

        Color c = mat.color;
        c.a = 1f;
        targetLight.color = c;
        targetLight.intensity = intensity;
    }
}
