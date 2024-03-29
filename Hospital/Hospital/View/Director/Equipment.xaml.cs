﻿using System;
using System.Collections.Generic;
using System.Windows;
using Model;
using Controller;
namespace Hospital.View.Director
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : Window
    {
        private readonly StaticEquipmentController staticController = new StaticEquipmentController();
        readonly List<StaticEquipment> staticEquipment = new List<StaticEquipment>();
        private readonly DynamicEquipmentController dynamicController = new DynamicEquipmentController();
        readonly List<DynamicEquipment> dynamicEquipment = new List<DynamicEquipment>();
        int id;
        int id_stat;
        public Equipment()
        {
            InitializeComponent();
            staticEquipment = staticController.GetAll();
            StaticDataGrid.ItemsSource = staticEquipment;
            dynamicEquipment = dynamicController.GetAll();
            DynamicDataGrid.ItemsSource = dynamicEquipment;
        }
        private StaticEquipment CreateEquipment()
        {
            int id = staticController.GenerateNewId();
            string textname = name.Text;
            string desc = description.Text;
            int quant = Int32.Parse(quantity.Text);
            int toRoomId = Int32.Parse(roomId.Text);
            staticController.Save(textname, toRoomId, quant, desc);
            //Room room = new Room(101, textname, (RoomType)1, 4, "test soba");
            return new StaticEquipment(id, textname, toRoomId, quant, desc);
        }

        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaticEquipment equipment = CreateEquipment();
            }
            catch
            {
                MessageBox.Show("You have to fill in all input boxes!");
            }
        }

        private void UpdateEquipment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaticEquipment equipment = (StaticEquipment)StaticDataGrid.SelectedItems[0];
                id_stat = equipment.Id;
                addEquipment.Visibility = Visibility.Collapsed;
                updateStaticButton.Visibility = Visibility.Visible;
                cancelupdateStaticButton.Visibility = Visibility.Visible;
                title.Content = "Edit equipment";

                name.Text = equipment.Name;
                quantity.Text = equipment.Quantity.ToString();
                description.Text = equipment.Description;
                roomId.Text = equipment.RoomId.ToString();
            }
            catch
            {
                MessageBox.Show("You have to fill in all input boxes!");
            }
        }

        private void UpdateStatic_Click(object sender, RoutedEventArgs e)
        {
            string textname = name.Text;
            string desc = description.Text;
            int quant = Int32.Parse(quantity.Text);

            StaticEquipment equipment = new StaticEquipment(id_stat, textname, Int32.Parse(roomId.Text), quant, desc);
            staticController.Update(equipment);
            id_stat = -1;

        }
        private void CancelupdateStatic_Click(object sender, RoutedEventArgs e)
        {
            name.Text = "";
            quantity.Text = "";
            description.Text = "";
            roomId.Text = "";

            addEquipment.Visibility = Visibility.Visible;
            updateStaticButton.Visibility = Visibility.Collapsed;
            cancelupdateStaticButton.Visibility = Visibility.Collapsed;
            title.Content = "Add new equipment";
        }
        private void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StaticEquipment equipment = (StaticEquipment)StaticDataGrid.SelectedItems[0];
                staticController.Delete(equipment.Id);
            }
            catch
            {
                MessageBox.Show("You have to select an equipmnet to delete!");
            }
        }

        private DynamicEquipment CreateDynamic()
        {
            int id = dynamicController.GenerateNewId();
            string textname = dynName.Text;
            string desc = dynDescription.Text;
            int quant = Int32.Parse(dynQuantity.Text);
            DynamicEquipment dynamic = new DynamicEquipment(id, textname, quant, desc);
            dynamicController.Save(dynamic);
            return new DynamicEquipment(id, textname, quant, desc);
        }

        private void AddDynamic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DynamicEquipment equipment = CreateDynamic();
            }
            catch
            {
                MessageBox.Show("You have to fill in all input boxes!");
            }
        }

        private void DeleteDynamic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DynamicEquipment equipment = (DynamicEquipment)DynamicDataGrid.SelectedItems[0];
                dynamicController.Delete(equipment.Id);
            }
            catch
            {
                MessageBox.Show("You have to select an equipment to delete!");
            }
        }

        private void UpdateDynamic_Show(object sender, RoutedEventArgs e)
        {
            try
            {
                DynamicEquipment equipment = (DynamicEquipment)DynamicDataGrid.SelectedItems[0];
                id = equipment.Id;
                addDynamic.Visibility = Visibility.Collapsed;
                updateDynamicButton.Visibility = Visibility.Visible;
                cancelupdateDynamicButton.Visibility = Visibility.Visible;
                dynTitle.Content = "Edit equipment";

                dynName.Text = equipment.Name;
                dynQuantity.Text = equipment.Quantity.ToString();
                dynDescription.Text = equipment.Description;
            }
            catch
            {
                MessageBox.Show("You have to fill in all input boxes!");
            }
        }

        private void UpdateDynamic_Click(object sender, RoutedEventArgs e)
        {
            string textname = dynName.Text;
            string desc = dynDescription.Text;
            int quant = Int32.Parse(dynQuantity.Text);
            DynamicEquipment equipment = new DynamicEquipment(id, textname, quant, desc);
            dynamicController.Update(equipment);
            id = -1;
        }

        private void CancelupdateDynamic_Click(object sender, RoutedEventArgs e)
        {
            dynName.Text = "";
            dynQuantity.Text = "";
            dynDescription.Text = "";

            addDynamic.Visibility = Visibility.Visible;
            updateDynamicButton.Visibility = Visibility.Collapsed;
            cancelupdateDynamicButton.Visibility = Visibility.Collapsed;
            title.Content = "Add new equipment";
        }

        private void Transfer_Static_Click(object sender, RoutedEventArgs e)
        {
            StaticTransfer staticTransfer = new StaticTransfer();
            staticTransfer.Show();

        }
        private void Transfer_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            Transfer dynamicTransfer = new Transfer();
            dynamicTransfer.Show();

        }

        private void Search_Static_Click(object sender, RoutedEventArgs e)
        {
            if(searchStatic.Text != ""){
                StaticDataGrid.ItemsSource = staticEquipment.FindAll(obj => obj.Name == searchStatic.Text);
            }
            else
            {
                StaticDataGrid.ItemsSource = staticController.GetAll();
            }
        }
        private void Search_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            if (searchDynamic.Text != "")
            {
                DynamicDataGrid.ItemsSource = dynamicEquipment.FindAll(obj => obj.Name == searchDynamic.Text);
            }
            else
            {
                DynamicDataGrid.ItemsSource = dynamicController.GetAll();
            }
        }
        private void Filter_Static_Click(object sender, RoutedEventArgs e)
        {
            //Search_Static_Click(sender, e);
            StaticDataGrid.ItemsSource = staticEquipment.FindAll(obj => obj.RoomId == Int32.Parse(filterStatic.Text));
        }
    }
}

