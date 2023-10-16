using UnityEngine;

public class BarChart : MonoBehaviour
{
    public int[] data; // Les données à afficher sur le graphique à barres
    public float barWidth = 0.5f; // Largeur de chaque barre
    public float spacing = 0.1f; // Espacement entre les barres
    public GameObject barPrefab; // Le prefab de la barre à utiliser
    public Color barColor = Color.blue; // Couleur des barres

    void Start()
    {
        if (data == null || data.Length == 0)
        {
            Debug.LogError("Aucune donnée à afficher sur le graphique à barres !");
            return;
        }

        float totalWidth = (barWidth + spacing) * data.Length;
        float startX = transform.position.x - totalWidth / 2 + barWidth / 2;

        for (int i = 0; i < data.Length; i++)
        {
            float barHeight = data[i];
            Vector3 barScale = new Vector3(barWidth, barHeight, 1f);
            Vector3 barPosition = new Vector3(startX + i * (barWidth + spacing), barHeight / 2, 0f);

            GameObject bar = Instantiate(barPrefab, barPosition, Quaternion.identity);
            bar.transform.localScale = barScale;
            bar.GetComponent<Renderer>().material.color = barColor;
            bar.transform.SetParent(transform);
        }
    }
}
