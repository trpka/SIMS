using DTO;
using Model;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Service
{
    public class AppointmentService
    {
        private IAppointmentRepository _appointmentRepository = new AppointmentRepository();
        private IEmployeeRepository _employeeRepository = new EmployeeRepository();
        private IPatientRepository _patientRepository = new PatientRepository();
        private IRoomRepository _roomRepository = new RoomRepository();

        public AppointmentService()
        {

        }


        public List<AppointmentDTO> GetAll()
        {
            List<Appointment> appointments = _appointmentRepository.GetAll();
            return ConvertListToDTO(appointments);
        }

        public AppointmentDTO GetById(int id)
        {
            return ConvertToDTO(_appointmentRepository.GetById(id));
        }

        public AppointmentDTO Save(AppointmentDTO appointmentDTO)
        {
            appointmentDTO.Id = GenerateNewId();
            Appointment appointment = ConvertToModel(appointmentDTO);
            _appointmentRepository.Save(appointment);
            AddAppointmentToParticipants(appointment);
            return ConvertToDTO(appointment);
        }

        public void AddAppointmentToParticipants(Appointment appointment)
        {
            AddAppointmentToDoctor(appointment);
            AddAppointmentToPatient(appointment);
            AddAppointmentToRoom(appointment);
        }

        public void AddAppointmentToDoctor(Appointment appointment)
        {
            Employee doctor = _employeeRepository.GetByJmbg(appointment.DoctorJmbg);
            doctor.Appointments.Add(appointment);
            _employeeRepository.Update(doctor);
        }

        public void AddAppointmentToPatient(Appointment appointment)
        {
            Patient patient = _patientRepository.GetByJmbg(appointment.PatientJmbg);
            patient.Appointments.Add(appointment);
            _patientRepository.Update(patient);
        }

        public void AddAppointmentToRoom(Appointment appointment)
        {
            Room room = _roomRepository.GetById(appointment.RoomId);
            room.Appointments.Add(appointment);
            _roomRepository.Update(room);
        }

        public AppointmentDTO Update(AppointmentDTO appointmentDTO)
        {
            Appointment appointment = ConvertToModel(appointmentDTO);
            AppointmentDTO updatedAppointment = ConvertToDTO(_appointmentRepository.Update(appointment));
            UpdateAppointmentParticipants(appointment);
            return updatedAppointment;
        }

        public void UpdateAppointmentParticipants (Appointment appointment)
        {
            UpdateAppointmentForDoctor(appointment);
            UpdateAppointmentForPatient(appointment);
            UpdateAppointmentForRoom(appointment);
        }

        public void UpdateAppointmentForDoctor(Appointment appointment)
        {
            Employee doctor = _employeeRepository.GetByJmbg(appointment.DoctorJmbg);
            int index = doctor.Appointments.FindIndex(obj => obj.Id == appointment.Id);
            doctor.Appointments[index] = appointment;
            _employeeRepository.Update(doctor);
        }

        public void UpdateAppointmentForPatient(Appointment appointment)
        {
            Patient patient = _patientRepository.GetByJmbg(appointment.PatientJmbg);
            int index = patient.Appointments.FindIndex(obj => obj.Id == appointment.Id);
            patient.Appointments[index] = appointment;
            _patientRepository.Update(patient);
        }

        public void UpdateAppointmentForRoom(Appointment appointment)
        {
            Room room = _roomRepository.GetById(appointment.RoomId);
            int index = room.Appointments.FindIndex(obj => obj.Id == appointment.Id);
            room.Appointments[index] = appointment;
            _roomRepository.Update(room);
        }
        
        public AppointmentDTO Delete(int id)
        {
            return ConvertToDTO(_appointmentRepository.Delete(id));
        }

        public List<AppointmentDTO> GetAppointmentsForDoctor(string jmbg)
        {
            return ConvertListToDTO(_appointmentRepository.GetAppointmentsForDoctor(jmbg));
        }

        public List<AppointmentDTO> GetAppointmentsForPatient(string jmbg)
        {
            return ConvertListToDTO(_appointmentRepository.GetAppointmentsForPatient(jmbg));
        }

        public List<AppointmentDTO> GetAppointmentsForRoom(int id)
        {
            return ConvertListToDTO(_appointmentRepository.GetAppointmentsForRoom(id));
        }

        public int GenerateNewId()
        {
            return _appointmentRepository.GenerateNewId();
        }

        public bool IsDoctorAvailable(AppointmentDTO appointment)
        {
            List<AppointmentDTO> appointments = GetAppointmentsForDoctor(appointment.DoctorJmbg);
            return IsTimeSlotFree(appointment, appointments);
        }

        public bool IsPatientAvailable(AppointmentDTO appointment)
        {
            List<AppointmentDTO> appointments = GetAppointmentsForPatient(appointment.PatientJmbg);
            return IsTimeSlotFree(appointment, appointments);
        }

        public bool IsRoomAvailable(AppointmentDTO appointment)
        {
            List<AppointmentDTO> appointments = GetAppointmentsForRoom(appointment.RoomId);
            return IsTimeSlotFree(appointment, appointments);
        }

        public bool IsTimeInFuture(DateTime appointmentStartTime)
        {
            if (appointmentStartTime.Ticks > DateTime.Now.Ticks)
                return true;
            return false;
        }

        public bool IsDateTimeBetween(DateTime dateTimeToCheck, DateTime startTime, DateTime endTime)
        {
            return dateTimeToCheck.Ticks >= startTime.Ticks && dateTimeToCheck.Ticks < endTime.Ticks;
        }

        public bool IsTimeSlotFree(AppointmentDTO appointmentToCheck, List<AppointmentDTO> appointments)
        {
            DateTime appointmentToCheckEndTime = appointmentToCheck.StartTime.AddMinutes(appointmentToCheck.DurationInMinutes);
            foreach (AppointmentDTO appointment in appointments)
            {
                if (appointmentToCheck.Id != appointment.Id)
                {
                    DateTime appointmentEndTime = appointment.StartTime.AddMinutes(appointment.DurationInMinutes);

                    //Provera da li postoji pregled u tom terminu
                    if (IsDateTimeBetween(appointmentToCheck.StartTime, appointment.StartTime, appointmentEndTime) || 
                            IsDateTimeBetween (appointmentToCheckEndTime, appointment.StartTime, appointmentEndTime))
                    {
                        return false;
                    }
                }

            }
            return true;
        }




        // Stari kod, za brisanje kad se prebaci sve na novo
        public bool AppointmentIsTaken(AppointmentDTO appointment, string doctorId)
        {
            List<Appointment> appointments = _appointmentRepository.GetAppointmentsForDoctor(doctorId);

            foreach (Appointment app in appointments)
            {
                if (app.Id != appointment.Id)
                {
                    DateTime endTime = app.StartTime.AddMinutes(app.DurationInMinutes);
                    DateTime appointmentEndTime = appointment.StartTime.AddMinutes(appointment.DurationInMinutes);

                    //Provera da li postoji pregled u tom terminu
                    if ((appointment.StartTime.Ticks >= app.StartTime.Ticks && appointment.StartTime.Ticks <= endTime.Ticks) ||
                        appointmentEndTime.Ticks >= app.StartTime.Ticks && appointmentEndTime.Ticks <= endTime.Ticks)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AppointmentValidationWithoutOverlaping(AppointmentDTO appointment)
        {
            List<Appointment> appointments = _appointmentRepository.GetAll();

            if (!IsTimeInFuture(appointment.StartTime))
            {
                return true;
            }

            foreach (Appointment app in appointments)
            {
                if (app.Id != appointment.Id)
                {
                    DateTime endTime = app.StartTime.AddMinutes(app.DurationInMinutes);
                    DateTime appointmentEndTime = appointment.StartTime.AddMinutes(appointment.DurationInMinutes);


                }
                else
                {
                    //Provera da li je vreme updeta u narednih 24h
                    if (DateTime.Now.AddDays(1).Ticks > app.StartTime.Ticks)
                    {
                        return true;
                    }
                    //Provera da li pomera pregled za datum preko 2 dana kasnije
                    if (app.StartTime.AddDays(2).Ticks < appointment.StartTime.Ticks)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AppointmentTimeIsInvalid(AppointmentDTO appointment)
        {
            List<Appointment> appointments = _appointmentRepository.GetAll();

            if (!IsTimeInFuture(appointment.StartTime))
            {
                return true;
            }

            foreach (Appointment app in appointments)
            {
                if (app.Id != appointment.Id)
                {
                    DateTime endTime = app.StartTime.AddMinutes(app.DurationInMinutes);
                    DateTime appointmentEndTime = appointment.StartTime.AddMinutes(appointment.DurationInMinutes);

                    //Provera da li postoji pregled u tom terminu
                    if ((appointment.StartTime.Ticks >= app.StartTime.Ticks && appointment.StartTime.Ticks <= endTime.Ticks) ||
                        appointmentEndTime.Ticks >= app.StartTime.Ticks && appointmentEndTime.Ticks <= endTime.Ticks)
                    {
                        return true;
                    }
                }
                else
                {
                    //Provera da li je vreme updeta u narednih 24h
                    if (DateTime.Now.AddDays(1).Ticks > app.StartTime.Ticks)
                    {
                        return true;
                    }
                    //Provera da li pomera pregled za datum preko 2 dana kasnije
                    if (app.StartTime.AddDays(2).Ticks < appointment.StartTime.Ticks || app.StartTime.AddDays(-2).Ticks > appointment.StartTime.Ticks)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<AppointmentDTO> GetAppointmentsFromPast(String patientJmbg)
        {
            List<Appointment> appointments = _appointmentRepository.GetAppointmentsForPatient(patientJmbg);
            List<Appointment> appointmentsInPast = new List<Appointment>();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.StartTime < DateTime.Now)
                    appointmentsInPast.Add(appointment);
            }
            return ConvertListToDTO(appointmentsInPast);


        }

        private AppointmentDTO ConvertToDTO(Appointment appointment)
        {
            Employee doctor = _employeeRepository.GetByJmbg(appointment.DoctorJmbg);
            Patient patient = _patientRepository.GetByJmbg(appointment.PatientJmbg);
            Room room = _roomRepository.GetById(appointment.RoomId);

            AppointmentDTO appointmentDTO = new AppointmentDTO
                (
                    appointment.Id,
                    appointment.AppointmentType,
                    appointment.StartTime,
                    appointment.DurationInMinutes,
                    doctor.User.Jmbg,
                    doctor.User.FirstName,
                    doctor.User.LastName,
                    doctor.Specialization,
                    patient.User.Jmbg,
                    patient.User.FirstName,
                    patient.User.LastName,
                    room.Id,
                    room.Name
                );
            return appointmentDTO;
        }

        private List<AppointmentDTO> ConvertListToDTO(List<Appointment> appointments)
        {
            List<AppointmentDTO> appointmentDTOs = new List<AppointmentDTO>();
            foreach (Appointment appointment in appointments)
            {
                appointmentDTOs.Add(ConvertToDTO(appointment));
            }
            return appointmentDTOs;
        }

        private Appointment ConvertToModel(AppointmentDTO appointmentDTO)
        {
            Appointment appointment = new Appointment
                (
                    appointmentDTO.Id,
                    appointmentDTO.AppointmentType,
                    appointmentDTO.StartTime,
                    appointmentDTO.DurationInMinutes,
                    appointmentDTO.PatientJmbg,
                    appointmentDTO.DoctorJmbg,
                    appointmentDTO.RoomId
                );
            return appointment;
        }

        private List<Appointment> ConvertListToModel(List<AppointmentDTO> appointmentsDTOs)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (AppointmentDTO appointmentDTO in appointmentsDTOs)
            {
                appointments.Add(ConvertToModel(appointmentDTO));
            }
            return appointments;
        }
    }
}