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
        List<AppointmentGetBySampleDto> GetAllBySample(DateTime date, int sampleId);
        int CheckAndDeleteMauAndNuocTieu(DateTime dateTime);
        int CheckAndDeleteTeBaoAndDich(DateTime dateTime);
        int CheckAndDeletePhan(DateTime dateTime);
        //DUCBM
        List<SampleGetting> GetSampleGettingsBySampleGroupId(int sampleGroupId);
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
            catch (Exception e)
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
                var paId = paRepo.GetFirst(p => p.IdentityCardNumber == data.IdentityCardNumber).PatientId;
                var appCode = CreateAppReturnCode(new Appointment
                {
                    PatientId = paId,
                    Date = DateTime.Now.Date,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //xep lich hen cho tung loai xet nghiem
                var appId = appRepo.GetFirst(p => p.AppointmentCode == appCode).AppointmentId;
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
                            GettingDate = DateTime.Now.Date,
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
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 2,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        });
                    }
                }
                if (data.TeBaoHoc == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(2);
                    if (slotAndTable != null)
                    {
                        sgRepo.Create(new SampleGetting
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 3,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
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
                            GettingDate = DateTime.Now.Date,
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
                            GettingDate = DateTime.Now.Date,
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

        public TableAndSlotId GetEmptyTableAndSlot(int sampleGroupId)
        {
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var tabRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);
            var sgRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);

            //lay gio hien tai
            var nowTime = DateTime.Now;
            var nowTimeToSecond = nowTime.Hour * 3600 + nowTime.Minute * 60 + nowTime.Second;
            //lay tat ca slot
            var slots = slotRepo.GetAll().Where(p => p.StartTime >= nowTimeToSecond
                            && p.SampleGroupId==sampleGroupId)
                            .OrderBy(p => p.StartTime).ToList();
            var tables = tabRepo.GetAll()
                            .Where(p => p.SampleGroupId == sampleGroupId)
                            .OrderBy(p => p.TableId).ToList();
            foreach (var slot in slots)
            {
                var sgs = sgRepo.GetAll().Where(p => p.SlotId == slot.SlotId && p.GettingDate == DateTime.Now.Date).ToList();
                //mau va nuoc tieu
                if (sampleGroupId == 1)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                //neu co 9 cuoc hen ma co 10 cai ban thi lay cai ban stt la 9(tuc la cai ban thu 10) do c# list dem tu 0
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                else if (sampleGroupId == 2)
                {
                    if (sgs.Count < tables.Count) 
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                else if (sampleGroupId == 3)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                else if (sampleGroupId == 4)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
            }
            //return null co nghia la het slot het ban
            return null;
        }

        public List<AppointmentGetBySampleDto> GetAllBySample(DateTime date, int sampleId)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true);
            var sgs = sgRepo.GetAll().Where(p => p.SampleId == sampleId && p.IsDeleted != true && p.GettingDate == date);
            var sps = spRepo.GetAll().Where(p => p.IsDeleted != true);
            var slots = slotRepo.GetAll();
            var tables = tableRepo.GetAll();
            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                app = p,
                pa = c
            });
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new
            {
                sg = p,
                sp = c
            });
            var spSgSlots = spSgs.Join(slots, p => p.sg.SlotId, c => c.SlotId, (p, c) => new
            {
                spSg = p,
                slot = c
            });
            //var spSgSlotTable= spSgSlots.Join(tables, p => p.spSg.sg.TableId, c => c.TableId, (p, c) => new
            //{
            //    spSgSlot = p,
            //    table = c
            //});
            var count = 1;
            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new AppointmentGetBySampleDto
                {
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),
                    SampleName = p.spSg.sp.SampleName,
                    AppointmentCode = c.app.AppointmentCode,
                    OrderNumber = count++,
                    Phone = c.pa.PhoneNumber,
                    Address = c.pa.HomeAddress,
                    PatientName = c.pa.FullName,
                    Date = p.spSg.sg.GettingDate.Value.ToShortDateString(),
                    //Table = p.table.TableName,
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    IsPaid = p.spSg.sg.IsPaid

                }).ToList();
            return result;
        }

        public int CheckAndDeleteMauAndNuocTieu(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            //lay so giay trong slot
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var apps = appRepo.GetAll().ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 1 || p.SampleId == 2)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CheckAndDeleteTeBaoAndDich(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var apps = appRepo.GetAll().ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid == false && (p.SampleId == 3 || p.SampleId == 5)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public int CheckAndDeletePhan(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var apps = appRepo.GetAll().ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid == false && (p.SampleId == 4)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }

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
