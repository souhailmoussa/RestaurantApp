using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Logging
{
    public class Logger
    {
        private static string logPath => $"{Directory.GetCurrentDirectory()}\\log.{DateTime.Now:yyyy.MM.dd}.txt";
        private static string logInfoPath => $"{Directory.GetCurrentDirectory()}\\logInfo.{DateTime.Now:yyyy.MM.dd}.txt";

        public static void Log(string message, [CallerMemberName]string source = null)
        {
            //TODO: Send message to the logging Queue
            var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Source: {source ?? "NoSource"}
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Message: {message}";

            File.AppendAllText(logPath, logTextInfo);
        }

        public static void Log(ExceptionContext context, [CallerMemberName]string source = null)
        {
            //TODO: Send message to the logging Queue
            var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Source: {source ?? "NoSource"}
Url: {context?.HttpContext?.Request?.Method} {context?.HttpContext?.Request?.Path}
Payload: {GetBody(context)}
Message: {context?.Exception?.Message}
StackTrace: {context?.Exception?.StackTrace}
InnerMessage: {context?.Exception?.InnerException?.Message}
InnerStackTrace: {context?.Exception?.InnerException?.StackTrace}";

            File.AppendAllText(logPath, logText);
        }

        public static void Log(Exception exception, [CallerMemberName]string source = null)
        {
            //TODO: Send message to the logging Queue
            var logText = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: ERROR
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Source: {source ?? "NoSource"}
Message: {exception?.Message}
StackTrace: {exception?.StackTrace}";

            File.AppendAllText(logPath, logText);
        }

        public static void LogInfo(string message, [CallerMemberName]string source = null)
        {
            //TODO: Send message to the logging Queue
            var logTextInfo = $@"{Environment.NewLine}------------------------------New Entry------------------------------
Type: INFO
Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}
Source: {source ?? "NoSource"}
Message: {message}";

            File.AppendAllText(logInfoPath, logTextInfo);
        }

        private static string GetBody(ExceptionContext context)
        {
            var result = "";
            try
            {
                var req = context.HttpContext.Request;

                // Allows using several time the stream in ASP.Net Core
                // req.EnableRewind();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                if (req.Body != null)
                {
                    using (StreamReader reader
                        = new StreamReader(req.Body, System.Text.Encoding.UTF8, true, 1024, true))
                    {
                        result = reader.ReadToEnd();
                    }

                    // Rewind, so the core is not lost when it looks the body for the request
                    req.Body.Position = 0;
                }

                // Do your work with bodyStr
            }
            catch
            {
                // Ignored
            }
            return result;
        }
    }
}
