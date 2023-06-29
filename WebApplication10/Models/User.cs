using System;

namespace WebApplication9.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public DateTime AddedOn { get; set; }
        public string? AddedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public bool DeletedFlag { get; set; }
        public string? OTP { get; set; }
    }
}
