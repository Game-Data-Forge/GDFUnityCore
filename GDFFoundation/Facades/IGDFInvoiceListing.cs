using System.Collections.Generic;

namespace GDFFoundation
{
    public interface IGDFInvoiceListing
    {
        int GetCurrentPage();
        int GetTotalPage();
        List<IGDFInvoiceResume> GetInvoiceResumeList();
    }
}