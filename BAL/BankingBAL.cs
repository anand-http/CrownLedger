using ClosedXML.Excel;
using fintech;
using fintech.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using static fintech.Models.CommonModel;
using fintech.Models;

namespace fintech.BAL
{
    public class BankingBAL
    {
        private BankingDAL dal = new BankingDAL();

        public bool SaveBankAccount(BankAccountCreate model, int entityId,int? loginId, out string messageCode)
        {
            return new BankingDAL().SaveBankAccount(model, entityId, loginId, out messageCode);
        }

    }
}
