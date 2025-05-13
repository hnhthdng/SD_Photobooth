using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.ExportProductDTO
{
    public class ExportProductResponseDTO
    {
        public string ProductId { get; set; }
        public string PublishedState { get; set; } = "published";
        public string PurchaseType { get; set; } = "managed_by_android";
        public bool AutoTranslate { get; set; } = false;
        public string Locale { get; set; } = "vi";
        public string Title { get; set; }
        public string Description { get; set; }
        public bool AutoFillPrices { get; set; } = false;
        public string Price { get; set; }
        public string PricingTemplateId { get; set; } = "";
        public string EEAWithdrawalRightType { get; set; } = "DIGITAL_CONTENT";
        public string ReducedVATRates { get; set; } = "";
        public string CommunicationsAndAmusementTaxes { get; set; } = "";
        public bool TokenizedDigitalAssetDeclared { get; set; } = false;
    }
}
