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
    public float delayBetweenSpawn = 1.0f;


    private int updateCount = 0;


    List<KeyValuePair<float, int>> bandList;
    LinkedList<Projectile> spawnList;
    

    public void Start()
    {
        bandList = new List<KeyValuePair<float, int>>();
        spawnList = new LinkedList<Projectile>();
        
    }

    public void FixedUpdate()
    {
        this.Spawn();

        if (updateCount * Time.fixedDeltaTime > delayBetweenSpawn)
            updateCount = 0;
        else
        {
            updateCount++;
            return;
        }

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

        spawnList.AddLast(new Projectile(2.0f * Mathf.PI * bandList[0].Value / bandList.Count, velocityMultiplier, 0.0f));
        for (int i = 1; i < bandList.Count; i++)
        {
            mean = sum / counter;
            next = bandList[i].Key;
            int j = bandList[i].Value;
            for (; current == next && i < bandList.Count; i++)
            {
                spawnList.AddLast(new Projectile(2.0f * Mathf.PI * j / bandList.Count, velocityMultiplier, 0.0f));
                next = (float)bandList[i].Key;
            }
            spawnList.AddLast(new Projectile(2.0f * Mathf.PI * j / bandList.Count, velocityMultiplier, 0.0f));
            
            counter++;
            sum += current;
            previousValue = next;
        }
    }

    private void Spawn()
    {
        if (spawnList.Count == 0)
            return;
        foreach (Projectile projectile in spawnList)
        {
            if (projectile.delay <= 0.0f)
            {
                GameObject spawn = Instantiate(prefab);
                spawn.GetComponent<Enemy>().SetAngleAndVelocity(projectile.angle, projectile.vel);
                spawnList.RemoveFirst();
            }
            else
                spawnList.remo
        }

    }
}
