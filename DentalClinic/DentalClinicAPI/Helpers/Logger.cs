using System;
using System.IO;

namespace DentalClinicAPI.Helpers
{
    public static class Logger
    {
        public static void Log(string errorMessage)
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
                    if (errorMessage != null)
                    {
                        writer.WriteLine(errorMessage);
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
