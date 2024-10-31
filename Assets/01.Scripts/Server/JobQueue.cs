using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class JobQueue : MonoBehaviour
{
    // 메인 스레드에서 실행할 작업을 저장할 스레드 안전한 큐
    private static readonly ConcurrentQueue<Action> jobQueue = new ConcurrentQueue<Action>();

    // 외부에서 작업을 큐에 추가하는 메서드
    public static void Enqueue(Action job)
    {
        jobQueue.Enqueue(job);
    }

    private void Update()
    {
        // 큐에 있는 모든 작업을 메인 스레드에서 순차적으로 실행
        while (jobQueue.TryDequeue(out Action job))
        {
            job?.Invoke();
        }
    }
}