using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication9.Data;
using WebApplication9.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebApplication9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            // Check if the user exists in the database
            var existingUser = _context.Users.FirstOrDefault(u => u.MobileNo == user.MobileNo);
            if (existingUser == null)
            {
                return BadRequest("User not found.");
            }

            // Generate OTP and send it
            string otp = GenerateOTP();
            SendOTP(    user.MobileNo, otp);

            // Save the OTP to the user in the database
            existingUser.OTP = otp;
            _context.SaveChanges();

            return Ok("OTP sent.");
        }

        [HttpPost("verify")]
        public IActionResult Verify(User user)
        {
            // Check if the user exists in the database
            var existingUser = _context.Users.FirstOrDefault(u => u.MobileNo == user.MobileNo);
            if (existingUser == null)
            {
                return BadRequest("User not found.");
            }

            // Check if the OTP matches
            if (existingUser.OTP == user.OTP)
            {
                return Ok("OTP verified.");
            }
            else
            {
                return BadRequest("Incorrect OTP.");
            }
        }

        private string GenerateOTP()
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            int otpValue = random.Next(100000, 999999);
            return otpValue.ToString();
        }

        private void SendOTP(string mobileNo, string otp)
        {
            if (mobileNo != null)
            {
                // Use the Twilio API to send the OTP SMS
                const string accountSid = "YOUR_TWILIO_ACCOUNT_SID";
                const string authToken = "YOUR_TWILIO_AUTH_TOKEN";
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: $"Your OTP is: {otp}",
                    from: new Twilio.Types.PhoneNumber("YOUR_TWILIO_PHONE_NUMBER"),
                    to: new Twilio.Types.PhoneNumber(mobileNo)
                );

                Console.WriteLine(message.Sid);
            }
        }
    }
}
