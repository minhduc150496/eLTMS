﻿using AutoMapper;
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

                cfg.CreateMap<Sample, SampleDto>()
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.SampleName))
                .ForMember(dst => dst.SampleDuration, src => src.MapFrom(x => x.SampleGroup.GettingDuration))
                .ForMember(dst => dst.OpenTime, src => src.MapFrom(x => x.SampleGroup.OpenTime))
                .ForMember(dst => dst.CloseTime, src => src.MapFrom(x => x.SampleGroup.CloseTime))
                .ForMember(dst => dst.LabTests, src => src.MapFrom(x => x.LabTests));

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
                cfg.CreateMap<SampleGetting, SampleGettingDto>()
                .ForMember(dst => dst.FinishTime, src => src.MapFrom(x => x.FinishTime))
                .ForMember(dst => dst.StartTime, src => src.MapFrom(x => x.StartTime))
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleId))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName));

                cfg.CreateMap<ExportPaper, ExportPaperDto>()
                .ForMember(dst => dst.ExportPaperId, src => src.MapFrom(x => x.ExportPaperId))
                .ForMember(dst => dst.ExportPaperCode, src => src.MapFrom(x => x.ExportPaperCode))
                .ForMember(dst => dst.CreateDate, src => src.MapFrom(x => x.CreateDate))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                 .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.ExportPaperDetailDtos, src => src.MapFrom(x => x.ExportPaperDetails));

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

                cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember(dst => dst.EmployeeID, src => src.MapFrom(x => x.EmployeeId))
                .ForMember(dst => dst.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dst => dst.RoleDisplay, src => src.MapFrom(x => GetRoleName(x.Account.Role)))
                .ForMember(dst => dst.Role, src => src.MapFrom(x => x.Account.Role))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Gender))
                .ForMember(dst => dst.HomeAddress, src => src.MapFrom(x => x.HomeAddress))
                .ForMember(dst => dst.DateOfStart, src => src.MapFrom(x => x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToString("dd-MM-yyyy") : ""));
                cfg.CreateMap<LabTesting, LabTestingDto>()
               .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTest.LabTestName))
               .ForMember(dst => dst.LabTestingIndexDtos, src => src.MapFrom(x => x.LabTestingIndexes));
            });

        }
        private static string GetRoleName(string name)
        {
            if (name == "Nurse")
            {
                return "Y tá";
            }
            else if (name == "Doctor")
            {
                return "Bác sĩ";
            }
            else if (name == "Doctor")
            {
                return "Bác sĩ";
            }
            else if (name == "Warehouse")
            {
                return "Quản kho";
            }
            else if (name == "Admin")
            {
                return "Quản lý";
            }
            else if (name == "Lab")
            {
                return "Kỹ thuật viên";
            }
            else if (name == "Receptionist")
            {
                return "Tiếp tân";
            }
            else
            {
                return "";
            }

        }
    }
}