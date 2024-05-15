using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication1.Services
{
    public class OtpService
    {
        private static readonly OtpService instance = new OtpService();
        public static OtpService Instance => instance;

        private const int DefaultOtpLength = 6; 
        private  int DefaultValidityPeriodMinutes = 1; 


        private readonly Dictionary<string, (DateTime, int)> _otpDictionary;



        private OtpService()
        {
            _otpDictionary = new Dictionary<string, (DateTime, int)>();
        }

        public int GetDefaultValidityPeriod()
        {
           return DefaultValidityPeriodMinutes; 
        }
        public void SetDefaultValidityPeriod(int value)
        {
            DefaultValidityPeriodMinutes = value;
        }
        public void CleanupExpiredOtps()
        {
            var current = DateTime.Now;
            var keysToRemove = new List<string>();

            foreach (var otp in _otpDictionary)
            {
                var (creationTime, validityPeriod) = otp.Value;
                if ((current - creationTime).TotalMinutes > validityPeriod)
                {
                    keysToRemove.Add(otp.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                _otpDictionary.Remove(key);
            }
        }

        public Dictionary<string, (DateTime CreationTime, int ValidityPeriod)> GetAllOtpEntries()
        {
            lock (_otpDictionary)
            {
                return new Dictionary<string, (DateTime, int)>(_otpDictionary);
            }
        }

        public string GenerateOtp(int length = DefaultOtpLength)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                const string chars = "0123456789";
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                char[] result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[randomBytes[i] % chars.Length];
                }
                string otp = new string(result);

            
                var hashedOtp = ComputeHash(otp);
                _otpDictionary.Add(hashedOtp, (DateTime.Now, GetDefaultValidityPeriod()));

                return otp;
            }
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }


        public bool ValidateOtp(string otp)
        {
            if (string.IsNullOrEmpty(otp))
                return false;

            var hashedOtp = ComputeHash(otp);
            //int validityPeriod = GetDefaultValidityPeriod();

            if (!_otpDictionary.TryGetValue(hashedOtp, out var optCreationAndValidity))
                return false;

            (DateTime creationTime, int validityPeriod) = optCreationAndValidity;
            TimeSpan elapsedTime = DateTime.Now - creationTime;
            if (elapsedTime.TotalMinutes > validityPeriod)
            {
                _otpDictionary.Remove(hashedOtp);
                return false;
            }
            return true;
        }
    }
}