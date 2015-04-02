using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CircularSpectrum : MonoBehaviour
{
    public enum LoopType
    {
        Continuous,
        Mirrored
    }

    public AudioSource source;
    public int channel;
    public float radius = 10.0f;
    public float phase = 0.0f;
    public float multiplier = 10.0f;
    public LoopType loopType = LoopType.Continuous;
    [Range(1, 8)]
    public int loopCount = 1;
    public AudioSpectrum.BandType bandType = AudioSpectrum.BandType.ThirtyOneBand;
    
    LineRenderer lineRenderer;
    AudioSpectrum spectrumAnalyzer;
    int vertexCount;
    Vector3[] points;

    // Use this for initialization
    void Start()
    {
        spectrumAnalyzer = new AudioSpectrum(source, channel);
        spectrumAnalyzer.bandType = bandType;
        vertexCount = loopCount * AudioSpectrum.bandTypeSizes[(int)bandType];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(vertexCount + 1);
        lineRenderer.SetWidth(0.05f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        this.GetPoints();

    }

    public Vector3[] GetPoints()
    {
        bool mirror = false;
        float max;
        int counter = 0;
        float[] bands = spectrumAnalyzer.GetBand(out max);

        CheckArray();
        for (int i = 0; i < loopCount; i++)
        {
            int j = 0;
            if (mirror)
                j = bands.Length - 1;

            while (j < bands.Length && j > -1)
            {
                points[counter].x = Mathf.Cos(phase - 2.0f * Mathf.PI * counter / (float)vertexCount) * (multiplier * bands[j] + radius);
                points[counter].y = Mathf.Sin(phase - 2.0f * Mathf.PI * counter / (float)vertexCount) * (multiplier * bands[j] + radius);

                lineRenderer.SetPosition(counter, points[counter]);

                if (mirror)
                    j--;
                else
                    j++;
                counter++;
            }

            if (loopType == LoopType.Mirrored)
                mirror = !mirror;
        }

        points[counter].x = Mathf.Cos(phase) * (multiplier * bands[0] + radius);
        points[counter].y = Mathf.Sin(phase) * (multiplier * bands[0] + radius);
        lineRenderer.SetPosition(counter, points[counter]);
        return points;
    }

    private void CheckArray()
    {
        if(points == null)
            points = new Vector3[vertexCount + 1];
        for (int i = 0; i < points.Length + 1; i++)
            if(points[i] == null)
                points[i] = new Vector3();
    }
}
