using System;
using System.Management;

namespace FilonenkoTaskManager.Tools
{
    internal static class UsernameSearcher
    {
        internal static string GetProcessOwner(int processId)
        {
            try
            {
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                foreach (var o in processList)
                {
                    var obj = (ManagementObject) o;
                    object[] argList = new string[] { string.Empty, string.Empty };
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {
                        // return DOMAIN\user
                        return argList[1] + "\\" + argList[0];
                    }
                }
            }
            catch(ManagementException) { }
            return "NO OWNER";
        }
    }
}
