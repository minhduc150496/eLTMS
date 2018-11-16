using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using eLTMS.Web.Utils;

namespace eLTMS.Web.Api
{
    public class AppointmentApiController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentApiController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }
        
        [HttpGet]
        [Route("api/SendMessage")]
        public IHttpActionResult SendMessage()
        {
            var tokens = new List<string>();
            tokens.Add("fbrXtn3nY94:APA91bGxtktkg-3aYO-D9LEDiDD1sYClUPwjrfag7zFRejq6gfqHgheX1k7Qzad1A_MQhD-TEcyXfiZVUwv8ZLl78Dk51lMhKEyYIXcr-DcgSvSTW0DU0gJTRk9126kKxbL4DUoOHg_z");
            tokens.Add("eu0prJJXZtE:APA91bFqjZ0XWrAkgDdd7CFgjpy80w3f6mJrPS1n1Q7NIl9Z9Eng4OruP56Qqlk-r5eAPtX7ciN_R04IBQVcsJAksGP-fArymagg9PhQYe6Pxc3UjCMGNEqZuqS4qC6g3AT-pi64_G0B");
            tokens.Add("f2tPIkvPYmc:APA91bG9DMXwl5hpUAYU1uYFyn-e7r3BR6Qb4yJdvk7BUwfE-_PZZ94fqrIxRfkQ-9scD6ZNmBdL_BQc-EUiHOTQ9hhF0rFO2c_ycidzZUmSKzjEGxsymF3Ul9TY8VgU_stLN_U6UZ0-");
            foreach(var token in tokens)
            {
                var data = new
                {
                    to = token,
                    data = new
                    {
                        message = "Je suis un garcon",
                        name = "DucBM",
                        userId = "123",
                        status = true
                    }
                };
                try
                {
                    SendNotificationUtils.SendNotification(data);
                } catch (Exception ex)
                {
                    //
                }
            }
            return Ok();
        }
        
        [HttpPost]
        [Route("api/appointment/create")]
        public HttpResponseMessage Create(AppointmentDto appoinDto)
        {
            // call to AppointmentService
            var success = this._appointmentService.Create(appoinDto);
            var obj = new
            {
                Success = success,
                Message = success ? "Tạo mới thành công!" : "Có lỗi xảy ra. Xin vui lòng thử lại"
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-new-appointments-by-patient-id")]
        public HttpResponseMessage GetNewAppointment(int patientId)
        {
            var app = _appointmentService.GetNewApp(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(app);

            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-old-appointments-by-patient-id")]
        public HttpResponseMessage GetOldAppointment(int patientId)
        {
            var app = _appointmentService.GetOldApp(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(app); 
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpPut]
        [Route("api/appointment/update-appointment")]
        public HttpResponseMessage UpdateAppointment(AppointmentDto appointmentDto)
        {
            var success = _appointmentService.UpdateAppointment
                (appointmentDto.AppointmentCode, appointmentDto.SampleGettingDtos);
            var obj = new
            {
                Success = success,
                Message = success ? "Cập nhật thành công!" : "Xin vui lòng thử lại"
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [HttpDelete]
        [Route("api/appointment/delete-appointment")]
        public HttpResponseMessage DeleteAppointment(string appointmentCode)
        {
            var success = _appointmentService.DeleteAppointment(appointmentCode);
            var obj = new
            {
                Success = success,
                Message = success ? "Hủy cuộc hẹn thành công!" : "Xin vui lòng thử lại"
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-results-by-patient-id")]
        public HttpResponseMessage GetResultByPatientId(int patientId)
        {
            var app = _appointmentService.GetResult(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetResultDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-code")]
        public HttpResponseMessage GetResultByAppCode(string appCode)
        {
            var app = _appointmentService.GetResultByAppCode(appCode);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetResultDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-by-phone-and-date")]
        public HttpResponseMessage GetAppByPhoneNDate(string phone)
        {
            var app = _appointmentService.GetAppByPhone(phone);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetByPhoneDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

    }
}
