using System;

namespace Model
{
    public class Account
    {
        public long ID { get; set; }

        public Guid IdentifierID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
