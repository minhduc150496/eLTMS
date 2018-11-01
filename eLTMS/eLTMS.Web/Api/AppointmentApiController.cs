using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using eLTMS.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

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
<<<<<<< HEAD
            // manually map: apoinDto and appointment (entity)
            //Appointment appointment = new Appointment();
            //appointment.PatientId = appoinDto.PatientId;
            //appointment.SampleGettings = new List<SampleGetting>();
            //foreach (var sampleGettingDto in appoinDto.SampleGettingDtos)
            //{
            //    var sampleGetting = new SampleGetting();
            //    sampleGetting.SampleId = sampleGettingDto.SampleId;
            //    sampleGetting.GettingDate = DateTimeUtils.ConvertStringToDate(sampleGettingDto.GettingDate);
            //    sampleGetting.StartTime = sampleGettingDto.StartTime;
            //    sampleGetting.FinishTime = sampleGettingDto.FinishTime;
            //    appointment.SampleGettings.Add(sampleGetting);
            //}
            //appointment.LabTestings = new List<LabTesting>();
            //if (appoinDto.SampleGettingDtos != null)
            //{
            //    foreach (var sampleGettingDto in appoinDto.SampleGettingDtos)
            //    {
            //        if (sampleGettingDto.LabTestIds != null)
            //        {
            //            foreach (var labTestId in sampleGettingDto.LabTestIds)
            //            {
            //                LabTesting labTesting = new LabTesting();
            //                labTesting.LabTestId = labTestId;
            //                appointment.LabTestings.Add(labTesting);
            //            }
            //        }
            //    }
            //}
            ////
            //var success = this._appointmentService.Create(appointment);
            //var obj = new
            //{
            //    Success = success,
            //    Message = success ? "Tạo mới thành công!" : "Xin vui lòng thử lại"
            //};
            //var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return null;
=======
            // Convert AppointmentDto to Appointment (DTO to Entity)
            Appointment appointment = Mapper.Map<AppointmentDto, Appointment>(appoinDto);

            appointment.PatientId = appoinDto.PatientId;
            appointment.SampleGettings = new List<SampleGetting>();
            foreach (var sampleGettingDto in appoinDto.SampleGettingDtos)
            {
                var sampleGetting = new SampleGetting();
                sampleGetting.SampleId = sampleGettingDto.SampleId;
                sampleGetting.GettingDate = DateTimeUtils.ConvertStringToDate(sampleGettingDto.GettingDate);
                try
                {
                    sampleGetting.StartTime = TimeSpan.Parse(sampleGettingDto.StartTime);
                    sampleGetting.FinishTime = TimeSpan.Parse(sampleGettingDto.FinishTime);
                }
                catch (Exception ex)
                {
                    // bao loi 
                }
                foreach (var labTestId in sampleGettingDto.LabTestIds)
                {
                    var labTesting = new LabTesting();
                    labTesting.LabTestId = labTestId;
                    sampleGetting.LabTestings.Add(labTesting);
                }
                appointment.SampleGettings.Add(sampleGetting);
            }
            // call to AppointmentService
            var success = this._appointmentService.Create(appointment);
            var obj = new
            {
                Success = success,
                Message = success ? "Tạo mới thành công!" : "Có lỗi xảy ra. Xin vui lòng thử lại"
            };
            var response = Request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
>>>>>>> 74cc0cdad23dc50a11c25ae504a8a36a1e7922c5
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
