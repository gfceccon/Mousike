using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    struct Projectile
    {
        public float angle;
        public float vel;
        public float delay;

        public Projectile(float angle, float vel, float delay)
        {
            this.angle = angle;
            this.vel = vel;
            this.delay = delay;
        }
    }

    class FloatIntComparer : IComparer<KeyValuePair<float, int>>
    {
        public int Compare(KeyValuePair<float, int> x, KeyValuePair<float, int> y)
        {
            return x.Key > y.Key ? 1 : -1;
        }
    }

    public GameObject prefab;
    public CircularSpectrum spectrum;
    public float minimumBand = 1.0f;
    public float maximumDifferenceBetweenBands = 10.0f;
    public float delay = 0.02f;
    public float delayBetweenBands = 1.0f;
    public float velocityMultiplier = 0.5f;

    List<KeyValuePair<float, int>> bandList;
    LinkedList<Projectile> spawnList;
    

    public void Start()
    {
        bandList = new List<KeyValuePair<float, int>>();
        spawnList = new LinkedList<Projectile>();
        
    }

    public void Update()
    {
        float[] spec = spectrum.BandList;
        if (spec.Length == 0)
            return;

        bandList.Clear();
        for (int i = 0; i < spec.Length; i++)
            bandList.Add(new KeyValuePair<float, int>(spec[i], i));
        bandList.Sort(new FloatIntComparer());
        float maxValue, mean, previousValue, sum, current, next;
        int counter = 1;

        maxValue = mean = previousValue = sum = current = bandList[0].Key;
        
        if (maxValue < minimumBand)
            return;
        
        for (int i = 0; i < bandList.Count; i++)
        {
            next = current = (float)bandList[i].Key;
            int j = bandList[i].Value;
            mean = sum / counter;

            spawnList.AddLast(new Projectile(j * 2.0f / (float)bandList.Count * Mathf.PI, velocityMultiplier * current, delay * mean / current));
            while (current == previousValue && i < bandList.Count - 1)
            {
                i++;
                spawnList.AddLast(new Projectile(j * 2.0f / (float)bandList.Count * Mathf.PI, velocityMultiplier * current, delay));
                current = bandList[i].Key;
            }
            counter++;
            sum += current;
            previousValue = next;
        }

        this.Spawn();
    }

    private void Spawn()
    {
        if (spawnList.Count == 0)
            return;

        Projectile first = spawnList.First.Value;
        first.delay -= Time.deltaTime;
        if (first.delay <= 0.0f)
        {
            GameObject spawn = Instantiate(prefab);
            spawn.GetComponent<Enemy>().SetAngleAndVelocity(first.angle, first.vel);
            spawnList.RemoveFirst();
        }

    }
}
