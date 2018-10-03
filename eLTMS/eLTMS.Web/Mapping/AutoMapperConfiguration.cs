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

                cfg.CreateMap<Testing, TestingDto>()
                .ForMember(dst => dst.TestingID, src => src.MapFrom(x => x.TestingId))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTest.LabTestName))
                .ForMember(dst => dst.BookedDateString, src => src.MapFrom(x => x.BookedDate.Value.ToShortDateString()))
                .ForMember(dst => dst.BookedTimeString, src => src.MapFrom(x => x.BookedTime.Value.Hours + ":" + x.BookedTime.Value.Minutes));
            });
            
        }
    }
}