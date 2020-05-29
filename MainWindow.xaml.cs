using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpelCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool bClearingHistory = false;
        bool bError = false;

        private double GetNumber(string text)
        {
            if (bError == false)
            {
                if (text != "")
                {
                    bError = false;
                    return Convert.ToDouble(text);
                }
                else
                {
                    MessageBox.Show("Niet alle vakken zijn ingevuld!");
                }
            }
            bError = true;
            return default;
        }

        private double GetAnswer(double dNum1, string sOperator, double dNum2)
        {
            bClearingHistory = false;
            switch (sOperator)
            {
                case ":":
                    {
                        return Math.Round((dNum1 / dNum2), 5);
                    }
                case "x":
                    {
                        return Math.Round((dNum1 * dNum2), 5);
                    }
                case "-":
                    {
                        return Math.Round((dNum1 - dNum2), 5);
                    }
                case "+":
                    {
                        return Math.Round((dNum1 + dNum2), 5);
                    }
                default:
                    {
                        return default;
                    }
            }
        }

        private void AddAnswerToHistory(double dNum1, string sOperator, double dNum2, double dAnswer)
        {
            string sResult = dNum1.ToString() + " " + sOperator + " " + dNum2.ToString() + " = " + dAnswer.ToString();

            ListBoxItem item = new ListBoxItem();
            item.Content = (lbxHistory.Items.Count + 1).ToString() + ": " + sResult;

            lbxHistory.Items.Add(item);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bError = false;
            double dNum1 = GetNumber(tbxNum1.Text);
            string sOperator = "";
            double dNum2 = GetNumber(tbxNum2.Text);

            if(bError == false)
            {
                if (cbxOperator.Text != "Selecteer operator")
                {
                    sOperator = cbxOperator.Text;
                }
                else
                {
                    MessageBox.Show("Er is geen operator geselecteerd!");
                    return;
                }

                double dAnswer = GetAnswer(dNum1, sOperator, dNum2);

                tbxNum1.Text = dAnswer.ToString();
                lblResult.Content = dAnswer.ToString();
                AddAnswerToHistory(dNum1, sOperator, dNum2, dAnswer);
            }
        }

        private void btnDeleteHistory_Click(object sender, RoutedEventArgs e)
        {
            bClearingHistory = true;
            lbxHistory.Items.Clear();
        }

        private void lbxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(bClearingHistory == false)
            {
                ListBoxItem lbxItem = (ListBoxItem)lbxHistory.SelectedItem;
                string[] arrLbxItem = lbxItem.Content.ToString().Split(' ');
                tbxNum1.Text = arrLbxItem[1];
                for (int i = 0; i < cbxOperator.Items.Count; i++)
                {
                    ComboBoxItem cbxItem = (ComboBoxItem)cbxOperator.Items[i];
                    if (cbxItem.Content.ToString() == arrLbxItem[2])
                    {
                        cbxOperator.SelectedIndex = i;
                    }
                }
                tbxNum2.Text = arrLbxItem[3];
                lblResult.Content = arrLbxItem[5];
            }
        }
    }
}
