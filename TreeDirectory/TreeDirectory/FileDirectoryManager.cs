using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using TreeDirectory.FolderTree;

namespace TreeDirectory
{
    /*
     This class generates takes the hardoded input as a list of strings and generates the folder strutcure in the ouput
     The FoldersByPath dictionary holds unique paths (flattened paths) as keys and their folder struture as their values. 
     Based on differend directory commands defined in the enum, corresponding methods (created, move, delete or list) is called 
     and List method populates the output accordingy
         */
    public class FileDirectoryManager
    {
        public enum DirectoryCommands
        {
            NONE = 0,
            CREATE = 1,
            LIST = 2,
            DELETE = 3,
            MOVE = 4
        }
        
        Dictionary<string, Folder> _folderByPath = null;
        Dictionary<string, Folder> FoldersByPath
        {
            get
            {
                if(_folderByPath == null)
                {
                    _folderByPath = new Dictionary<string, Folder>();
                }
                return _folderByPath;
            }
            set
            {
                _folderByPath = value;
            }
        }
        List<string> _output = null;
        List<string> Output
        {
            get
            {
                if (_output == null)
                {
                    return _output = new List<string>();
                }
                return _output;
            }
            set
            {
                _output = value;
            }
        }
        public void ParseFoldersFromInput()
        {
            var inputLines = new List<string>()
            {
                "CREATE fruits",
                "CREATE vegetables",
                "CREATE grains",
                "CREATE fruits/apples",
                "CREATE fruits/apples/fuji", 
                "LIST", 
                "CREATE grains/squash",
                "MOVE grains/squash vegetables",
                "CREATE foods",
                "MOVE grains foods",
                "MOVE fruits foods",
                "MOVE vegetables foods",
                "LIST",
                "DELETE fruits/apples",
                "DELETE foods/fruits/apples",
                "LIST"
            };
            foreach(string line in inputLines )
            {
                Output.Add(line);
                List<string> parsedLine = line.Split(" ").ToList();
                string commandName = parsedLine.Count > 0 ? parsedLine[0] : null;
                DirectoryCommands command = (DirectoryCommands)Enum.Parse(typeof(DirectoryCommands), commandName);
                switch(command)
                {
                    case DirectoryCommands.CREATE:
                        CreateFolders(parsedLine);
                        break;
                    case DirectoryCommands.LIST:
                        ListFolders();
                        break;
                    case DirectoryCommands.MOVE:
                        MoveFolder(parsedLine);
                        break;
                    case DirectoryCommands.DELETE:
                        DeleteFolder(parsedLine);
                        break;
                    case DirectoryCommands.NONE:
                        break;

                }

            }

        }

        private void DeleteFolder(List<string> parsedLine)
        {
            List<string> foundKeysToDelete = FoldersByPath.Keys.Where(k => k.StartsWith((parsedLine[1]))).ToList();
            if (foundKeysToDelete.Count > 0)
            {
                foreach(string item in foundKeysToDelete)
                {
                    FoldersByPath.Remove(item);
                    if (item.IndexOf("/") > 0)
                    {
                        string newPath = item.Substring(0, parsedLine[1].LastIndexOf("/"));
                        CreateSubfolders(newPath);
                    }
                }
            }
            else
            {
                Output.Add($"Cannot delete {parsedLine[1]}");
            }
        }

        private void MoveFolder(List<string> parsedLine)
        {
            string newPath = string.Empty;
            List<Folder> existingFolders = new List<Folder>();
            List<Folder> newSubFolders = new List<Folder>();
            string destination = string.Empty;
            string newName = string.Empty;
            string inputDestination = parsedLine[2];

            if (parsedLine[1].IndexOf("/") > 0) // if the first param of move command has a slash, it means only move its children to the destination
            {
                var lastSlashPosition = parsedLine[1].LastIndexOf("/");
                var folderPathToStay = parsedLine[1].Substring(0, lastSlashPosition);
                var folderPathToMove = parsedLine[1].Substring(lastSlashPosition + 1);
                foreach (var key in FoldersByPath.Keys)
                {
                    if (key.StartsWith(folderPathToStay))
                    {
                        newPath = $"{parsedLine[2]}/{folderPathToMove}";
                        destination = FoldersByPath.Keys.SingleOrDefault(k => k.StartsWith(inputDestination));
                        if (string.IsNullOrEmpty(destination))
                        {
                            Output.Add("Destination folder does not exist, please create it first");
                        }
                        List<string> subFolders = folderPathToMove.Split('/').ToList();
                        
                        foreach (var subfolder in subFolders)
                        {
                            newSubFolders.Add(new Folder() { Name = subfolder, Folders = new List<Folder>() });
                        }                        
                       
                        if (folderPathToStay.IndexOf("/") > 0)
                        {
                            newName = folderPathToStay.Substring(folderPathToStay.LastIndexOf("/"));
                            folderPathToStay = newName;
                        }
                        FoldersByPath.Add(newPath, new Folder() { Name = newName, Folders = FoldersByPath[destination].Folders.Concat(newSubFolders).ToList()});
                        FoldersByPath.Remove(destination);
                        existingFolders = FoldersByPath[key].Folders;
                        FoldersByPath.Remove(key);
                        FoldersByPath.Add(folderPathToStay, new Folder() { Name = newName, Folders = existingFolders.Except(newSubFolders).ToList() });
                        break;
                    }
                }

            }
            else
            {
                destination = FoldersByPath.Keys.Any(k => k.StartsWith(inputDestination)) ? inputDestination : string.Empty;
                if (string.IsNullOrEmpty(destination))
                {
                    Output.Add("Destination folder does not exist, please create it first");
                }
                foreach (var key in FoldersByPath.Keys)
                {                    
                    if (key.StartsWith(parsedLine[1]))
                    {
                        newPath = $"{inputDestination}/{key}";
                        CreateSubfolders(newPath);                       
                        FoldersByPath.Remove(key);                        
                        break;
                    }
                }
            }
        }

        private void ListFolders()
        {            
            if(FoldersByPath != null && FoldersByPath.Count > 0)//looping through each path
            {
                HashSet<string> alreadyPrinted = new HashSet<string>();

                foreach (var item in FoldersByPath.Keys)
                {
                    if (item.IndexOf("/") < 0)//there is only one folder in this path
                    {
                        Output.Add(item);
                    }
                    else
                    {
                        var folders = item.Split("/").ToList();
                        string indentation = string.Empty;
                        if (!alreadyPrinted.Contains(folders[0]))
                        {
                            Output.Add(folders[0]);
                            alreadyPrinted.Add(folders[0]);
                        }

                        for (int i = 1; i < folders.Count; i++)
                        {
                            if (alreadyPrinted.Contains(folders[i])) continue;
                            indentation += "  ";
                            Output.Add($"{indentation}{folders[i]}");
                            alreadyPrinted.Add(folders[i]);
                        }


                    }

                }
                
            }
            foreach(var item in Output)
            {
                Console.WriteLine(item);
            }
            Output = new List<string>();
        }

        private void CreateFolders(List<string> strings)
        {   
            foreach (var str in strings)
            {
                if (strings[0] == str) continue;
                if (str.IndexOf("/") > 0) // we have sub folders
                {
                    CreateSubfolders(str);
                }
                else // we have only one folder for now 
                {
                    var singleFolder = new Folder
                    {
                        Name = str,
                        Folders = new List<Folder>()
                    };
                    FoldersByPath.Add(str, singleFolder);                   
                }
            }           
        }
        private void CreateSubfolders(string folderPath)
        {
            var lastSlashPosition = folderPath.LastIndexOf("/");
            var parentFolderPath = folderPath.Substring(0, lastSlashPosition);
            var newFolderName = folderPath.Substring(lastSlashPosition + 1);
            var newFolder = new Folder() { Name = newFolderName };
            if (FoldersByPath.ContainsKey(parentFolderPath))// if we already have the parent folder path, we should remove it as it takes O(1) time complexity and add a new one instead of updating the existing entry
            {
                List<Folder> existingFolders = FoldersByPath[parentFolderPath].Folders;                
                FoldersByPath.Remove(parentFolderPath);
                existingFolders.Insert(0, newFolder);
                var newFolderToAdd = new Folder()
                {
                    Name = newFolderName,
                    Folders = existingFolders
                };
                FoldersByPath.Add(folderPath, newFolderToAdd);
            }
            else
            {
                FoldersByPath.Add(folderPath, newFolder);
            }

        }              
    }
}
