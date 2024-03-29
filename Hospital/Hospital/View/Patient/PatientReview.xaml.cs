using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Controller;
using Model;
using DTO;
using System;
using System.Windows;
using Repository;

namespace Hospital
{
    /// <summary>
    /// Interaction logic for PatientReview.xaml
    /// </summary>
    public partial class PatientReview : Window
    {
        AppointmentController appointmentController = new AppointmentController();
        EmployeeController employeeController = new EmployeeController();
        public PatientReview()
        {
            InitializeComponent();
            AppointmentsListView.ItemsSource = appointmentController.GetAppointmentsFromPast("5");



        }

        Review getReviewFromForm()
        {
            Review review;
                
                int information = Int32.Parse(informationTextBox.Text);
                int speed = Int32.Parse(speedTextBox.Text);
                int overall = Int32.Parse(overallTextBox.Text);
                int kindness = Int32.Parse(kindnessTextBox.Text);
                string description = descriptionTextBox.Text;

            //if (isBetween(0, information, 10) &&
            //   isBetween(0, speed, 10) &&
            //   isBetween(0, overall, 10) &&
            //   isBetween(0, kindness, 10))
            //{


            //}   
            return review = new Review(speed, kindness, information, overall, description);




        }

        private bool isBetween(int min, int value, int max)
        {
            if (value >= min && value <= max)
                return true;
            else return false;
        }



        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorRadioButton.IsChecked == true)
            {
                AppointmentDTO appointmentDTO = (AppointmentDTO)AppointmentsListView.SelectedItems[0];
                employeeController.RateDoctor(appointmentDTO.DoctorJmbg, getReviewFromForm());
            }
            else
            {
                ReviewRepository reviewRepository = new ReviewRepository();
                reviewRepository.Save(getReviewFromForm());
                
            }
        }

        private void NewAppointmentButtonClick(object sender, RoutedEventArgs e)
        {
            var new_window = new PatientWindow();
            new_window.Show();
            this.Close();
        }

        private void showAppointments_Click(object sender, RoutedEventArgs e)
        {
            var new_window = new PatientAppointments();
            new_window.Show();
            this.Close();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var new_window = new PatientReview();
            new_window.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var new_window = new PatientReferral();
            new_window.Show();
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var newWindow = new PatientMedicalRecord();
            newWindow.Show();
            //this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var newWindow = new PatientReminder();
            newWindow.Show();
            //this.Close();
        }


    }
}
