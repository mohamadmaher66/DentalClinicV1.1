using System;
using System.IO;

namespace DentalClinicAPI.Helpers
{
    public static class Logger
    {
        public static void Log(Exception ex)
        {
            try
            {
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Logs";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = Path.Combine(filePath, DateTime.Now.ToString("yyyy-MM-dd-HH-MM-ss") + "_" + Guid.NewGuid().ToString("N") + ".log");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (ex.Message != null)
                    {
                        writer.WriteLine("Message: " + ex.Message);
                        writer.WriteLine();
                        writer.WriteLine("Inner Exception: " + ex.InnerException);
                        writer.WriteLine();
                        writer.WriteLine("Stack Trace: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
