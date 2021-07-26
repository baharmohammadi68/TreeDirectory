using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDirectory.FolderTree
{
    interface IFolder
    {
        string Name { get; set; }
        List<Folder> Folders { get; set; }
       // List<File> Files { get; set; }
    }
}
