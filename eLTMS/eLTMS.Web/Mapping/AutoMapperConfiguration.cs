﻿using AutoMapper;
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
                .ForMember(dst => dst.sampleId, src => src.MapFrom(x => x.SampleId))
                .ForMember(dst => dst.sampleName, src => src.MapFrom(x => x.SampleName))
                .ForMember(dst => dst.labTests, src => src.MapFrom(x => x.LabTests))
                .ForMember(dst => dst.sampleDuration, src => src.MapFrom(x => x.SampleGroup.GettingDuration));

                cfg.CreateMap<LabTest, LabTestDto>()
                .ForMember(dst => dst.LabTestId, src => src.MapFrom(x => x.LabTestId))
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTestName))
                .ForMember(dst => dst.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dst => dst.Price, src => src.MapFrom(x => x.Price));
                
            });
            
        }
    }
}