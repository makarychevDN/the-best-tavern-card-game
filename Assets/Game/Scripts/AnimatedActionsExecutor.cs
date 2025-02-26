using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class AnimatedActionsExecutor : MonoBehaviour
{
    private Queue<Func<Task>> animationQueue = new Queue<Func<Task>>(); // Очередь анимаций
    private bool isProcessing = false;

    public bool IsExecuting => isProcessing;

    public async Task EnqueueAnimation(Func<Task> animationTask)
    {
        animationQueue.Enqueue(animationTask); // Добавляем анимацию в очередь

        if (!IsExecuting)
        {
            await ExecuteQueue(); // Запускаем выполнение очереди, если оно не идет
        }
    }

    private async Task ExecuteQueue()
    {
        isProcessing = true;

        while (animationQueue.Count > 0)
        {
            Func<Task> animationTask = animationQueue.Dequeue(); // Берем анимацию из очереди
            await animationTask(); // Ждем завершения анимации
        }

        isProcessing = false;
    }
}
