﻿namespace KLib.MSGraph.Data
{
    public class DriveItem
    {
        public string id;
        public string name;
        public string url;
        public int size;
        public FolderFacet folder;
        public ParentReference parentReference;
        public RemoteItem remoteItem;
        public FileSystemInfo fileSystemInfo;
    }
}