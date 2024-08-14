using System;

namespace Model
{
    public class AccountVerification
    {
        public long ID { get; set; }

        public Guid IdentifierID { get; set; }

        public long AccountID { get; set; }

        public string Category { get; set; }

        public string Code { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string SendResult { get; set; }
    }
}
