using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityEditor;

//-----------------------------------------------------------------------------------------------------
public class PerformanceStats : MonoBehaviour
{
    StringBuilder tx;
    public Text perfStats;

    private readonly float updateInterval = 1.0f;
    private float lastInterval; // Last interval end time
    private float frames = 0; // Frames over current interval

    private float framesavtick = 0;
    private float framesav = 0.0f;

    public static int collisions;

    public static bool debugMode;

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        framesav = 0;
        collisions = 0;
        tx = new StringBuilder
        {
            Capacity = 200
        };
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        debugMode = false;
        perfStats.enabled = false;
    }

    private void Update()
    {
        if (debugMode)
        {
            ++frames;

            var timeNow = Time.realtimeSinceStartup;

            if (timeNow > lastInterval + updateInterval)
            {

                float fps = frames / (timeNow - lastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                ++framesavtick;
                framesav += fps;
                float fpsav = framesav / framesavtick;

                tx.Length = 0;

                tx.AppendFormat("Time : {0} ms\nCurrent FPS: {1}\nAvgFPS: {2}\nGPUMemory : {3}\nSysMemory : {4}\n", ms, fps, fpsav, SystemInfo.graphicsMemorySize, SystemInfo.systemMemorySize)

                .AppendFormat("TotalAllocatedMemory : {0}mb\nTotalReservedMemory : {1}mb\nTotalUnusedReservedMemory : {2}mb",
                UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1048576,
                UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1048576,
                UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1048576
                );

                tx.AppendFormat("\nGameObjects : {0}\nCollisionTriggersLastSecond : {1}", FindObjectsOfType<GameObject>().Length, collisions);
                tx.AppendFormat("\nPress P to hide performance stats\nPress I to become invulnerable\nPress L to level up once");

                perfStats.text = tx.ToString();
                frames = 0;
                collisions = 0;
                lastInterval = timeNow;
            }            
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            debugMode = !debugMode;
            perfStats.enabled = debugMode;
        }        
    }
}
