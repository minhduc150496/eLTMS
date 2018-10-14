using AutoMapper;
using eLTMS.AdminWeb.Models.dto;
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
                .ForMember(dst => dst.SuppliesTypeId, src => src.MapFrom(x => x.SuppliesTypeId))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note));

                cfg.CreateMap<Patient, PatientDto>()
                .ForMember(dst => dst.PatientId, src => src.MapFrom(x => x.PatientId))
                .ForMember(dst => dst.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Gender))
                .ForMember(dst => dst.PatientCode, src => src.MapFrom(x => x.PatientCode))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dst => dst.HomeAddress, src => src.MapFrom(x => x.HomeAddress))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.CompanyAddress, src => src.MapFrom(x => x.CompanyAddress));

                cfg.CreateMap<ImportPaper, ImportPaperDto>()
                .ForMember(dst => dst.ImportPaperId, src => src.MapFrom(x => x.ImportPaperId))
                .ForMember(dst => dst.ImportPaperCode, src => src.MapFrom(x => x.ImportPaperCode))
                .ForMember(dst => dst.CreateDate, src => src.MapFrom(x => x.CreateDate))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.ImportPaperDetailDtos, src => src.MapFrom(x => x.ImportPaperDetails));

                cfg.CreateMap<ImportPaperDetail, ImportPaperDetailDto>()
                .ForMember(dst => dst.ImportPaperDetailId, src => src.MapFrom(x => x.ImportPaperDetailId))
                .ForMember(dst => dst.ImportPaperId, src => src.MapFrom(x => x.ImportPaperId))
                .ForMember(dst => dst.SuppliesId, src => src.MapFrom(x => x.SuppliesId))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit))
                .ForMember(dst => dst.Quantity, src => src.MapFrom(x => x.Quantity))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted));

                cfg.CreateMap<ExportPaper, ExportPaperDto>()
                .ForMember(dst => dst.ExportPaperId, src => src.MapFrom(x => x.ExportPaperId))
                .ForMember(dst => dst.ExportPaperCode, src => src.MapFrom(x => x.ExportPaperCode))
                .ForMember(dst => dst.CreateDate, src => src.MapFrom(x => x.CreateDate))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.ExportPaperDetailDtos, src => src.MapFrom(x => x.ExportPaperDetails));

                cfg.CreateMap<ExportPaperDetail, ExportPaperDetailDto>()
                .ForMember(dst => dst.ExportPaperDetailId, src => src.MapFrom(x => x.ExportPaperDetailId))
                .ForMember(dst => dst.ExportPaperId, src => src.MapFrom(x => x.ExportPaperId))
                .ForMember(dst => dst.SuppliesId, src => src.MapFrom(x => x.SuppliesId))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit))
                .ForMember(dst => dst.Quantity, src => src.MapFrom(x => x.Quantity))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted));

                cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember(dst => dst.EmployeeID, src => src.MapFrom(x => x.EmployeeId))
                .ForMember(dst => dst.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dst => dst.Role, src => src.MapFrom(x => x.Account.Role))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Gender))
                .ForMember(dst => dst.HomeAddress, src => src.MapFrom(x => x.HomeAddress))
                .ForMember(dst => dst.DateOfStart, src => src.MapFrom(x => x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToString("dd-MM-yyyy") : ""));
            });
        }
    }
}