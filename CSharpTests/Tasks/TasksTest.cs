using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSharpTests.Tasks
{
    /// <summary>
    /// Summary description for TasksTest
    /// https://www.themoviedb.org/settings/api
    /// </summary>
    [TestClass]
    public class TasksTest
    {
        public string apiKeyV3 = "1db2aeb638cafcc7f1ec52b8340a35a5";
        public string apiKeyV4 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxZGIyYWViNjM4Y2FmY2M3ZjFlYzUyYjgzNDBhMzVhNSIsInN1YiI6IjU5ODc2YmQxOTI1MTQxNTNjODAwZDUzYSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.86I4-6RfFCi4zxmE1W7xdRrMG_qZYcBfXpuyX-P6Cg0";
        public string depardieuId = "16927";
        public string blierId = "24379";
        public string gabinId = "11544";
        public string venturaId = "15397";
        public string delonId = "15135";
        public string fernandelId = "69958";
        public string blancheId = "39645";
        public string meurisseId = "12267";
        public string poiretId = "18778";
        public string deNiroId = "380";
        public string rourkeId = "2295";



        public string theMovieDbApiEndpoint = "https://api.themoviedb.org/3/person";
        public List<string> _ids;
        public TasksTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestInitialize()]
        public void TestInit()
        {
            _ids = new List<string>() { depardieuId, blierId, gabinId, venturaId , delonId , fernandelId, blancheId , meurisseId, poiretId, deNiroId, rourkeId };
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Concurrency_Issue()
        {
            var sum = 0;
            var t1 = Task.Factory.StartNew(
             () =>
                 {
                     sum = sum + Task.Factory.StartNew(
                                               () =>
                                                  {
                                                      var x = 0;
                                                      for (int i = 0; i < 10000; i++)
                                                      {
                                                          x = i / 2 * 3;
                                                      }
                                                      return 1;
                                                  }).Result;
                 }
            );
            var t2 = Task.Factory.StartNew(
             () =>
             {
                 sum = sum + Task.Factory.StartNew(
                                               () =>
                                               {
                                                   var x = 0;
                                                   for (int i = 0; i < 6000000; i++)
                                                   {
                                                       x = i / 2 * 3;
                                                   }
                                                   return 2;
                                               }).Result;
             }
            );
            var t3 = Task.Factory.StartNew(
             () =>
             {
                 sum = sum + Task.Factory.StartNew(
                                               () =>
                                               {
                                                   var x = 0;
                                                   for (int i = 0; i < 5000000; i++)
                                                   {
                                                       x = i / 2 * 3;
                                                   }
                                                   return 3;
                                               }).Result;
             }
            );
            var tasks = new List<Task>() { t1, t2, t3 };
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(sum);

        }

        [TestMethod]
        public void Concurrency_Solution_Lock()
        {
            var sum = 0;
            var o = new object();
            var t1 = Task.Factory.StartNew(
             () =>
             {
                 lock (o)
                 {
                     sum = sum + Task.Factory.StartNew(
                                       () =>
                                       {
                                           var x = 0;
                                           for (int i = 0; i < 10000; i++)
                                           {
                                               x = i / 2 * 3;
                                           }
                                           return 1;
                                       }).Result;
                 }
             }
            );
            var t2 = Task.Factory.StartNew(
             () =>
             {
                 lock (o)
                 {
                     sum = sum + Task.Factory.StartNew(
                                                   () =>
                                                   {
                                                       var x = 0;
                                                       for (int i = 0; i < 6000000; i++)
                                                       {
                                                           x = i / 2 * 3;
                                                       }
                                                       return 2;
                                                   }).Result;
                 }
             }
            );
            var t3 = Task.Factory.StartNew(
             () =>
             {
                 lock (o)
                 {
                     sum = sum + Task.Factory.StartNew(
                                                   () =>
                                                   {
                                                       var x = 0;
                                                       for (int i = 0; i < 5000000; i++)
                                                       {
                                                           x = i / 2 * 3;
                                                       }
                                                       return 3;
                                                   }).Result;
                 }
             }
            );
            var tasks = new List<Task>() { t1, t2, t3 };
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(sum);

        }

        [TestMethod]
        public void Task_Continue()
        {

            var t1 = Task.Factory.StartNew(
             () =>
             {
                 var x = 0;
                 for (int i = 0; i < 1000000; i++)
                 {
                     x = i / 2 * 3;
                 }
                 return 1;
             }
            );


            var t2 = t1.ContinueWith(
                (antecedent) =>
                {
                    var x = 0;
                    for (int i = 0; i < 1000000; i++)
                    {
                        x = i / 2 * 3;
                    }
                    var result = antecedent.Result;
                    return ++result;
                }
            );


            t2.Wait();
            Assert.AreEqual(t2.Result, 2);

        }

        [TestMethod]
        public void Task_Continue_In_Main_Thread()
        {

            var t1 = Task.Factory.StartNew(
             () =>
             {
                 var x = 0;
                 for (int i = 0; i < 1000000; i++)
                 {
                     x = i / 2 * 3;
                 }
                 return 1;
             }
            );


            var t2 = t1.ContinueWith(
                (antecedent) =>
                {
                    var x = 0;
                    for (int i = 0; i < 1000000; i++)
                    {
                        x = i / 2 * 3;
                    }
                    var result = antecedent.Result;
                    return ++result;
                }
                , TaskScheduler.FromCurrentSynchronizationContext()
            );


            t2.Wait();
            Assert.AreEqual(t2.Result, 2);

        }

        [TestMethod]
        public void Task_Exceptions()
        {

            var t1 = Task.Factory.StartNew(
             () =>
             {
                 var x = 0;
                 for (int i = 0; i < 1000000; i++)
                 {
                     x = i / 2 * 3;
                 }
                 throw new Exception("Task Failed");
                 return 1;
             }
            );

            try
            {
                t1.Wait();
            }
            catch (AggregateException ae)
            {
                var aef = ae.Flatten();
                Assert.IsTrue(aef.InnerExceptions.Any(e => e.Message == "Task Failed"));
            }

            try
            {
                var result = t1.Result;
            }
            catch (AggregateException ae)
            {
                var aef = ae.Flatten();
                Assert.IsTrue(aef.InnerExceptions.Any(e => e.Message == "Task Failed"));
            }

        }

        [TestMethod]
        public void Task_Exceptions_Wait_Any()
        {

            var t1 = Task.Factory.StartNew(
             () =>
             {
                 var x = 0;
                 for (int i = 0; i < 1000000; i++)
                 {
                     x = i / 2 * 3;
                 }
                 throw new Exception("Task Failed");
                 return 1;
             }
            );

            var tasks = new List<Task>() { t1 };
            var awaitedT1Index = Task.WaitAny(tasks.ToArray());
            var awaitedT1 = tasks[awaitedT1Index];
            Console.WriteLine("Wait any doesnt throw exeptions..");
            Assert.IsNotNull(awaitedT1);

            var ae = awaitedT1.Exception;
            var aef = ae.Flatten();
            Assert.IsTrue(aef.InnerExceptions.Any(e => e.Message == "Task Failed"));
        }

        [TestMethod]
        public void Movie_Task_Sequencial()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var id in _ids)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    var endPoint = $"{theMovieDbApiEndpoint}/{id}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                    var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                    var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var statusCode = httpResponseMessage.StatusCode;
                    var converter = new ExpandoObjectConverter();
                    dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                    Console.WriteLine($"{id} a tourné dans  {jsonContentAsObject.cast.Count} films");
                }
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"tenps d'execution : {elapsedMs} ms");

        }

        [TestMethod]
        public void Movie_Task_Wait_All()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task<Tuple<string,string>>>();

            foreach (var id in _ids)
            {
                var t = Task.Factory.StartNew(
                     (personId) =>
                         {
                             using (HttpClient httpClient = new HttpClient())
                             {
                                 httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                                 var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                                 var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                                 var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                                 var statusCode = httpResponseMessage.StatusCode;
                                 var converter = new ExpandoObjectConverter();
                                 dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                                 return new Tuple<string, string>($"{personId}",$"{jsonContentAsObject.cast.Count}");
                             }
                         },id
                );
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            foreach (Task<Tuple<string, string>> t in tasks)
            {
                var r = t.Result;
                Console.WriteLine($"{r.Item1} a tourné dans {r.Item2} films");
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"tenps d'execution : {elapsedMs} ms");
        }

        [TestMethod]
        public void Movie_Task_Wait_All_One_By_One()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task<Tuple<string, string>>>();
            foreach (var id in _ids)
            {
                var t = Task.Factory.StartNew(
                     (personId) =>
                     {
                         using (HttpClient httpClient = new HttpClient())
                         {
                             httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                             var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                             var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                             var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                             var statusCode = httpResponseMessage.StatusCode;
                             var converter = new ExpandoObjectConverter();
                             dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                             return new Tuple<string, string>($"{personId}", $"{jsonContentAsObject.cast.Count}");
                         }
                     }, id
                );
                tasks.Add(t);
            }


            while (tasks.Count>0)
            {
                var taskIndex = Task.WaitAny(tasks.ToArray());
                var r = tasks[taskIndex].Result;
                Console.WriteLine($"{r.Item1} a tourné dans {r.Item2} films");
                tasks.RemoveAt(taskIndex);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"tenps d'execution : {elapsedMs} ms");
        }

        [TestMethod]
        public void Movie_Task_Wait_All_One_By_One_By_Core()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task<Tuple<string, string>>>();
            var nbOfTasks = _ids.Count;
            var nbOfStartedTasks = 0;
            var nbCores = Environment.ProcessorCount;

            for (int i = 0; i < nbCores; i++)
            {
                if (i< nbOfTasks)
                {
                    nbOfStartedTasks++;

                    var t = Task.Factory.StartNew(
                         (personId) =>
                         {
                             using (HttpClient httpClient = new HttpClient())
                             {
                                 httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                                 var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                                 var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                                 var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                                 var statusCode = httpResponseMessage.StatusCode;
                                 var converter = new ExpandoObjectConverter();
                                 dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                                 return new Tuple<string, string>($"{personId}", $"{jsonContentAsObject.cast.Count}");
                             }
                         }, _ids[i]
                    );
                    tasks.Add(t); 
                }
            }

            while (tasks.Count > 0)
            {
                var taskIndex = Task.WaitAny(tasks.ToArray());
                var r = tasks[taskIndex].Result;
                Console.WriteLine($"{r.Item1} a tourné dans {r.Item2} films");
                tasks.RemoveAt(taskIndex);

                if (nbOfStartedTasks < nbOfTasks)
                {
                                   
                    tasks.Add(
                        Task.Factory.StartNew(
                         (personId) =>
                         {
                             using (HttpClient httpClient = new HttpClient())
                             {
                                 httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                                 var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                                 var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                                 var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                                 var statusCode = httpResponseMessage.StatusCode;
                                 var converter = new ExpandoObjectConverter();
                                 dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                                 return new Tuple<string, string>($"{personId}", $"{jsonContentAsObject.cast.Count}");
                             }
                         }, _ids[nbOfStartedTasks]
                    )
                   );
                    nbOfStartedTasks++;
                }

            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"tenps d'execution : {elapsedMs} ms");
        }

        [TestMethod]
        public void Movie_Task_Wait_All_One_By_One_By_Core_Long_Running()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task<Tuple<string, string>>>();
            var nbOfTasks = _ids.Count;
            var nbOfStartedTasks = 0;
            var nbCores = Environment.ProcessorCount;

            for (int i = 0; i < nbCores; i++)
            {
                if (i < nbOfTasks)
                {
                    nbOfStartedTasks++;

                    var t = Task.Factory.StartNew(
                         (personId) =>
                         {
                             using (HttpClient httpClient = new HttpClient())
                             {
                                 httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                                 var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                                 var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                                 var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                                 var statusCode = httpResponseMessage.StatusCode;
                                 var converter = new ExpandoObjectConverter();
                                 dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                                 return new Tuple<string, string>($"{personId}", $"{jsonContentAsObject.cast.Count}");
                             }
                         }, _ids[i],TaskCreationOptions.LongRunning
                    );
                    tasks.Add(t);
                }
            }

            while (tasks.Count > 0)
            {
                var taskIndex = Task.WaitAny(tasks.ToArray());
                var r = tasks[taskIndex].Result;
                Console.WriteLine($"{r.Item1} a tourné dans {r.Item2} films");
                tasks.RemoveAt(taskIndex);

                if (nbOfStartedTasks < nbOfTasks)
                {

                    tasks.Add(
                        Task.Factory.StartNew(
                         (personId) =>
                         {
                             using (HttpClient httpClient = new HttpClient())
                             {
                                 httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                                 var endPoint = $"{theMovieDbApiEndpoint}/{personId}/movie_credits?api_key={apiKeyV3}&language=fr-FR";
                                 var httpResponseMessage = httpClient.GetAsync(endPoint).Result;
                                 var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                                 var statusCode = httpResponseMessage.StatusCode;
                                 var converter = new ExpandoObjectConverter();
                                 dynamic jsonContentAsObject = JsonConvert.DeserializeObject<ExpandoObject>(content, converter);
                                 return new Tuple<string, string>($"{personId}", $"{jsonContentAsObject.cast.Count}");
                             }
                         }, _ids[nbOfStartedTasks],TaskCreationOptions.LongRunning
                    )
                   );
                    nbOfStartedTasks++;
                }

            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"tenps d'execution : {elapsedMs} ms");
        }



        //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
        }
    }
}
