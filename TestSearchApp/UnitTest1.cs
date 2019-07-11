using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchApp2;
using SolrNet;
using SolrNet.Attributes;
using System;

namespace TestSearchApp
{
    [TestClass]
    public class UnitTest1
    {
    //    [TestMethod]
        public void TestSearchExistingDoc()
        {
            string prodtosearch = "SampleProd";
            int expectedrescount = 1;
            SearchCls s = new SearchCls();
            SolrQueryResults<Product> res=s.searchProd("ProdName", prodtosearch);
            int actual = 0;
            if (res != null)
            {
                actual = res.Count;
                Assert.AreEqual(expectedrescount, actual);
            }
            Assert.Fail();
        }

        [TestMethod]
        public void TestSearchNonExistingDoc()
        {
            string prodtosearch = "NonExistingProduct";
            int expectedrescount = 0;
            SearchCls s = new SearchCls();
            SolrQueryResults<Product> res = s.searchProd("ProdName", prodtosearch);
            int actual = 0;
            if (res != null)
            {
                actual = res.Count;
                Assert.AreEqual(expectedrescount, actual);
            }
            else
            {
                Assert.Fail();

            }
        }

   //     [TestMethod]
        public void TestDeleteNonExistingDoc()
        {
            string prodtosearch = "NonExistingProduct";
            int expectedstatus = 0;
            SearchCls s = new SearchCls();
            int status=s.DeleteFromIndex("ProdName", prodtosearch);
            Console.WriteLine(status);
            Assert.AreEqual(status, expectedstatus);
        }

        [TestMethod]
        public void IndexNewDocument() {
            SearchCls s = new SearchCls();
            Product p = new Product();
            p.Id = new[] { "100" };
            p.InStock = new[] { true };
            p.Manufacturer = new[] { "Samsung" };
            p.price = new[] { new Decimal(38000.00) };
            p.categories = new[] { "Electronics", "Entertainment" };
            p.ProductName = new[] { "Telivision" };
            int actualstatus=s.AddtoIndex(p);
            int expectedstatus = 0;
            SolrQueryResults<Product> res=s.searchProd("ProdName", "Television");
            Assert.AreEqual(expectedstatus,actualstatus);
            if (res != null)
            {
                Assert.AreEqual(res.Count, 1);
            }
            else
            {
                Assert.Fail();
            }
        }

     //   [TestMethod]
        public void TestDeleteNewlyAddedDoc()
        {
            string prodtosearch = "Television";
            int expectedstatus = 0;
            SearchCls s = new SearchCls();
            int status = s.DeleteFromIndex("ProdName", prodtosearch);
            Console.WriteLine(status);
            Assert.AreEqual(status, expectedstatus);
            SolrQueryResults<Product> res = s.searchProd("ProdName", "Television");
            Assert.AreEqual(res.Count, 0);
        }
    }
}
