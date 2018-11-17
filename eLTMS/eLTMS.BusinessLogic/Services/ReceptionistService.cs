using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IReceptionistService
    {
        bool Add(AppointmentAddDto data);
        bool ChangeIsPaid(int sampleGettingId);
        List<Appointment> GetAllAppointment();
        List<AppointmentGetBySampleDto> GetAllBySample(int sampleId);
        List<SampleGetting> GetSampleGettingsBySampleGroupId(int sampleGroupId); // DucBM
    }
    class ReceptionistService : IReceptionistService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ReceptionistService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        //ten ten
        public string CreateAppReturnCode(Appointment appointment)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            try
            {
                // Convert AppointmentDto to Appointment
                var now = DateTime.Now;
                var sDate = now.ToString("yyyy-MM-dd");
                var count = appointmentRepo.CountByDate(sDate);
                var code = sDate + "-" + count;
                appointment.AppointmentCode = code;
                appointment.Status = "NEW";
                // Create
                appointmentRepo.Create(appointment);
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return appointment.AppointmentCode;
        }

        //ten ten
        public bool ChangeIsPaid(int sampleGettingId)
        {
            try
            {
                var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
                var sampleGetting = sgRepo.GetFirst(p => p.SampleGettingId == sampleGettingId);
                sampleGetting.IsPaid = true;
                sgRepo.Update(sampleGetting);
                UnitOfWork.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }
        
        //ten ten
        public bool Add(AppointmentAddDto data)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var accRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            try
            {
                //tao account
                //accRepo.Create(new Account
                //{
                //    PhoneNumber = data.Phone,

                //    //passWord default: qwe123
                //    Password = "qwe123",
                //    IsDeleted = false
                //});
                //UnitOfWork.SaveChanges();
                
                //tao benh nhan
                //var accId = accRepo.GetByPhoneNumber(data.Phone).AccountId;
                paRepo.Create(new Patient
                {
                    //AccountId = accId,
                    IsOnline = false,
                    IdentityCardNumber = data.IdentityCardNumber,
                    DateOfBirth = data.DateOfBirth,
                    HomeAddress = data.Address,
                    FullName = data.Name,
                    PhoneNumber = data.Phone,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //tao cuoc hen
                var paId = paRepo.GetFirst(p=>p.PhoneNumber==data.Phone).PatientId;
                var appCode= CreateAppReturnCode(new Appointment
                {
                    PatientId = paId,
                    Date= DateTime.Now.Date,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //xep lich hen cho tung loai xet nghiem
                var appId = appRepo.GetFirst(p=>p.AppointmentCode==appCode).AppointmentId;
                if (data.Mau == true)
                {
                    //tim slot va ban trong
                    var slotAndTable = GetEmptyTableAndSlot(1);
                    //neu con ban va slot trong thi moi tao lich hen
                    if (slotAndTable != null)
                    {
                        //tao lich hen loai xet nghiem mau 
                        sgRepo.Create(new SampleGetting
                        {
                            AppointmentId = appId,
                            SampleId = 1,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                    
                }
                if (data.NuocTieu == true)
                {
                    //tim slot va ban trong
                    var slotAndTable = GetEmptyTableAndSlot(1);
                    //neu con ban va slot trong thi moi tao lich hen
                    if (slotAndTable != null)
                    {
                        sgRepo.Create(new SampleGetting
                        {
                            AppointmentId = appId,
                            SampleId = 2,
                            SlotId=slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                }
                if (data.TeBaoHoc == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(2);
                    if(slotAndTable!=null) {
                        sgRepo.Create(new SampleGetting
                        {
                            AppointmentId = appId,
                            SampleId = 3,
                            SlotId= slotAndTable.slotId,
                            TableId= slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                }
                if (data.Phan == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(3);
                    if (slotAndTable != null)
                    {
                        sgRepo.Create(new SampleGetting
                        {
                            AppointmentId = appId,
                            SampleId = 4,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                }
                if (data.Dich == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(4);
                    if (slotAndTable != null)
                    {
                        sgRepo.Create(new SampleGetting
                        {
                            AppointmentId = appId,
                            SampleId = 5,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                }

                UnitOfWork.SaveChanges();
            }
            catch (Exception ex) { return false; }
            return true;
        }
        
        //ten ten
        public List<Appointment> GetAllAppointment()
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAllApp();
            //var sampleGettingRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
            //foreach(var app in apps)
            //{
            //    app.SampleGettings= sampleGettingRepo.GetAll().Where(p=>p.Appointment.ap)
            //}
            return apps;
        }

        //ten ten
        public class TableAndSlotId
        {
            public int tableId { get; set; }
            public int slotId { get; set; }
        }

        //ten ten
        public TableAndSlotId GetEmptyTableAndSlot(int sampleGroupId)
        {
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var tabRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);
            var tabs = tabRepo.GetAllTable().Where(p=>p.SampleGroupId==sampleGroupId);
            var sgRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
            foreach (var tab in tabs)
            {
                var sg = sgRepo.GetAll().Where(p => p.TableId == tab.TableId).ToList();
                var apps = appRepo.GetAll();
                var sgApp = sg.Join(apps, p => p.AppointmentId, c => c.AppointmentId, (p, c) => new
                {
                    p.TableId,
                    p.SlotId,
                    c.Date
                }).ToList()
                .Where(p=>p.Date==DateTime.Now.Date).ToList();
                if (sampleGroupId == 1)
                {
                    if (sgApp.Count >= 30) break;
                    else
                    {
                        var slots = slotRepo.GetAll().Where(p => (p.FinishTime - p.StartTime) == 600)
                            .OrderBy(p=>p.StartTime).ToList();
                        return new TableAndSlotId
                        {
                            tableId = tab.TableId,
                            slotId = slots[sgApp.Count].SlotId
                        };
                    }
                }
                else if (sampleGroupId == 2)
                {
                    if (sgApp.Count >= 21) break;
                    else
                    {
                        var slots = slotRepo.GetAll().Where(p => (p.FinishTime - p.StartTime) == 900)
                            .OrderBy(p => p.StartTime).ToList();
                        return new TableAndSlotId
                        {
                            tableId = tab.TableId,
                            slotId = slots[sgApp.Count].SlotId
                        };
                    }
                }
                else if (sampleGroupId == 3 || sampleGroupId == 4)
                {
                    if (sgApp.Count >= 16) break;
                    else
                    {
                        var slots = slotRepo.GetAll().Where(p => (p.FinishTime - p.StartTime) == 1200)
                            .OrderBy(p => p.StartTime).ToList();
                        return new TableAndSlotId
                        {
                            tableId = tab.TableId,
                            slotId = slots[sgApp.Count].SlotId
                        };
                    }
                }
            }
            //return null co nghia la het slot het ban
            return null;
        }

        public List<AppointmentGetBySampleDto> GetAllBySample(int sampleId)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll();
            var pas = paRepo.GetAll();
            var sgs = sgRepo.GetAll().Where(p => p.SampleId == sampleId);
            var sps = spRepo.GetAll();
            var slots = slotRepo.GetAll();
            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new {
                app=p,
                pa=c
            });
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new {
                sg=p,
                sp=c
            });
            var spSgSlots= spSgs.Join(slots, p => p.sg.SlotId, c => c.SlotId, (p, c) => new
            {
                spSg=p,
                slot=c
            });
            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new AppointmentGetBySampleDto
                {
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),
                    SampleName = p.spSg.sp.SampleName,
                    AppointmentCode = c.app.AppointmentCode,
                    Phone = c.pa.PhoneNumber,
                    Address = c.pa.HomeAddress,
                    PatientName = c.pa.FullName,
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    IsPaid = p.spSg.sg.IsPaid

                }).ToList();
            return result;
        }

        // DucBM
        public List<SampleGetting> GetSampleGettingsBySampleGroupId(int sampleGroupId)
        {
            var repo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var result = repo.GetBySampleGroupIdForReceptionist(sampleGroupId);
            return result;
        }

    }
}
