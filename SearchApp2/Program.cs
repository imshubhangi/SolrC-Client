using System;
using SolrNet;
using SolrNet.Attributes;
using System.Collections.Generic;
using CommonServiceLocator;

namespace SearchApp2
{
    public class SearchCls
    {
        public SearchCls()
        {
            try
            {
                Startup.Init<Product>("http://192.168.0.101:8983/solr/Products");
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        

        public int AddtoIndex(Product p)  {
            Console.WriteLine("Adding Following Doc in Solr Index...");
            Console.WriteLine("Id:"+new List<String>(p.Id)[0]
                +"\nInStock"+p.InStock
                +"\nManufacturer:"+p.Manufacturer
                +"\nPrice:"+p.price
                +"\nProdName:"+p.ProductName
                +"\nCategories:"+p.categories);
            try { 
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
                ResponseHeader rh=solr.Add(p);
                Console.WriteLine("Response Status ="+rh.Status+"\t Operation Elapsed time :"+rh.QTime);
                solr.Commit();
                Console.WriteLine("Indexing of doc is successfull...");
                return rh.Status;
            }
            catch (Exception e) { Console.WriteLine(e); return -1; }
        }

        public int DeleteFromIndex(String field, String value) {
            Console.WriteLine("Deleting Document with "+field+" = "+value);
            ResponseHeader rh=null;
            try
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
                rh=solr.Delete(new SolrQueryByField(field, value));
                Console.WriteLine("Response Status =" + rh.Status + "\t Operation Elapsed time :" + rh.QTime+"\t #params:"+rh.Params.Count);
                foreach (KeyValuePair<String,String> pair in rh.Params)
                {
                    Console.WriteLine(pair.Key + " : " +pair.Value);
                }
                solr.Commit();
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                return -1;
            }
            return (rh.Status);
        }

        public SolrQueryResults<Product> searchProd(String fieldname, String val) {
            SolrQueryResults<Product> res=null;
            try {
                Console.WriteLine("Searching Product with "+fieldname+" = "+val);
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
                res = solr.Query(new SolrQueryByField(fieldname, val));
                ResponseHeader rh = res.Header;
                Console.WriteLine("Response Status =" + rh.Status + "\t Operation Elapsed time :" + rh.QTime);
                Console.WriteLine("Total Number of Documents in Search Result="+res.Count);
                if (res.Count > 0)
                {
                    for (int resultindex = 0; resultindex < res.Count; resultindex++)
                    {
                        foreach (Object o in res[resultindex].ProductName)
                        {
                            Console.WriteLine("ProductName=" + o);
                        }
                        foreach (Object o in res[resultindex].Id)
                        {
                            Console.WriteLine("id=" + o);
                        }
                        foreach (Object o in res[resultindex].InStock)
                        {
                            Console.WriteLine("InStock=" + o);
                        }
                        foreach (Object o in res[resultindex].Manufacturer)
                        {
                            Console.WriteLine("Manufacturer=" + o);
                        }
                        foreach (Object o in res[resultindex].price)
                        {
                            Console.WriteLine("Price=" + o);
                        }

                    }//for
                }//if
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e);
            }
            return res;            
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
           /* SearchCls s = new SearchCls();
            s.searchProd("ProdName","Telivision");
            Console.WriteLine("===========================================");
            Product p = new Product();
            p.Id = new[] {"1"};
            p.InStock = new[] {true};
            p.Manufacturer = new[] {"Samsung"};
            p.price = new[] {new Decimal(38000.00)};
            p.categories = new[] { "Electronics", "Entertainment" };
            p.ProductName = new[] {"Telivision"};
            s.AddtoIndex(p);
            Console.WriteLine("===========================================\n");
            s.searchProd("ProdName","Telivision");
            Console.WriteLine("===========================================\n");
            s.DeleteFromIndex("ProdName","Telivision");
            Console.WriteLine("===========================================\n");
            s.searchProd("ProdName","Telivision");
            Console.WriteLine("===========================================\n");
            */
        }
    }

    public class Product {
        [SolrField("ProdName")]
        public ICollection<string> ProductName { get; set; }
        [SolrUniqueKey("Id")]
        public ICollection<string> Id { get; set; }
        [SolrField("Manufacturer")]
        public ICollection<string> Manufacturer { get; set; }
        [SolrField("Price")]
        public ICollection<decimal> price { set; get; }
        [SolrField("InStock")]
        public ICollection<Boolean> InStock { get; set; }
        [SolrField("Categories")]
        public ICollection<string> categories { get; set; }
    }
}
