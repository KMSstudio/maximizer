using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance { get; private set; }

    public ResourceValue resources = new ResourceValue();
    public ResourceValue incomePerSecond = new ResourceValue(100, 100, 100);
    public ResourceValue entityCost = new ResourceValue(200, 0, 0);

    private float timer = 0f;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= 1f) {
            timer -= 1f;
            resources += incomePerSecond;
        }
    }

    // Check if there are enough resources to create an entity
    public bool IsEntityCreatable() {
        Debug.Log("IsEntityCreatable call");
        return resources >= entityCost;
    }

    // Consume resources for entity creation
    public void ExeEntityCreate() {
        Debug.Log("ExeEntityCreate call");
        if (IsEntityCreatable()) {
            resources -= entityCost;
        } else {
            Debug.LogWarning("Not enough resources to create entity.");
        }
    }
}