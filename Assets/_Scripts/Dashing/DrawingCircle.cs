using System.Collections.Generic;
using UnityEngine;

public class DrawingCircle : MonoBehaviour {
    [SerializeField] private int subdivisions = 30;
    [SerializeField] private float radius = 5f;
    [SerializeField] private GameObject dotPrefab;

    private List<CircleDot> dots;

    private void Awake()
    {
        dots = new List<CircleDot>();
    }

    private void Start()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;

        for (int i = 0; i < subdivisions; i++)
        {
            float xPosition = radius * Mathf.Cos(angleStep * i);
            float zPosition = radius * Mathf.Sin(angleStep * i);
            Vector3 pointInCircle = new Vector3(xPosition, zPosition, 0);

            GameObject circleObject = Instantiate(dotPrefab, transform);
            CircleDot dot = circleObject.GetComponent<CircleDot>();
            dot.SetPosition(pointInCircle);

            dots.Add(dot);
        }
    }

    public void HideCircle() { foreach (var dot in dots) dot.Hide(); }

    public void ShowCircle() { foreach (var dot in dots) dot.Show(); }
}
