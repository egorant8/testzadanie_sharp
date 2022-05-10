using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestZadanie
{
    public partial class Form1 : Form
    {
        private static string password = "part";
        private static string login = "part";
        private string token = "";
        WebClient wb = new WebClient();

        public Form1()
        {
            InitializeComponent();
            wb.Encoding = Encoding.UTF8;
            string resp;
            try
            {
                string auth_resp = wb.DownloadString("http://178.57.218.210:198/token?login=" + login + "&password=" + password);
                JsonDocument doc = JsonDocument.Parse(auth_resp);
                JsonElement root = doc.RootElement;
                token = root.GetProperty("token").GetString();
                label1.Text = "Авторизация: успешно.";

                comboBox1.Items.Clear();
                resp = wb.DownloadString("http://178.57.218.210:198/commands/types?token=" + token);
                JsonDocument doc1 = JsonDocument.Parse(resp);
                JsonElement root1 = doc1.RootElement;
                foreach (var item in root1.GetProperty("items").EnumerateArray())
                {
                    //Console.WriteLine(item);
                    comboBox1.Items.Add(item.GetProperty("name").GetString());
                }
            }
            catch (Exception)
            {
                label1.Text = "Авторизация: не прошла.";
            }
        }

        void hidden(TextBox pr1, Label lr1, TextBox Hpr2, Label Hlr2, TextBox Hpr3, Label Hlr3)
        {
            pr1.Visible = false;
            lr1.Visible = false;
            Hpr2.Visible = false;
            Hlr2.Visible = false;
            Hpr3.Visible = false;
            Hlr3.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (token != "")
            {
                hidden(parameter_default_value1, label3, parameter_default_value2, label4, parameter_default_value3, label4);
                string resp = wb.DownloadString("http://178.57.218.210:198/commands/types?token=" + token);
                JsonDocument doc = JsonDocument.Parse(resp);
                JsonElement root = doc.RootElement;
                foreach (var item in root.GetProperty("items").EnumerateArray())
                {
                    if (comboBox1.Text == item.GetProperty("name").GetString())
                    {
                        MessageBox.Show(item.GetProperty("parameter_name1").ToString());
                        if (item.GetProperty("parameter_name1").ToString() != "")
                        {
                            parameter_default_value1.Text = Convert.ToString(item.GetProperty("parameter_default_value1"));
                            label5.Text = Convert.ToString(item.GetProperty("parameter_name1"));
                            parameter_default_value1.Visible = true;
                            label5.Visible = true;
                        }
                        if (item.GetProperty("parameter_name2").ToString() != "")
                        {
                            parameter_default_value2.Text = Convert.ToString(item.GetProperty("parameter_default_value2"));
                            label4.Text = Convert.ToString(item.GetProperty("parameter_name2"));
                            parameter_default_value2.Visible = true;
                            label4.Visible = true;
                        }
                        if (item.GetProperty("parameter_name3").ToString() != "")
                        {
                            parameter_default_value3.Text = Convert.ToString(item.GetProperty("parameter_default_value3"));
                            label3.Text = Convert.ToString(item.GetProperty("parameter_name3"));
                            parameter_default_value3.Visible = true;
                            label3.Visible = true;
                        }
                        break;
                    }
                }
            }
        }
    }
}
