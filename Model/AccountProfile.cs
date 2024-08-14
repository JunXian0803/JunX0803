using System;

namespace Model
{
    public class AccountProfile
    {
        public long ID { get; set; }

        public Guid IdentifierID { get; set; }

        public long AccountID { get; set; }

        public string ProfilePicture { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NRIC { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
