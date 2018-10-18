using AutoMapper;
using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
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
            // manually map: apoinDto and appointment (entity)
            Appointment appointment = new Appointment();
            appointment.PatientId = appoinDto.PatientId;
            appointment.SampleGettings = new List<SampleGetting>();
            foreach (var sampleGettingDto in appoinDto.SampleGettingDtos)
            {
                var sampleGetting = new SampleGetting();
                sampleGetting.SampleId = sampleGettingDto.SampleId;
                sampleGetting.StartTime = sampleGettingDto.StartTime;
                sampleGetting.FinishTime = sampleGettingDto.FinishTime;
                appointment.SampleGettings.Add(sampleGetting);
            }
            appointment.LabTestings = new List<LabTesting>();
            if (appoinDto.SampleGettingDtos != null)
            {
                foreach (var sampleGettingDto in appoinDto.SampleGettingDtos)
                {
                    if (sampleGettingDto.LabTestIds != null)
                    {
                        foreach (var labTestId in sampleGettingDto.LabTestIds)
                        {
                            LabTesting labTesting = new LabTesting();
                            labTesting.LabTestId = labTestId;
                            appointment.LabTestings.Add(labTesting);
                        }
                    }
                }
            }
            //
            var success = this._appointmentService.Create(appointment);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, success);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-new-appointment-by-patient-id")]
        public HttpResponseMessage GetNewAppointment(int patientId)
        {
            var app = _appointmentService.GetNewApp(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-old-appointment-by-patient-id")]
        public HttpResponseMessage GetOldAppointment(int patientId)
        {
            var app = _appointmentService.GetOldApp(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }

        [HttpGet]
        [Route("api/appointment/get-result-by-patient-id")]
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
            var app = _appointmentService.GetAppByPhoneNDate(phone);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentGetByPhoneAndDateDto>>(app);
            var response = Request.CreateResponse(HttpStatusCode.OK, appDtos);
            return response;
        }
    }
}
