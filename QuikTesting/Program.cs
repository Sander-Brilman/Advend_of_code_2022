using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

Hashtable table = new()
{
    { "file1", "1234" },
    { "file2", "1234" },
    { "dir1", new Hashtable() 
        {
            { "file3", "1234" },
            { "file4", "1234" },
            { "dir2", new Hashtable()
                {
                    { "file5", "1234" },
                    { "file6", "1234" },
                }
            }
        }
    }
};

List<string> path = new()
{
    "dir1",
    "dir2",
};

Hashtable layer = (Hashtable)table[path[0]];

for (int i = 1; i < path.Count; i++)
{

    layer = (Hashtable)layer[path[i]];
}

layer.Add("file7", "69");

Console.ReadLine();