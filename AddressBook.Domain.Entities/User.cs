using AddressBook.Core.Entities.Base;

namespace AddressBook.Core.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
