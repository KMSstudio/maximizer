using UnityEngine;

public abstract class GeneralArmy : MonoBehaviour {
    protected Renderer rend;

    protected virtual void Start() {
        rend = GetComponentInChildren<Renderer>();
        if (rend == null) { Debug.LogWarning("Renderer not found on " + gameObject.name); }
        else { UpdateColor(); }
    }

    protected virtual void Update() {
        UpdateColor();
    }

    protected abstract void UpdateColor();

    public virtual void MoveTo(Vector3 target) {
        Vector3 adjustedTarget = target + new Vector3(0f, 0.5f, 0f);
        StopAllCoroutines();
        StartCoroutine(MoveSmooth(adjustedTarget));
    }

    protected System.Collections.IEnumerator MoveSmooth(Vector3 target) {
        while (Vector3.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
            yield return null;
        }
    }

    public virtual void TakeDamage(float amount) {
        // To be overridden
    }
}