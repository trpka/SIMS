using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Repository
{
    public class AppointmentRepository
    {
        private readonly string fileLocation = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Data\\appointments.json";
        private List<Appointment> appointments = new List<Appointment>();

        public AppointmentRepository()
        {
            if (!File.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            using (StreamReader r = new StreamReader(fileLocation))
            {
                string json = r.ReadToEnd();
                if (json != "")
                {
                    appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
                }
            }
        }

        public List<Appointment> GetAll()
        {
            return appointments;
        }

        public void WriteToJson()
        {
            string json = JsonConvert.SerializeObject(appointments);
            File.WriteAllText(fileLocation, json);
        }

        public Appointment GetById(int id)
        {
            return appointments.Find(obj => obj.Id == id);
        }

        public bool UniqueId(int id)
        {
            if (appointments.FindIndex(obj => obj.Id == id) == -1)
                return true;
            else
                return false;
        }

        public void Save(Appointment appointment)
        {
            appointments.Add(appointment);
            WriteToJson();
        }

        public void Delete(int id)
        {
            int index = appointments.FindIndex(obj => obj.Id == id);
            appointments.RemoveAt(index);
            WriteToJson();
        }

        public void Update(Appointment appointment)
        {
            int index = appointments.FindIndex(obj => obj.Id == appointment.Id);
            appointments[index] = appointment;
            WriteToJson();
        }

        public List<Appointment> GetAppointmentsForDoctor(String jmbg)
        {
            return appointments.FindAll(appointment => appointment.Doctor.User.Jmbg == jmbg);
        }

        public List<Appointment> GetAppointmentsForPatient(String jmbg)
        {
            return appointments.FindAll(appointment => appointment.Patient.User.Jmbg == jmbg);
        }

    }
}