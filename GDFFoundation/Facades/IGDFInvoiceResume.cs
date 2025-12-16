using System;
using System.Collections.Generic;

namespace GDFFoundation
{
    public enum GDFInvoiceStatus
    {
        None, 
        Paid, 
        Cancelled,
        Pending,
        Refunded,
        PartiallyRefunded,
    }
    public interface IGDFInvoiceResume
    {
        public string GetReference();
        public string GetOtherId();
        public DateTime GetDate();
        public decimal GetAmount();
        public string GetAmountWithCurrency();
        public string GetPdfUrl();
        public bool GetPaid();
        public GDFInvoiceStatus GetStatus();
        public List<string> GetAssociatedInvoiceList();
    }
}