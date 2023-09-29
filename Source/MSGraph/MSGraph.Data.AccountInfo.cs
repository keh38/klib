namespace KLib.MSGraph.Data
{
    public class AccountInfo
    {
        public AccountInfo() { }
        public AccountInfo(string objectID, string tokenID)
        {
            ObjectID = objectID;
            TokenID = tokenID;
        }

        public string ObjectID;
        public string TokenID;
    }
}