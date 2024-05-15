using NUnit.Framework;
using WebApplication1.Services;
using System;
using System.Threading;

namespace OtpServiceTests
{
    public class Tests
    {
        private OtpService _otpService;

        [SetUp]
        public void Setup()
        {
            _otpService = OtpService.Instance;
            _otpService.CleanupExpiredOtps(); 
        }

        [Test]
        public void GenerateOtp_DefaultLength_GeneratesCorrectLength()
        {   
            string otp = _otpService.GenerateOtp();
            Assert.AreEqual(otp.Length, 6);
        }

        [Test]
        public void ValidateOtp_WithNewlyGeneratedOtp_ReturnsTrue()
        {
            string otp = _otpService.GenerateOtp();
            bool isValid = _otpService.ValidateOtp(otp);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ValidateOtp_WithExpiredOtp_ReturnsFalse()
        {
            string otp = _otpService.GenerateOtp();
            Thread.Sleep((_otpService.GetDefaultValidityPeriod() + 1) * 60000);
            bool isValid = _otpService.ValidateOtp(otp);
            Assert.IsFalse(isValid);
        }

        [Test]
        public void CleanupExpiredOtps_AfterValidityPeriod_RemoveExpiredOtps()
        {
            string otp = _otpService.GenerateOtp();
            Thread.Sleep((_otpService.GetDefaultValidityPeriod() + 1) * 60000);
            _otpService.CleanupExpiredOtps();
            bool isValid = _otpService.ValidateOtp(otp);
            Assert.IsFalse(isValid);
        }
        [Test]
        public void CleanupExpiredOtps_AfterGeneration_KeepValidOtps()
        {
            string otp = _otpService.GenerateOtp();
            _otpService.CleanupExpiredOtps();
            bool isValid = _otpService.ValidateOtp(otp);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GetDefaultValidityPeriod_Initially_ReturnsDefault()
        {
            Assert.AreEqual(1, _otpService.GetDefaultValidityPeriod());
        }

        [Test]
        public void SetDefaultValidityPeriod_UpdateValue_UpdatesSuccessfully()
        {
            _otpService.SetDefaultValidityPeriod(10);
            Assert.AreEqual(10, _otpService.GetDefaultValidityPeriod());
            _otpService.SetDefaultValidityPeriod(1);
        }
    }
}