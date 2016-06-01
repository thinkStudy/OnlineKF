using System;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Collections;

/// <summary>
/// ThreadWorker 的摘要说明
/// </summary>
/// 

namespace OnlineKF.Utils
{
    public class ThreadWorker
    {
        public ThreadWorker()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //        
        }
        private static Queue workQueue = new Queue();
        private static int[] forLock = new int[0];//锁变量
        private const int MAX_THREAD_NUM = 1;//最大线程数
        private static int threadNum = 0;//当前线程数

        //private static log4net.ILog log = log4net.LogManager.GetLogger("applog");

        /// <summary>
        /// 增加工作队列
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="param"></param>
        public static void AddWork(ThreadWorker.DoThreadWork worker, Object param)
        {
            lock (workQueue)
            {
                workQueue.Enqueue(new ThreadCaller(worker, param));
            }
            StartWork();
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        private static void StartWork()
        {
            lock (forLock)
            {
                if (threadNum < MAX_THREAD_NUM)
                {
                    ThreadStart myThreadDelegate = new ThreadStart(ThreadWorker.DoWork);
                    Thread myThread = new Thread(myThreadDelegate);
                    myThread.IsBackground = true;
                    myThread.Start();
                    threadNum = threadNum + 1;
                }
            }
        }

        /// <summary>
        /// 执行工作
        /// </summary>
        private static void DoWork()
        {
            try
            {
                while (true)
                {
                    ThreadCaller caller = null;
                    lock (workQueue)
                    {
                        if (workQueue.Count > 0)
                        {
                            caller = (ThreadCaller)workQueue.Dequeue();
                        }
                    }
                    if (caller != null)
                    {
                        caller.worker(caller.param);
                    }
                    else
                        Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
               // log.Error(ex);
            }
            finally
            {
                lock (forLock)
                {
                    threadNum = threadNum - 1;
                }
            }
        }

        /// <summary>
        /// 等待调用的委托
        /// </summary>
        /// <returns></returns>
        public delegate void DoThreadWork(Object param);

    }

    public class ThreadCaller
    {
        public ThreadCaller(ThreadWorker.DoThreadWork worker, Object param)
        {
            this.worker = worker;
            this.param = param;
        }
        public ThreadWorker.DoThreadWork worker;
        public Object param;
    }
}