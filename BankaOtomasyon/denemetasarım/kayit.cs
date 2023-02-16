﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Speech.Synthesis.TtsEngine;

namespace denemetasarım
{
    public partial class kayit : Form
    {

        SqlConnection baglanti = giris.baglanti; //form1 den connection u aldı
        int Move;
        int Mouse_X;
        int Mouse_Y;

        public kayit()
        {
            InitializeComponent();
        }



        bool control;

        public void kayitol()
        {       string sifre=bunifuTextBox2.Text;
             string sifre2=bunifuTextBox3.Text;
            // string eposta=bunifuTextBox4.Text;
                
            baglanti.Open();
            SqlCommand KayitControl = new SqlCommand("Select*from login_register", baglanti);
            SqlDataReader ControlOku = KayitControl.ExecuteReader();//tablodaki tm değerli okur

            //şifre diğer hesaplarla aynıysa kayıt yaptırmıyor
            while (ControlOku.Read())
            {
                if (sifre == ControlOku["password"].ToString().TrimEnd() && sifre2 == ControlOku["re_password"].ToString().TrimEnd())//boşluklar için trim end
                {
                   control= true;
                    
                }
               

                
            }
            baglanti.Close();
            if (control)
            {
                MessageBox.Show("ltfen farklı sifre değerler igiriniz");
                bunifuTextBox2.BackColor = Color.Red;
                bunifuTextBox3.BackColor = Color.Red;
                
                control= false; 
            }
            else if (string.IsNullOrEmpty(bunifuTextBox1.Text) || string.IsNullOrEmpty(bunifuTextBox2.Text) ||
                string.IsNullOrEmpty(bunifuTextBox3.Text) || string.IsNullOrEmpty(bunifuTextBox4.Text) || string.IsNullOrEmpty(bunifuTextBox5.Text)
                || string.IsNullOrEmpty(bunifuTextBox6.Text) || string.IsNullOrEmpty(bunifuTextBox7.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz", "UYARI!!");
                bunifuTextBox2.BackColor = Color.Transparent;
                bunifuTextBox3.BackColor = Color.Transparent;

            }
            else if (bunifuTextBox2.Text != bunifuTextBox3.Text)
            {
                MessageBox.Show("Lütfen aynı şifre değerlerini giriniz", "UYARI!!");
                bunifuTextBox3.Clear();
                bunifuTextBox2.BackColor = Color.Red;
                bunifuTextBox3.BackColor = Color.Red;


            }
            else
            {

                

                Random random= new Random();
                int RastgeleIban;
                RastgeleIban=random.Next(100000000,999999999);
                float İlkBakiyeDegeri = 0;
                baglanti.Open();
                SqlCommand command = new SqlCommand("Insert into login_register (adi,soyadi,username,password,re_password,email,phone,bakiye,iban) values ('" + bunifuTextBox6.Text + "','" + bunifuTextBox7.Text + "','" + bunifuTextBox1.Text + "','" + bunifuTextBox2.Text + "','" + bunifuTextBox3.Text + "','" + bunifuTextBox4.Text + "','" + bunifuTextBox5.Text +"','"+İlkBakiyeDegeri+"','"+RastgeleIban+ "')", baglanti);

                command.ExecuteNonQuery();//birden fazla veriyle işlem yapmak için
                baglanti.Close();
                bunifuTextBox1.BackColor = Color.Green;
                bunifuTextBox2.BackColor = Color.Green;
                bunifuTextBox3.BackColor = Color.Green;
                bunifuTextBox4.BackColor = Color.Green;
                bunifuTextBox5.BackColor = Color.Green;
                MessageBox.Show("Kyıt işleminiz başarıyla yapılmıştır", "Tekbirkler!!");


                giris giris = new giris();

                giris.Show();

                this.Hide();

            }
        }

        private void btn_kayitol_Click(object sender, EventArgs e)
        {

            kayitol();
            
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void bunifuIconButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuIconButton3_Click(object sender, EventArgs e)
        {
            this.Close();
            giris girisgiris = new giris();
            girisgiris.Show();
        }

        private void bunifuTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);/*e.KeyChar propertisi ile basılan tuşun ne olduğunu öğrendik, char.IsDigit() ve char.IsControl()
                                                                                * fonksiyonları da basılan tuş bi karakter yani char tipinde değer olduğu için arka planda ASCII kodlarını karşılaştırıp
                                                                                * basılan tuş sayısal bi ifade ise true, değilse false döndürmektedir.
                                                                                * e.Handled propertisi ile de engelleme işlemini yapmış oluyoruz. Yani kodun sağ tarafından true gelirse engelleme yapılacak,
                                                                                * false gelirse engelleme yapılmayacaktır, yani tam olarak istediğimiz işlem yapılacaktır.*/
        }

        private void bunifuTextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar); //sadece harf girmek için
        }

        private void bunifuTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar); //sadece harf girmek

        }

        private void kayit_Load(object sender, EventArgs e)
        {

        }
    }
}
