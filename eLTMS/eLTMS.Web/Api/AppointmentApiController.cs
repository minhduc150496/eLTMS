using eLTMS.BusinessLogic.Services;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models;
using eLTMS.Web.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public HttpResponseMessage Login(AppointmentDto appoinDto)
        {
            // manually map: apoinDto and appointment (entity)
            Appointment appointment = new Appointment();
            appointment.PatientId = appoinDto.PatientId;
            appointment.SampleGettings = new List<SampleGetting>();
            foreach (var sampleGettingDto in appoinDto.Samples)
            {
                var sampleGetting = new SampleGetting();
                sampleGetting.SampleId = sampleGettingDto.SampleId;
                sampleGetting.StartTime = sampleGettingDto.StartTime;
                sampleGetting.FinishTime = sampleGettingDto.FinishTime;
                appointment.SampleGettings.Add(sampleGetting);
            }
            appointment.LabTestings = new List<LabTesting>();
            foreach (var sampleGettingDto in appoinDto.Samples)
            {
                foreach (var labTestId in sampleGettingDto.LabTestIds)
                {
                    LabTesting labTesting = new LabTesting();
                    labTesting.LabTestId = labTestId;
                    appointment.LabTestings.Add(labTesting);
                }
            }
            //
            var success = this._appointmentService.Create(appointment);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, success);
            return response;
        }
    }
}
