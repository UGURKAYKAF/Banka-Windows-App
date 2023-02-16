using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Bunifu.UI.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text.pdf;//pdf ekleme
using iTextSharp.text;
using System.IO;

namespace denemetasarım
{
    public partial class Anasayfa : Form
    {

        SqlConnection baglanti = giris.baglanti;

        public Anasayfa()
        {
            InitializeComponent();

            bunifuFormDock1.SubscribeControlToDragEvents(panel1);//panel kontrol


            label2.Text = GetRate("USD").ToString();

            label3.Text = GetRate("EUR").ToString();

            label4.Text = GetRate("GBP").ToString();

            label5.Text = GetRate("CNY").ToString();

            label6.Text = GetRate("RUB").ToString();



        }
        public double EklenmisBakiye;

        public void BakiyeEkle()
        {
            try
            {
                baglanti.Open();
                SqlCommand BakiyeEkle = new SqlCommand("Update  login_register  set bakiye+= ('" + bunifuTextBox1.Text + "' )where password=@deger;", baglanti);
                BakiyeEkle.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                BakiyeEkle.ExecuteNonQuery();//birden fazla veriyle işlem yapmak için
                if (string.IsNullOrEmpty(bunifuTextBox1.Text))
                {
                    // MessageBox.Show("lütfen geçerli bir birim giriniz", "Uyarı!!");
                    errorProvider1.SetError(bunifuTextBox1, "Lütfen Geçerli bir değer giriniz !!");


                }
                else if (bunifuTextBox1.Text == "0")
                {
                    errorProvider1.Clear();
                    // MessageBox.Show("lütfen geçerli bir birim giriniz", "Uyarı!!");
                    errorProvider1.SetError(bunifuTextBox1, "Lütfen Geçerli bir değer giriniz !!");
                }
                else
                {
                    MessageBox.Show("Para yatırma işleminiz başarılı", "Tekbirkler!!");
                    errorProvider1.Clear();


                    SqlCommand YeniBakiye = new SqlCommand("Select bakiye from login_register where password=@deger;", baglanti);
                    YeniBakiye.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                    SqlDataReader YeniBakiyeOku = YeniBakiye.ExecuteReader();//tablodaki tm değerli okur

                    while (YeniBakiyeOku.Read())
                    {
                        EklenmisBakiye = Convert.ToDouble(YeniBakiyeOku["bakiye"]);
                    }

                    label11.Text = EklenmisBakiye.ToString() + " " + "TL";


                    // listBox1.Items.Add("Tarih:"+""+dt+"\n"+"YATIRILAN TUTAR:" + "" + bunifuTextBox1.Text+"TL" + "\n" + "GÜNCEL BAKİYENİZ:" + "" + label11.Text+"TL"+"\n");
                    //listBox1.Items.Add("Tarih:" + " " + dt);
                    TarihSaatEkleme();
                    listBox1.Items.Add("Yatırılan Tutar:" + " " + bunifuTextBox1.Text + "TL");
                    listBox1.Items.Add("Güncel Bakiyeniz:" + " " + label11.Text);
                    listBox1.Items.Add("----------------------------------------------");
                }


            }
            catch (Exception)
            {
                errorProvider1.Clear();
                // MessageBox.Show("lütfen geçerli bir birim giriniz", "Uyarı!!");
                errorProvider1.SetError(bunifuTextBox1, "Lütfen Geçerli bir değer giriniz !!");


            }
            finally { baglanti.Close(); }

        }

        public void PdfOlustur()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "PDF DOSYALARI(*.pdf)|*.pdf";
            file.Title = "İSLEMLER";
            if (file.ShowDialog() == DialogResult.OK)
            {
                FileStream dosya = File.Open(file.FileName, FileMode.Create);
                Document pdf = new Document();
                PdfWriter.GetInstance(pdf, dosya);
                pdf.Open();
                pdf.AddAuthor("Ciftlik Bank");
                pdf.AddCreator("Serhat KILIÇ CEO");
                pdf.AddTitle("Dökümasyon");
                pdf.AddSubject("İşlem dökümasyonunuz");
                pdf.AddCreationDate();

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    Paragraph paragraph = new Paragraph(listBox1.Items[i].ToString());
                    pdf.Add(paragraph);

                }




                pdf.Close();
                MessageBox.Show("işlem başarılı");
            }
        }

        public void ResimGetir()
        {  
            try
            {
                        baglanti.Open();
                        SqlCommand YeniResimGetir = new SqlCommand("Select resim from login_register where password= @deger", baglanti);
                        YeniResimGetir.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                        SqlDataReader YeniResimOku = YeniResimGetir.ExecuteReader();//tablodaki tm değerli okur

                        while (YeniResimOku.Read())
                        {
                         //system.drawing eklenmesinin sebebi using iTextSharp.text; kütüphanesi eklendiğinde hata oluştu nereden erişmemiz gerektiğini belirledik
                              bunifuPictureBox6.Image = System.Drawing.Image.FromFile(YeniResimOku["resim"].ToString()); //veritabanında resim kolonu boş olduğunda giriş yapılmıyor hata veriyor //bir resim uzantısı olduğunda başarıyle giriş yapılabiliyor
                              bunifuPictureBox7.Image = System.Drawing.Image.FromFile(YeniResimOku["resim"].ToString());                                                                           
                }
            }
            catch (Exception)
            {


            }
            finally {baglanti.Close();}
           
        }

        public void SifreGüncelle()
        {
            if (!string.IsNullOrEmpty(bunifuTextBox4.Text) && !string.IsNullOrEmpty(bunifuTextBox5.Text) && (bunifuTextBox4.Text == giris.statikButon.Text))
            {
                baglanti.Open();
                SqlCommand YeniSifre = new SqlCommand("Update login_register set password=@sifre1,re_password=@sifre2 where password=@deger AND password=@deger2;", baglanti);
                YeniSifre.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                YeniSifre.Parameters.AddWithValue("@deger2", bunifuTextBox4.Text);
                YeniSifre.Parameters.AddWithValue("@sifre1", bunifuTextBox5.Text);
                YeniSifre.Parameters.AddWithValue("@sifre2", bunifuTextBox5.Text);
                YeniSifre.ExecuteNonQuery();
                MessageBox.Show("Şifreniz başarıyla değiştirilmiştir");
                errorProvider1.Clear();

            }
            else if (!(bunifuTextBox4.Text == giris.statikButon.Text))
            {
                errorProvider1.SetError(bunifuTextBox4, "Lütfen eski şifrenizi giriniz!!");

            }
            else if (bunifuTextBox5.Text == "")
            {
                errorProvider1.SetError(bunifuTextBox5, "Lütfen Geçerli bir Şifre değeri giriniz !!");

            }
            else
            {
                errorProvider1.SetError(bunifuTextBox4, "Lütfen eski şifrenizi giriniz!!");
                errorProvider1.SetError(bunifuTextBox5, "Lütfen Geçerli bir Şifre değeri giriniz !!");

            }

            baglanti.Close();

        }


        public double bakiye;

        public void BakiyeGetir()
        {
            
             try
             {

                 baglanti.Open();//bu kısımda lbel 1 ve label 7 ye anasayfada kullanıcı sifresine göre ad soyad getirtmeyi yazıyyoruz ve Id
                 SqlCommand BakiyeOku = new SqlCommand("Select bakiye from login_register where password=@deger", baglanti);
                 BakiyeOku.Parameters.AddWithValue("@deger", giris.statikButon.Text); //giriş formunda static buton degeri bunifuTextBox2'ydi bunun textini alıp girilen değere göre
                                                                                    //veritabanından veri aldık yani kimin şifresi ise o kişinin değerlerini çekebiliyoruz
                 SqlDataReader oku = BakiyeOku.ExecuteReader();//tablodaki tm değerli okur

                 while (oku.Read())
                 {
                     bakiye = Convert.ToDouble(oku["bakiye"]);
                 }


                 label11.Text = bakiye.ToString() + " " + "TL";            

             }
             catch (Exception)
             {

             }
             finally { baglanti.Close(); }
       
        }
       
        public void PostaTelUpdate()
        {
            if (!string.IsNullOrEmpty(bunifuTextBox6.Text) && !string.IsNullOrEmpty(bunifuTextBox3.Text))
            {
                errorProvider1.Clear();
                baglanti.Open();
                SqlCommand YeniTelPosta = new SqlCommand("Update login_register set email=@mail,phone=@number where password=@deger;", baglanti);
                YeniTelPosta.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                YeniTelPosta.Parameters.AddWithValue("@mail", bunifuTextBox6.Text);
                YeniTelPosta.Parameters.AddWithValue("@number", bunifuTextBox3.Text);
                YeniTelPosta.ExecuteNonQuery();
                MessageBox.Show("Güncelleme işleminiz başarılı");
            }
            else if (bunifuTextBox6.Text == "")
            {
                errorProvider1.SetError(bunifuTextBox6, "Lütfen Geçerli bir değer giriniz !!");

            }
            else if (bunifuTextBox3.Text == "")
            {
                errorProvider1.SetError(bunifuTextBox3, "Lütfen Geçerli bir değer giriniz !!");

            }
            else
            {
                errorProvider1.SetError(bunifuTextBox6, "Lütfen Geçerli bir değer giriniz!!");
                errorProvider1.SetError(bunifuTextBox3, "Lütfen Geçerli bir değer giriniz!!");

            }

            baglanti.Close();

        }


        private string addZero(int p)
        {
            if (p.ToString().Length == 1)
                return "0" + p;
            return p.ToString();
        }

        private decimal GetRate(string code)
        {
            string url = string.Empty;
            var date = DateTime.Now;
            if (date.Date == DateTime.Today)
                url = "http://www.tcmb.gov.tr/kurlar/today.xml";
            else
                url = string.Format("http://www.tcmb.gov.tr/kurlar/{0}{1}/{2}{1}{0}.xml", date.Year, addZero(date.Month), addZero(date.Day));

            System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(url);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var result = document.Descendants("Currency")
            .Where(v => v.Element("ForexBuying") != null && v.Element("ForexBuying").Value.Length > 0)
            .Select(v => new Currency
            {
                Code = v.Attribute("Kod").Value,
                Rate = decimal.Parse(v.Element("ForexBuying").Value.Replace('.', ','))
            }).ToList();
            return result.FirstOrDefault(s => s.Code == code).Rate;
        }

        public class Currency
        {
            public string Code { get; set; }
            public decimal Rate { get; set; }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            gezentipcr.Top=((Control)sender).Top;
            bunifuPages1.SetPage("tabPage1");
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            gezentipcr.Top = ((Control)sender).Top;
            bunifuPages1.SetPage("tabPage2");
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

            gezentipcr.Top = ((Control)sender).Top;
            bunifuPages1.SetPage("tabPage3");
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {

            gezentipcr.Top = ((Control)sender).Top;
            bunifuPages1.SetPage("tabPage4");
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {

            gezentipcr.Top = ((Control)sender).Top;
            bunifuPages1.SetPage("tabPage5");
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {

            gezentipcr.Top = ((Control)sender).Top;
            bunifuPages1.SetPage("tabPage6");
        }

      
      
        private void Anasayfa_Load(object sender, EventArgs e)
        {
            BakiyeGetir();

            ResimGetir();

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuIconButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton11_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuIconButton1_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {         
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void bunifuIconButton4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void bunifuIconButton6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void bunifuIconButton8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void bunifuIconButton10_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
                
            }

        }

        private void bunifuIconButton12_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı kapatmak istediğinizden emin misiniz!", "BankaOtomasyon", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();

            }
            else if (dialogResult == DialogResult.No)
            {
               
            }

        }

        public void TarihSaatEkleme()/*fonksiyonsuz 376.satıra tarih ve saat eklediğimizde
                                 her bakiye ekledimizde ayni dakika ve sanıyeyi veriyordu 
                                 fonksiyonlu bir şekilde sürekli güncellenecek bir method oluşturuldu*/
        {
            
            string dt = DateTime.Now.ToLongTimeString();
            string dt2 = DateTime.Now.ToLongDateString();
            listBox1.Items.Add("Tarih:" + " " + dt2+"/"+dt);

        }



        private void BakiyeEkleBtn_Click(object sender, EventArgs e)
        {

            BakiyeEkle();

        }

        
      


        private void ResimEkle_btn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            bunifuPictureBox6.ImageLocation = openFileDialog1.FileName;
            bunifuPictureBox7.ImageLocation = openFileDialog1.FileName;
            bunifuTextBox2.Text = openFileDialog1.FileName;
        }
        private void ResimKaydet_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bunifuTextBox2.Text))
            {
                errorProvider1.Clear();
                    baglanti.Open();
                    SqlCommand YeniResim = new SqlCommand("Update  login_register  set resim= ('" + bunifuTextBox2.Text + "' )where password=@deger;", baglanti);
                    YeniResim.Parameters.AddWithValue("@deger", giris.statikButon.Text);
                    YeniResim.ExecuteNonQuery();
                    MessageBox.Show("kayit eklendi");
            }
            else
            {
                errorProvider1.SetError(bunifuTextBox2, "Lütfen Geçerli bir resim ekleyiniz !!");
                
            }
            baglanti.Close();
        }

       

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                bunifuTextBox4.PasswordChar = '\0';
                bunifuTextBox5.PasswordChar = '\0';
                checkBox1.ImageIndex = 1;


            }
            else
            {
                bunifuTextBox4.PasswordChar = '*';
                bunifuTextBox5.PasswordChar = '*';

                checkBox1.ImageIndex = 0;
            }
        }

        private void bnf_btn_SifreGüncelle_Click(object sender, EventArgs e)
        {
            SifreGüncelle();
        }

        private void bnf_btn_postaTelUpdate_Click(object sender, EventArgs e)
        {
           PostaTelUpdate();
        }
        private void bnf_btn_PdfOlustur_Click(object sender, EventArgs e)
        {
            PdfOlustur();
        }

        private void bunifuTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);/*e.KeyChar propertisi ile basılan tuşun ne olduğunu öğrendik, char.IsDigit() ve char.IsControl()
                                                                                * fonksiyonları da basılan tuş bi karakter yani char tipinde değer olduğu için arka planda ASCII kodlarını karşılaştırıp
                                                                                * basılan tuş sayısal bi ifade ise true, değilse false döndürmektedir.
                                                                                * e.Handled propertisi ile de engelleme işlemini yapmış oluyoruz. Yani kodun sağ tarafından true gelirse engelleme yapılacak,
                                                                                * false gelirse engelleme yapılmayacaktır, yani tam olarak istediğimiz işlem yapılacaktır.*/
        }

        private void bnf_btn_ibnYolla_Click(object sender, EventArgs e)
        {
         


            baglanti.Open();
            SqlCommand ParaYolla = new SqlCommand("Update  login_register  set bakiye-= ('" + bnf_btnParayolla.Text + "' )where password=@deger;", baglanti);
            ParaYolla.Parameters.AddWithValue("@deger", giris.statikButon.Text);
            ParaYolla.ExecuteNonQuery();
            
            SqlCommand paraCek = new SqlCommand("Update  login_register  set bakiye+= ('" + bnf_btnParayolla.Text + "' )where iban=@iban;", baglanti);
            paraCek.Parameters.AddWithValue("@iban", ibn_bunifuTextBox7.Text);
            paraCek.ExecuteNonQuery();


            MessageBox.Show("kayit eklendi");

            baglanti.Close();
        }

      
    }
}
