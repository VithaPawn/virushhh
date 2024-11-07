using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DrawingCircle : MonoBehaviour {
    [Header("Dots")]
    [SerializeField] private int subdivisions;
    [SerializeField] private GameObject dotPrefab;
    [Header("Radius")]
    [SerializeField] private float originalRadius;
    [SerializeField] private List<RadiusLevel> radiusLevels;
    private RadiusLevel currentRadiusLevel;
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannelSO startDashingSO;
    [SerializeField] private VoidEventChannelSO endDashingSO;
    [Header("Elements Affect Circle")]
    [SerializeField] private IntegerVariableSO killCountEachDashSO;

    private List<CircleDot> dots;

    private void Awake()
    {
        dots = new List<CircleDot>();
        if (radiusLevels == null || radiusLevels.Count == 0)
        {
            radiusLevels = new List<RadiusLevel>();
            radiusLevels.Add(new RadiusLevel(1));
        }
        currentRadiusLevel = radiusLevels[0];
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
        UpdateCircleVisual();
    }

    private void OnDashing()
    {
        HideCircle();
        killCountEachDashSO.ResetValueToZero();
    }

    private void AfterDashing()
    {
        //Save the RadiusLevel before update to compare
        RadiusLevel previousRadiusLevel = currentRadiusLevel;
        UpdateRadius();

        //Only update visual if the radius level changes
        if (previousRadiusLevel != currentRadiusLevel)
        {
            UpdateCircleVisual();
        }

        //Show the circle
        ShowCircle();
    }

    private void HideCircle() { foreach (var dot in dots) dot.Hide(); }

    private void ShowCircle() { foreach (var dot in dots) dot.Show(); }

    public float GetRadius() { return originalRadius * currentRadiusLevel.ratioComparedToOriginal; }

    private void UpdateRadius()
    {
        if (killCountEachDashSO.IsEqualZero() && radiusLevels.Contains(currentRadiusLevel))
        {
            int nextIndex = radiusLevels.IndexOf(currentRadiusLevel) + 1;
            currentRadiusLevel = nextIndex < radiusLevels.Count ? radiusLevels[nextIndex] : currentRadiusLevel;
        }
        else
        {
            currentRadiusLevel = radiusLevels[0];
        }
    }

    private void UpdateCircleVisual()
    {
        if (dots.Count == 0)
        {
            GenerateDotPositions((index, pos) =>
            {
                GameObject circleObject = Instantiate(dotPrefab, transform);
                CircleDot dot = circleObject.GetComponent<CircleDot>();
                dot.SetLocalPosition(pos);

                dots.Add(dot);
            });
        }
        else if (dots.Count == subdivisions)
        {
            GenerateDotPositions((index, pos) =>
            {
                dots[index].SetLocalPosition(pos);
            });
        }
        else
        {
            Debug.LogWarning("There is something wrong when generating dashing limited area!");
        }
    }

    private void GenerateDotPositions(Action<int, Vector3> dotHandler)
    {
        float angleStep = 2f * Mathf.PI / subdivisions;
        for (int i = 0; i < subdivisions; i++)
        {
            float xPosition = GetRadius() * Mathf.Cos(angleStep * i);
            float yPosition = GetRadius() * Mathf.Sin(angleStep * i);
            Vector3 dotPosition = new Vector3(xPosition, yPosition, 0);

            dotHandler(i, dotPosition);
        }
    }

    #region RadiusLevel Class
    [System.Serializable]
    public class RadiusLevel {
        [Range(0f, 1f)] public float ratioComparedToOriginal;

        public RadiusLevel(float value)
        {
            ratioComparedToOriginal = value;
        }
    }
    #endregion RadiusLevel Class
}
