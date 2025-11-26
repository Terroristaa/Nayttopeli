using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private float floatSpeed;
    public Material[] fontSharedMaterials
    {
        get => damageText != null ? damageText.fontSharedMaterials : null;
        set { if (damageText != null) damageText.fontSharedMaterials = value; }
    }

    void Start() {
        floatSpeed = Random.Range(0.1f, 1.5f);
        Destroy(gameObject, 1);
    }

    void Update() {
        transform.position += Vector3.up * Time.deltaTime * floatSpeed;
    }

    public void SetValue(int value){
        damageText.text = value.ToString();
    }
}
