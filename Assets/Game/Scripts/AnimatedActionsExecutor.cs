using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class AnimatedActionsExecutor : MonoBehaviour
{
    private Queue<Func<Task>> animationQueue = new Queue<Func<Task>>();
    private bool isProcessing = false;

    public bool IsExecuting => isProcessing;

    public async Task EnqueueAnimation(Func<Task> animationTask)
    {
        animationQueue.Enqueue(animationTask);

        if (!IsExecuting)
        {
            await ExecuteQueue();
        }
    }

    private async Task ExecuteQueue()
    {
        isProcessing = true;

        while (animationQueue.Count > 0)
        {
            Func<Task> animationTask = animationQueue.Dequeue();
            await animationTask();
        }

        isProcessing = false;
    }

    public async Task WaitForCompletion()
    {
        while (IsExecuting)
        {
            await Task.Yield();
        }
    }
}
