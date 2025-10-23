using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using BLL.DTOs;
using BLL.IServices;
using DAL.IRepository;
using DAL.Entities;

namespace BLL.Services
{
    public class TestDriveAppointmentService : ITestDriveAppointmentService
    {
        private readonly ITestDriveAppointmentRepo _repo;
        private readonly IMapper _mapper;
        private readonly IRealtimeNotifier? _notifier;

        // Bạn có thể inject ILogger nếu muốn log
        public TestDriveAppointmentService(
            ITestDriveAppointmentRepo repo,
            IMapper mapper,
            IRealtimeNotifier? notifier = null)
        {
            _repo = repo;
            _mapper = mapper;
            _notifier = notifier;
        }

        // ================== READ ==================

        public async Task<List<TestDriveAppointmentDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAppointments();
            return list.Select(_mapper.Map<TestDriveAppointmentDTO>).ToList();
        }

        public async Task<List<TestDriveAppointmentDTO>> GetByDayAsync(DateTime date)
        {
            var list = await _repo.GetAppointmentsInDay(date);
            return list.Select(_mapper.Map<TestDriveAppointmentDTO>).ToList();
        }

        public async Task<TestDriveAppointmentDTO?> GetByIdAsync(int id)
        {
            // Repo chưa có GetById -> lấy từ danh sách (đủ dùng cho Razor Page)
            var list = await _repo.GetAllAppointments();
            var e = list.FirstOrDefault(x => x.AppointmentId == id);
            return e == null ? null : _mapper.Map<TestDriveAppointmentDTO>(e);
        }

        // ================== WRITE ==================

        public async Task<TestDriveAppointmentDTO> CreateAsync(TestDriveAppointmentDTO dto)
        {
            ValidateBusiness(dto);

            // chống trùng (slot 60', buffer 15')
            var overlap = await _repo.ExistsOverlapAsync(
                dto.CarVersionId, dto.ColorId, dto.DateTime,
                durationMinutes: 60, bufferMinutes: 15);

            if (overlap)
                throw new ArgumentException("Đã có lịch hẹn khác trong khung giờ này.");

            var entity = _mapper.Map<TestDriveAppointment>(dto); // đã ignore navigation trong AutoMapper
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            var result = _mapper.Map<TestDriveAppointmentDTO>(entity);

            if (_notifier != null)
                await _notifier.AppointmentCreated(result);

            return result;
        }

        public async Task UpdateAsync(TestDriveAppointmentDTO dto)
        {
            ValidateBusiness(dto);

            var overlap = await _repo.ExistsOverlapAsync(
                dto.CarVersionId, dto.ColorId, dto.DateTime,
                durationMinutes: 60, bufferMinutes: 15,
                excludeAppointmentId: dto.AppointmentId);

            if (overlap)
                throw new ArgumentException("Lịch hẹn bị trùng thời gian với một lịch khác.");

            // Cách an toàn: load entity rồi set scalar/FK (tránh navigation bị Attach sai)
            var all = await _repo.GetAllAppointments();
            var entity = all.FirstOrDefault(x => x.AppointmentId == dto.AppointmentId);
            if (entity == null)
                throw new InvalidOperationException("Không tìm thấy lịch hẹn.");

            entity.CustomerId = dto.CustomerId;
            entity.CarVersionId = dto.CarVersionId;
            entity.ColorId = dto.ColorId;
            entity.DateTime = dto.DateTime;
            entity.Status = dto.Status;
            entity.Feedback = dto.Feedback;

            _repo.Update(entity);
            await _repo.SaveAsync();

            if (_notifier != null)
                await _notifier.AppointmentUpdated(dto);
        }

        public async Task DeleteAsync(int id)
        {
            var all = await _repo.GetAllAppointments();
            var e = all.FirstOrDefault(x => x.AppointmentId == id);
            if (e == null) return;

            await _repo.DeleteTestDriveAppointments(new List<TestDriveAppointment> { e });

            if (_notifier != null)
                await _notifier.AppointmentDeleted(id);
        }

        // ================== UTIL ==================

        public async Task<bool> IsSlotAvailableAsync(
            int carVersionId,
            int colorId,
            DateTime startLocal,
            int durationMinutes = 60,
            int bufferMinutes = 15,
            int? excludeAppointmentId = null)
        {
            var overlap = await _repo.ExistsOverlapAsync(
                carVersionId, colorId, startLocal,
                durationMinutes, bufferMinutes, excludeAppointmentId);
            return !overlap;
        }

        // ================== BUSINESS RULES ==================

        private static void ValidateBusiness(TestDriveAppointmentDTO dto)
        {
            // 1) Không cho đặt trong quá khứ
            if (dto.DateTime <= DateTime.Now)
                throw new ArgumentException("Thời điểm lái thử phải ở tương lai.");

            // 2) Khung giờ làm việc + thời lượng slot (mặc định)
            var open = new TimeSpan(8, 0, 0);      // 08:00
            var close = new TimeSpan(17, 30, 0);   // 17:30
            const int durationMinutes = 60;        // slot 60'

            var start = dto.DateTime;
            var end = start.AddMinutes(durationMinutes);

            // 2a) Slot phải nằm trong giờ làm việc
            if (start.TimeOfDay < open || end.TimeOfDay > close)
            {
                var openStr = open.ToString(@"hh\:mm");
                var closeStr = close.ToString(@"hh\:mm");
                throw new ArgumentException(
                    $"Thời gian đặt lịch hợp lệ: {openStr} – {closeStr} (mỗi lượt {durationMinutes} phút).");
            }

            // 2b) Slot PHẢI kết thúc trong cùng ngày (không qua ngày hôm sau)
            if (end.Date != start.Date)
                throw new ArgumentException("Chỉ cho phép lái thử trong ngày (không qua ngày hôm sau).");
        }
    }
}
