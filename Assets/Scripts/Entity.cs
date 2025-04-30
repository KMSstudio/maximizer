using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public bool isSelected = false;
    private Renderer rend;
    private Color defaultColor;
    public Color selectedColor = Color.yellow;

    void Start() {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }

    void Update() {
        rend.material.color = isSelected ? selectedColor : defaultColor;
    }

    public void MoveTo(Vector3 target) {
        // $$$$ Temporary Coordinate Adj
        Vector3 adjustedTarget = new Vector3(target.x, 0.5f, target.z);

        StopAllCoroutines();
        StartCoroutine(MoveSmooth(adjustedTarget));
    }

    private System.Collections.IEnumerator MoveSmooth(Vector3 target) {
        while (Vector3.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
            yield return null;
        }
    }
}