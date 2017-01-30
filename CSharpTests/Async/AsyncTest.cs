using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpTests.Async
{
    public sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {

        public List<KeyValuePair<SendOrPostCallback, object>> CallBacks =
            new List<KeyValuePair<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null) throw new ArgumentNullException(nameof(d));
            Console.WriteLine("A delegate/state pair was added to the List of callBacks");
            CallBacks.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("Synchronously sending is not supported.");
        }

    }
    /// <summary>
    /// We need to improve this test after learning lock and concurent collections
    /// </summary>
    [TestClass]
    public class AsyncTest
    {
        public TestContext TestContext { get; set; }

        public List<Task> IoTasks = new List<Task>();

        public async Task<string> LoginLibraryAsync(int k)
        {
            //Simulate IO operation 
            var ioTask = Task.Run(() =>
            {
                #region IO Thread

                TestContext.WriteLine($" An Io operation is starting on thread : {Thread.CurrentThread.ManagedThreadId}");

                for (var j = 0; j < 100000; j++)
                {
                    TestContext.WriteLine("-");
                }
                //throw new Exception("Exception inside the third party library...Aie Aie Aie");
                return "Successfully logged";

                #endregion
            });


            IoTasks.Add(ioTask);

            try
            {
                var methodResult = await ioTask;

                //The state machines executes THIS CHUNK OF CODE ON THE MAIN THREAD (THE FAKE UI THREAD)
                TestContext.WriteLine(
                    $"The LoginLibraryAsync Callback is now executed from : {Thread.CurrentThread.ManagedThreadId} the value of k was stored in state : {k}"
                );
                //throw new Exception("Exception inside our callback...Aie Aie Aie");

                //Warning the returned task is not ioTask!!
                return methodResult;
            }
            catch (Exception)
            {
                throw new Exception("The task is faulted");
            }
        }

        [TestMethod]
        public void Assert_Async()
        {
            //HERE WE ARE IN THE MAIN MESSAGE LOOP

            var syncCtx = new SingleThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncCtx);

            TestContext.WriteLine($"Main Thread {Thread.CurrentThread.ManagedThreadId}");

            for (var k = 0; k < 1; k++)
            {
                TestContext.WriteLine($"Call the async Method");
                var loginTask = LoginLibraryAsync(k);

                //DEAD LOCK 
                //Because the async operation needs to finish in the main thread
                //(But the main thread is not free he's waiting the completion of the Task)
                //LoginLibraryAsync().Wait();

                var ioTaskCompleted = false;

                TestContext.WriteLine("WORKING ON THE MAIN THREAD");
                for (var i = 0; i < 200000; i++)
                {
                    TestContext.WriteLine(".");

                    if (syncCtx.CallBacks.Count == 1 && !ioTaskCompleted)
                    {
                        ioTaskCompleted = true;
                        foreach (var t in IoTasks)
                        {
                            TestContext.WriteLine(
                                $"task id : {t.Id}  iscompleted : {t.IsCompleted} isFaulted : {t.IsFaulted} ");
                        }
                    }
                }

                TestContext.WriteLine($" LOGINTASK task id : {loginTask.Id}  iscompleted : {loginTask.IsCompleted} isFaulted : {loginTask.IsFaulted}");
                Assert.AreEqual(loginTask.IsCompleted,false);
                foreach (var workItem in syncCtx.CallBacks)
                {
                    var storedDelegate = workItem.Key;
                    var state = workItem.Value;
                    storedDelegate(state);
                }
                TestContext.WriteLine($" LOGINTASK task id : {loginTask.Id}  iscompleted : {loginTask.IsCompleted} isFaulted : {loginTask.IsFaulted}");
                Assert.AreEqual(loginTask.IsCompleted, true);
                try
                {
                    TestContext.WriteLine(loginTask.Result);
                }
                catch (Exception ex)
                {

                    TestContext.WriteLine(ex.Message);
                }

                /*Cleanup ioTasks*/
                IoTasks = new List<Task>();
                /*Cleanup callBacks*/
                syncCtx.CallBacks = new List<KeyValuePair<SendOrPostCallback, object>>();

                TestContext.WriteLine($"The Main Loop has runned {k + 1} times");
            }

        }

    } 

}
