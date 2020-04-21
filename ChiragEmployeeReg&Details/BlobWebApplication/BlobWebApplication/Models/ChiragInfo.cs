using Microsoft.Azure.Cosmos.Table;


namespace BlobWebApplication.Models
{
    public class ChiragInfo : TableEntity
    {
        public ChiragInfo() { }
        public ChiragInfo(string LastName, string FirstName)
        {
            PartitionKey = LastName;
            RowKey = FirstName;
        }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string ResumeUrl { get; set; }
    }
}