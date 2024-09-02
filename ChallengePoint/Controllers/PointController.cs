using Asp.Versioning;
using AutoMapper;
using ChallengePoint.Application.ViewModel;
using ChallengePoint.Domain.DTO;
using ChallengePoint.Domain.Interface;
using ChallengePoint.Domain.Models;
using ChallengePoint.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ChallengePoint.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PointController(IPointRepository pointRepository, ICollaboratorRepository collaboratorRepository, IMapper mapper) : ControllerBase
    {
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly ICollaboratorRepository _collaboratorRepository = collaboratorRepository;
        private readonly IMapper _mapper = mapper;

        [HttpPost("clockin")]
        public async Task<IActionResult> ClockIn([FromBody] ClockInViewModel clockIn)
        {
            try
            {
                // Verifica se o horário de entrada foi fornecido
                if (string.IsNullOrEmpty(clockIn.ClockIn))
                {
                    return BadRequest("ClockIn time is required.");
                }

                // Verifica se a matrícula foi fornecida
                if (string.IsNullOrEmpty(clockIn.Enrollment))
                {
                    return BadRequest("Enrollment is required.");
                }

                var collaborator = await _collaboratorRepository.GetByEnrollmentAsync(clockIn.Enrollment);
                if (collaborator == null)
                {
                    return NotFound("Collaborator not found.");
                }

                DateTime clockInDateTime;
                try
                {
                    clockInDateTime = HourUtil.ConvertIsoToDateTime(clockIn.ClockIn);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid ClockIn time format.");
                }

                // Verifica se é um dia útil
                if (!IsBusinessDay(clockInDateTime))
                {
                    return BadRequest("Clock-in is only allowed on business days (Monday to Friday).");
                }

                // Verifica se está dentro do horário comercial
                if (!IsWithinBusinessHours(clockInDateTime))
                {
                    return BadRequest("Clock-in is only allowed between 08:00 and 18:00.");
                }

                var existingClockIn = await _pointRepository.GetClockInByDateAsync(collaborator.Id, clockInDateTime.Date);
                // Verifica se já existe um registro de entrada para o dia
                if (existingClockIn != null)
                {
                    return BadRequest("Clock-in already recorded for today.");
                }

                var lastClockIn = await _pointRepository.GetLastClockInAsync(collaborator.Id);
                // Verifica se há um registro de entrada sem saída correspondente
                if (lastClockIn != null && lastClockIn.ClockOut == null)
                {
                    var clockOutViewModel = new ClockOutViewModel
                    {
                        clockOut = DateTime.Now.ToString("o"),
                        Enrollment = clockIn.Enrollment
                    };
                    await ClockOut(clockOutViewModel);
                }

                var timekeepingModel = new TimekeepingModel
                {
                    ClockIn = clockInDateTime,
                    CollaboratorId = collaborator.Id,
                    ServerTimestampIn = DateTime.Now
                };

                await _pointRepository.AddPointAsync(timekeepingModel);
                return Ok();
            }
            catch (Exception ex)
            {
                // Logar o erro ex se necessário
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost("clockout")]
        public async Task<IActionResult> ClockOut([FromBody] ClockOutViewModel clockOut)
        {
            try
            {
                // Verifica se o horário de saída foi fornecido
                if (string.IsNullOrEmpty(clockOut.clockOut))
                {
                    return BadRequest("ClockOut time is required.");
                }

                var endTime = DateTime.Parse(clockOut.clockOut);
                // Verifica se é um dia útil
                if (endTime.DayOfWeek == DayOfWeek.Saturday || endTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest("Clock-in is only allowed on business days (Monday to Friday).");
                }

                // Verifica se a matrícula foi fornecida
                if (string.IsNullOrEmpty(clockOut.Enrollment))
                {
                    return BadRequest("Enrollment is required.");
                }

                var collaborator = await _collaboratorRepository.GetByEnrollmentAsync(clockOut.Enrollment);
                if (collaborator == null)
                {
                    return NotFound("Collaborator not found.");
                }

                var clockIn = await _pointRepository.GetClockInForCollaboratorAndDayAsync(
                    collaborator.Id,
                    endTime.Date);

                if (clockIn == null)
                {
                    return NotFound("No clock-in found for the specified collaborator and day.");
                }

                // Verifica se o clockOut já foi registrado
                if (clockIn.ClockOut != null)
                {
                    return BadRequest("Clock-out already recorded for this clock-in.");
                }

                DateTime clockOutDateTime;
                try
                {
                    clockOutDateTime = HourUtil.ConvertIsoToDateTime(clockOut.clockOut);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid clockOut format.");
                }

                // Verifica se é um dia útil
                if (!IsBusinessDay(clockOutDateTime))
                {
                    return BadRequest("Clock-out is only allowed on business days (Monday to Friday).");
                }

                // Verifica se está dentro do horário comercial (com tolerância após as 18:00)
                if (!IsWithinBusinessHours(clockOutDateTime, allowAfterHours: true))
                {
                    return BadRequest("Clock-out is only allowed between 08:00 and 18:00 (with some tolerance after 18:00).");
                }

                // Atualiza o registro de entrada existente com o horário de saída
                clockIn.ClockOut = clockOutDateTime;
                clockIn.ServerTimestampOut = DateTime.Now;

                await _pointRepository.UpdatePointAsync(clockIn);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                var timekeepingRecords = await _pointRepository.GetAllPointsAsync();

                if (timekeepingRecords == null || !timekeepingRecords.Any())
                {
                    return NotFound("No timekeeping records found.");
                }

                var timekeepingDtos = _mapper.Map<IEnumerable<TimekeepingDTO>>(timekeepingRecords);
                return Ok(timekeepingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        private static bool IsBusinessDay(DateTime dateTime) => dateTime.DayOfWeek >= DayOfWeek.Monday && dateTime.DayOfWeek <= DayOfWeek.Friday;

        private static bool IsWithinBusinessHours(DateTime dateTime, bool allowAfterHours = false)
        {
            TimeSpan startTime = new(8, 0, 0); // 08:00
            TimeSpan endTime = new(18, 0, 0); // 18:00

            if (allowAfterHours)
            {
                endTime = endTime.Add(new TimeSpan(0, 30, 0)); // Permite 30 minutos adicionais
            }

            return dateTime.TimeOfDay >= startTime && dateTime.TimeOfDay <= endTime;
        }
    }
}