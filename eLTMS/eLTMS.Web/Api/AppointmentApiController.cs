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
using eLTMS.Models;

namespace eLTMS.Web.Api
{
    public class AppointmentApiController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentApiController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }

        [HttpPost]
        [Route("api/appointment/create")]
        public HttpResponseMessage Create(AppointmentDto appoinDto)
        {
            if (appoinDto.id != null)
            {
                appoinDto.PatientId = appoinDto.id;
            }
            // call to AppointmentService
            var result = _appointmentService.Create(appoinDto);

            // push noti
            if (result.Success)
            {
                var tokens = _appointmentService.GetAllTokens();
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
                        data = new
                        {
                            message = "Có thêm cuộc hẹn mới. ",
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

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-appointments-by-patient-id")]
        public HttpResponseMessage GetAppointmentsByPatientId(int patientId) // DucBM
        {
            var appDtos = _appointmentService.GetAppointmentsByPatientId(patientId);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpPut]
        [Route("api/appointment/update-appointment")]
        public HttpResponseMessage UpdateAppointment(AppointmentDto appointmentDto)
        {
            
            var result = this._appointmentService.UpdateAppointment(appointmentDto.AppointmentId, appointmentDto.SampleGettingDtos);

            // push noti
            if (result.Success)
            {
                var tokens =  _appointmentService.GetAllTokens();
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
                        data = new
                        {
                            message = "Có cuộc hẹn vừa thay đổi. ",
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
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpDelete]
        [Route("api/appointment/delete-appointment")]
        public HttpResponseMessage DeleteAppointment(int appointmentId)
        {
            var result = this._appointmentService.DeleteAppointment(appointmentId);

            // send noti
            if (result.Success)
            {
                var tokens = _appointmentService.GetAllTokens();
                foreach (var token in tokens)
                {
                    var data = new
                    {
                        to = token.TokenString,
                        data = new
                        {
                            message = "Có cuộc hẹn vừa được hủy. ",
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
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
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

        [HttpGet]
        [Route("api/appointment/get-result-by-appointment-id")]
        public HttpResponseMessage GetResultByAppointmentId(int appointmentId)
        {
            var app = _appointmentService.GetResultDoneByAppointmentId(appointmentId);
            var appDto = Mapper.Map<Appointment, ResultOfAppointmentDto>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDto);
            return response;
        }

    }
}
