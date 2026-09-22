using UnityEngine;
public class EnemyThreatVisual : MonoBehaviour
{
    // Reference to the enemy's stats component (contains this enemy's Color / identity).
    // If left unassigned in the inspector, we try to find it in Awake().
    [SerializeField] EnemyStats stats;

    [Header("Brightness")]
    // Brightness tiers used to indicate threat level (direct = highest, immune = lowest).
    [SerializeField] float brightDirect = 1.00f;
    [SerializeField] float brightStrong = 0.70f;
    [SerializeField] float brightWeak = 0.40f;
    [SerializeField] float brightImmune = 0.18f;

    [Header("Glow")]
    // Emission multipliers used to make high-threat enemies "glow" more.
    [SerializeField] float glowDirect = 2.0f;
    [SerializeField] float glowStrong = 0.8f;
    [SerializeField] float glowWeak = 0.2f;
    [SerializeField] float glowImmune = 0.0f;

    //Settings for Lights and glowing SFX.
    [Header("Light")]
    [SerializeField] Light glowLight;
    [SerializeField] float lightRange = 4f;
    [SerializeField] float lightDirect = 3.0f;
    [SerializeField] float lightStrong = 1.8f;
    [SerializeField] float lightWeak = 0.8f;
    [SerializeField] float lightImmune = 0.25f;

    // Shader property IDs are cached for performance so we don't look them up repeatedly.
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");
    static readonly int EmissionId = Shader.PropertyToID("_EmissionColor");

    // Materials that we will modify at runtime for the currently active color group.
    Material[] liveMats;
    // The original colors for each material so we can dim/brighten relative to them.
    Color[] originalColors;
    // Track last seen values so we only rebind or reapply when necessary.
    gameManager.ColorType lastEnemyColor = (gameManager.ColorType)(-1);
    gameManager.ColorType lastPlayerColor = (gameManager.ColorType)(-1);

    // Awake: ensure we have a reference to the EnemyStats component.
    // This is lightweight and runs once when the object is created.
    void Awake()
    {
        if (stats == null)
        {
            stats = GetComponent<EnemyStats>();
        }
    }

    // LateUpdate: main runtime loop that decides whether to (re)bind materials
    // and apply visual changes based on the player's current active color.
    // Using LateUpdate ensures visuals are applied after other updates.
    void LateUpdate()
    {
        // Guard: require a game manager and stats to proceed.
        if (gameManager.instance == null || stats == null)
        {
            return;
        }

        // If the enemy's color group changed (or we haven't bound yet) bind the new group.
        if (liveMats == null || stats.Color != lastEnemyColor)
        {
            BindGroup(stats.Color);
        }

        // If binding failed or there's nothing to update, exit early.
        if (liveMats == null || liveMats.Length == 0)
        {
            return;
        }

        // Get the player's active color and skip work if nothing changed.
        gameManager.ColorType playerColor = gameManager.instance.activeColor;
        if (playerColor == lastPlayerColor && stats.Color == lastEnemyColor)
        {
            return;
        }

        // Record the player color and apply updated visuals.
        lastPlayerColor = playerColor;
        Apply(playerColor, stats.Color);
    }

    // BindGroup: find the transform that contains per-color child folders (e.g., "Red", "Blue")
    // and enable only the folder matching 'color'. Collect the group's MeshRenderer materials
    // and cache their original colors so we can modify them efficiently.
    void BindGroup(gameManager.ColorType color)
    {
        Transform body = FindColorFolderParent(transform);
        if (body == null)
        {
            // No color-folder parent found; nothing to bind.
            return;
        }

        string want = color.ToString();
        Transform group = null;

        // Iterate children of the body; activate the folder that matches the desired color name.
        for (int i = 0; i < body.childCount; i++)
        {
            Transform folder = body.GetChild(i);
            bool match = string.Equals(folder.name, want, System.StringComparison.OrdinalIgnoreCase);
            folder.gameObject.SetActive(match);
            if (match)
            {
                group = folder;
            }
        }

        if (group == null)
        {
            // No matching folder found.
            return;
        }

        // Gather all MeshRenderers in the active group and cache their materials and base colors.
        MeshRenderer[] rends = group.GetComponentsInChildren<MeshRenderer>(true);
        liveMats = new Material[rends.Length];
        originalColors = new Color[rends.Length];

        for (int i = 0; i < rends.Length; i++)
        {
            Material mat = rends[i].material;
            // Ensure emission is enabled so setting emission color has effect.
            mat.EnableKeyword("_EMISSION");
            liveMats[i] = mat;
            // Prefer a shader's _BaseColor if available, otherwise use the material's color.
            originalColors[i] = mat.HasProperty(BaseColorId)
                ? mat.GetColor(BaseColorId)
                : mat.color;
        }

        // Update tracking values so we don't rebind unnecessarily.
        lastEnemyColor = color;
        lastPlayerColor = (gameManager.ColorType)(-1);

        if (glowLight == null)
        {
            glowLight = GetComponentInChildren<Light>();
        }
        if (glowLight != null)
        {
            glowLight.range = lightRange;
        }
    }

    // FindColorFolderParent: helper that searches the transform hierarchy for a parent
    // that contains multiple named color folders (e.g., RED / GREEN / BLUE).
    // Heuristic: a transform with at least two children named like color names is returned.
    static Transform FindColorFolderParent(Transform root)
    {
        Transform[] all = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < all.Length; i++)
        {
            int hits = 0;
            for (int c = 0; c < all[i].childCount; c++)
            {
                string n = all[i].GetChild(c).name.ToUpperInvariant();
                if (n == "RED" || n == "ORANGE" || n == "YELLOW" ||
                    n == "GREEN" || n == "BLUE" || n == "PURPLE")
                {
                    hits++;
                }
            }
            // If a candidate has at least two color-named children we assume it's the folder parent.
            if (hits >= 2)
            {
                return all[i];
            }
        }
        return null;
    }

    // Apply: core logic that maps the damage multiplier between player and enemy colors
    // to visual tiers (brightness and glow), then applies those values to each cached material.
    // This method iterates over 'liveMats' and modifies their base and emission colors.
    void Apply(gameManager.ColorType playerColor, gameManager.ColorType enemyColor)
    {
        // Compute damage multiplier; this drives the visual tiers.
        float mult = gameManager.damageCalc(1f, playerColor, enemyColor);

        float bright;
        float glow;

        // Map multiplier to discrete visual tiers. Thresholds reflect game balance (tunable).
        if (mult >= 1.9f)
        {
            bright = brightDirect; glow = glowDirect;
        }
        else if (mult >= 0.9f)
        {
            bright = brightStrong; glow = glowStrong;
        }
        else if (mult >= 0.4f)
        {
            bright = brightWeak; glow = glowWeak;
        }
        else
        {
            bright = brightImmune; glow = glowImmune;
        }

        // Apply computed brightness and emission to every cached material.
        for (int i = 0; i < liveMats.Length; i++)
        {
            if (liveMats[i] == null)
            {
                continue;
            }

            // Scale the original color by the brightness tier. Keep alpha at 1 for rendering.
            Color albedo = originalColors[i] * bright;
            albedo.a = 1f;

            liveMats[i].SetColor(BaseColorId, albedo);
            liveMats[i].SetColor(ColorId, albedo);
            // Emission uses the original color multiplied by the glow tier for consistent hue.
            liveMats[i].SetColor(EmissionId, originalColors[i] * glow);
        }

        if (glowLight != null)
        {
            Color lightColor = originalColors.Length > 0
                ? originalColors[0]
                : Color.white;

            if (gameManager.instance.colorMaterials.TryGetValue(enemyColor, out Material src) && src != null)
            {
                lightColor = src.color;
            }
            lightColor.a = 1f;
            glowLight.color = lightColor;

            if (mult >= 1.9f)
            {
                glowLight.intensity = lightDirect;
            }
            else if (mult >= 0.9f)
            {
                glowLight.intensity = lightStrong;
            }
            else if (mult >= 0.4f)
            {
                glowLight.intensity = lightWeak;
            }
            else
            {
                glowLight.intensity = lightImmune;
            }
        }

    }
}