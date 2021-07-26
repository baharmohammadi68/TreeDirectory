using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDirectory.FolderTree
{
    public class Folder: IFolder
    {
        public string Name { get; set; }
        public List<Folder> Folders { get; set; }
       // public List<File> Files { get; set; }
    }
}
