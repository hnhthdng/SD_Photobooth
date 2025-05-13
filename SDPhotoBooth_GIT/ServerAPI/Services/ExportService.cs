using BusinessLogic.DTO.ExportProductDTO;
using ServerAPI.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI.Service
{
    public class ExportService : IExportService
    {
        public byte[] ExportProductsToCsv(IEnumerable<ExportProductResponseDTO> products)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Product ID,Published State,Purchase Type,Auto Translate,Locale; Title; Description,Auto Fill Prices,Price,Pricing Template ID,EEA Withdrawal Right Type,Reduced VAT Rates,Communications and amusement taxes,Tokenized digital asset declared");

            foreach (var p in products)
            {
                var title = p.Title.Replace(";", ",");
                var description = p.Description.Replace(";", ",");
                sb.AppendLine($"{p.ProductId},{p.PublishedState},{p.PurchaseType},{p.AutoTranslate},{p.Locale};{title};{description},{p.AutoFillPrices},{p.Price},{p.PricingTemplateId},{p.EEAWithdrawalRightType},{p.ReducedVATRates},{p.CommunicationsAndAmusementTaxes},{p.TokenizedDigitalAssetDeclared}");
            }

            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }
    }
}
