using System;

namespace TreeDirectory
{
    class Program
    {
        static void Main(string[] args)
        {            
            FileDirectoryManager mgr = new FileDirectoryManager();
            mgr.ParseFoldersFromInput();
        }
    }
}
