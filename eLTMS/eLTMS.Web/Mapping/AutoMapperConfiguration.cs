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
                .ForMember(dst => dst.sampleName, src => src.MapFrom(x => x.SampleName))
                .ForMember(dst => dst.labTests, src => src.MapFrom(x => x.LabTests));

                cfg.CreateMap<LabTest, LabTestDto>()
                .ForMember(dst => dst.labTestName, src => src.MapFrom(x => x.LabTestName))
                .ForMember(dst => dst.description, src => src.MapFrom(x => x.Description))
                .ForMember(dst => dst.price, src => src.MapFrom(x => x.Price));
                

            });
            
        }
    }
}