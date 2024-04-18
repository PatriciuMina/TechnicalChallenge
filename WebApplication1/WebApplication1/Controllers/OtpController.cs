using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [System.Web.Http.RoutePrefix("api/otp")]
    public class OtpController : ApiController
    {

        private readonly OtpService _otpService;
        public OtpController()
        {
            _otpService = OtpService.Instance; 
        }
       
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("generate")]
        public IHttpActionResult GenerateOtp()
        {
            string otp = _otpService.GenerateOtp();
            int toastDuration = _otpService.GetDefaultValidityPeriod()*60000;
            return Ok(new { Otp = otp, ToastDuration = toastDuration });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("validate")]
        public IHttpActionResult ValidateOtp([FromBody] OtpModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Otp))
                return BadRequest("No OTP provided.");

            bool isValid = _otpService.ValidateOtp(model.Otp);
            if (isValid)
            {
                return Ok("OTP is valid.");
            }
            else
            {
                return BadRequest("Invalid OTP.");
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("set-validity")]
        public IHttpActionResult SetDefaultValidityPeriod([FromBody] int minutes)
        {
            try
            {
                _otpService.SetDefaultValidityPeriod(minutes);
                return Ok($"Validity period set to {minutes} minutes.");
            }
            catch (Exception ex)
            {
                return BadRequest("Could not set validity period: " + ex.Message);
            }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("allotps")]
        public IHttpActionResult GetAllOtps()
        {
            var otps = _otpService.GetAllOtpEntries();
            return Ok(otps.Select(kvp => new { OTP = kvp.Key, CreatedAt = kvp.Value }));
        }
    }
}