using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace yml_auto_prom
{
    class Program
    {
        public static string ConnectionString = "Data Source=192.168.4.100;" +
           // public string ConnectionString = "Data Source=192.168.4.85;" +
           "Initial Catalog=ShopMebli;" +
           "Persist Security Info=False;" +
           // "User ID=Vitalik_Z;Password=VitalikDriverRu;" +
           "User ID=kitchen;Password=0921;" +
           "MultipleActiveResultSets=False;" +
           "Encrypt=True;" +
           "TrustServerCertificate=True;" +
           "Connection Timeout=30;";
        static List<String> dlyadobavlennya = new List<String>();
        static Dictionary<string, string> dictionaryProduct = new Dictionary<string, string>();
        static SqlConnection sqlCon;
        static Dictionary<string, string> checkednodes = new Dictionary<string, string>();

        static List<Product> product = new List<Product>();


        static void Main(string[] args)
        {
            // MainAsync();
            Console.WriteLine("Start!!!!");

            string filename;
            List<Product> vsitovaru = vzyatutovaruexec();
            product.AddRange(vsitovaru);
            // Console.WriteLine("good! 222222222222222");


            //new TaskFactory()
            //.StartNew(() =>
            //{
            //    Console.WriteLine(444444);
            //    return 1;
            //})
            //.ContinueWith(x =>
            //{
            //    //Prints out System.Threading.Tasks.Task`1[System.Int32]
            //    Console.WriteLine(x);
            //    //Prints out 1
            //    Console.WriteLine(x.Result);
            //});



            // Trace.WriteLine("111111111");
            DateTime dt = DateTime.Now;

            var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);

            //  string descriptiontop = "111";
            string descriptiontop = "<div class=\"ck-alert ck-alert_theme_green\">" +
 "<div><span class=\"ck-alert__title\"></span>" +
 "<div id =\"gt-res-content\">" +
 "<div class=\"trans-verified-button-large\" dir=\"ltr\" id=\"gt-res-dir-ctr\">" +
 "<div id =\"tts_button\"><object data=\"//ssl.gstatic.com/translate/sound_player2.swf\" height=\"18\" id=\"tts\" type=\"application/x-shockwave-flash\" width=\"18\"></object></div>" +

 "<p style=\"text-align: center;\"><span style=\"font-size:14px;\"><span class=\"short_text\" id=\"result_box\" lang=\"ru\">Рады приветствовать Вас в интернет-магазине &quot;<a href=\"https://shopars.com.ua/\" target=\"_blank\">АРС</a>&quot;.</span></span></p>" +
 "</div>" +
 "</div>" +

 "<p style =\"text-align: center;\"><span style=\"font-size:14px;\">Если у Вас возникли вопросы, Вы можете связаться с нашим менеджером по телефону <strong>+380961764274 </strong> или <a href=\"https://shopars.com.ua/contacts\" target=\"_blank\"><strong><span class=\"short_text\" id=\"result_box\" lang=\"ru\">Написать на почту.</span></strong></a></span></p>" +

 "<h3 style =\"text-align: center;\"><span style=\"font-size:14px;\">Также Вы можете ознакомиться с полным ассортиментом товаров.</span></h3>" +

 "<p style = \"text-align: center;\"><span style= \"font-size:14px;\"><a href= \"https://shopars.com.ua/product_list\" target=\"_blank\"><strong> ПОСМОТРЕТЬ АССОРТИМЕНТ</strong></a></span></p>" +
 "<span></span></div>" +
 "</div>";
            string descriptionbottom = "<p style =\"text-align:justify\">&nbsp;</p>" +

"<div>" +
"<p style =\"text-align: center;\"><span style=\"font-size:18px;\"><strong>Информация для заказа</strong></span></p>" +

"<div class=\"ck-list-horizontal ck-list-horizontal_type_lite ck-theme-grey\">" +
"<div class=\"ck-list-horizontal__table\">" +
"<div class=\"ck-list-horizontal__table-item\">" +
"<div class=\"ck-list-horizontal__image-wrapper\"><img alt=\"\" class=\"ck-list-horizontal__image \" src=\"https://my.prom.ua/media/images/1348335915_w640_h2048_66666666.png?PIMAGE_ID=1348335915\"></div>" +

"<div class=\"ck-list-horizontal__text\">" +
"<div class=\"ck-list-horizontal__title\" style=\"text-align:center\">Нажмите кнопку КУПИТЬ</div>" +

"<p style =\"text-align:center\">Или Вы можете нажать кнопку &quot;перезвоните мне&quot; или позвонить по указанному номеру.</p>" +
"</div>" +
"</div>" +

"<div class=\"ck-list-horizontal__table-item ck-list-horizontal__table-item_type_narrow-45\" style=\"text-align:center\">&nbsp;</div>" +

"<div class=\"ck-list-horizontal__table-item\">" +
"<div class=\"ck-list-horizontal__image-wrapper\"><img alt=\"\" class=\"ck-list-horizontal__image\" src=\"https://my.prom.ua/media/images/1348316879_w640_h2048_2222bez_imeni2.png?PIMAGE_ID=1348316879\" style=\"\" /></div>" +

"<div class=\"ck-list-horizontal__text\">" +
"<div class=\"ck-list-horizontal__title\" style=\"text-align:center\">Мы с Вами свяжемся</div>" +

"<p style =\"text-align:center\">После оформления заказа Наш менеджер свяжется с Вами в ближайшее время для уточнения всех деталей.</p>" +
"</div>" +
"</div>" +

"<div class=\"ck-list-horizontal__table-item ck-list-horizontal__table-item_type_narrow-45\" style=\"text-align:center\">&nbsp;</div>" +

"<div class=\"ck-list-horizontal__table-item\">" +
"<div class=\"ck-list-horizontal__image-wrapper\"><img alt=\"\" class=\"ck-list-horizontal__image\" src=\"https://my.prom.ua/media/images/1348317034_w640_h2048_3333bez_imeni2.png?PIMAGE_ID=1348317034\" style=\"\" /></div>" +

"<div class=\"ck-list-horizontal__text\">" +
"<div class=\"ck-list-horizontal__title\" style=\"text-align:center\">Получите номер посылки</div>" +

"<p style =\"text-align:center\">Отследить местоположение заказа можно с помощью номера ТТН, который мы пришлем вам на телефон.</p></div></div></div></div></div>" +

"<h2 style=\"text-align:center\">&nbsp;</h2 ><h2 style=\"text-align:center\"><span style=\"font-size:18px;\"><strong>Все акции и </strong><span style=\"font-size:18px;\"><strong >новинки </strong ></span ><strong> Вы сможете найти на нашем сайте <a href = \"https://mebli.ars.ua/\" target=\"_blank\">mebli.ars.ua</a></strong></span></h2>" +
"<h2 style=\"text-align:center\"><a href=\"https://mebli.ars.ua/\"><img alt=\"\" src=\"https://my.prom.ua/media/images/1342006587_w640_h2048_logo.png?PIMAGE_ID=1342006587\" style=\"width: 346px; height: 78px;\"/></a></h2><p> &nbsp;</p>" +
"<h2 style =\"text-align:center\"><strong><span style=\"font-size:18px;\">Мы в социальных сетях</span></strong></h2>" +

"<p>&nbsp;</p>" +

"<div class=\"ck-social ck-social_type_lite ck-theme-grey\">" +
"<div class=\"ck-social__image-wrapper\"><a href=\"https://www.instagram.com/ars.ua/\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://a.radikal.ru/a39/1808/6f/c2c5b884e24a.png\" style=\"width:100px;height:100px\" /></a></div>" +

//"<div class=\"ck-social__image-wrapper\"><a href=\"https://plus.google.com/+ІнтернетмагазинМеблевогоЦентруАРСТернопіль\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://c.radikal.ru/c11/1809/11/5301f2db9445.png\" style=\"width:100px;height:100px\" /></a></div>" +

"<div class=\"ck-social__image-wrapper ck-social__image-wrapper_type_without-margin\"><a href=\"https://www.facebook.com/ARS.Ukraine/\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://b.radikal.ru/b38/1808/59/7f0bff7cd5d0.png\" style=\"width:100px;height:100px\" /></a></div>" +
"</div>" +

"<div class=\"b-tab-list__item js-content-item b-online-edit\">&nbsp;</div>";

            string value = String.Format("{0:yyyy-MM-dd_HH-mm}", dt);
            //if (InputBox("Назва файлу для створення", "Введіть назву XML файлу:", ref value) == DialogResult.OK)
            //{
            //filename = value + "_prom.xml";
            //}
            filename = "YML_price" + "_prom.xml";


            //filename = "YML_" + String.Format("{0:yyyy-MM-dd_HH-mm}", dt) + "_rozetka.xml";

            var xmlWriter = new XmlTextWriter(filename, null);

            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';
            xmlWriter.Indentation = 1;

            xmlWriter.WriteStartDocument(); // <?xml version="1.0"?>+



            xmlWriter.WriteStartElement("yml_catalog");
            xmlWriter.WriteStartAttribute("date");



            xmlWriter.WriteString(today);
            //  xmlWriter.WriteString(DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"));
            xmlWriter.WriteEndAttribute();




            xmlWriter.WriteStartElement("shop");

            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString("Apeyron");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("company");
            xmlWriter.WriteString("ТОВ Торгова Група \"АРС кераміка\"");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("url");
            xmlWriter.WriteString("https://mebli.ars.ua/");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("currencies");
            xmlWriter.WriteStartElement("currency");
            xmlWriter.WriteStartAttribute("id");
            xmlWriter.WriteString("UAH");

            xmlWriter.WriteStartAttribute("rate");
            xmlWriter.WriteString("1");
            xmlWriter.WriteEndAttribute();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("categories");



            //var listcat = Category(Category_DT());


            //for (int l = 0; l < listcat.Count; l++)
            //{


            //    if (listcat[l].parentcategoryid == 0)

            //    {
            //        xmlWriter.WriteStartElement("category");
            //        xmlWriter.WriteStartAttribute("id");
            //        xmlWriter.WriteString(listcat[l].id);
            //        xmlWriter.WriteEndAttribute();
            //        xmlWriter.WriteString(listcat[l].name);
            //        xmlWriter.WriteEndElement();

            //    }
            //    else
            //    {

            //        xmlWriter.WriteStartElement("category");
            //        xmlWriter.WriteStartAttribute("id");
            //        xmlWriter.WriteString(listcat[l].id);
            //        xmlWriter.WriteEndAttribute();
            //        xmlWriter.WriteStartAttribute("parentId");
            //        xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
            //        xmlWriter.WriteEndAttribute();
            //        xmlWriter.WriteString(listcat[l].name);
            //        xmlWriter.WriteEndElement();
            //    }


            //}



            var listcat = Category(Category_DT());
            for (int l = 0; l < listcat.Count; l++)
            {

                int xxx = product.Where(p => p.catId == listcat[l].id).Count();
                if (xxx > 0)

                {

                    if (listcat[l].parentcategoryid == 0)

                    {
                        xmlWriter.WriteStartElement("category");
                        xmlWriter.WriteStartAttribute("id");
                        xmlWriter.WriteString(listcat[l].id);
                        xmlWriter.WriteEndAttribute();
                        xmlWriter.WriteString(listcat[l].name);
                        xmlWriter.WriteEndElement();

                    }
                    else
                    {

                        xmlWriter.WriteStartElement("category");
                        xmlWriter.WriteStartAttribute("id");
                        xmlWriter.WriteString(listcat[l].id);
                        xmlWriter.WriteEndAttribute();

                        xmlWriter.WriteStartAttribute("parentId");
                        xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
                        xmlWriter.WriteEndAttribute();

                        xmlWriter.WriteString(listcat[l].name);
                        xmlWriter.WriteEndElement();

                        CategoryXMLPlus(ref xmlWriter, l, listcat[l].parentcategoryid);

                        //var ppp = listcat.Where(p => Convert.ToInt32(p.id) == listcat[l].parentcategoryid).ToList();

                        //if (ppp[0].parentcategoryid >= 0)
                        //{

                        //    xmlWriter.WriteStartElement("category");
                        //    xmlWriter.WriteStartAttribute("id");
                        //    xmlWriter.WriteString(ppp[0].id);
                        //    xmlWriter.WriteEndAttribute();
                        //    xmlWriter.WriteStartAttribute("parentId");
                        //    xmlWriter.WriteString(ppp[0].parentcategoryid.ToString());
                        //    xmlWriter.WriteEndAttribute();

                        //    xmlWriter.WriteString(ppp[0].name);
                        //    xmlWriter.WriteEndElement();


                        //}

                    }

                }
            }

            if (listpovtorne.Count > 0)
            {
                //for (i = 0; i <= listpovtorne.Count; i++)
                foreach (var item in listpovtorne)
                {
                    xmlWriter.WriteStartElement("category");
                    xmlWriter.WriteStartAttribute("id");
                    xmlWriter.WriteString(item.Value.id);
                    xmlWriter.WriteEndAttribute();
                    xmlWriter.WriteString(item.Value.name);
                    xmlWriter.WriteEndElement();
                }

            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("offers");
            for (int p = 0; p < product.Count; p++)   //foreach (var eproduct in product)
            {

                //SetControlPropertyValue(progressBar1, "value", 123);

                //Thread.Sleep(10);
                //  var   th1 = new System.Threading.Thread(AddressOf progressBar1);





                //   comboBox1.Items.Add(eproduct.Key + "  " + eproduct.Value);


                var productid = product[p].id;// var productid = eproduct.id;
                                              //var productid = dlyadobavlennya[k];


                // var b = vzyatuattribute(Convert.ToInt32(productid));
                // var a = ReadDataInfo(productid);
                // var c = Tovartocat(productid)[0];


                var b = vzyatuattribute(Convert.ToInt32(productid));

                // var a = vzyatuinfo(productid);

                // var c = Tovartocat(productid)[0];
                var picture = Take_picture(productid);


                xmlWriter.WriteStartElement("offer");

                xmlWriter.WriteStartAttribute("id");
                xmlWriter.WriteString(productid);  //offer id
                xmlWriter.WriteEndAttribute();



                xmlWriter.WriteStartAttribute("available");


                if (product[p].preorder != true)
                {
                    xmlWriter.WriteString("true");
                }

                else
                    xmlWriter.WriteString("false");

                xmlWriter.WriteEndAttribute();


                //гарантоване наличие
                if (product[p].preorder != true)
                {
                    xmlWriter.WriteStartAttribute("presence_sure");
                    xmlWriter.WriteString("true");
                    xmlWriter.WriteEndAttribute();
                }




                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteString(product[p].url);
                xmlWriter.WriteEndElement();





                xmlWriter.WriteStartElement("price");

                var price = product[p].price;
                if (price > decimal.ToInt32(price))
                    price = (decimal.Round(price, 2));
                else
                    price = (decimal.ToInt32(price));

                xmlWriter.WriteString(price.ToString());

                xmlWriter.WriteEndElement();

                /////old price


                var oldprice = product[p].oldprice;
                if (oldprice > decimal.ToInt32(oldprice))
                    oldprice = (decimal.Round(oldprice, 2));
                else
                    oldprice = (decimal.ToInt32(oldprice));

                if (oldprice > 0)
                {
                    xmlWriter.WriteStartElement("oldprice");


                    xmlWriter.WriteString(oldprice.ToString());

                    xmlWriter.WriteEndElement();
                }
                /////old price

                if (product[p].vendor != null)
                {
                    xmlWriter.WriteStartElement("vendor");
                    xmlWriter.WriteString(product[p].vendor);
                    xmlWriter.WriteEndElement();
                }

                if (product[p].sku.Length > 1)
                {
                    xmlWriter.WriteStartElement("vendorCode");
                    xmlWriter.WriteString(product[p].sku);
                    xmlWriter.WriteEndElement();
                }


                xmlWriter.WriteStartElement("currencyId");
                xmlWriter.WriteString("UAH");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("categoryId");
                //xmlWriter.WriteString(c);
                xmlWriter.WriteString(product[p].catId);
                xmlWriter.WriteEndElement();



                for (int i = 0; i < picture.Count; i++)
                {
                    xmlWriter.WriteStartElement("picture");
                    xmlWriter.WriteString(picture[i].picture_url);
                    xmlWriter.WriteEndElement();

                }

                xmlWriter.WriteStartElement("quantity_in_stock");
                //////////////
                //xmlWriter.WriteString("1");
                xmlWriter.WriteString(product[p].stockquantity);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(product[p].name.ToString());
                Console.Clear();
                Console.WriteLine("Додається товар: №" + p + " id=" + product[p].id.ToString() + "__" + product[p].name.ToString());

                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("description");
                //xmlWriter.WriteCData(product[p].fulldescription.ToString());
                //string descriptionbottom2 = "<h2 style =\"text-align:center\"><a href=\"https://mebli.ars.ua/\"><img alt=\"\" src=\"" + picture[0].picture_url + "\" style =\"width: 346px; height: 78px;\" /></a></h2>";
                //string picture_description;
                //try
                //{  picture_description = picture[0].picture_url != "null" ? (picture[0].picture_url) : (""); }
                //catch
                //{
                //    picture_description = "";
                //}
                string picture_description = "";
                string descriptionbottom2 = "<div class=\"ck-image-text-bottom ck-image-text-bottom_type_lite ck-theme-grey\">" +
"<div class=\"ck-image-text-bottom__image-wrapper\"><img alt=\"\" class=\"ck-image-text-bottom__image\" src=\"" + picture_description + "\" style=\"width: 25 %; height: 25 % \"/></div></div>" +
                "<style=\"max-width:25%;max-height:25%;\"/></div></div>";

                xmlWriter.WriteCData(descriptiontop + product[p].fulldescription.ToString() + descriptionbottom);  //   все ок для прома!
               
                //  xmlWriter.WriteCData(descriptiontop + product[p].fulldescription.ToString() + descriptionbottom2 + descriptionbottom);  //з фоткою
                xmlWriter.WriteEndElement();

                for (int i = 0; i < b.Count; i++)
                {
                    //  listBox1.Items.Add(b[i].id.ToString() + "(" + a[i].price.ToString() + ")" + "=" + b[i].name.ToString() + "/Sku= " + b[i].nameoption.ToString());
                    xmlWriter.WriteStartElement("param");
                    xmlWriter.WriteStartAttribute("name");
                    xmlWriter.WriteString(b[i].name.ToString().Replace(":", ""));
                    xmlWriter.WriteEndAttribute();
                    xmlWriter.WriteString(b[i].nameoption.ToString());
                    xmlWriter.WriteEndElement();
                }
                //xmlWriter.WriteStartElement("param");
                // xmlWriter.WriteStartAttribute("name");
                //  xmlWriter.WriteString("Категория");
                //  xmlWriter.WriteEndAttribute();
                // xmlWriter.WriteString("Прихожая");
                // xmlWriter.WriteEndElement();


                xmlWriter.WriteEndElement();//offer
                                            //this.BeginInvoke((MethodInvoker)delegate {   
                                            //    label1.Text = "Додано до XML: ";//+ product[p].name;
                                            //});
                string product_name = product[p].name;
                ////////////////////////////////////////  // this.BeginInvoke(new Action(() => this.label1.Text = "Додано до XML: " + product_name));
                //  this.BeginInvoke((Action)(() => label1.Refresh()));

                //this.BeginInvoke((Action)(() => {
                //    label1.Text = "Додано до XML: " + product[p].name;
                //    label1.Refresh();
                //}));


            }

            xmlWriter.WriteEndElement(); //offers
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();





            xmlWriter.Close();

            // }, TaskCreationOptions.AttachedToParent);

            // MainAsync();


            Console.WriteLine("Файл XML  сформовано!");
            Console.WriteLine("http://mebli.ars.ua/Content/FileManager/" + FileCopyXML(filename));
        // Console.ReadKey();
        }

        static private void ReadXMLFile()
        {
            string input = @"<?xml version=""1.0"" encoding=""utf-16""?><List>
<Employee><ID>1</ID><First>David</First>
  <Last>Smith</Last><Salary>10000</Salary></Employee>
<Employee><ID>3</ID><First>Mark</First>
  <Last>Drinkwater</Last><Salary>30000</Salary></Employee>
<Employee><ID>4</ID><First>Norah</First>
  <Last>Miller</Last><Salary>20000</Salary></Employee>
<Employee><ID>12</ID><First>Cecil</First>
  <Last>Walker</Last><Salary>120000</Salary></Employee>
</List>";
using (StringReader stringReader = new StringReader(input))
        using (XmlTextReader reader = new XmlTextReader(stringReader))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Employee":
                            Console.WriteLine();
                            break;
                        case "ID":
                            Console.WriteLine("ID: " + reader.ReadString());
                            break;
                        case "First":
                            Console.WriteLine("First: " + reader.ReadString());
                            break;
                        case "Last":
                            Console.WriteLine("Last: " + reader.ReadString());
                            break;
                        case "Salary":
                            Console.WriteLine("Salary: " + reader.ReadString());
                            break;
                    }
}
            }
        }
}
        public class Employee
        {
            public string ID { get; set; }
           
            public string First { get; set; }
            public string Last { get; set; }
            public string Salary { get; set; }
            

            public override string ToString()
            {
                return string.Format("{0} {1}", First, Last);
            }
        }
        static  private void MainAsync()
        {
            Console.WriteLine("Start!!");

            string filename;
            List<Product> vsitovaru = vzyatutovaruexec();
            product.AddRange(vsitovaru);
          
          
            DateTime dt = DateTime.Now;

            var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);

            //  string descriptiontop = "111";
          
            string value = String.Format("{0:yyyy-MM-dd_HH-mm}", dt);
         
            filename = "YML_price" + "_prom.xml";


            var xmlWriter = new XmlTextWriter(filename, null);

            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';
            xmlWriter.Indentation = 1;

            xmlWriter.WriteStartDocument(); // <?xml version="1.0"?>+



            xmlWriter.WriteStartElement("yml_catalog");
            xmlWriter.WriteStartAttribute("date");



            xmlWriter.WriteString(today);
              xmlWriter.WriteEndAttribute();




            xmlWriter.WriteStartElement("shop");

            xmlWriter.WriteStartElement("name");
            xmlWriter.WriteString("Apeyron");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("company");
            xmlWriter.WriteString("ТОВ торгова група \"АРС кераміка\"");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("url");
            xmlWriter.WriteString("https://mebli.ars.ua/");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("currencies");
            xmlWriter.WriteStartElement("currency");
            xmlWriter.WriteStartAttribute("id");
            xmlWriter.WriteString("UAH");

            xmlWriter.WriteStartAttribute("rate");
            xmlWriter.WriteString("1");
            xmlWriter.WriteEndAttribute();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("categories");



       



            var listcat = Category(Category_DT());
            for (int l = 0; l < listcat.Count; l++)
            {

                int xxx = product.Where(p => p.catId == listcat[l].id).Count();
                if (xxx > 0)

                {

                    if (listcat[l].parentcategoryid == 0)

                    {
                        xmlWriter.WriteStartElement("category");
                        xmlWriter.WriteStartAttribute("id");
                        xmlWriter.WriteString(listcat[l].id);
                        xmlWriter.WriteEndAttribute();
                        xmlWriter.WriteString(listcat[l].name);
                        xmlWriter.WriteEndElement();

                    }
                    else
                    {

                        xmlWriter.WriteStartElement("category");
                        xmlWriter.WriteStartAttribute("id");
                        xmlWriter.WriteString(listcat[l].id);
                        xmlWriter.WriteEndAttribute();

                        xmlWriter.WriteStartAttribute("parentId");
                        xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
                        xmlWriter.WriteEndAttribute();

                        xmlWriter.WriteString(listcat[l].name);
                        xmlWriter.WriteEndElement();

                        CategoryXMLPlus(ref xmlWriter, l, listcat[l].parentcategoryid);

                       

                    }

                }
            }

            if (listpovtorne.Count > 0)
            {
                //for (i = 0; i <= listpovtorne.Count; i++)
                foreach (var item in listpovtorne)
                {
                    xmlWriter.WriteStartElement("category");
                    xmlWriter.WriteStartAttribute("id");
                    xmlWriter.WriteString(item.Value.id);
                    xmlWriter.WriteEndAttribute();
                    xmlWriter.WriteString(item.Value.name);
                    xmlWriter.WriteEndElement();
                }

            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("offers");
            for (int p = 0; p < product.Count; p++)   //foreach (var eproduct in product)
            {

                //SetControlPropertyValue(progressBar1, "value", 123);

                //Thread.Sleep(10);
                //  var   th1 = new System.Threading.Thread(AddressOf progressBar1);





                //   comboBox1.Items.Add(eproduct.Key + "  " + eproduct.Value);


                var productid = product[p].id;// var productid = eproduct.id;
                                              //var productid = dlyadobavlennya[k];


                // var b = vzyatuattribute(Convert.ToInt32(productid));
                // var a = ReadDataInfo(productid);
                // var c = Tovartocat(productid)[0];


                var b = vzyatuattribute(Convert.ToInt32(productid));

                // var a = vzyatuinfo(productid);

                // var c = Tovartocat(productid)[0];
                var picture = Take_picture(productid);


                xmlWriter.WriteStartElement("offer");

                xmlWriter.WriteStartAttribute("id");
                xmlWriter.WriteString(productid);  //offer id
                xmlWriter.WriteEndAttribute();



                xmlWriter.WriteStartAttribute("available");


                if (product[p].preorder != true)
                {
                    xmlWriter.WriteString("true");
                }

                else
                    xmlWriter.WriteString("false");

                xmlWriter.WriteEndAttribute();


                //гарантоване наличие
                if (product[p].preorder != true)
                {
                    xmlWriter.WriteStartAttribute("presence_sure");
                    xmlWriter.WriteString("true");
                    xmlWriter.WriteEndAttribute();
                }




                xmlWriter.WriteStartElement("url");
                xmlWriter.WriteString(product[p].url);
                xmlWriter.WriteEndElement();





                xmlWriter.WriteStartElement("price");

                var price = product[p].price;
                if (price > decimal.ToInt32(price))
                    price = (decimal.Round(price, 2));
                else
                    price = (decimal.ToInt32(price));

                xmlWriter.WriteString(price.ToString());

                xmlWriter.WriteEndElement();

                /////old price


                var oldprice = product[p].oldprice;
                if (oldprice > decimal.ToInt32(oldprice))
                    oldprice = (decimal.Round(oldprice, 2));
                else
                    oldprice = (decimal.ToInt32(oldprice));

                if (oldprice > 0)
                {
                    xmlWriter.WriteStartElement("oldprice");


                    xmlWriter.WriteString(oldprice.ToString());

                    xmlWriter.WriteEndElement();
                }
                /////old price

                if (product[p].vendor != null)
                {
                    xmlWriter.WriteStartElement("vendor");
                    xmlWriter.WriteString(product[p].vendor);
                    xmlWriter.WriteEndElement();
                }

                if (product[p].sku.Length > 1)
                {
                    xmlWriter.WriteStartElement("vendorCode");
                    xmlWriter.WriteString(product[p].sku);
                    xmlWriter.WriteEndElement();
                }


                xmlWriter.WriteStartElement("currencyId");
                xmlWriter.WriteString("UAH");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("categoryId");
                //xmlWriter.WriteString(c);
                xmlWriter.WriteString(product[p].catId);
                xmlWriter.WriteEndElement();



               
                xmlWriter.WriteStartElement("quantity_in_stock");
                //////////////
                //xmlWriter.WriteString("1");
                xmlWriter.WriteString(product[p].stockquantity);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(product[p].name.ToString());
                Console.Clear();
                Console.WriteLine("Додається товар: №" + p + " id=" + product[p].id.ToString() + "__" + product[p].name.ToString());

                xmlWriter.WriteEndElement();

                               //xmlWriter.WriteStartElement("param");
                // xmlWriter.WriteStartAttribute("name");
                //  xmlWriter.WriteString("Категория");
                //  xmlWriter.WriteEndAttribute();
                // xmlWriter.WriteString("Прихожая");
                // xmlWriter.WriteEndElement();


                xmlWriter.WriteEndElement();//offer
                                            //this.BeginInvoke((MethodInvoker)delegate {   
                                            //    label1.Text = "Додано до XML: ";//+ product[p].name;
                                            //});
                string product_name = product[p].name;
                ////////////////////////////////////////  // this.BeginInvoke(new Action(() => this.label1.Text = "Додано до XML: " + product_name));
                //  this.BeginInvoke((Action)(() => label1.Refresh()));

                //this.BeginInvoke((Action)(() => {
                //    label1.Text = "Додано до XML: " + product[p].name;
                //    label1.Refresh();
                //}));


            }

            xmlWriter.WriteEndElement(); //offers
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();





            xmlWriter.Close();

            // }, TaskCreationOptions.AttachedToParent);

            // MainAsync();


            Console.WriteLine("Файл XML  сформовано!");
           Console.WriteLine("http://mebli.ars.ua/Content/FileManager/" + FileCopyXML(filename));
            Console.ReadKey();
        }



        //не діючий метод
         static private Task SformuvatuXMLPromAsync()
        {
 string filename;
            List<Product> vsitovaru = vzyatutovaruexec();
            product.AddRange(vsitovaru);
           


            //new TaskFactory()
            //.StartNew(() =>
            //{
            //    Console.WriteLine(444444);
            //    return 1;
            //})
            //.ContinueWith(x =>
            //{
            //    //Prints out System.Threading.Tasks.Task`1[System.Int32]
            //    Console.WriteLine(x);
            //    //Prints out 1
            //    Console.WriteLine(x.Result);
            //});

            Task outer;
              outer = Task.Factory.StartNew(() =>
            {
              
                DateTime dt = DateTime.Now;

                var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);

              
            // }, TaskCreationOptions.AttachedToParent);
           });
         outer.Wait(); // ожидаем выполнения внешней задачи
            return outer;
       
            //return Task.Factory.StartNew(() =>  { return 1; });   

        }




        static private List<Product> vzyatutovaruexec()
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {

                        sqlCon.Open();



                        //string sqlStr = "EXEC dbo.us_selectpromzvarych @categoryid = '" + id + "'";
                        string sqlStr = "EXEC dbo.us_selectpromzvarych 0,2";

                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStr, sqlCon))
                        {
                            sqlDa.Fill(ds);
                        }


                    }
                    catch
                    {


                    }
                    finally

                    {

                        sqlCon.Close();
                    }
                }
                List<string> vendorlist = new List<string>();
                List<Product> L2Product = new List<Product>();
                List<string> vendorlistdistinct = new List<string>();

                using (DataTable dt = ds.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        L2Product.Add(new Product()
                        {

                            id = dr["Id"].ToString(),
                            name = dr["Name"].ToString(),
                            price = Convert.ToDecimal(dr["Price"].ToString()),
                            published = dr["Published"].ToString(),
                            sku = dr["SKU"].ToString(),
                            fulldescription = dr["FullDescription"].ToString(),
                            url = "https://mebli.ars.ua/" + dr["Slug_rus"].ToString(),
                            vendor = dr["Vendor"].ToString(),
                            catId = dr["CategoryId"].ToString(),
                            preorder = Convert.ToBoolean(dr["DisableBuyButton"]),
                            oldprice = Convert.ToDecimal(dr["OldPrice"].ToString()),
                            stockquantity = dr["StockQuantity"].ToString(),
                            //для першого селекта
                            // id = dr["ProductId"].ToString(),
                            //name = dr["ProductName"].ToString(),

                        });


                        vendorlist.Add(dr["Vendor"].ToString());



                    }


                }




                return L2Product;

            }

        }

        private string Manufacture(string id)
        {
            string manufacture = "";


            SqlDataReader rdr = null;

            try
            {
                sqlCon = new SqlConnection(ConnectionString);

                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("SELECT [Name] FROM[ShopMebli].[dbo].[Manufacturer] where id = (SELECT[ManufacturerId] FROM[ShopMebli].[dbo].[Product_Manufacturer_Mapping] where ProductId ='" + id + "')", sqlCon);

                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record

                while (rdr.Read())
                {

                    manufacture = rdr["Name"].ToString();

                }


            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (sqlCon != null)
                {
                    sqlCon.Close();
                }

            }
            return manufacture;
        }


        private Dictionary<string, string> List_Manufacture()
        {
            string manufacture, manufactureid = "";

            Dictionary<string, string> Dict_manufactured = new Dictionary<string, string>();


            SqlDataReader rdr = null;

            try
            {
                sqlCon = new SqlConnection(ConnectionString);

                sqlCon.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM[ShopMebli].[dbo].[Manufacturer] where Deleted=0", sqlCon);

                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record



                while (rdr.Read())
                {
                    Dict_manufactured.Add(

                         manufactureid = rdr["Id"].ToString(),
                         manufacture = rdr["Name"].ToString()
                 );

                }


            }
            finally
            {
                // close the reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // Close the connection
                if (sqlCon != null)
                {
                    sqlCon.Close();
                }

            }
            return Dict_manufactured;
        }

        private List<Product> vzyatuinfo(string id)
        {
            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {

                        sqlCon.Open();

                        string sqlStr = "SELECT M.name as Vendor,P.Slug as Slug_rus,C.CategoryId as ProductCategoryId,D.* FROM[ShopMebli].[dbo].[Product] AS D left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId=D.Id left join[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId=D.id left join[ShopMebli].[dbo].[Manufacturer] M on M.ID=PM.ManufacturerId left join[ShopMebli].[dbo].[Product_Category_Mapping] C on C.ProductId=D.Id where(D.Id='" + id + "' AND P.LanguageId= 0 AND P.IsActive= 1 AND P.EntityName='Product')";

                        // string sqlStr = "select * from Product Where id='" + id + "'";
                        string sqlStr2 = "SELECT [Name] FROM[ShopMebli].[dbo].[Manufacturer] where id = (SELECT[ManufacturerId] FROM[ShopMebli].[dbo].[Product_Manufacturer_Mapping] where ProductId ='" + id + "'";


                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStr, sqlCon))
                        {
                            sqlDa.Fill(ds);

                        }


                    }
                    catch
                    {


                    }
                    finally

                    {

                        sqlCon.Close();
                    }
                }

                List<Product> Schedule2 = new List<Product>();

                using (DataTable dt = ds.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Schedule2.Add(new Product()
                        {
                            id = dr["Id"].ToString(),
                            name = dr["Name"].ToString(),
                            price = Convert.ToDecimal(dr["Price"].ToString()),
                            published = dr["Published"].ToString(),
                            sku = dr["SKU"].ToString(),
                            fulldescription = dr["FullDescription"].ToString(),
                            url = "https://mebli.ars.ua/" + dr["Slug_rus"].ToString(),
                            vendor = dr["Vendor"].ToString(),
                            catId = dr["ProductCategoryId"].ToString(),

                        });
                    }

                }




                return Schedule2;


            }
        }
        static private List<Picture> Take_picture(string id)
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();

                        string sqlStr = "SELECT D.*,p.SeoFilename,p.MimeType FROM[ShopMebli].[dbo].[Product_Picture_Mapping] AS D left join[ShopMebli].[dbo].[Picture] P on P.id=D.PictureId where D.ProductId= '" + id + "' ORDER BY DisplayOrder,PictureId";
                        //  string sqlStr = "select * from Product_Category_Mapping where CategoryId= '" + id + "'";


                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStr, sqlCon))
                        {
                            sqlDa.Fill(ds);
                        }


                    }
                    catch
                    {


                    }
                    finally

                    {

                        sqlCon.Close();
                    }
                }

                List<Picture> LPicture = new List<Picture>();

                using (DataTable dt = ds.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var strr = "0000000" + dr["PictureId"].ToString();
                        (strr).Substring(strr.Length - 7);
                        string typeimage = dr["MimeType"].ToString().Replace("image/", "");
                        LPicture.Add(new Picture()
                        {

                            picture_Id = dr["PictureId"].ToString(),
                            // name = dr["Name"].ToString(),
                            picture_type = typeimage,
                            picture_url = "https://mebli.ars.ua/content/images/thumbs/" + (strr).Substring(strr.Length - 7) + "_" + dr["SeoFilename"].ToString() + "." + typeimage,

                        });
                    }

                }


                return LPicture;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }

        }

        static DataTable dt_category;
        //private List<Category> Category()
        static private DataTable Category_DT()
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                      // string sqlStr = "select [Id],[Name],[ParentCategoryId],[PictureId],[Published],[Deleted] from Category  where Deleted=0 AND Published=1";

                        string sqlStr = "select [Id],[Name],[ParentCategoryId],[PictureId],[Published],[Deleted] from Category  where Deleted=0 AND Published=1 and (ParentCategoryId in (SELECT distinct Id from Category where Published=1 and deleted=0 )or ParentCategoryId=0) order by id";

                        //string sqlStr = "exec us_selectpromzvarych 1";






                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStr, sqlCon))
                        {
                            sqlDa.Fill(ds);
                        }


                    }
                    catch
                    {


                    }
                    finally

                    {

                        sqlCon.Close();
                    }
                }

                dt_category = ds.Tables[0];


            }
            return dt_category;
        }

        static private List<Category> Category(DataTable dt)
        {

            List<Category> ListCategory = new List<Category>();
            foreach (DataRow dr in dt.Rows)
            {
                ListCategory.Add(new Category()
                {
                    id = dr["Id"].ToString(),
                    name = dr["Name"].ToString(),
                    parentcategoryid = Convert.ToInt32(dr["ParentCategoryId"].ToString()),

                });
                Console.WriteLine("Добавляємо категорію id=" + dr["Id"].ToString() + " name:" + dr["Name"].ToString());
            }
            return ListCategory;

        }

        static private List<Attribute> vzyatuattribute(int id)
        {
            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {

                        sqlCon.Open();

                        string sqlStr = "SELECT A.Name,B.Name as NameOption, B.Id from  SpecificationAttribute A, SpecificationAttributeOption B where A.id in (SELECT[Id] from SpecificationAttribute where id in (SELECT[SpecificationAttributeId] from SpecificationAttributeOption where id in (SELECT[SpecificationAttributeOptionId] FROM[ShopMebli].[dbo].[Product_SpecificationAttribute_Mapping] where ProductId = '" + id + "')))  and A.id = B.SpecificationAttributeId and B.id in (SELECT[SpecificationAttributeOptionId] FROM[ShopMebli].[dbo].[Product_SpecificationAttribute_Mapping] where ProductId = '" + id + "')";

                        //   string sqlStr = "select * from Product_SpecificationAttribute_Mapping Where id='" + id + "'";



                        using (SqlDataAdapter sqlDa = new SqlDataAdapter(sqlStr, sqlCon))
                        {
                            sqlDa.Fill(ds);
                        }


                    }
                    catch
                    {


                    }
                    finally

                    {

                        sqlCon.Close();
                    }
                }

                List<Attribute> Schedule2 = new List<Attribute>();

                using (DataTable dt = ds.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Schedule2.Add(new Attribute()
                        {
                            id = Convert.ToInt32(dr["Id"].ToString()),
                            name = dr["Name"].ToString(),
                            nameoption = dr["NameOption"].ToString(),

                        });
                    }

                }



                return Schedule2;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }
        }

        static Dictionary<int, Category> listpovtorne = new Dictionary<int, Category>();

        static private void CategoryXMLPlus(ref XmlTextWriter xmlWriter, int l, int parentcategoryid)
        {

            var listcat = Category(Category_DT());
            var ppp = listcat.Where(p => Convert.ToInt32(p.id) == parentcategoryid).ToList();

            if (ppp[0].parentcategoryid > 0)
            {

                xmlWriter.WriteStartElement("category");
                xmlWriter.WriteStartAttribute("id");
                xmlWriter.WriteString(ppp[0].id);
                xmlWriter.WriteEndAttribute();
                xmlWriter.WriteStartAttribute("parentId");
                xmlWriter.WriteString(ppp[0].parentcategoryid.ToString());
                xmlWriter.WriteEndAttribute();

                xmlWriter.WriteString(ppp[0].name);
                xmlWriter.WriteEndElement();

                // this.BeginInvoke(new Action(() => this.tbUrl.Text = ppp[0].parentcategoryid.ToString()));
                // this.BeginInvoke(new Action(() => this.tbUrl.Refresh()));


            }

            if (ppp[0].parentcategoryid == 0)
            {

                if (!listpovtorne.ContainsKey(Convert.ToInt32(ppp[0].id)))
                {
                    //itemProps.Add(propName, propValue);
                    listpovtorne[Convert.ToInt32(ppp[0].id)] = ppp[0];
                }



                //xmlWriter.WriteStartElement("category");
                //xmlWriter.WriteStartAttribute("id");
                //xmlWriter.WriteString(ppp[0].id);
                //xmlWriter.WriteEndAttribute();
                ////xmlWriter.WriteStartAttribute("parentId");
                ////xmlWriter.WriteString(ppp[0].parentcategoryid.ToString());
                ////xmlWriter.WriteEndAttribute();

                //xmlWriter.WriteString(ppp[0].name);
                //xmlWriter.WriteEndElement();
                return;
            }



            parentcategoryid = ppp[0].parentcategoryid;
            CategoryXMLPlus(ref xmlWriter, l, parentcategoryid);
        }

        static private void CategoryXMLHotlinePlus(ref XmlTextWriter xmlWriter, int l, int parentcategoryid)
        {

            var listcat = Category(Category_DT());
            var ppp = listcat.Where(p => Convert.ToInt32(p.id) == parentcategoryid).ToList();

            if (ppp[0].parentcategoryid > 0)
            {

                xmlWriter.WriteStartElement("category");
                xmlWriter.WriteStartElement("id");
                xmlWriter.WriteString(ppp[0].id);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("parentId");
                xmlWriter.WriteString(ppp[0].parentcategoryid.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(ppp[0].name);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                // this.BeginInvoke(new Action(() => this.tbUrl.Text = ppp[0].parentcategoryid.ToString()));
                // this.BeginInvoke(new Action(() => this.tbUrl.Refresh()));


            }

            if (ppp[0].parentcategoryid == 0)
            {

                if (!listpovtorne.ContainsKey(Convert.ToInt32(ppp[0].id)))
                {
                    //itemProps.Add(propName, propValue);
                    listpovtorne[Convert.ToInt32(ppp[0].id)] = ppp[0];
                }



                //xmlWriter.WriteStartElement("category");
                //xmlWriter.WriteStartAttribute("id");
                //xmlWriter.WriteString(ppp[0].id);
                //xmlWriter.WriteEndAttribute();
                ////xmlWriter.WriteStartAttribute("parentId");
                ////xmlWriter.WriteString(ppp[0].parentcategoryid.ToString());
                ////xmlWriter.WriteEndAttribute();

                //xmlWriter.WriteString(ppp[0].name);
                //xmlWriter.WriteEndElement();
                return;
            }



            parentcategoryid = ppp[0].parentcategoryid;
            CategoryXMLHotlinePlus(ref xmlWriter, l, parentcategoryid);
        }


      static  private string FileCopyXML(string filename)
        {
            string[] fileList;
            string networkfilename = "";
            //Directory.GetCurrentDirectory() + "" + name

            //  var networkPath = @"\\192.168.4.100\d$\Root\shopmebli.ars.ua\Content\files\ExportImport";
            var networkPath = @"\\192.168.4.100\d$\Root\shopmebli.ars.ua\Content\FileManager";

            var credentials = new NetworkCredential("Vitalik_Z", "VitalikDriverRu");

            using (new NetworkConnection(networkPath, credentials))
            {


                string sourceDir = Directory.GetCurrentDirectory();
                string backupDirbac = Directory.GetCurrentDirectory();
                string backupDir = networkPath;
                DateTime dt = DateTime.Now;
                string value = String.Format("{0:yyyy-MM-dd_HH-mm}", dt);

                try
                {
                    string[] xml_list = Directory.GetFiles(sourceDir, filename);



                    foreach (string f in xml_list)
                    {
                        // Remove path from the file name.
                        string fName = f.Substring(sourceDir.Length + 1);

                        // Use the Path.Combine method to safely append the file name to the path.
                        // Will overwrite if the destination file already exists.
                        if (fName.Contains("rozetka"))
                        {
                            File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDirbac, "YML_price_rozetka_" + value + ".xml"), true);
                        }
                        if (fName.Contains("hotline"))
                        {
                            File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDirbac, "YML_price_hotline_" + value + ".xml"), true);
                        }

                        if (fName.Contains("prom"))
                        {
                            File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDirbac, "YML_price_prom_" + value + ".xml"), true);
                        }
                        File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
                        networkfilename = fName;
                    }


                    //MessageBox.Show("Ссилку сформовано");



                    foreach (string f in xml_list)
                    {
                        //File.Delete(f);
                    }
                }

                catch (DirectoryNotFoundException dirNotFound)
                {
                   Console.WriteLine(dirNotFound.Message);
                }
            }
            return networkfilename;
        }



    }
}

