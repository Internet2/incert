﻿using System;
using System.Reflection;
using System.Text;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class ResultExtensions
    {
        private static readonly ILog Log = Logger.Create();


        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class IncludeInErrorDetails : Attribute { }

        public static void LogIfError(this IResult result)
        {
            var errorResult = result as ErrorResult;
            if (errorResult == null)
                return;

            if (errorResult.Logged)
                return;
            
            Log.Warn(errorResult.GetDetails());
            Log.Error(errorResult.ErrorName);
            errorResult.Logged = true;
        }

        public static void LogFatalIfError(this IResult result)
        {
            var errorResult = result as ErrorResult;
            if (errorResult == null)
                return;

            Log.Fatal(errorResult.ErrorName);
        }

        public static string GetDetails(this IResult result)
        {
            return GetDetails(result as ErrorResult);
        }

        public static string GetDetails(this ErrorResult result)
        {
            try
            {
                if (result == null)
                    return "unknown issue";
                
                var buffer = new StringBuilder();
                buffer.AppendFormat("Error details: {0}", result.ErrorName);
                

                foreach (var property in result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.GetCustomAttribute<IncludeInErrorDetails>() == null)
                        continue;
                    
                    buffer.AppendLine();
                    buffer.AppendFormat("{0} = {1}", property.Name, property.GetValue(result));
                }

                return buffer.ToString();
            }
            catch (Exception)
            {
                return result != null 
                    ? string.Format("Error details for {0} not available", result.ErrorName) 
                    : string.Format("Error details not available");
            }
        }
    }
}
