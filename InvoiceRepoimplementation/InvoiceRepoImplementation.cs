using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceRepoImplementation
{
    public class InvoiceRepository 
    {
        private readonly IQueryable<Invoice> _incoices;
        public InvoiceRepository(IQueryable<Invoice> invoices)
        {
            _incoices = invoices ?? throw new ArgumentNullException(nameof(invoices), "invoices can not be null");
        }

        /// <summary>
        /// Should return a total value of an invoice with a given id. If an invoice does not exist null should be returned.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public decimal? GetTotal(int invoiceId)
        {
            var invoice = _incoices.FirstOrDefault(x => x.Id == invoiceId);
            if (invoice == null)
            {
                return null;
            }

            return invoice.InvoiceItems.Sum(x => x.Price * x.Count);
        }

        /// <summary>
        /// Should return a total value of all unpaid invoices.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalOfUnpaid()
        {
            var unpaidList = _incoices.Where(x => x.AcceptanceDate == null).ToList();
            decimal unpaid = 0;
            foreach (var single in unpaidList)
            {
                unpaid += single.InvoiceItems.Sum(x => x.Price * x.Count);
            }
            return unpaid;
        }

        /// <summary>
        /// Should return a dictionary where the name of an invoice item is a key and the number of bought items is a value.
        /// The number of bought items should be summed within a given period of time (from, to). Both the from date and the end date can be null.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to)
        {
            if (from == null)
            {
                if (to ==null)
                {
                    var dic = new Dictionary<string, long>();
                    var data = _incoices
                        .SelectMany(x => x.InvoiceItems)
                        .Select(x => new { x.Count, x.Name }).ToList();

                    foreach (var t in data)
                    {
                        dic[t.Name] = dic.GetValueOrDefault(t.Name, 0) + t.Count;
                    }

                    return dic;
                }

                var dic1 = new Dictionary<string, long>();
                var data1 = _incoices.Where(x => x.CreationDate <= to.Value)
                    .SelectMany(x => x.InvoiceItems)
                    .Select(x => new { x.Count, x.Name }).ToList();

                foreach (var t in data1)
                {
                    dic1[t.Name] = dic1.GetValueOrDefault(t.Name, 0) + t.Count;
                }

                return dic1;

            }

            // the rest part which is  form != null 

            // to == null
            if (  to != null)
            {
                var dic2 = new Dictionary<string, long>();
                var data2 = _incoices.Where(x => x.CreationDate >= from.Value && x.CreationDate <= to.Value)
                    .SelectMany(x => x.InvoiceItems)
                    .Select(x => new { x.Count, x.Name }).ToList();

                foreach (var t in data2)
                {
                    dic2[t.Name] = dic2.GetValueOrDefault(t.Name, 0) + t.Count;
                }

                return dic2;
            }
            // to == null
           
                var dic3 = new Dictionary<string, long>();
                var data3 = _incoices.Where(x => x.CreationDate >= from.Value)
                    .SelectMany(x => x.InvoiceItems)
                    .Select(x => new { x.Count, x.Name }).ToList();

                foreach (var t in data3)
                {
                    dic3[t.Name] = dic3.GetValueOrDefault(t.Name, 0) + t.Count;
                }

                return dic3;
            
        }
    }
}


public class Invoice
{
    // A unique numerical identifier of an invoice (mandatory)
    public int Id { get; set; }
    // A short description of an invoice (optional).
    public string Description { get; set; }
    // A number of an invoice e.g. 134/10/2018 (mandatory).
    public string Number { get; set; }
    // An issuer of an invoice e.g. Metz-Anderson, 600  Hickman Street,Illinois (mandatory).
    public string Seller { get; set; }
    // A buyer of a service or a product e.g. John Smith, 4285  Deercove Drive, Dallas (mandatory).
    public string Buyer { get; set; }
    // A date when an invoice was issued (mandatory).
    public DateTime CreationDate { get; set; }
    // A date when an invoice was paid (optional).
    public DateTime? AcceptanceDate { get; set; }
    // A collection of invoice items for a given invoice (can be empty but is never null).
    public IList<InvoiceItem> InvoiceItems { get; }

    public Invoice()
    {
        InvoiceItems = new List<InvoiceItem>();
    }
}



public class InvoiceItem
{
    // A name of an item e.g. eggs.
    public string Name { get; set; }
    // A number of bought items e.g. 10.
    public uint Count { get; set; }
    // A price of an item e.g. 20.5.
    public decimal Price { get; set; }
}


