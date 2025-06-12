using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRLog
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class SOOrderISOExt : PXCacheExtension<SOOrder>
    {

		#region UsrNCRNumber
		public abstract class usrNCRNumber : BqlString.Field<usrNCRNumber> { }

		[PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(SearchFor<ISORecord.docType>.Where<ISORecord.docType.IsEqual<Messages.nCR>>),
            typeof(ISORecord.docNumber),
            typeof(ISORecord.docType),
            typeof(ISORecord.sOOrderNbr), ValidateValue = false)]
		[PXUIField(DisplayName = "NCR Number", Enabled = false)]
		public virtual string UsrNCRNumber
		{
			get;
			set;
		}
        #endregion

        #region UsrECNNumber
        public abstract class usrECNNumber : BqlString.Field<usrECNNumber> { }

        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(SearchFor<ISORecord.docType>.Where<ISORecord.docType.IsEqual<Messages.eCN>>),
            typeof(ISORecord.docNumber),
            typeof(ISORecord.docType),
            typeof(ISORecord.sOOrderNbr), ValidateValue = false)]
        [PXUIField(DisplayName = "ECN Number", Enabled = false)]
        public virtual string UsrECNNumber
        {
            get;
            set;
        }
        #endregion

    }

}
