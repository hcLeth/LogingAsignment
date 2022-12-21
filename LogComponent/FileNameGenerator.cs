using System;
using LogComponent.Interfaces;

namespace LogComponent
{
    public class FileNameGenerator : IFileNameGenerator
    {
        public string GenerateFileName()
        {
            return @"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log";
        }
    }
}