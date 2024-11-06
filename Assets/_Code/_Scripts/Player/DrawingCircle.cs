using System.Collections.Generic;
using UnityEngine;

public class DrawingCircle : MonoBehaviour {
    [Header("Dots")]
    [SerializeField] private int subdivisions;
    [SerializeField] private GameObject dotPrefab;
    [Header("Radius")]
    [SerializeField] private float originalRadius;
    [SerializeField] private List<RadiusLevel> radiusLevels;
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;
    [Header("Elements Affect Circle")]
    [SerializeField] private IntegerVariableSO killCountBeforeDashSO;

    private List<CircleDot> dots;

    private void Awake()
    {
        dots = new List<CircleDot>();
    }

    private void OnEnable()
    {
        startDashingSO.OnEventRaised += OnDashing;
        endDashingSO.OnEventRaised += AfterDashing;
    }

    private void OnDisable()
    {
        startDashingSO.OnEventRaised -= OnDashing;
        endDashingSO.OnEventRaised -= AfterDashing;
    }

    private void Start()
    {
        float angleStep = 2f * Mathf.PI / subdivisions;

        for (int i = 0; i < subdivisions; i++)
        {
            float xPosition = originalRadius * Mathf.Cos(angleStep * i);
            float zPosition = originalRadius * Mathf.Sin(angleStep * i);
            Vector3 pointInCircle = new Vector3(xPosition, zPosition, 0);

            GameObject circleObject = Instantiate(dotPrefab, transform);
            CircleDot dot = circleObject.GetComponent<CircleDot>();
            dot.SetPosition(pointInCircle);

            dots.Add(dot);
        }
    }

    private void OnDashing()
    {
        HideCircle();
        killCountBeforeDashSO.ResetValueToZero();
    }

    private void AfterDashing()
    {

        ShowCircle();
    }

    public void HideCircle() { foreach (var dot in dots) dot.Hide(); }

    public void ShowCircle() { foreach (var dot in dots) dot.Show(); }

    public float GetRadius() { return originalRadius; }

    #region RadiusLevel Class
    [System.Serializable]
    public class RadiusLevel {
        public float percentageComparedToOriginal;
    }
    #endregion RadiusLevel Class
}
