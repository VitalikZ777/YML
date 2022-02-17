using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace YML_Forms
{
    public partial class Form1 : Form
    {
        public string ConnectionString = "Data Source=192.168.4.100;" +
            // public string ConnectionString = "Data Source=192.168.4.85;" +
            "Initial Catalog=ShopMebli;" +
            "Persist Security Info=False;" +
            // "User ID=Vitalik_Z;Password=VitalikDriverRu;" +
            "User ID=kitchen;Password=0921;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=True;" +
            "Connection Timeout=30;";



        public Form1()
        {
            InitializeComponent();

         treeView1.AfterExpand += treeView1_AfterExpand;

            treeView1.CheckBoxes = true;
            CreateTreeCat(treeView1.Nodes, 0);
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += ThreadExceptionHandler;

        }

        private static void ThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            //log or display the exception
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("CurrentDomain_UnhandledException:" + ((Exception)e.ExceptionObject).Message);
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show("TaskScheduler_UnobservedTaskException:" + e.Exception.Message);
        }


        List<String> dlyadobavlennya = new List<String>();
        Dictionary<string, string> dictionaryProduct = new Dictionary<string, string>();
        SqlConnection sqlCon;
        Dictionary<string, string> checkednodes = new Dictionary<string, string>();


        //------------------------------------------------------замість нього є vzyatutovaru
        private List<Product> ReadDataInfo(string id)
        {
            SqlDataReader rdr = null;

            List<Product> ProductInfo = new List<Product>();
            try
            {
                sqlCon = new SqlConnection(ConnectionString);

                sqlCon.Open();
                //SqlCommand cmd = new SqlCommand("SELECT [Name] FROM[ShopMebli].[dbo].[Manufacturer] where id = (SELECT[ManufacturerId] FROM[ShopMebli].[dbo].[Product_Manufacturer_Mapping] where ProductId ='" + id + "')", sqlCon);
                SqlCommand cmd = new SqlCommand("select * from Product Where id='" + id + "'", sqlCon);

                rdr = cmd.ExecuteReader();

                // print the CategoryName of each record
                while (rdr.Read())
                {
                    MessageBox.Show(rdr["Name"].ToString());
                    ProductInfo.Add(new Product()
                    {
                        id = rdr["Id"].ToString(),
                        name = rdr["Name"].ToString(),
                        price = Convert.ToDecimal(rdr["Price"].ToString()),
                        published = rdr["Published"].ToString(),
                        sku = rdr["SKU"].ToString(),
                        fulldescription = rdr["FullDescription"].ToString(),

                    });
                }
                cmd = new SqlCommand("SELECT [Name],[Id] FROM[ShopMebli].[dbo].[Manufacturer] where id = (SELECT[ManufacturerId] FROM[ShopMebli].[dbo].[Product_Manufacturer_Mapping] where ProductId ='" + id + "')", sqlCon);

                while (rdr.Read())
                {

                    var id1 = rdr["Id"].ToString();

                    var prod = ProductInfo.Find(x => x.id.Contains(id1));
                    if (prod != null)
                    { prod.manufactured = rdr["Name"].ToString(); }
                    //manufactured = rdr["Name"].ToString();
                        ;
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
            return ProductInfo;
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

        private List<Product> vzyatutovaru(string id, int nayavnist)
        {


            string sqlfragment = "";

            if (nayavnist == 0)
            {
                sqlfragment = "AND D.DisableBuyButton=1";
            }
            else if (nayavnist == 1)
            {
                sqlfragment = "AND D.DisableBuyButton=0";
            }
            else if (nayavnist == 2)
            {
                sqlfragment = "";
            }



            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                        //   string sqlStr = "select C.*,P.Name AS ProductName from Product_Category_Mapping AS C left join [ShopMebli].[dbo].[Product] P on P.Id=C.ProductId where C.CategoryId='" + id + "'";

                        //                        string sqlStr = "; WITH #tree(namegr, nkey, level, levelsort, pathstr)AS( SELECT Name, Id, 0, 0,',' + cast(Id as varchar(max)) + ','FROM Category WHERE ParentCategoryId = 0 " +
                        //                            "UNION ALL SELECT Category.name, Category.id, #tree.level+1, #tree.levelsort+1, #tree.pathstr+cast(Category.id as varchar(max))+',' FROM Category INNER JOIN #tree " +
                        //              "ON #tree.nkey = Category.ParentCategoryId) SELECT M.name as Vendor,C.CategoryId,P.Slug as Slug_rus,D.* FROM[ShopMebli].[dbo].[Product] AS D " +
                        //"left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId=D.Id " +
                        //"left join[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId=D.id " +
                        //"left join[ShopMebli].[dbo].[Manufacturer] M on M.ID=PM.ManufacturerId " +
                        //"left join Product_Category_Mapping C on C.ProductId=D.id " +
                        //"where(P.LanguageId= 0 AND P.IsActive=1  AND P.EntityName='Product' AND D.Deleted= 0 AND D.Price>=0 AND DisableBuyButton=" + sqlfragment+ " AND D.Published=1 ) and C.CategoryId in " +
                        //              "(select nkey from #tree  where pathstr like '%,"+id+",%' )";


                        string sqlStr = ";WITH #tree(namegr, nkey, level, levelsort, pathstr) " +

         "AS (SELECT Name, Id, 0, 0, ',' + cast(Id as varchar(max)) + ',' " +


         "FROM[ShopMebli].[dbo].[Category] WHERE ParentCategoryId = 0 UNION ALL " +

         "SELECT Category.name, Category.id, #tree.level+1, #tree.levelsort+1, #tree.pathstr+cast(Category.id as varchar(max))+',' " +

         "FROM[ShopMebli].[dbo].[Category] INNER JOIN #tree ON #tree.nkey = Category.ParentCategoryId) " +


             "SELECT M.name as Vendor, C.CategoryId, P.Slug as Slug_rus, D.* FROM[ShopMebli].[dbo].[Product] AS D " +

                    "left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId = D.Id " +

                    "left join[ShopMebli].[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId = D.id " +

                    "left join[ShopMebli].[dbo].[Manufacturer] M on M.ID = PM.ManufacturerId " +

                    "left join[ShopMebli].[dbo].[Product_Category_Mapping] C on C.ProductId = D.id " +

                    "where D.ParentGroupedProductId = 0 and(P.LanguageId = 0  AND P.IsActive = 1 AND D.Deleted = 0  AND P.EntityName = 'Product' AND D.Price >= 0  AND D.Published = 1 " + sqlfragment + ") " +

                                "AND  C.CategoryId in (select nkey from #tree  where pathstr like '%," + id + ",%') " +
                                "and D.id not in (select distinct ParentGroupedProductId from[ShopMebli].[dbo].[Product] where ParentGroupedProductId <> 0) " +

                    "union all " +

                    "select M.name as Vendor,C.CategoryId, P.Slug as Slug_rus, D.* from[ShopMebli].[dbo].[Product] as D " +

                    "left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId=D.Id " +
"left join[ShopMebli].[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId=D.ParentGroupedProductId " +
"left join[ShopMebli].[dbo].[Manufacturer] M on M.ID=PM.ManufacturerId " +
"left join(select P2.id, P2.Name from [ShopMebli].[dbo].[Product] P2 where P2.id in (select ParentGroupedProductId from [ShopMebli].[dbo].[Product] where ParentGroupedProductId>0)) F on F.Id=D.ParentGroupedProductId " +
"left join[ShopMebli].[dbo].[Product_Category_Mapping] C on C.ProductId=F.id " +
"where ParentGroupedProductId>0 and(P.LanguageId= 0  AND P.IsActive= 1 AND D.Deleted= 0  AND P.EntityName= 'Product' AND D.Price>=0  AND D.Published= 1 " + sqlfragment + ") " +

                               " AND C.CategoryId in (select nkey from #tree  where pathstr like '%," + id + ",%') " +


             " order by D.id";

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
                            //для першого селекта
                            // id = dr["ProductId"].ToString(),
                            //name = dr["ProductName"].ToString(),

                        });

                        
                        vendorlist.Add(dr["Vendor"].ToString());
                      
                        

                    }

                    
                }
                listBox1.Items.Clear();
                    vendorlistdistinct = vendorlist.Distinct().ToList();
                for (int i = 0; i < vendorlistdistinct.Count; i++)
                {
                   
                    listBox1.Items.Add(vendorlistdistinct[i].ToString());
               
                 
                }

                return L2Product;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }

        }


        //перегрузка методу vzyatutovaru
        private List<Product> vzyatutovaru(string id, int nayavnist, string sqlplusstring)
        {


            string sqlfragment = "";

            if (nayavnist == 0)
            {
                sqlfragment = "AND D.DisableBuyButton=1 AND D.StockQuantity=0";
            }
            else if (nayavnist == 1)
            {
                sqlfragment = "AND D.DisableBuyButton=0 AND  D.StockQuantity>0";
            }
            else if (nayavnist == 2)
            {
                sqlfragment = "";
            }



            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                        //   string sqlStr = "select C.*,P.Name AS ProductName from Product_Category_Mapping AS C left join [ShopMebli].[dbo].[Product] P on P.Id=C.ProductId where C.CategoryId='" + id + "'";

                        //                        string sqlStr = "; WITH #tree(namegr, nkey, level, levelsort, pathstr)AS( SELECT Name, Id, 0, 0,',' + cast(Id as varchar(max)) + ','FROM Category WHERE ParentCategoryId = 0 " +
                        //                            "UNION ALL SELECT Category.name, Category.id, #tree.level+1, #tree.levelsort+1, #tree.pathstr+cast(Category.id as varchar(max))+',' FROM Category INNER JOIN #tree " +
                        //              "ON #tree.nkey = Category.ParentCategoryId) SELECT M.name as Vendor,C.CategoryId,P.Slug as Slug_rus,D.* FROM[ShopMebli].[dbo].[Product] AS D " +
                        //"left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId=D.Id " +
                        //"left join[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId=D.id " +
                        //"left join[ShopMebli].[dbo].[Manufacturer] M on M.ID=PM.ManufacturerId " +
                        //"left join Product_Category_Mapping C on C.ProductId=D.id " +
                        //"where(P.LanguageId= 0 AND P.IsActive=1  AND P.EntityName='Product' AND D.Deleted= 0 AND D.Price>=0 AND DisableBuyButton=" + sqlfragment+ " AND D.Published=1 ) and C.CategoryId in " +
                        //              "(select nkey from #tree  where pathstr like '%,"+id+",%' )";


                        string sqlStr = ";WITH #tree(namegr, nkey, level, levelsort, pathstr) " +

         "AS (SELECT Name, Id, 0, 0, ',' + cast(Id as varchar(max)) + ',' " +


         "FROM[ShopMebli].[dbo].[Category] WHERE ParentCategoryId = 0 UNION ALL " +

         "SELECT Category.name, Category.id, #tree.level+1, #tree.levelsort+1, #tree.pathstr+cast(Category.id as varchar(max))+',' " +

         "FROM[ShopMebli].[dbo].[Category] INNER JOIN #tree ON #tree.nkey = Category.ParentCategoryId) " +


             "SELECT M.name as Vendor, C.CategoryId, P.Slug as Slug_rus, D.* FROM[ShopMebli].[dbo].[Product] AS D " +

                    "left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId = D.Id " +

                    "left join[ShopMebli].[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId = D.id " +

                    "left join[ShopMebli].[dbo].[Manufacturer] M on M.ID = PM.ManufacturerId " +

                    "left join[ShopMebli].[dbo].[Product_Category_Mapping] C on C.ProductId = D.id " +

                    "where D.ParentGroupedProductId = 0 " + sqlplusstring + " and(P.LanguageId = 0  AND P.IsActive = 1 AND D.Deleted = 0  AND P.EntityName = 'Product' AND D.Price > 0  AND D.Published = 1 " + sqlfragment + " ) " +

                                "AND  C.CategoryId in (select nkey from #tree  where pathstr like '%," + id + ",%') " +
                                "and D.id not in (select distinct ParentGroupedProductId from[ShopMebli].[dbo].[Product] where ParentGroupedProductId <> 0) " +

                    "union all " +

                    "select M.name as Vendor,C.CategoryId, P.Slug as Slug_rus, D.* from[ShopMebli].[dbo].[Product] as D " +

                    "left join[ShopMebli].[dbo].[UrlRecord] P on P.EntityId=D.Id " +
"left join[ShopMebli].[dbo].[Product_Manufacturer_Mapping] PM on PM.ProductId=D.ParentGroupedProductId " +
"left join[ShopMebli].[dbo].[Manufacturer] M on M.ID=PM.ManufacturerId " +
"left join(select P2.id, P2.Name from [ShopMebli].[dbo].[Product] P2 where P2.id in (select ParentGroupedProductId from [ShopMebli].[dbo].[Product] where ParentGroupedProductId>0)) F on F.Id=D.ParentGroupedProductId " +
"left join[ShopMebli].[dbo].[Product_Category_Mapping] C on C.ProductId=F.id " +
"where ParentGroupedProductId>0 " + sqlplusstring + " and(P.LanguageId= 0  AND P.IsActive= 1 AND D.Deleted= 0  AND P.EntityName= 'Product' AND D.Price>0  AND D.Published= 1 " + sqlfragment + " ) " + //AND  D.DisplayStockAvailability=1 

                               " AND C.CategoryId in (select nkey from #tree  where pathstr like '%," + id + ",%') " +


             " order by D.id";

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
                            stockquantity= dr["StockQuantity"].ToString(),
                            //для першого селекта
                            // id = dr["ProductId"].ToString(),
                            //name = dr["ProductName"].ToString(),

                        });


                        vendorlist.Add(dr["Vendor"].ToString());



                    }


                }
                listBox1.Items.Clear();
                vendorlistdistinct = vendorlist.Distinct().ToList();
                for (int i = 0; i < vendorlistdistinct.Count; i++)
                {

                    listBox1.Items.Add(vendorlistdistinct[i].ToString());


                }

                return L2Product;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }

        }

        private List<Product> vzyatutovaruexec(string id)
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                      
                        sqlCon.Open();
                      


                        //string sqlStr = "EXEC dbo.us_selectpromzvarych @categoryid = '" + id + "'";
                        string sqlStr = "EXEC dbo.us_selectpromzvarych";

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
                listBox1.Items.Clear();
                vendorlistdistinct = vendorlist.Distinct().ToList();
                for (int i = 0; i < vendorlistdistinct.Count; i++)
                {

                    listBox1.Items.Add(vendorlistdistinct[i].ToString());


                }

                return L2Product;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }

        }

        private List<Picture> Take_picture(string id)
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
                        string typeimage=dr["MimeType"].ToString().Replace("image/", "");
                        LPicture.Add(new Picture()
                        {
                           
                            picture_Id = dr["PictureId"].ToString(),
                            // name = dr["Name"].ToString(),
                            picture_type= typeimage,
                            picture_url = "https://mebli.ars.ua/content/images/thumbs/" + (strr).Substring(strr.Length - 7) + "_" + dr["SeoFilename"].ToString() + "."+ typeimage,

                        });
                    }

                }


                return LPicture;
                // textBox1.Text = Schedule2[0].specificationattributeoptionid.ToString();

            }

        }

        DataTable dt_category;
        //private List<Category> Category()
        private DataTable Category_DT()
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                        string sqlStr = "select [Id],[Name],[ParentCategoryId],[PictureId],[Published],[Deleted] from Category  where Deleted=0 AND Published=1";


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

        private List<Category> Category(DataTable dt)
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

            }
            return ListCategory;

        }

        private void CreateTreeCat(TreeNodeCollection parentNode, int parentID)
        {

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                        string sqlStr = "select [Id],[Name],[ParentCategoryId],[PictureId],[Published],[Deleted] from Category  where Deleted=0 AND Published=1";


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

                List<Category> listCategory = new List<Category>();

                using (DataTable dt = ds.Tables[0])
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["ParentCategoryId"]) == parentID)
                        {
                            String id = dr["Id"].ToString();
                            String name = dr["Name"].ToString();
                            int parentcategoryid = Convert.ToInt32(dr["ParentCategoryId"].ToString());


                            // Dictionary<string, string> dictionaryCategory = new Dictionary<string, string>();





                            TreeNodeCollection newParentNode = parentNode.Add(id, name).Nodes;


                            CreateTreeCat(newParentNode, Convert.ToInt32(dr["Id"]));

                            //dictionaryCategory.Add(

                            //   dr["Id"].ToString(),
                            //  dr["Name"].ToString()

                            // );

                            //foreach (KeyValuePair<string, string> keyValue in dictionaryCategory)
                            //  {
                            // keyValue.Value представляет класс Person
                            //  comboBox1.Items.Add(keyValue.Key + " - " + keyValue.Value);
                            // }
                        }


                    }





                }






            }


        }

        private List<Attribute> vzyatuattribute(int id)
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

        Dictionary <int,Category> listpovtorne = new Dictionary<int, Category>();

        private void CategoryXMLPlus(ref XmlTextWriter xmlWriter,int l, int parentcategoryid)
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

        private void CategoryXMLHotlinePlus(ref XmlTextWriter xmlWriter, int l, int parentcategoryid)
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


        string filename;

        private Task SformuvatuXMLAsync()
        {

            progressBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            progressBar1.Maximum = product.Count;
            if (product.Count == 0)
                progressBar1.Value = 0;
            else
                progressBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar1.Step = 1;



            return Task.Factory.StartNew(() =>
            {


                DateTime dt = DateTime.Now;

                var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);


                string value = String.Format("{0:yyyy-MM-dd_HH-mm}", dt);


                //ВВести назву файлу
                //if (InputBox("Назва файлу для створення", "Введіть назву XML файлу:", ref value) == DialogResult.OK)
                //{
                //    filename = value+ "_rozetka.xml";
                //}


                filename = "YML_price" +   "_rozetka.xml";
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


                    this.BeginInvoke((Action)(() => progressBar1.PerformStep()));
                    //progressBar1.PerformStep();


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
                        xmlWriter.WriteString("true");
                    else
                        xmlWriter.WriteString("false");

                    xmlWriter.WriteEndAttribute();

                    xmlWriter.WriteStartElement("url");
                    xmlWriter.WriteString(product[p].url);
                    xmlWriter.WriteEndElement();





                    xmlWriter.WriteStartElement("price");

                    var price = product[p].price;
                    if (price > decimal.ToInt32(price))
                        price = (decimal.Round(price, 2));
                    else
                        price = (decimal.ToInt32(price));

                    xmlWriter.WriteString(price.ToString().Replace(",","."));

                    xmlWriter.WriteEndElement();

                    /////old price
                    

                    var oldprice = product[p].oldprice;
                    if (oldprice > decimal.ToInt32(oldprice))
                        oldprice = (decimal.Round(oldprice, 2));
                    else
                        oldprice = (decimal.ToInt32(oldprice));

                    if (oldprice > 0)
                    {
                        xmlWriter.WriteStartElement("price_old");


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

                    xmlWriter.WriteStartElement("stock_quantity");
                    //////////////
                    xmlWriter.WriteString("1");
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("name");
                    xmlWriter.WriteString(product[p].name.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("description");
                    xmlWriter.WriteCData(product[p].fulldescription.ToString());
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

                    this.BeginInvoke(new Action(() => this.label1.Text = "Додано до XML: " + product_name));
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



                //  this.BeginInvoke((Action)(() => MessageBox.Show("Файл сформовано")));
                MessageBox.Show("Файл сформовано");



            });



            //MessageBox.Show("Файл сформовано");
        }

        private Task SformuvatuXMLPromAsync()
        {

            progressBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            progressBar1.Maximum = product.Count;
            if (product.Count == 0)
                progressBar1.Value = 0;
            else
                progressBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar1.Step = 1;



            return Task.Factory.StartNew(() =>
            {


                DateTime dt = DateTime.Now;

                var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);

                string descriptiontop = "<div class=\"ck-alert ck-alert_theme_green\">"+
    "<div><span class=\"ck-alert__title\"></span>"+
"<div id =\"gt-res-content\">"+
"<div class=\"trans-verified-button-large\" dir=\"ltr\" id=\"gt-res-dir-ctr\">"+
"<div id =\"tts_button\"><object data=\"//ssl.gstatic.com/translate/sound_player2.swf\" height=\"18\" id=\"tts\" type=\"application/x-shockwave-flash\" width=\"18\"></object></div>"+

"<p style=\"text-align: center;\"><span style=\"font-size:14px;\"><span class=\"short_text\" id=\"result_box\" lang=\"ru\">Рады приветствовать Вас в интернет-магазине &quot;<a href=\"https://shopars.com.ua/\" target=\"_blank\">АРС</a>&quot;.</span></span></p>"+
"</div>"+
"</div>"+

"<p style =\"text-align: center;\"><span style=\"font-size:14px;\">Если у Вас возникли вопросы, Вы можете связаться с нашим менеджером по телефону <strong>+380961764274 </strong> или <a href=\"https://shopars.com.ua/contacts\" target=\"_blank\"><strong><span class=\"short_text\" id=\"result_box\" lang=\"ru\">Написать на почту.</span></strong></a></span></p>"+

"<h3 style =\"text-align: center;\"><span style=\"font-size:14px;\">Также Вы можете ознакомиться с полным ассортиментом товаров.</span></h3>"+

"<p style = \"text-align: center;\"><span style= \"font-size:14px;\"><a href= \"https://shopars.com.ua/product_list\" target=\"_blank\"><strong> ПОСМОТРЕТЬ АССОРТИМЕНТ</strong></a></span></p>"+
"<span></span></div>"+
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

"<h2 style=\"text-align:center\">&nbsp;</h2 ><h2 style=\"text-align:center\"><span style=\"font-size:18px;\"><strong>Все акции и </strong><span style=\"font-size:18px;\"><strong >новинки </strong ></span ><strong> Вы сможете найти на нашем сайте <a href = \"https://mebli.ars.ua/\" target=\"_blank\">mebli.ars.ua</a></strong></span></h2>"+
"<h2 style=\"text-align:center\"><a href=\"https://mebli.ars.ua/\"><img alt=\"\" src=\"https://my.prom.ua/media/images/1342006587_w640_h2048_logo.png?PIMAGE_ID=1342006587\" style=\"width: 346px; height: 78px;\"/></a></h2><p> &nbsp;</p>"+
 "<h2 style =\"text-align:center\"><strong><span style=\"font-size:18px;\">Мы в социальных сетях</span></strong></h2>" +

"<p>&nbsp;</p>" +

"<div class=\"ck-social ck-social_type_lite ck-theme-grey\">" +
"<div class=\"ck-social__image-wrapper\"><a href=\"https://www.instagram.com/mebli.ars.ua/\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://a.radikal.ru/a39/1808/6f/c2c5b884e24a.png\" style=\"width:100px;height:100px\" /></a></div>" +

"<div class=\"ck-social__image-wrapper\"><a href=\"https://plus.google.com/+ІнтернетмагазинМеблевогоЦентруАРСТернопіль\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://c.radikal.ru/c11/1809/11/5301f2db9445.png\" style=\"width:100px;height:100px\" /></a></div>" +

"<div class=\"ck-social__image-wrapper ck-social__image-wrapper_type_without-margin\"><a href=\"https://www.facebook.com/mebli.ars.ua\" target=\"_blank\"><img alt=\"\" class=\"ck-social__image\" src=\"https://b.radikal.ru/b38/1808/59/7f0bff7cd5d0.png\" style=\"width:100px;height:100px\" /></a></div>" +
"</div>"+

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


                    this.BeginInvoke((Action)(() => progressBar1.PerformStep()));
                    //progressBar1.PerformStep();


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
                        xmlWriter.WriteString("true");
                    else
                        xmlWriter.WriteString("false");

                    xmlWriter.WriteEndAttribute();

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
                    xmlWriter.WriteCData(descriptiontop+product[p].fulldescription.ToString()+ descriptionbottom);
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
                    this.BeginInvoke(new Action(() => this.label1.Text = "Додано до XML: " + product_name));
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



                //  this.BeginInvoke((Action)(() => MessageBox.Show("Файл сформовано")));
                MessageBox.Show("Файл сформовано");



            });



            //MessageBox.Show("Файл сформовано");
        }

        private Task SformuvatuXMLHotlineAsync()
        {

            progressBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            progressBar1.Maximum = product.Count;
            if (product.Count == 0)
                progressBar1.Value = 0;
            else
                progressBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar1.Step = 1;



            return Task.Factory.StartNew(() =>
            {


                DateTime dt = DateTime.Now;

                var today = String.Format("{0:yyyy-MM-dd HH:mm}", dt);


                string value = String.Format("{0:yyyy-MM-dd_HH-mm}", dt);


                //if (InputBox("Назва файлу для створення", "Введіть назву XML файлу:", ref value) == DialogResult.OK)
                //{
                // filename = value + "_hotline.xml";
                //}
                filename = "YML_price" + "_hotline.xml";
                //string filename = "XML_" + String.Format("{0:yyyy-MM-dd_HH-mm}", dt) + "_hotline.xml";

                var xmlWriter = new XmlTextWriter(filename, null);

                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.IndentChar = '\t';
                xmlWriter.Indentation = 1;

                xmlWriter.WriteStartDocument(); // <?xml version="1.0"?>+



                xmlWriter.WriteStartElement("price");
                xmlWriter.WriteStartElement("date");



                xmlWriter.WriteString(today);
                //  xmlWriter.WriteString(DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss"));
                xmlWriter.WriteEndElement();






                xmlWriter.WriteStartElement("firmName");
                xmlWriter.WriteString("ТОВ торгова група \"АРС кераміка\"");
                xmlWriter.WriteEndElement();



                xmlWriter.WriteStartElement("rate");
                xmlWriter.WriteString("");
                xmlWriter.WriteEndElement();
                //   xmlWriter.WriteStartElement("currencies");
                //   xmlWriter.WriteStartElement("currency");
                //  xmlWriter.WriteStartAttribute("id");
                //   xmlWriter.WriteString("UAH");

                //    xmlWriter.WriteStartAttribute("rate");
                //    xmlWriter.WriteString("1");
                //    xmlWriter.WriteEndAttribute();
                //    xmlWriter.WriteEndElement();
                //    xmlWriter.WriteEndElement();


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

                            xmlWriter.WriteStartElement("id");
                            xmlWriter.WriteString(listcat[l].id);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteStartElement("name");
                            xmlWriter.WriteString(listcat[l].name);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteEndElement();

                        }
                        else
                        {

                            xmlWriter.WriteStartElement("category");
                            xmlWriter.WriteStartElement("id");
                            xmlWriter.WriteString(listcat[l].id);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("parentId");
                            xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteStartElement("name");
                            xmlWriter.WriteString(listcat[l].name);
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteEndElement();

                            CategoryXMLHotlinePlus(ref xmlWriter, l, listcat[l].parentcategoryid);
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
                        xmlWriter.WriteStartElement("id");
                        xmlWriter.WriteString(item.Value.id);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(item.Value.name);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();

                    }

                }
                //var listcat = Category(Category_DT());

                //for (int l = 0; l < listcat.Count; l++)
                //{
                //    if (listcat[l].parentcategoryid == 0)

                //    {
                //        xmlWriter.WriteStartElement("category");
                //        xmlWriter.WriteStartElement("id");
                //        xmlWriter.WriteString(listcat[l].id);
                //        xmlWriter.WriteEndElement();

                //        xmlWriter.WriteStartElement("name");
                //        xmlWriter.WriteString(listcat[l].name);
                //        xmlWriter.WriteEndElement();

                //        xmlWriter.WriteEndElement();

                //    }
                //    else
                //    {

                //        xmlWriter.WriteStartElement("category");
                //        xmlWriter.WriteStartElement("id");
                //        xmlWriter.WriteString(listcat[l].id);
                //        xmlWriter.WriteEndElement();

                //        xmlWriter.WriteStartElement("name");
                //        xmlWriter.WriteString(listcat[l].name);
                //        xmlWriter.WriteEndElement();

                //        xmlWriter.WriteStartElement("parentId");
                //        xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
                //        xmlWriter.WriteEndElement();

                //        xmlWriter.WriteEndElement();
                //        //xmlWriter.WriteStartElement("category");
                //        //xmlWriter.WriteStartAttribute("id");
                //        //xmlWriter.WriteString(listcat[l].id);
                //        //xmlWriter.WriteEndAttribute();
                //        //xmlWriter.WriteStartAttribute("parentId");
                //        //xmlWriter.WriteString(listcat[l].parentcategoryid.ToString());
                //        //xmlWriter.WriteEndAttribute();
                //        //xmlWriter.WriteString(listcat[l].name);
                //        //xmlWriter.WriteEndElement();

                //    }
                //}

                xmlWriter.WriteEndElement();
                //xmlWriter.WriteString("2");
                //xmlWriter.WriteEndAttribute();
                //xmlWriter.WriteString("Мужская");
                //xmlWriter.WriteEndElement();
                //xmlWriter.WriteEndElement();
                //xmlWriter.WriteStartElement("offers");


                xmlWriter.WriteStartElement("items");
                for (int p = 0; p < product.Count; p++)   //foreach (var eproduct in product)
                {

                    //SetControlPropertyValue(progressBar1, "value", 123);

                    //Thread.Sleep(10);
                    //  var   th1 = new System.Threading.Thread(AddressOf progressBar1);


                    this.BeginInvoke((Action)(() => progressBar1.PerformStep()));
                    //progressBar1.PerformStep();


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


                    xmlWriter.WriteStartElement("item");

                    xmlWriter.WriteStartElement("id");
                    xmlWriter.WriteString(productid);  //offer id
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("categoryId");
                    //xmlWriter.WriteString(c);
                    xmlWriter.WriteString(product[p].catId);
                    xmlWriter.WriteEndElement();


                    if (product[p].sku.Length > 1)
                    {
                        xmlWriter.WriteStartElement("code");
                        xmlWriter.WriteString(product[p].sku);
                        xmlWriter.WriteEndElement();
                    }


                    if (product[p].vendor != null)
                    {
                        xmlWriter.WriteStartElement("vendor");
                        xmlWriter.WriteString(product[p].vendor);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteStartElement("name");
                    xmlWriter.WriteString(product[p].name.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("description");
                    xmlWriter.WriteCData(product[p].fulldescription.ToString());
                    xmlWriter.WriteEndElement();


                    xmlWriter.WriteStartElement("url");
                    xmlWriter.WriteString(product[p].url);
                    xmlWriter.WriteEndElement();


                    for (int i = 0; i < picture.Count; i++)
                    {
                        xmlWriter.WriteStartElement("image");
                        xmlWriter.WriteString(picture[i].picture_url);
                        xmlWriter.WriteEndElement();

                    }


                    xmlWriter.WriteStartElement("priceRUAH");

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

                 //////////////////////



                    xmlWriter.WriteStartElement("stock");


                    if (product[p].preorder != true)
                        xmlWriter.WriteString("В наличии");
                    else
                        xmlWriter.WriteString("Под заказ");

                    xmlWriter.WriteEndElement();














                    // xmlWriter.WriteStartElement("currencyId");
                    // xmlWriter.WriteString("UAH");
                    /// xmlWriter.WriteEndElement();







                    //  xmlWriter.WriteStartElement("stock_quantity");

                    //   xmlWriter.WriteString("1");
                    //   xmlWriter.WriteEndElement();



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
                    this.BeginInvoke(new Action(() => this.label1.Text = "Додано до XML: " + product_name));
                    //  this.BeginInvoke((Action)(() => label1.Refresh()));

                    //this.BeginInvoke((Action)(() => {
                    //    label1.Text = "Додано до XML: " + product[p].name;
                    //    label1.Refresh();
                    //}));


                }


                xmlWriter.WriteEndElement();






                xmlWriter.Close();



                //  this.BeginInvoke((Action)(() => MessageBox.Show("Файл сформовано")));
                MessageBox.Show("Файл сформовано");



            });



            //MessageBox.Show("Файл сформовано");
        }


        private async void button3_Click(object sender, EventArgs e)
        {


            // await SformuvatuXMLAsync();

            try
            {
                if (radioButton1.Checked)
                {
                    radioButton2.Checked = false;
                    await SformuvatuXMLAsync();

                    //FileCopyXML(filename);
                    tbUrl.Text = "http://mebli.ars.ua/Content/FileManager/" + FileCopyXML(filename);
                    //   tbUrl.Text = "http://mebli.ars.ua/content/files/exportimport/"+FileCopyXML(filename);

                }
                else {
                    await SformuvatuXMLAsync();
                }
            }
            catch (Exception ex)
            {

            }

        }


        private string FileCopyXML(string filename){
            string[] fileList;
            string networkfilename="";
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
                        networkfilename =  fName;
                    }


                    //MessageBox.Show("Ссилку сформовано");

                    

                    foreach (string f in xml_list)
                    {
                        //File.Delete(f);
                    }
                }

                catch (DirectoryNotFoundException dirNotFound)
                {
                    MessageBox.Show(dirNotFound.Message);
                }
            }
return networkfilename;
        }

        private List<string> Tovartocat(string id)
        {
            List<string> ListProduct = new List<string>();

            using (DataSet ds = new DataSet())
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        // var id = textBox1.Text;
                        sqlCon.Open();
                        string sqlStr = "select * from Product_Category_Mapping where ProductId= '" + id + "'";


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




                using (DataTable dt = ds.Tables[0])
                {


                    foreach (DataRow dr in dt.Rows)
                    {
                        var id1 = dr["CategoryId"].ToString();





                        ListProduct.Add(id1);

                    }

                }


            }
            return ListProduct;

        }




        private Dictionary<string, string> RecursiveTree(TreeNode treeNode, string selec)
        {

            //   if (treeNode.TreeView.HasChildren)
            //       MessageBox.Show("Ост");

           
            // List<String> checkednodes = new List<String>();


           

            if (treeNode.Checked && selec=="")
            {

                

                checkednodes.Add(treeNode.Name, treeNode.Text);
                treeNode.BackColor = Color.Yellow;

                //    listBox1.Items.Add(treeNode.Name);



                //  selectedNode += myNode.Text + "11111111111";
                //   countIndex++;
            }
            else
            {
             //   if (treeNode.IsSelected)
              //  {




                    treeNode.BackColor = Color.LightGray;


               // }
                checkednodes.Remove(treeNode.Name);
                //   listBox1.Items.Remove(treeNode.Name);
              //  treeNode.BackColor = Color.White;
            }

            // listBox1.Items.Add(treeNode.Name);

            foreach (TreeNode node in treeNode.Nodes)
            {

                //    listBox1.Items.Clear();
                //   listBox1.Items.Add(node.Name);


            }

            return checkednodes;
        }

        Dictionary<string, string> product_dict = new Dictionary<string, string>();
        List<Product> product = new List<Product>();



        string sqlstringadd = " AND (";
        string sqlstringaddplus = "";

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            tbUrl.Text = "";
            RecursiveTree(e.Node,"");

            List<Product> vnayavnosti;// = vzyatutovaru(e.Node.Name.ToString(), 1);
            List<Product> nemavnayavnosti;// = vzyatutovaru(e.Node.Name.ToString(), 0);
            List<Product> vsitovaru;// = vzyatutovaru(e.Node.Name.ToString(), 2);
            List<string> myList = null ;
            if (e.Node.Checked)
            {

                if (listBox1.SelectedItems.Count > 0)
                {

                   myList = listBox1.SelectedItems.Cast<String>().ToList();


                    foreach (string itemselected in myList)
                    {

                        sqlstringadd += " M.Name='" + itemselected + "' OR ";

                    }

                    

                int sqlstringaddlength = sqlstringadd.Length - 3;
                   sqlstringaddplus =    sqlstringadd.Remove(sqlstringaddlength, 3)+")";

                }


                Nastroyki ini_file = new Nastroyki()

                {

                    ini_preorder = checkBox_preorder.Checked,
                    ini_vsitovaru = checkBox_vsitovaru.Checked,

                    ini_node_name = e.Node.Name.ToString(),


                    ini_vnayavnosti = checkBox_vnayavnosti.Checked,
                    ini_manufact = myList

                };


                CheckBox checkBtn = this.Controls.OfType<CheckBox>().Where(x => x.Checked).FirstOrDefault();
                if (checkBtn != null)
                {
                    if (checkBtn.Checked == true)
                    {
                        switch (checkBtn.Name)
                        {
                            case "checkBox_vnayavnosti":

                               // vnayavnosti = vzyatutovaruexec("147");

                                vnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 1, sqlstringaddplus);
                                product.AddRange(vnayavnosti);
                                label_progress.Text = "Всього товарів додано: " + product.Count;
                                break;

                            case "checkBox_vsitovaru":
                                vsitovaru = vzyatutovaru(e.Node.Name.ToString(), 2, sqlstringaddplus);
                                product.AddRange(vsitovaru);
                                label_progress.Text = "Всього товарів додано: " + product.Count;

                                break;

                            case "checkBox_preorder":
                                nemavnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 0, sqlstringaddplus);
                                product.AddRange(nemavnayavnosti);


                                label_progress.Text = "Всього товарів додано: " + product.Count;
                                break;

                        }

                    }
                }
            }

            else {
                CheckBox checkBtn = this.Controls.OfType<CheckBox>().Where(x => x.Checked).FirstOrDefault();
                if (checkBtn.Checked == true)
            {
                switch (checkBtn.Name)
                {
                    case "checkBox_vnayavnosti":

                        vnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 1, sqlstringaddplus);
                        for (var i = 0; i < vnayavnosti.Count; i++)
                        {
                            product.RemoveAll(r => r.id == vnayavnosti[i].id);

                            // MessageBox.Show("Забрано");
                        }
                        label_progress.Text = "Всього товарів додано: " + product.Count;
                        break;

                    case "checkBox_vsitovaru":
                        vsitovaru = vzyatutovaru(e.Node.Name.ToString(), 2, sqlstringaddplus);
                        for (var i = 0; i < vsitovaru.Count; i++)
                        {
                            product.RemoveAll(r => r.id == vsitovaru[i].id);

                            // MessageBox.Show("Забрано");
                        }
                        label_progress.Text = "Всього товарів додано: " + product.Count;

                        break;

                    case "checkBox_preorder":
                        nemavnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 0, sqlstringaddplus);
                        for (var i = 0; i < nemavnayavnosti.Count; i++)
                        {
                            product.RemoveAll(r => r.id == nemavnayavnosti[i].id);

                            // MessageBox.Show("Забрано");

                        }
                        label_progress.Text = "Всього товарів додано: " + product.Count;

                        break;

                }

                // }
            }


                //        if (checkBox_preorder.Checked)
                //    {

                //       nemavnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 0);
                //       product.AddRange(nemavnayavnosti);


                //        label_progress.Text = "Всього товарів додано: " + product.Count;
                //    }
                //    else
                //    {
                //        if (!checkBox_vsitovaru.Checked)
                //        {
                //            vnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 1, sqlstringadd);
                //            product.AddRange(vnayavnosti);
                //            label_progress.Text = "Всього товарів додано: " + product.Count;
                //        }
                //    }

                //    if (checkBox_vsitovaru.Checked)
                //    {
                //        vsitovaru = vzyatutovaru(e.Node.Name.ToString(), 2);
                //        product.AddRange(vsitovaru);
                //        label_progress.Text = "Всього товарів додано: " + product.Count;
                //    }



                //}

                //else
                //{
                //    if (checkBox_preorder.Checked)
                //    {
                //        nemavnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 0);
                //        for (var i = 0; i < nemavnayavnosti.Count; i++)
                //        {
                //            product.RemoveAll(r => r.id == nemavnayavnosti[i].id);

                //            // MessageBox.Show("Забрано");

                //        }
                //        label_progress.Text = "Всього товарів додано: " + product.Count;
                //    }
                //    else
                //    {
                //        vnayavnosti = vzyatutovaru(e.Node.Name.ToString(), 1);
                //        for (var i = 0; i < vnayavnosti.Count; i++)
                //        {
                //            product.RemoveAll(r => r.id == vnayavnosti[i].id);

                //            // MessageBox.Show("Забрано");
                //        }
                //        label_progress.Text = "Всього товарів додано: " + product.Count;
                //    }

                //    if (checkBox_vsitovaru.Checked)
                //    {

                //        vsitovaru = vzyatutovaru(e.Node.Name.ToString(), 2);
                //        for (var i = 0; i < vsitovaru.Count; i++)
                //        {
                //            product.RemoveAll(r => r.id == vsitovaru[i].id);

                //            // MessageBox.Show("Забрано");
                //        }
                //        label_progress.Text = "Всього товарів додано: " + product.Count;
                //    }


                //}
                //  MessageBox.Show(vnayavnosti.Count+"+"+nemavnayavnosti.Count+"-"+product.Count.ToString());

            }
        }




        private void button10_Click(object sender, EventArgs e)
        {
            var a = vzyatutovaru("119", 1);

            var b = Manufacture("29045");

            var z = Category(Category_DT());


        }

        private async void button11_Click(object sender, EventArgs e)
        {
            // await SformuvatuXMLAsync();

            try
            {
                if (radioButton1.Checked)
                {

                    await SformuvatuXMLPromAsync();
                    radioButton2.Checked = false;
                    //FileCopyXML(filename);

                    tbUrl.Text = "http://mebli.ars.ua/Content/FileManager/" + FileCopyXML(filename);
                }
                else
                {
                    await SformuvatuXMLPromAsync();
                }
            }
            catch (Exception ex)
            {
                tbUrl.Text =  ex.Message;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                
                if (radioButton1.Checked)
                {

                    await SformuvatuXMLHotlineAsync();
                    radioButton2.Checked = false;
                    tbUrl.Text = "http://mebli.ars.ua/Content/FileManager/" + FileCopyXML(filename);

                }
                else {
                    await SformuvatuXMLHotlineAsync();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private void checkBox_vsitovaru_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_vsitovaru.Checked)
            {
                checkBox_preorder.Checked = false;
                checkBox_vnayavnosti.Checked = false;
            }
        }

        private void checkBox_preorder_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_preorder.Checked)
            {
                
                checkBox_vsitovaru.Checked = false;
                checkBox_vnayavnosti.Checked = false;
            }
        }

      

        private void checkBox_vnayavnosti_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_vnayavnosti.Checked)
            {
                checkBox_vsitovaru.Checked = false;
                checkBox_preorder.Checked = false;
            }
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void cbManufacture_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox comboBox = (ComboBox)sender;
            //label3.Text = comboBox.Text;
            // Save the selected employee's name, because we will remove
            // the employee's name from the list.
          //  string selectedEmployee = (string)ComboBox1.SelectedItem;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ListBox listBox = (ListBox)sender;
            //if (listBox.SelectedItems.Count != 0)

            //{

            //    // If so, loop through all checked items.

            //    for (int x = 0; x <= listBox.SelectedItems.Count; x++)

            //    {

            //       checkedListBox1.Items.Add("Checked :" + checkedListBox1.SelectedItems[x].ToString());

            //    }
            //}
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        List<string> listmanvub = new List<string>();
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 0)
            {
                foreach (string item in listBox1.SelectedItems)
                {
                    listmanvub.Add(item);

                }


            }

            if (listBox1.SelectedItems.Count >1 )
            {


            }
        }

       private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
       {


            //    if (e.Node.Checked)
            //    {
            //        label3.Text = "Було чекнуто";
            //    }
            //    else
            //    {
            //        label3.Text = "Не Було чекнуто";
            //    }
           // listBox1.Items.Clear();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tbUrl.Text = "";

            if (e.Node.Checked)
            {
                e.Node.BackColor = Color.White;
            }

           
           RecursiveTree(e.Node, "AfterSelect");
            
           List<Product>  vsitovaru = vzyatutovaru(e.Node.Name.ToString(), 2);


        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

           listBox1.Items.Clear();
            if (e.Node.BackColor == Color.Yellow)
            {
               
            }
            else
            {
                e.Node.BackColor = Color.White;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            List<Product> vnayavnosti = vzyatutovaruexec("147");
            product.AddRange(vnayavnosti);
            label_progress.Text = "Всього товарів додано: " + product.Count;
           

         

        }
    }
}


