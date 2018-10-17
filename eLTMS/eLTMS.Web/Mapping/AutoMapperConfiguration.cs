using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Web.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Supply, SupplyDto>()
                .ForMember(dst => dst.SuppliesId, src => src.MapFrom(x => x.SuppliesId))
                .ForMember(dst => dst.SuppliesName, src => src.MapFrom(x => x.SuppliesName))
                .ForMember(dst => dst.SuppliesTypeName, src => src.MapFrom(x => x.SupplyType.SuppliesTypeName))
                .ForMember(dst => dst.SuppliesCode, src => src.MapFrom(x => x.SuppliesCode))
                .ForMember(dst => dst.Quantity, src => src.MapFrom(x => x.Quantity))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note));

                cfg.CreateMap<Sample, SampleDto>()
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.SampleName))
                .ForMember(dst => dst.labTests, src => src.MapFrom(x => x.LabTests));

                cfg.CreateMap<LabTest, LabTestDto>()
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTestName))
                .ForMember(dst => dst.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dst => dst.Price, src => src.MapFrom(x => x.Price));

                cfg.CreateMap<SampleGetting, SampleGettingDto>()
                .ForMember(dst => dst.FinishTime, src => src.MapFrom(x => x.FinishTime))
                .ForMember(dst => dst.StartTime, src => src.MapFrom(x => x.StartTime))
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleId))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName));

                cfg.CreateMap<Appointment, AppointmentDto>()
                .ForMember(dst => dst.AppCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.testPurpose, src => src.MapFrom(x => x.TestPurpose))
                .ForMember(dst => dst.SampleGetting, src => src.MapFrom(x => x.SampleGettings));

                cfg.CreateMap<Appointment, AppointmentGetByPhoneAndDateDto>()
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.Patient.PhoneNumber))
                .ForMember(dst => dst.Address, src => src.MapFrom(x => x.Patient.HomeAddress))
                .ForMember(dst => dst.AppointmentCode, src => src.MapFrom(x => x.AppointmentCode));
                cfg.CreateMap<Appointment, AppointmentGetResultDto>()
                .ForMember(dst => dst.AppCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.Age, src => src.MapFrom(x => x.Patient.DateOfBirth))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Patient.Gender))
                .ForMember(dst => dst.DoctorName, src => src.MapFrom(x => x.Employee.FullName))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Patient.Gender))
                .ForMember(dst => dst.TestPurpose, src => src.MapFrom(x => x.TestPurpose))
                .ForMember(dst => dst.EnterTime, src => src.MapFrom(x => x.EnterTime))
                .ForMember(dst => dst.ReturnTime, src => src.MapFrom(x => x.ReturnTime))
                .ForMember(dst => dst.Conclusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.ResultApproved, src => src.MapFrom(x => x.ResultApproved))
                .ForMember(dst => dst.LabTestingDtos, src => src.MapFrom(x => x.LabTestings));

                cfg.CreateMap<Appointment, AppointmentUpdateDto>()
                .ForMember(dst => dst.AppCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientId, src => src.MapFrom(x => x.PatientId))
                .ForMember(dst => dst.DoctorId, src => src.MapFrom(x => x.DoctorId))
                .ForMember(dst => dst.IsEmergency, src => src.MapFrom(x => x.IsEmergency))
                .ForMember(dst => dst.TestPurpose, src => src.MapFrom(x => x.TestPurpose))
                .ForMember(dst => dst.EnterTime, src => src.MapFrom(x => x.EnterTime))
                .ForMember(dst => dst.ReturnTime, src => src.MapFrom(x => x.ReturnTime))
                .ForMember(dst => dst.Conlusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.ResultApproved, src => src.MapFrom(x => x.ResultApproved))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status));

                cfg.CreateMap<LabTestingIndex, LabTestingIndexDto>()
                .ForMember(dst => dst.IndexName, src => src.MapFrom(x => x.IndexName))
                .ForMember(dst => dst.IndexValue, src => src.MapFrom(x => x.IndexValue))
                .ForMember(dst => dst.LowNormalHigh, src => src.MapFrom(x => x.LowNormalHigh))
                .ForMember(dst => dst.NormalRange, src => src.MapFrom(x => x.NormalRange))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit));

                cfg.CreateMap<LabTesting, LabTestingDto>()
               .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTest.LabTestName))
               .ForMember(dst => dst.LabTestingIndexDtos, src => src.MapFrom(x => x.LabTestingIndexes));
            });

        }
    }
}