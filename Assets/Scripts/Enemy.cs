using UnityEngine;

public class Enemy : MonoBehaviour {
    private Renderer rend;
    private Color defaultColor = Color.red;

    void Start() {
        rend = GetComponentInChildren<Renderer>();
        if (rend == null) { Debug.LogWarning("Renderer not found on Enemy or children."); }
        else { rend.material.color = defaultColor; }
    }

    void Update() {
        // In future: Add AI behavior
    }

    public void MoveTo(Vector3 target) {
        Vector3 adjustedTarget = target + new Vector3(0f, 0.5f, 0f);
        StopAllCoroutines();
        StartCoroutine(MoveSmooth(adjustedTarget));
    }

    private System.Collections.IEnumerator MoveSmooth(Vector3 target) {
        while (Vector3.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
            yield return null;
        }
    }

    public void TakeDamage(float amount) {
        // Placeholder for damage logic
    }
}