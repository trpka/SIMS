﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Controller;

namespace Hospital.View.Director
{
    /// <summary>
    /// Interaction logic for StaticDetail.xaml
    /// </summary>
    public partial class StaticDetail : Window
    {
        private string pomocnoIme;
        private readonly StaticEquipmentController staticEquipmentController = new StaticEquipmentController();
        private Model.StaticEquipment staticEquipment1;
        public StaticDetail(string name, int quantity, string description)
        {
            InitializeComponent();
            ime.Content = name;
            kolicina.Content = quantity;
            opis.Content = description;
            
            staticEquipment1 = staticEquipmentController.GetName(name);

        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            
            staticEquipmentController.Delete(staticEquipment1.Id);
            MessageBox.Show("Uspesno ste obrisali sobu " + pomocnoIme);
            pomocnoIme = "";
            Back_Click(sender, e);
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Static stat = new Static();
            stat.Show();
            this.Close();
        }
        private void ShowUpdate_Click(object sender, RoutedEventArgs e)
        {
            ime.Visibility = Visibility.Collapsed;
            kolicina.Visibility = Visibility.Collapsed;
            opis.Visibility = Visibility.Collapsed;

            updateName.Visibility = Visibility.Visible;
            updateQuantity.Visibility = Visibility.Visible;
            updateDescription.Visibility = Visibility.Visible;

            updateName.Text = ime.Content.ToString();
            updateQuantity.Text = kolicina.Content.ToString();
            updateDescription.Text = opis.Content.ToString();


            deleteButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
            prihvatiButton.Visibility = Visibility.Visible;
            odustaniButton.Visibility = Visibility.Visible;

        }

        private void Prihvati_Click(object sender, RoutedEventArgs e)
        {
            staticEquipment1.Name = updateName.Text;
            staticEquipment1.Description = updateDescription.Text;
            staticEquipment1.Quantity = Int32.Parse(updateQuantity.Text);

            staticEquipmentController.Update(staticEquipment1);
            MessageBox.Show("Uspesno ste azurirali " + updateName.Text);

            ime.Content = staticEquipment1.Name;
            kolicina.Content = staticEquipment1.Quantity;
            opis.Content = staticEquipment1.Description;

            Ponisti_Click(sender, e);


        }
        private void Ponisti_Click(object sender, RoutedEventArgs e)
        {
            ime.Visibility = Visibility.Visible;
            kolicina.Visibility = Visibility.Visible;
            opis.Visibility = Visibility.Visible;

            updateName.Visibility = Visibility.Collapsed; 
            updateQuantity.Visibility = Visibility.Collapsed;
            updateDescription.Visibility = Visibility.Collapsed;

            deleteButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;
            prihvatiButton.Visibility = Visibility.Collapsed;
            odustaniButton.Visibility = Visibility.Collapsed;
        }

        private void Premesti_Click(object sender, RoutedEventArgs e)
        {
            StaticRoomTransfer staticRoomTransfer = new StaticRoomTransfer(staticEquipment1);
            staticRoomTransfer.Show();
            Close();
        }
    }
}
