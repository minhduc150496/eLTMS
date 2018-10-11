using AutoMapper;
using eLTMS.Web.Models.dto;
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
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName))
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.Sample.SampleGroupId));

                cfg.CreateMap<Appointment, AppointmentDto>()
                .ForMember(dst => dst.AppId, src => src.MapFrom(x => x.AppointmentId))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.SampleGetting, src => src.MapFrom(x => x.SampleGettings));




            });
            
        }
    }
}