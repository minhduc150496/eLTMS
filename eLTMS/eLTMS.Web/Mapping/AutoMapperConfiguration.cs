using AutoMapper;
using eLTMS.DataAccess.Models;
using eLTMS.Web.Models.dto;
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
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note));
            });
        }
    }
}