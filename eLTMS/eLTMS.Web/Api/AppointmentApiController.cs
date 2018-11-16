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
        [Route("api/SendMessage")] // just for testing
        public IHttpActionResult SendMessage()
        {
            var tokens = _appointmentService.GetAllTokens();
            foreach(var token in tokens)
            {
                var data = new
                {
                    to = token.TokenString,
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
            if (success)
            {
                var tokens = _appointmentService.GetAllTokens();
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
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
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }
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
