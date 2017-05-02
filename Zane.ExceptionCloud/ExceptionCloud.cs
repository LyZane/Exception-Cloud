using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCloud
{
    public class Configuration
    {
        private Uri DefaultCloudHost = new Uri("http://Exception.cloud");
        public Uri CloudHost { get { return DefaultCloudHost; } internal set { DefaultCloudHost = value; } }
    }
    public static class GlobalConfiguration
    {
        public readonly static Configuration Configuration = new Configuration();
        /// <summary>
        /// 组件是否已初始化
        /// </summary>
        public static bool Initialized { get; private set; }
        /// <summary>
        /// 检查当前的配置项，并启动日志服务。
        /// </summary>
        /// <param name="config"></param>
        public static void Startup(this Configuration config)
        {
            if (Initialized)
            {
                throw new MethodAccessException("ExceptionCloud is Initialized.");
            }
        }

        public static Configuration SetCloudHost(this Configuration config,Uri host,bool check = false)
        {
            if (Initialized)
            {
                throw new MethodAccessException("Must be before initialization.");
            }
            if (check)
            {
                //new HttpRequestMessage(HttpMethod.Get,host).

                var client = new HttpClient();
                var t = client.GetStringAsync(host);
                t.Wait();
                string str = t.Result; 
            }
            config.CloudHost = host;
            return config;
        }

        //public static ExceptionCloud.Config Config { get; set; }
        public static Configuration CatchGlobeException(this Configuration config)
        {
            if (!Initialized)
            {
                throw new MethodAccessException("Must be after initialization.");
            }
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            TaskScheduler.UnobservedTaskException += UnobservedTaskException;
            return config;
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (e.ExceptionObject as Exception);
            StackTrace st = new StackTrace(ex, true);
            var temp = st.GetFrames();
        }

        private static void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            StackTrace st = new StackTrace(exception, true);
            var temp = st.GetFrames();
        }
    }
}
